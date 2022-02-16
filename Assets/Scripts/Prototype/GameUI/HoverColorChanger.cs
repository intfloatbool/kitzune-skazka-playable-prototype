using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Prototype.GameUI
{
    public class HoverColorChanger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Color _defaultColor = Color.white;
        [SerializeField] private Color _hoverColor = Color.yellow;
        [SerializeField] private Image _targetImage;

        private void OnValidate()
        {
            if (!_targetImage)
                _targetImage = GetComponentInChildren<Image>();
        }

        private void OnEnable()
        {
            SetImgColor(_defaultColor);
        }

        private void OnDisable()
        {
            SetImgColor(_defaultColor);
        }

        private void SetImgColor(Color color)
        {
            if (_targetImage)
                _targetImage.color = color;

        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            SetImgColor(_hoverColor);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            SetImgColor(_defaultColor);
        }
    }
}