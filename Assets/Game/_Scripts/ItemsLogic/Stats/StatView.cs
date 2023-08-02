using System;
using _Scripts.Utils;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.ItemsLogic.Stats
{
    public class StatView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _tmpText;
        [SerializeField] private Image _arrowImageRed;
        [SerializeField] private Image _arrowImageGreen;
        [SerializeField] private CanvasGroup _canvasGroup;

        public void SetText(string text) => _tmpText.text = text;

        public void SetRedDifferenceArrow()
        {
            _arrowImageRed.DOFade(1, 0);
            _arrowImageGreen.DOFade(0, 0);
        }

        public void SetGreenDifferenceArrow()
        {
            _arrowImageRed.DOFade(0, 0);
            _arrowImageGreen.DOFade(1, 0);
        }

        private void OnEnable()
        {
            _arrowImageRed.DOFade(0, 0);
            _arrowImageGreen.DOFade(0, 0);
        }

        public void Show()
        {
            _canvasGroup.Show(0.15f);
        }

        public void Hide()
        {
            _canvasGroup.Hide(0.15f);
        }
    }
}