using System.Collections.Generic;
using System.Globalization;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace _Scripts.Money
{
    public class MoneyView : MonoBehaviour
    {
        [SerializeField] private Transform _resourceParent;
        [SerializeField] private TMP_Text _resourceCountText;
        [SerializeField] private Image _resourceIcon;
        [SerializeField] private Image _resourceViewPrefab;
        [SerializeField] private int _maxSpawnCount = 15;
        [SerializeField] private int _minAddedValueAtOneTime = 10;
        [SerializeField] private float _randomRadius = 100;

        private readonly Stack<Image> _flyResources = new();
        private Sequence _flySequence;
        private float _currentResourceCount;
        private Vector3 FlyTarget => _resourceIcon.transform.position;

        private void Awake() => SetMoneyView();

        private void OnEnable()
        {
            MoneyHandler.OnMoneyAdd += AddFlyResource;
            MoneyHandler.OnMoneySubtracted += SubtractResource;
        }

        private void OnDisable()
        {
            MoneyHandler.OnMoneyAdd -= AddFlyResource;
            MoneyHandler.OnMoneySubtracted -= SubtractResource;
        }

        private void SetMoneyView()
        {
            _currentResourceCount = MoneyHandler.GetMoneyCount();
            RefreshText();
        }


        private void AddFlyResource(float value, Vector3 screenPosition)
        {
            int resourceCount = Mathf.CeilToInt(value / _minAddedValueAtOneTime);
            resourceCount = Mathf.Clamp(resourceCount, 1, _maxSpawnCount);
            int defaultIncreaseValue = Mathf.FloorToInt(value / resourceCount);
            float lastIncreaseValue = value - (defaultIncreaseValue * resourceCount) + defaultIncreaseValue;
            Sequence globalFlySequence = DOTween.Sequence();
            List<Image> flyResourceTemp = new List<Image>();

            for (var i = 0; i < resourceCount; i++)
            {
                int index = i;
                Image flyResource = GetFlyResource();
                Transform flyTransform = flyResource.transform;
                flyTransform.DOKill();
                flyResourceTemp.Add(flyResource);
                flyTransform.localScale = Vector3.zero;
                
                if (screenPosition == default)
                    flyTransform.localPosition = Random.insideUnitCircle * _randomRadius;
                else
                    flyTransform.position = screenPosition + (Vector3)(Random.insideUnitCircle * _randomRadius);

                flyResource.gameObject.SetActive(true);
                _flySequence?.Kill();
                _flySequence = DOTween.Sequence();
                float delay = i == 0 ? 0 : Random.Range(0.1f, 0.25f);
                _flySequence.Append(flyTransform.DOScale(1f, 0.2f).SetEase(Ease.OutCirc).SetDelay(delay));
                _flySequence.Join(flyTransform.DOMove(FlyTarget, 0.6f).SetEase(Ease.InBack));
                _flySequence.OnComplete(() =>
                {
                    flyTransform.DOScale(0f, 0.2f).SetEase(Ease.InCirc).OnComplete(delegate
                    {
                        flyResource.gameObject.SetActive(false);
                        flyTransform.SetParent(_resourceParent);
                    });

                    AddResource(index == resourceCount - 1 ? lastIncreaseValue : defaultIncreaseValue);
                });

                globalFlySequence.Join(_flySequence);
            }
            
            globalFlySequence.OnComplete(()=>
            {
                foreach (var resource in flyResourceTemp)
                {
                    resource.gameObject.SetActive(false);
                    resource.transform.localPosition = Vector3.zero;
                    _flyResources.Push(resource);
                }
            });
        }

        private void SubtractResource(float value)
        {
            _currentResourceCount -= value;
            ScaleIcon();
            RefreshText(); 
        }

        private Image GetFlyResource()
        {
            if (_flyResources.Count == 0) 
                SpawnResource();
            
            Image resource = _flyResources.Pop();
            return resource;
        }
        
        private void SpawnResource()
        {
            Image resource = Instantiate(_resourceViewPrefab, _resourceParent);
            resource.gameObject.SetActive(false);
            resource.transform.localPosition = Vector3.zero;
            _flyResources.Push(resource);
        }

        private void AddResource(float value)
        {
            _currentResourceCount += value;
            ScaleIcon();
            RefreshText();
        }
        
        private void RefreshText()
        {
            float showCount = Mathf.Clamp(_currentResourceCount, 0, int.MaxValue);
            int intCount = Mathf.RoundToInt(showCount);
            string text = intCount.ToString(CultureInfo.InvariantCulture);
            _resourceCountText.text = text;
        }
        
        private void ScaleIcon()
        {
            _resourceIcon.transform.DOKill();
            _resourceIcon.transform.DOScale(1.15f, 0.1f).SetEase(Ease.OutQuint).
                OnComplete(() => _resourceIcon.transform.DOScale(1f, 0.3f));
        }
    }
}