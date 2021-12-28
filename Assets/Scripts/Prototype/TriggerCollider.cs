using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace Prototype
{
    [RequireComponent(typeof(Collider2D))]
    public class TriggerCollider : MonoBehaviour
    {
        public Action<TriggerableObject, Collider2D> OnTriggerCallback { get; set; }

        private void OnValidate()
        {
            var col = GetComponent<Collider2D>();
            Assert.IsTrue(col.isTrigger, "col.isTrigger == true");
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.TryGetComponent(out TriggerableObject triggerableObject))
            {
                OnTriggerCallback?.Invoke(triggerableObject, col);
                //Debug.Log("Triggered: " + triggerableObject.name);
            }
        }
    }
}
