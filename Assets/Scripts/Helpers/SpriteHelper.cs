using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Helpers
{
    public static class SpriteHelper
    {
        public static IEnumerator FateToAlpha(SpriteRenderer rend, float alpha)
        {
            float baseAlpha = rend.color.a;
            if (baseAlpha != alpha)
            {
                float velocityFactor = 0.05f;
                float minAlpha;
                float maxAlpha;
                float diff;
                if (baseAlpha < alpha)
                {
                    minAlpha = baseAlpha;
                    maxAlpha = alpha;
                    diff = 1.0f;
                }
                else
                {
                    minAlpha = alpha;
                    maxAlpha = baseAlpha;
                    diff = -1.0f;
                }
                while (rend.color.a != alpha)
                {
                    float newAlpha = rend.color.a + diff * velocityFactor;
                    newAlpha = Mathf.Clamp(newAlpha, minAlpha, maxAlpha);
                    rend.color = new Color(rend.color.r, rend.color.g, rend.color.b, newAlpha);
                    yield return null;
                }
            }
        }
    }
}
