using System.Collections;
using UnityEngine;

public class SoloEffect : BaseEffect
{
    [SerializeField] protected bool looping;
	[SerializeField] protected float lingeringDuration = 0f;
	[SerializeField] protected int framerateOverride = 12;

	protected new IEnumerator Start ()
    {
        base.Start();

        timeBetweenFrames = 1f / ((framerateOverride != 0) ? framerateOverride : sprites.Length);
        WaitForSeconds waitTime = new WaitForSeconds(timeBetweenFrames);

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

        Destroy(gameObject);
	}
}