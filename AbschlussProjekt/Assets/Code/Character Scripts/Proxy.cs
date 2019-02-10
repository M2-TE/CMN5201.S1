using CombatEffectElements;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Proxy : MonoBehaviour
{
	public Entity Entity;
	public Slider HealthBar;
	public CombatEffectPool CombatEffectPool;

	private AudioSource m_audioSource;
	private AudioSource audioSource
	{
		get { return m_audioSource ?? (m_audioSource = GetComponent<AudioSource>()); }
	}

	private AssetManager _amInstance;
	private AssetManager amInstance
	{
		get { return _amInstance ?? (_amInstance = AssetManager.Instance); }
	}

	[SerializeField] private Image allyTargetIndicator;
	[SerializeField] private Image enemyTargetIndicator;
	[SerializeField] private Image selectedIndicator;

	public void SetIndicatorActive(int teamID, bool activeState)
	{
		switch (teamID)
		{
			case 0:
				enemyTargetIndicator.enabled = false;
				allyTargetIndicator.enabled = activeState;
				if(gameObject.activeInHierarchy) StartCoroutine(IndicatorWobble(allyTargetIndicator));
				break;

			case 1:
				allyTargetIndicator.enabled = false;
				enemyTargetIndicator.enabled = activeState;
				if (gameObject.activeInHierarchy) StartCoroutine(IndicatorWobble(enemyTargetIndicator));
				break;
		}

	}

	public void SetSelected(bool selectedState)
	{
		selectedIndicator.enabled = selectedState;
	}

	public void PlaySfx(AudioClip[] sfxArr)
	{
		if(sfxArr.Length > 0) audioSource.PlayOneShot(sfxArr[Random.Range(0, sfxArr.Length)]);
	}

	private IEnumerator IndicatorWobble(Image wobbleImage)
	{
		var wobbleCurve = amInstance.LoadBundle<MiscSettings>(amInstance.Paths.SettingsPath, "Misc Settings").WobbleCurve;
		var wobbleTrans = wobbleImage.transform;
		var baseScale = wobbleTrans.localScale;
		var timer = 0f;
		var curveLength = wobbleCurve[wobbleCurve.length - 1].time;
		while (timer < 1f)
		{
			wobbleTrans.localScale = baseScale * wobbleCurve.Evaluate(timer);
			timer += Time.deltaTime;
			if (wobbleImage.enabled) yield return null;
			else break;
		}

		wobbleTrans.localScale = baseScale;
		yield return null;
	}
}
