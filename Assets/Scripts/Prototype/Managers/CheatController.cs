using System;
using UnityEngine;

namespace Prototype.Managers
{
    public class CheatController : MonoBehaviour
    {
        [SerializeField] private float _timeScaleChange = 0.3f;
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.KeypadPlus))
            {
                var currentTimeScale = GameHelper.GetTimeScale();
                currentTimeScale += _timeScaleChange;
                currentTimeScale = Mathf.Clamp(currentTimeScale, 0, float.MaxValue);
                GameHelper.SetTimeScale(currentTimeScale);
            }
            
            if (Input.GetKeyDown(KeyCode.KeypadMinus))
            {
                var currentTimeScale = GameHelper.GetTimeScale();
                currentTimeScale -= _timeScaleChange;
                currentTimeScale = Mathf.Clamp(currentTimeScale, 0, float.MaxValue);
                GameHelper.SetTimeScale(currentTimeScale);
            }
            
            if (Input.GetKeyDown(KeyCode.Keypad0))
            {
                GameHelper.SetTimeScale(0);
            }
            
            if (Input.GetKeyDown(KeyCode.Keypad1))
            {
                GameHelper.SetTimeScale(1);
            }
        }
    }
}
