using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Utils
{
    [RequireComponent(typeof(Button))]
    public class ButtonDecorator : MonoBehaviour
    {
        public event Action OnClick;

        [SerializeField] private float _scalerDuration = 0.15f;
        [SerializeField] private Button _button;
        
        private Sequence _sequence;
        private bool _interactable = true;

        public void Show(bool immediate = false) => Toggle(true, immediate);
        public void Hide(bool immediate = false) => Toggle(false, immediate);
        public void SetInteractable(bool value) => _interactable = value;

        private void Awake() => _button.onClick.AddListener(ClickButton);
        private void OnDestroy() => _button.onClick.RemoveListener(ClickButton);

        private void ClickButton()
        {
            if (_interactable == false) 
                return;

            OnClick?.Invoke();
        }

        private void Toggle(bool value, bool immediate)
        {
            SetInteractable(value);

            _sequence?.Kill();

            if (immediate)
            {
                transform.localScale = value ? Vector3.one : Vector3.zero;
            }
            else
            {
                _sequence = DOTween.Sequence();
                _sequence.Append(transform.DOScale(1.25f, _scalerDuration));
                _sequence.Append(transform.DOScale(value ? 1f : 0f, _scalerDuration));
            }
        }
    }
}