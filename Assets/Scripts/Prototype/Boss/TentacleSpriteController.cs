using Prototype.SpriteWork;
using UnityEngine;

namespace Prototype.Boss
{
    public class TentacleSpriteController : SpriteController
    {
        [SerializeField] private Color _activateColor;
        [SerializeField] private Color _deactivateColor;
        [SerializeField] private Tentacle _tentacle;

        private void OnValidate()
        {
            _sprites = GetComponentsInChildren<SpriteRenderer>();
        }

        private void Awake()
        {
            _tentacle.OnTentacleActivated += OnTentacleActivated; 
            _tentacle.OnTentacleDeactivated += OnTentacleDeactivated;
        }

        private void OnDestroy()
        {
            _tentacle.OnTentacleActivated -= OnTentacleActivated; 
            _tentacle.OnTentacleDeactivated -= OnTentacleDeactivated;
        }
        
        private void OnTentacleActivated(Tentacle tentacle)
        {
            ApplyForAllSprites((sprite) =>
            {
                sprite.color = _activateColor;
            });
        }

        private void OnTentacleDeactivated(Tentacle tentacle)
        {
            ApplyForAllSprites((sprite) =>
            {
                sprite.color = _deactivateColor;
            });
        }
    }
}