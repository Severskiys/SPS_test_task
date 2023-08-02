using System.Collections.Generic;
using _Scripts.ItemsLogic.Stats;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.ItemsLogic.Items
{
    public class ItemCard : MonoBehaviour
    {
        [SerializeField] private Image _itemIcon;
        [SerializeField] private TMP_Text _itemName;
        [SerializeField] private Transform _itemStatsParent;
        [SerializeField] private StatView _statViewPrefab;

        protected Dictionary<StatType, StatView> StatsToTextMap = new Dictionary<StatType, StatView>();
        protected Item NewItem;

        public void FillCard(Item item)
        {
            NewItem = item;
            _itemIcon.sprite = item.Sprite;
            _itemIcon.SetNativeSize();
            _itemIcon.DOFade(1, 0.15f);
            NewItem.SetPosition(_itemIcon.transform.position);
            _itemName.text = item.Type.ToString();
            _itemName.DOFade(1, 0.15f);
            CreateStatsView(item);
            FillStatsView(item);
        }

        private void CreateStatsView(Item item)
        {
            if (StatsToTextMap.Count == 0)
            {
                foreach (KeyValuePair<StatType, int> stat in item.Stats)
                {
                    StatView statView = Instantiate(_statViewPrefab, _itemStatsParent);
                    StatsToTextMap.Add(stat.Key, statView);
                }
            }
        }

        private void FillStatsView(Item item)
        {
            foreach (KeyValuePair<StatType, int> stat in item.Stats)
            {
                string statText = stat.Key + ": " + stat.Value;
                StatsToTextMap[stat.Key].SetText(statText);
                StatsToTextMap[stat.Key].Show();
            }
        }

        public void ClearCard()
        {
            _itemIcon.DOFade(0, 0);
            _itemName.DOFade(0, 0);

            if (StatsToTextMap.Count > 0)
            {
                foreach (var pair in StatsToTextMap)
                {
                    pair.Value.Hide();
                }
            }
        }
    }
}