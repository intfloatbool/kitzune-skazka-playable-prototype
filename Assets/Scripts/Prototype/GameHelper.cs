using UnityEngine;

namespace Prototype
{
    public class GameHelper
    {
        public static float GetTimeScale() => Time.timeScale;
        public static void SetTimeScale(float timeScale)
        {
            Time.timeScale = timeScale;
        }
    }
}