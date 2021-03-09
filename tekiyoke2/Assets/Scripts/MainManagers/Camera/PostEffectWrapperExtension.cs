
using DG.Tweening;

public static class PostEffectWrapperExtension
{
    public static Tween To(this PostEffectWrapper self, float endValue, float duration)
    {
        return DOTween.To
        (
            self.GetVolume,
            self.SetVolume,
            endValue,
            duration
        );
    }
}
