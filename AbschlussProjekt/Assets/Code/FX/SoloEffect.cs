using System.Collections;
using UnityEngine;

public class SoloEffect : BaseEffect
{
	private float combatDuration;
	public float CombatDuration
	{
		get
		{
			return
				combatDuration != 0f
				? combatDuration
				: combatDuration = TimeBetweenFrames * sprites.Length + lingeringDuration;
		}
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
	[SerializeField] protected float lingeringDuration = 0f;
	[SerializeField] protected int framerateOverride = 0;
	[SerializeField] protected bool waitForAudio = true;

	protected new IEnumerator Start ()
    {
        base.Start();
		
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
}