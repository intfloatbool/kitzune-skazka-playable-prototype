using System;
using UnityEngine;

namespace Prototype.SpriteWork
{
    public class SpriteController : MonoBehaviour
    {
        [SerializeField] protected SpriteRenderer[] _sprites;

        protected virtual void ApplyForAllSprites(Action<SpriteRenderer> actionWithSprite)
        {
            foreach (var sprite in _sprites)
            {
                if (sprite)
                    actionWithSprite(sprite);
            }
        }
    }
}