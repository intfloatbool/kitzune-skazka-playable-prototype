using UnityEngine;

namespace Prototype
{
    public class TriggerableObject : MonoBehaviour
    {
        [SerializeField] private UnitType _unitType;
        public UnitType UnitType => _unitType;
    }
}
