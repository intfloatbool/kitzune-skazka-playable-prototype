using Prototype.Managers;
using UnityEngine;

namespace Prototype.Player
{
    public class FoxPlayerTutorial : FoxPlayer
    {
        [SerializeField] private Transform _basicPosition;
        public override void Kill()
        {
            SoundManager.PlaySound("kitsune_death");
            transform.position = _basicPosition.position;
        }
    }
}