using System.Collections.Generic;
using System.Linq;
using _Scripts.ItemsLogic.InventoryScripts;
using _Scripts.ItemsLogic.Stats;
using _Scripts.Money;
using UnityEngine;

namespace _Scripts.ItemsLogic.Items
{
    public class Item
    {
        private readonly ItemType _type;
        private readonly Sprite _sprite;
        private readonly Dictionary<StatType, int> _stats;
        private Vector3 _position;

        public Item(ItemType type, Sprite sprite, Dictionary<StatType, int> stats)
        {
            _type = type;
            _sprite = sprite;
            _stats = stats;
        }

        public void SetPosition(Vector3 position) => _position = position;
        
        public ItemType Type => _type;

        public Sprite Sprite => _sprite;
        public IReadOnlyDictionary<StatType, int> Stats => _stats;

        public int GetStat(StatType type) => _stats[type];

        public void Destroy()
        {
            int itemStats = _stats.Sum(s => s.Value);
            MoneyHandler.Add(itemStats, _position);
        }
    }
}