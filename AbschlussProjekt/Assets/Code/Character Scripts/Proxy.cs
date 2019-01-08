using CombatEffectElements;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Proxy : MonoBehaviour
{
	public Entity Entity;
	public Slider HealthBar;
	public CombatEffectPool CombatEffectPool;
	
	[SerializeField] private Image allyTargetIndicator;
	[SerializeField] private Image enemyTargetIndicator;
	private Vector3 baseScale;
	private bool wobbling;

	public void SetIndicatorActive(int teamID, bool activeState)
	{
		switch (teamID)
		{
			case 0:
				enemyTargetIndicator.enabled = false;
				allyTargetIndicator.enabled = activeState;
				break;

			case 1:
				allyTargetIndicator.enabled = false;
				enemyTargetIndicator.enabled = activeState;
				break;
		}
	}

	public void Wobble()
	{
		if (!wobbling) StartCoroutine(TriggerWobble());
	}

	private IEnumerator TriggerWobble()
	{
		wobbling = true;
		baseScale = transform.localScale;

		AnimationCurve wobbleCurve = AssetManager.Instance.Settings.LoadAsset<MiscSettings>("Misc Settings").charWobbleCurve;
		float timer = 0f;
		float curveLength = wobbleCurve[wobbleCurve.length - 1].time;
		while(timer < 1f)
		{
			transform.localScale = baseScale * wobbleCurve.Evaluate(timer);
			timer += Time.deltaTime;
			yield return null;
		}

		wobbling = false;
		transform.localScale = baseScale;
		yield return null;
	}
}
