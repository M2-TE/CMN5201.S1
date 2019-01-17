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

	public void PlaySfx(AudioClip[] sfxArr)
	{
		if(sfxArr.Length > 0) audioSource.PlayOneShot(sfxArr[Random.Range(0, sfxArr.Length)]);
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
