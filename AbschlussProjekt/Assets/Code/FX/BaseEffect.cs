using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public abstract class BaseEffect : MonoBehaviour
{
	#region Variables
	#region Lighting
	[SerializeField] private bool flickeringEnabled = false;
	[SerializeField] private bool changingLightColors = false;

	private Light m_ownLight;
	protected Light ownLight
	{
		get
		{
			return m_ownLight ?? (overrideLight != null)
				? m_ownLight = overrideLight
				: m_ownLight = GetComponent<Light>();
		}
	}

	#region Range
	[SerializeField] private AnimationCurve lightRangeCurve;
	[SerializeField] private Light overrideLight;
	[SerializeField] private float flickeringIntensity = .8f;
	[SerializeField] private float maxFlickerStep = .4f;
	[SerializeField] private float maxFlickerMod = .5f;
	#endregion

	#region Color
	[SerializeField] private AnimationCurve rCurve;
	[SerializeField] private AnimationCurve gCurve;
	[SerializeField] private AnimationCurve bCurve;
	[SerializeField] private float overrideAlpha = 1f;
	[SerializeField] private float colorIntensity = .25f;
	private Color baseColor;
	#endregion
	#endregion

	#region Animation Settings
	[SerializeField] protected Sprite[] sprites;
    [SerializeField] protected int initialFrameOffset;
	[SerializeField] protected bool looping;

	protected float timeBetweenFrames;
	protected abstract float TimeBetweenFrames { get; }
	protected int currentFrame;
	protected SpriteRenderer ownSpriteRenderer;
	#endregion

	private float updateCounter = 0f;
	#endregion

	#region Startup
	protected virtual void Start()
    {
        ownSpriteRenderer = GetComponent<SpriteRenderer>();
		baseColor = ownLight.color;
        currentFrame = initialFrameOffset - 1;
    }

	public void PreCalcLightRangeCurve()
	{
		lightRangeCurve = new AnimationCurve();

		// set first key to base range
		lightRangeCurve.AddKey(0, ownLight.range);

		float randomVal;
		for (int i = 1; i < sprites.Length - 1; i++)
		{
			randomVal = Random.Range(lightRangeCurve.Evaluate(i - 1) - maxFlickerStep, lightRangeCurve.Evaluate(i - 1) + maxFlickerStep);
			//randomVal = Random.Range(baseLightRange - maxFlickerMod, baseLightRange + maxFlickerMod);
			lightRangeCurve.AddKey(i, Mathf.Clamp(randomVal, ownLight.range - maxFlickerMod, ownLight.range + maxFlickerMod));
		}

		if (looping)
		{
			// smoothing of transition back to first key (continuous curve)
			lightRangeCurve.AddKey(sprites.Length, ownLight.range);

			// add another key that wont be read during playmode (exists only for tangent smoothing during transition to first key)
			lightRangeCurve.AddKey(sprites.Length + 1, lightRangeCurve.Evaluate(1f));
		}
		else
		{
			// rolloff
			lightRangeCurve.AddKey(sprites.Length, 0f);
		}
	}

	public void PreCalcFrameColors()
	{
		rCurve = new AnimationCurve();
		gCurve = new AnimationCurve();
		bCurve = new AnimationCurve();
		for (int frame = 0; frame < sprites.Length; frame++)
		{
			Rect spriteRect = sprites[frame].rect;
			int xMin = (int)spriteRect.position.x;
			int yMin = (int)spriteRect.position.y;
			int xMax = (int)spriteRect.width;
			int yMax = (int)spriteRect.height;

			Color[,] colors = new Color[xMax - xMin, yMax - yMin];
			Color sumCol = new Color(0f, 0f, 0f, 0f);
			for (int x = xMin; x < xMax; x++)
			{
				for (int y = yMin; y < yMax; y++)
				{
					colors[x, y] = sprites[frame].texture.GetPixel(x, y);
					sumCol += sprites[frame].texture.GetPixel(x, y);
				}
			}
			sumCol /= spriteRect.width * spriteRect.height;

			rCurve.AddKey(frame, sumCol.r);
			gCurve.AddKey(frame, sumCol.g);
			bCurve.AddKey(frame, sumCol.b);
		}

		if (looping)
		{
			// smoothing of transition back to first key (continuous curve)
			rCurve.AddKey(sprites.Length, rCurve.Evaluate(0f));
			gCurve.AddKey(sprites.Length, gCurve.Evaluate(0f));
			bCurve.AddKey(sprites.Length, bCurve.Evaluate(0f));

			// add another key that wont be read during playmode (exists only for tangent smoothing during transition to first key)
			rCurve.AddKey(sprites.Length + 1, rCurve.Evaluate(1f));
			gCurve.AddKey(sprites.Length + 1, gCurve.Evaluate(1f));
			bCurve.AddKey(sprites.Length + 1, bCurve.Evaluate(1f));
		}
		else
		{
			// create rolloff
			rCurve.AddKey(sprites.Length, 0f);
			gCurve.AddKey(sprites.Length, 0f);
			bCurve.AddKey(sprites.Length, 0f);
		}
	}
	#endregion

	#region Repeating
	public virtual void SetSprite()
    {
        ownSpriteRenderer.sprite = sprites[currentFrame];
		updateCounter = 0f;
    }

	protected virtual void Update()
	{
		float timeStamp = currentFrame + updateCounter / TimeBetweenFrames;

		if (flickeringEnabled) ownLight.range = Mathf.Lerp(ownLight.range, lightRangeCurve.Evaluate(timeStamp), flickeringIntensity);
		
		if (changingLightColors) ownLight.color = Color.Lerp
				(baseColor, 
				new Color(rCurve.Evaluate(timeStamp), gCurve.Evaluate(timeStamp), bCurve.Evaluate(timeStamp), overrideAlpha), 
				colorIntensity);

		updateCounter += Time.deltaTime;
	}
	#endregion
}