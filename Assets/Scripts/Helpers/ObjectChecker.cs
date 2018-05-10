using UnityEngine;

namespace Assets.Scripts.Helpers
{
    public static class ObjectChecker
    {
        public static void CheckNullity(UnityEngine.Object obj, string error)
        {
            if (obj == null)
            {
                Debug.LogError(error);
            }
        }
    }
}
