using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public abstract class BaseEffect : MonoBehaviour
{
	#region Variables
	#region Lighting
	private Light ownLight;

	#region Range
	[Header("FX Lighting Range")]
	[SerializeField] private bool flickeringEnabled = false;
	[SerializeField] private bool preCalcFlickering = true;
	[SerializeField] private AnimationCurve lightRangeCurve;
	[SerializeField] private Light overrideLight;
	[Range(0f, 1f)] [SerializeField] private float flickeringIntensity = .8f;
	[SerializeField] private float maxFlickerStep = .4f;
	[SerializeField] private float maxFlickerMod = .5f;
	
	private float baseLightRange;
	#endregion

	#region Color
	[Header("FX Lighting Color")]
	[SerializeField] private bool changingLightColors = false;
	[SerializeField] private bool preCalcColors = true;
	[SerializeField] private AnimationCurve rCurve;
	[SerializeField] private AnimationCurve gCurve;
	[SerializeField] private AnimationCurve bCurve;
	[SerializeField] private float overrideAlpha = 1f;
	[Range(0f, 1f)] [SerializeField] private float colorIntensity = .25f;
	
	private Color baseColor;
	#endregion
	#endregion

	#region Animation Settings
	[Header("Anim Settings")]
	[SerializeField] protected Sprite[] sprites;
    [SerializeField] protected int initialFrameOffset;

	protected int currentFrame;
	protected float timeBetweenFrames;
	protected SpriteRenderer ownSpriteRenderer;
	#endregion

	private float updateCounter = 0f;
	#endregion

	#region Startup
	protected virtual void Start()
    {
        ownSpriteRenderer = GetComponent<SpriteRenderer>();
		ownLight = overrideLight ?? GetComponent<Light>();
		ownLight = (overrideLight == null) ? GetComponent<Light>() : overrideLight;
        currentFrame = initialFrameOffset - 1;
		
		if (preCalcFlickering) PreCalcLightRangeCurve();
		if (preCalcColors) PreCalcFrameColors();
    }

	protected void PreCalcLightRangeCurve()
	{
		baseLightRange = ownLight.range;
		lightRangeCurve = new AnimationCurve();

		// set first key to base range
		lightRangeCurve.AddKey(0, baseLightRange);

		float randomVal;
		for (int i = 1; i < sprites.Length - 1; i++)
		{
			randomVal = Random.Range(lightRangeCurve.Evaluate(i - 1) - maxFlickerStep, lightRangeCurve.Evaluate(i - 1) + maxFlickerStep);
			//randomVal = Random.Range(baseLightRange - maxFlickerMod, baseLightRange + maxFlickerMod);
			lightRangeCurve.AddKey(i, Mathf.Clamp(randomVal, ownLight.range - maxFlickerMod, ownLight.range + maxFlickerMod));
		}

		// smoothing of transition back to first key (continuous curve)
		lightRangeCurve.AddKey(sprites.Length - 1, baseLightRange + baseLightRange - lightRangeCurve.Evaluate(1f));
		lightRangeCurve.AddKey(sprites.Length, baseLightRange);
		// add another key that wont be read during playmode (exists only for tangent smoothing during transition to first key)
		lightRangeCurve.AddKey(sprites.Length + 1, lightRangeCurve.Evaluate(1f));
	}

	protected void PreCalcFrameColors()
	{
		baseColor = ownLight.color;
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
		float timeStamp = currentFrame + updateCounter / timeBetweenFrames;

		if (flickeringEnabled) ownLight.range = Mathf.Lerp(baseLightRange, lightRangeCurve.Evaluate(timeStamp), flickeringIntensity);
		else ownLight.range = GetNextLightRange();

		// TODO: color transition back to time: 0f with tangents
		if (changingLightColors) ownLight.color = Color.Lerp
				(baseColor, 
				new Color(rCurve.Evaluate(timeStamp), gCurve.Evaluate(timeStamp), bCurve.Evaluate(timeStamp), overrideAlpha), 
				colorIntensity);
		else GetNextLightColor();

		updateCounter += Time.deltaTime;
	}

	protected float GetNextLightRange()
	{
		//Debug.Log("NYI");
		return baseLightRange;
	}

	protected Color GetNextLightColor()
	{
		//Debug.Log("NYI");
		return baseColor;
	}
	#endregion
}