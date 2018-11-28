using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BaseEffect : MonoBehaviour {
    [SerializeField] public Sprite[] sprites;
    [SerializeField] protected int frameRateOverride;
    [SerializeField] protected bool looping;

    protected SpriteRenderer ownSpriteRenderer;
    protected float timeBetweenFrames;
    protected int currentFrame;

	protected IEnumerator Start ()
    {
        ownSpriteRenderer = GetComponent<SpriteRenderer>();
        timeBetweenFrames = 1f / ((frameRateOverride != 0) ? frameRateOverride : sprites.Length);
        currentFrame = -1;

        WaitForSeconds waitTime = new WaitForSeconds(timeBetweenFrames);
        while (true)
        {
            if (currentFrame < sprites.Length - 1) currentFrame++;
            else if (looping) currentFrame = 0;
            else break;

            SetFrame();
            yield return new WaitForSeconds(timeBetweenFrames);
        }
        Destroy(gameObject);
	}

    protected virtual void SetFrame()
    {
        ownSpriteRenderer.sprite = sprites[currentFrame];
    }
}
