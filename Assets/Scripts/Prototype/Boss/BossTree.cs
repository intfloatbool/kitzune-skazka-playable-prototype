using UnityEngine;

namespace Prototype.Boss
{
    public class BossTree : MonoBehaviour
    {
        [SerializeField] private TargetStepMover _stepMover;
        private void Start()
        {
            _stepMover.Reset();
            _stepMover.SetActiveMove(true);
        }
    }
}
