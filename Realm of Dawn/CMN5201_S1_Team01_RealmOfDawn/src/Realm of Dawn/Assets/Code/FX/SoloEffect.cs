using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoloEffect : BaseEffect
{
	private float rawCombatDuration;
	public float RawCombatDuration
	{
		get
		{
			return
				rawCombatDuration != 0f
				? rawCombatDuration
				: rawCombatDuration = TimeBetweenFrames * sprites.Length + lingeringDuration;
		}
	}

	protected AudioSource m_audioSource;
	protected AudioSource audioSource
	{
		get { return m_audioSource ?? (m_audioSource = GetComponent<AudioSource>()); }
	}

	protected override float TimeBetweenFrames
	{
		get
		{
			return timeBetweenFrames != 0f
				? timeBetweenFrames
				: timeBetweenFrames = 1f / ((framerateOverride != 0) ? framerateOverride : sprites.Length);
		}
	}
	
	[SerializeField] protected float lingeringDuration;
	[SerializeField] protected int framerateOverride;
	[SerializeField] protected bool waitForAudio = true;

	protected override void Start()
	{
		base.Start();

		// starts itself when set too looping, audio clip and audio properties will be
		// set in audio source component instead of using the PlaySfx func
		if (looping) StartCoroutine(PlayAnimation(0f));
	}

	public IEnumerator PlaySfx(AudioClip[] sfxArr, float audioDelay)
	{
		if (sfxArr.Length > 0)
		{
			yield return new WaitForSeconds(audioDelay);
			audioSource.PlayOneShot(sfxArr[Random.Range(0, sfxArr.Length)]);
		}
	}

	public IEnumerator PlayAnimation(float animationDelay)
	{
		yield return new WaitForSeconds(animationDelay);

		WaitForSeconds waitTime = new WaitForSeconds(TimeBetweenFrames);
		while (true)
		{
			if (currentFrame < sprites.Length - 1) currentFrame++;
			else if (looping) currentFrame = 0;
			else break;

			SetSprite();
			yield return waitTime;
		}

		if (this != null) ownSpriteRenderer.sprite = null;
		yield return new WaitForSeconds(lingeringDuration);

		if (waitForAudio)
		{
			AudioSource audioSource = null;
			if (this != null) audioSource = GetComponent<AudioSource>();
			while (audioSource != null && audioSource.isPlaying) yield return null;
		}

		if(this != null) Destroy(gameObject);
	}
}