using System;
using UnityEngine;

namespace EasingUtil
{
    public enum EaseEnum
    {
        Linear,
        InBack,
        InBounce,
        InCirc,
        InCubic,
        InElastic,
        InExpo,
        InQuad,
        InQuart,
        InQuint,
        InSine,
        OutBack,
        OutBounce,
        OutCirc,
        OutCubic,
        OutElastic,
        OutExpo,
        OutQuad,
        OutQuart,
        OutQuint,
        OutSine,
        InOutBack,
        InOutBounce,
        InOutCirc,
        InOutCubic,
        InOutElastic,
        InOutExpo,
        InOutQuad,
        InOutQuart,
        InOutQuint,
        InOutSine,
    }

    public static class EasingFunction
    {
        /// <summary>
        /// Gets the easing function
        /// </summary>
        /// <param name="type">ease type</param>
        /// <returns>easing function</returns>
        public static Func<float, float> Get(EaseEnum type)
        {
            switch (type)
            {
                case EaseEnum.Linear: return Linear;
                case EaseEnum.InBack: return InBack;
                case EaseEnum.InBounce: return InBounce;
                case EaseEnum.InCirc: return InCirc;
                case EaseEnum.InCubic: return InCubic;
                case EaseEnum.InElastic: return InElastic;
                case EaseEnum.InExpo: return InExpo;
                case EaseEnum.InQuad: return InQuad;
                case EaseEnum.InQuart: return InQuart;
                case EaseEnum.InQuint: return InQuint;
                case EaseEnum.InSine: return InSine;
                case EaseEnum.OutBack: return OutBack;
                case EaseEnum.OutBounce: return OutBounce;
                case EaseEnum.OutCirc: return OutCirc;
                case EaseEnum.OutCubic: return OutCubic;
                case EaseEnum.OutElastic: return OutElastic;
                case EaseEnum.OutExpo: return OutExpo;
                case EaseEnum.OutQuad: return OutQuad;
                case EaseEnum.OutQuart: return OutQuart;
                case EaseEnum.OutQuint: return OutQuint;
                case EaseEnum.OutSine: return OutSine;
                case EaseEnum.InOutBack: return InOutBack;
                case EaseEnum.InOutBounce: return InOutBounce;
                case EaseEnum.InOutCirc: return InOutCirc;
                case EaseEnum.InOutCubic: return InOutCubic;
                case EaseEnum.InOutElastic: return InOutElastic;
                case EaseEnum.InOutExpo: return InOutExpo;
                case EaseEnum.InOutQuad: return InOutQuad;
                case EaseEnum.InOutQuart: return InOutQuart;
                case EaseEnum.InOutQuint: return InOutQuint;
                case EaseEnum.InOutSine: return InOutSine;
                default: return Linear;
            }
        }

        static float Linear(float t) => t;

        static float InBack(float t) => t * t * t - t * Mathf.Sin(t * Mathf.PI);

        static float OutBack(float t) => 1f - InBack(1f - t);

        static float InOutBack(float t) =>
            t < 0.5f
                ? 0.5f * InBack(2f * t)
                : 0.5f * OutBack(2f * t - 1f) + 0.5f;

        static float InBounce(float t) => 1f - OutBounce(1f - t);

        static float OutBounce(float t) =>
            t < 4f / 11.0f ?
                (121f * t * t) / 16.0f :
            t < 8f / 11.0f ?
                (363f / 40.0f * t * t) - (99f / 10.0f * t) + 17f / 5.0f :
            t < 9f / 10.0f ?
                (4356f / 361.0f * t * t) - (35442f / 1805.0f * t) + 16061f / 1805.0f :
                (54f / 5.0f * t * t) - (513f / 25.0f * t) + 268f / 25.0f;

        static float InOutBounce(float t) =>
            t < 0.5f
                ? 0.5f * InBounce(2f * t)
                : 0.5f * OutBounce(2f * t - 1f) + 0.5f;

        static float InCirc(float t) => 1f - Mathf.Sqrt(1f - (t * t));

        static float OutCirc(float t) => Mathf.Sqrt((2f - t) * t);

        static float InOutCirc(float t) =>
            t < 0.5f
                ? 0.5f * (1 - Mathf.Sqrt(1f - 4f * (t * t)))
                : 0.5f * (Mathf.Sqrt(-((2f * t) - 3f) * ((2f * t) - 1f)) + 1f);

        static float InCubic(float t) => t * t * t;

        static float OutCubic(float t) => InCubic(t - 1f) + 1f;

        static float InOutCubic(float t) =>
            t < 0.5f
                ? 4f * t * t * t
                : 0.5f * InCubic(2f * t - 2f) + 1f;

        static float InElastic(float t) => Mathf.Sin(13f * (Mathf.PI * 0.5f) * t) * Mathf.Pow(2f, 10f * (t - 1f));

        static float OutElastic(float t) => Mathf.Sin(-13f * (Mathf.PI * 0.5f) * (t + 1)) * Mathf.Pow(2f, -10f * t) + 1f;

        static float InOutElastic(float t) =>
            t < 0.5f
                ? 0.5f * Mathf.Sin(13f * (Mathf.PI * 0.5f) * (2f * t)) * Mathf.Pow(2f, 10f * ((2f * t) - 1f))
                : 0.5f * (Mathf.Sin(-13f * (Mathf.PI * 0.5f) * ((2f * t - 1f) + 1f)) * Mathf.Pow(2f, -10f * (2f * t - 1f)) + 2f);

        static float InExpo(float t) => Mathf.Approximately(0.0f, t) ? t : Mathf.Pow(2f, 10f * (t - 1f));

        static float OutExpo(float t) => Mathf.Approximately(1.0f, t) ? t : 1f - Mathf.Pow(2f, -10f * t);

        static float InOutExpo(float v) =>
            Mathf.Approximately(0.0f, v) || Mathf.Approximately(1.0f, v)
                ? v
                : v < 0.5f
                    ? 0.5f * Mathf.Pow(2f, (20f * v) - 10f)
                    : -0.5f * Mathf.Pow(2f, (-20f * v) + 10f) + 1f;

        static float InQuad(float t) => t * t;

        static float OutQuad(float t) => -t * (t - 2f);

        static float InOutQuad(float t) =>
            t < 0.5f
                ? 2f * t * t
                : -2f * t * t + 4f * t - 1f;

        static float InQuart(float t) => t * t * t * t;

        static float OutQuart(float t)
        {
            var u = t - 1f;
            return u * u * u * (1f - t) + 1f;
        }

        static float InOutQuart(float t) =>
            t < 0.5f
                ? 8f * InQuart(t)
                : -8f * InQuart(t - 1f) + 1f;

        static float InQuint(float t) => t * t * t * t * t;

        static float OutQuint(float t) => InQuint(t - 1f) + 1f;

        static float InOutQuint(float t) =>
            t < 0.5f
                ? 16f * InQuint(t)
                : 0.5f * InQuint(2f * t - 2f) + 1f;

        static float InSine(float t) => Mathf.Sin((t - 1f) * (Mathf.PI * 0.5f)) + 1f;

        static float OutSine(float t) => Mathf.Sin(t * (Mathf.PI * 0.5f));

        static float InOutSine(float t) => 0.5f * (1f - Mathf.Cos(t * Mathf.PI));
    }
}
