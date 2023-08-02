using _Scripts.Utils;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.BattleScripts
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Image _fillableImage;
        [SerializeField] private Image _fillableBackImage;

        private float _maxHp;
        private bool _isShown;
        private Coroutine _hideRoutine;
        
        public void Init(float maxHp)
        {
            _maxHp = maxHp;
            _fillableImage.fillAmount = 1.0f;
            _fillableBackImage.fillAmount = 1.0f;
            _canvasGroup.Show(0.15f);
        }

        public void Hide()
        {
            _canvasGroup.Hide(.15f);
            _isShown = false;
        }

        public void ChangeValue(float currentHp, float timer)
        {
            float targetFillAmount = Mathf.Clamp01(currentHp / _maxHp);
            _fillableImage.fillAmount = targetFillAmount;
            _fillableBackImage.DOFillAmount(targetFillAmount, timer).SetEase(Ease.InQuart).SetLink(gameObject);
        }

        private void Awake()
        {
            _canvasGroup.Hide();
        }
    }
}