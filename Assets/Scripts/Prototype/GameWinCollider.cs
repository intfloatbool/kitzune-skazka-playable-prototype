using System;
using UnityEngine;

namespace Prototype
{
    [RequireComponent(typeof(TriggerCollider))]
    public class GameWinCollider : MonoBehaviour
    {
        [SerializeField] protected UnitType _unitToTrigger;
        private void Awake()
        {
            var triggerCollider = GetComponent<TriggerCollider>();
            triggerCollider.OnTriggerCallback = OnTriggerCallback;
        }

        private void OnTriggerCallback(TriggerableObject triggerable, Collider2D collider2D)
        {
            if (triggerable.UnitType == _unitToTrigger)
            {
                OnTriggered(triggerable, collider2D);
            }
        }

        protected virtual void OnTriggered(TriggerableObject triggerableObject, Collider2D collider)
        {
            if (GameManager.Instance)
            {
                GameManager.Instance.GameWin();
            }
            else
            {
                Debug.LogError("GameManager instance is missing!");
            }
        }
    }
}