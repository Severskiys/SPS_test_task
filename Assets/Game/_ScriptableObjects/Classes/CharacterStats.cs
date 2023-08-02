using System;
using System.Collections.Generic;
using _Scripts.ItemsLogic.Stats;
using UnityEngine;

namespace _ScriptableObjects.Classes
{
    [CreateAssetMenu(menuName = "ISN/Create CharacterStats", fileName = "CharacterStats", order = 0)]
    public class CharacterStats : ScriptableObject
    {
        [SerializeField] private List<Stat> _stats;

        private Dictionary<StatType, int> _statsMap = new Dictionary<StatType, int>();

        public IReadOnlyCollection<StatType> StatTypes
        {
            get
            {
                FillStatsDictionary();
                return _statsMap.Keys;
            }
        }

        public IReadOnlyDictionary<StatType, int> InitialStats
        {
            get
            {
                FillStatsDictionary();
                return _statsMap;
            }
        }

        private void FillStatsDictionary()
        {
            if (_statsMap.Count == 0)
            {
                foreach (var stat in _stats)
                    _statsMap.Add(stat.Type, stat.Value);
            }
        }
    }

    [Serializable]
    public class Stat
    {
        public StatType Type;
        public int Value;
    }
}