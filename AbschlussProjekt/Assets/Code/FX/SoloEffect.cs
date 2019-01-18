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

	public float initialAnimationDelay;
	[SerializeField] protected float initialAudioDelay;
	[SerializeField] protected float lingeringDuration;
	[SerializeField] protected int framerateOverride;
	[SerializeField] protected bool waitForAudio = true;
	private bool effectVisible = false;
	
	protected new IEnumerator Start ()
	{
		base.Start();


		yield return new WaitForSeconds(initialAnimationDelay);
		effectVisible = true;

		WaitForSeconds waitTime = new WaitForSeconds(TimeBetweenFrames);
		while (true)
        {
            if (currentFrame < sprites.Length - 1) currentFrame++;
            else if (looping) currentFrame = 0;
            else break;

            SetSprite();
            yield return waitTime;
        }

		ownSpriteRenderer.sprite = null;
		yield return new WaitForSeconds(lingeringDuration);

		if (waitForAudio)
		{
			AudioSource audioSource = GetComponent<AudioSource>();
			while (audioSource.isPlaying) yield return null;
		}

        Destroy(gameObject);
	}

	protected override void Update()
	{
		if (!effectVisible) ownLight.range = 0f;
		else base.Update();
	}

	public IEnumerator PlaySfx(AudioClip[] sfxArr)
	{
		yield return new WaitForSeconds(initialAudioDelay);
		if (sfxArr.Length > 0) audioSource.PlayOneShot(sfxArr[Random.Range(0, sfxArr.Length)]);
	}
}