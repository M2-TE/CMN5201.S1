public class CollectiveEffect : BaseEffect
{
    protected override void Start()
    {
        base.Start();
        currentFrame = initialFrameOffset - 1;

        CollectiveEffectManager.Register(this);
    }

    public override void SetSprite()
    {
        currentFrame = (currentFrame < sprites.Length - 1) ? currentFrame + 1 : currentFrame = 0;
        base.SetSprite();
    }
}
