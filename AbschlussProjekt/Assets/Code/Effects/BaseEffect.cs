using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public abstract class BaseEffect : MonoBehaviour
{
    [SerializeField] protected Sprite[] sprites;
    [SerializeField] protected int initialFrameOffset;

    protected SpriteRenderer ownSpriteRenderer;
    protected int currentFrame;

    protected virtual void Start()
    {
        ownSpriteRenderer = GetComponent<SpriteRenderer>();
        currentFrame = initialFrameOffset - 1;
    }

    public virtual void SetSprite()
    {
        ownSpriteRenderer.sprite = sprites[currentFrame];
    }
}