using Prototype.Boss;
using UnityEngine;

namespace Prototype
{
    public class TentacleWinCollider : GameWinCollider
    {
        protected override void OnTriggered(TriggerableObject triggerableObject, Collider2D collider)
        {
            var tentacle = triggerableObject.transform.root.GetComponentInChildren<Tentacle>();
            if (tentacle)
            {
                if (tentacle.CurrentState == Tentacle.TentacleState.ATTACK)
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
            else
            {
                Debug.LogError("Tentacle is missing!");
            }
        }
    }
}