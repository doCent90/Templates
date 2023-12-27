using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Core
{
    public static class ExtentionsCanvases
    {
        private static string[] _formatName = new[]
        {
            "", "K", "M", "T", "B", "S", "Q", "R", "X"
        };

        public const float Duration = 0.3f;
        public const float Delay = 0.5f;

        public static void EnableGroup(this CanvasGroup canvasGroup, float duration = Duration)
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.DOComplete(true);
            canvasGroup.DOFade(1f, duration);
        }

        public static void DelayedEnableGroup(this CanvasGroup canvasGroup, float duration = Duration, float delay = Delay)
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.DOComplete(true);
            canvasGroup.DOFade(1f, duration).SetDelay(delay);
        }

        public static void EnableInteractGroup(this CanvasGroup canvasGroup)
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }

        public static void DelayedDisableGroup(this CanvasGroup canvasGroup, float duration = Duration, float delay = Delay)
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.DOComplete(true);
            canvasGroup.DOFade(0f, duration).SetDelay(delay);
        }

        public static void DisableGroup(this CanvasGroup canvasGroup, float duration = Duration)
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.DOComplete(true);
            canvasGroup.DOFade(0f, duration);
        }

        public static void DisableGroups(this CanvasGroup[] canvasGroups, float duration = Duration)
        {
            foreach (CanvasGroup canvas in canvasGroups)
                canvas.DisableGroup();
        }

        public static void DisableInteractGroup(this CanvasGroup canvasGroup)
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }

        public static void EnableFade(this CanvasGroup canvasGroup, float duration = Duration)
        {
            canvasGroup.DOComplete(true);
            canvasGroup.DOFade(1f, duration);
        }

        public static void EnableFadeBack(this CanvasGroup canvasGroup, Action complete, float duration = Duration)
        {
            canvasGroup.DOComplete(true);
            canvasGroup.DOFade(1f, duration).OnComplete(() => complete.Invoke());
        }

        public static void DOFadeImage(this Image image, float value, float duration = Duration, Action complete = null)
        {
            image.DOFade(value, duration).OnComplete(() => complete.Invoke());
        }

        public static void DOValueBack(this Slider slider, float value, float duration = Duration, Action complete = null)
        {
            slider.DOValue(value, duration).OnComplete(() => complete.Invoke());
        }

        public static void DOFadeSprite(this SpriteRenderer sprite, float value, float duration = Duration, Action complete = null)
        {
            sprite.DOFade(value, duration).OnComplete(() => complete.Invoke());
        }

        public static void DisableFade(this CanvasGroup canvasGroup, float duration = Duration)
        {
            canvasGroup.DOComplete(true);
            canvasGroup.DOFade(0f, duration);
        }

        public static Tween GetDisableFade(this CanvasGroup canvasGroup, float duration = Duration)
        {
            return canvasGroup.DOFade(0f, duration);
        }

        public static string FormatNumbers(float value)
        {
            if (value <= 0)
                return "0";

            int index;
            int devide = 1000;

            if (value < devide)
                return value.ToString("0");

            for (index = 0; index < _formatName.Length; index++)
            {
                if (value >= devide)
                    value /= devide;
                else
                    break;
            }

            return value.ToString("0.0") + _formatName[index];
        }

        public static Tween DOFloat(this float currnetValue, float value, float time)
            => DOTween.To(() => currnetValue, x => currnetValue = x, value, time);

        public static Tween DOValue(this Slider slider, float value, float time)
            => DOTween.To(() => slider.value, x => slider.value = x, value, time);

        public static Tween DOFade(this MaskableGraphic maskable, float value, float time)
            => DOTween.To(() => maskable.color.a, x => maskable.color = new Color(maskable.color.r, maskable.color.g, maskable.color.b, x), value, time).SetEase(Ease.Linear);

        public static Tween DOFade(this SpriteRenderer sprite, float value, float time)
            => DOTween.To(() => sprite.color.a, x => sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, x), value, time).SetEase(Ease.Linear);

        public static Tween DOFade(this Image image, float value, float time)
            => DOTween.To(() => image.color.a, x => image.color = new Color(image.color.r, image.color.g, image.color.b, x), value, time).SetEase(Ease.Linear);

        public static Tween DOFade(this CanvasGroup canvasGroup, float value, float time) =>
            DOTween.To(() => canvasGroup.alpha, x => canvasGroup.alpha = x, value, time);

        public static Tween DORigidbody(this Vector2 body, Vector2 to, float time)
            => DOTween.To(() => body, x => body = x, to, time);
    }
}
