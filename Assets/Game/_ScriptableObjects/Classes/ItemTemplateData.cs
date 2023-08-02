using System;
using System.Collections.Generic;
using _Scripts.ItemsLogic;
using _Scripts.ItemsLogic.InventoryScripts;
using _Scripts.ItemsLogic.Stats;
using _Scripts.Utils;
using UnityEngine;

namespace _ScriptableObjects.Classes
{
    [CreateAssetMenu(menuName = "ISN/Create ItemData", fileName = "ItemData")]
    public class ItemTemplateData : ScriptableObject
    {
        [SerializeField] private List<StatRange> _statRanges;
        [SerializeField] private ItemType _itemType;
        [SerializeField] private Sprite _sprite;

        private Dictionary<StatType, Vector2> _statRangesMap = new Dictionary<StatType, Vector2>();
        private Dictionary<StatType, Vector2> StatRangesMap
        {
            get
            {
                if (_statRangesMap.Count == 0)
                    foreach (var statRange in _statRanges)
                        _statRangesMap.Add(statRange.Type, statRange.Range);
                return _statRangesMap;
            }
        }

        private List<StatType> _allTemplateStats = new List<StatType>();

        public IReadOnlyCollection<StatType> AllTemplateStats
        {
            get
            {
                if (_allTemplateStats.Count == 0)
                    foreach (var statRange in _statRanges)
                        _allTemplateStats.Add(statRange.Type);
                return _allTemplateStats;
            }
        }

        public int Index { get; set; }
        public ItemType Type => _itemType;

        public Sprite Sprite => _sprite;
        
        public int GetRandomStat(StatType type) => Mathf.RoundToInt(StatRangesMap[type].RandomValue());
        
    }

    [Serializable]
    public class StatRange
    {
        public StatType Type;
        public Vector2 Range;
    }
}