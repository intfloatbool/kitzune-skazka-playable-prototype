using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Prototype.Common
{
    [RequireComponent(typeof(Image))]
    public class ImageDoColor : MonoBehaviour
    {
        [SerializeField] private Color _colorToDo = Color.black;
        [SerializeField] private float _time = 0.5f;
        
        public void DoColor()
        {
            var image = GetComponent<Image>();
            image.DOColor(_colorToDo, _time);
        }
    }
}