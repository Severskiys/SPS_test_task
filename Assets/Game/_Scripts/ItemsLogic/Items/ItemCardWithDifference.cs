using System.Collections.Generic;
using System.Linq;
using _Scripts.ItemsLogic.Stats;
using TMPro;
using UnityEngine;

namespace _Scripts.ItemsLogic.Items
{
    public class ItemCardWithDifference : ItemCard
    {
        [SerializeField] private TMP_Text _differenceText;
        private Item _oldItem;
        
        public void FillItemDifference(Item oldItem)
        {
            _oldItem = oldItem;
            int difference = CalculateDifference();
            SetDifferenceView(difference);
            ShowDifferenceArrows();
        }

        private void ShowDifferenceArrows()
        {
            foreach (KeyValuePair<StatType, StatView> statView in StatsToTextMap)
            {
                int oldValue = _oldItem.Stats.GetValueOrDefault(statView.Key);
                int newValue = NewItem.Stats.GetValueOrDefault(statView.Key);

                if (oldValue > newValue)
                {
                    statView.Value.SetRedDifferenceArrow();
                    continue;
                }

                if (newValue > oldValue)
                    statView.Value.SetGreenDifferenceArrow();
            }
        }

        private int CalculateDifference()
        {
            int oldStatsSum = _oldItem.Stats.Sum(s => s.Value);
            int newStatsSum = NewItem.Stats.Sum(s => s.Value);
            int difference = newStatsSum - oldStatsSum;
            return difference;
        }

        private void SetDifferenceView(int difference)
        {
            _differenceText.text = difference.ToString();
            _differenceText.color = difference > 0 ? Color.green * 0.75f : Color.red * 0.4f;
        }

        public void ClearDifference()
        {
            _differenceText.text = " ";
        }
    }
}