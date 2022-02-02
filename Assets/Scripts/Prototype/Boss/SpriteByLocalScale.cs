using System;
using System.Linq;
using UnityEngine;

namespace Prototype.Boss
{
    public class SpriteByLocalScale : MonoBehaviour
    {
        [Serializable]
        private class SpriteByScale
        {
            public float scale;
            public Sprite sprite;
        }
        
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Transform _target;
        [SerializeField] private bool _isForX;
        [SerializeField] private bool _isForY;
        [SerializeField] private bool _isForZ;
        
        [Space]
        [Header("Order is matter!")]
        [SerializeField] private SpriteByScale[] _sprites;

        private void Update()
        {
            float targetScale = 0f;
            if (_isForX)
            {
                targetScale = _target.localScale.x;
            }

            if (_isForY)
            {
                targetScale = _target.localScale.y; 
            }

            if (_isForZ)
            {
                targetScale = _target.localScale.z;
            }

            SpriteByScale targetSprite = null;

            for (int i = 0; i < _sprites.Length; i++)
            {
                var spriteByScale = _sprites[i];
                if (Mathf.Approximately(targetScale, spriteByScale.scale) || spriteByScale.scale >= targetScale)
                {
                    targetSprite = spriteByScale;
                    break;
                }
            }
            
            if (targetSprite != null)
            {
                _spriteRenderer.sprite = targetSprite.sprite;
            }
        }
    }
}