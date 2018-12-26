using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Light))]
public abstract class BaseEffect : MonoBehaviour
{
	[Header("FX Lighting")]
	[SerializeField] protected AnimationCurve lightRangeCurve;
	[SerializeField] protected float maxFlickerStep;
	[SerializeField] protected float maxFlickerMod;

	[Header("Anim Settings")]
	[SerializeField] protected Sprite[] sprites;
    [SerializeField] protected int initialFrameOffset;

	protected Light ownLight;
	protected SpriteRenderer ownSpriteRenderer;
    protected int currentFrame;

    protected virtual void Start()
    {
        ownSpriteRenderer = GetComponent<SpriteRenderer>();
		ownLight = GetComponent<Light>();
        currentFrame = initialFrameOffset - 1;

		CalculateLightRangeCurve();
    }

	protected void CalculateLightRangeCurve()
	{
		lightRangeCurve = new AnimationCurve();
		lightRangeCurve.AddKey(0, ownLight.range);
		lightRangeCurve.AddKey(sprites.Length - 1, ownLight.range);

		float randomVal;
		for (int i = 1; i < sprites.Length - 1; i++)
		{
			//randomVal = Random.Range(ownLight.range - maxFlickerMod, ownLight.range + maxFlickerMod);
			//lightRangeCurve.AddKey(i, Mathf.MoveTowards(lightRangeCurve.Evaluate(i - 1) ,randomVal, maxFlickerStep));


			randomVal = Random.Range(lightRangeCurve.Evaluate(i - 1) - maxFlickerStep, lightRangeCurve.Evaluate(i - 1) + maxFlickerStep);
			lightRangeCurve.AddKey(i, Mathf.Clamp(randomVal, ownLight.range - maxFlickerMod, ownLight.range + maxFlickerMod));
		}
	}

    public virtual void SetSprite()
    {
        ownSpriteRenderer.sprite = sprites[currentFrame];
		ownLight.range = lightRangeCurve.Evaluate(currentFrame);
    }
}