using System.Collections.Generic;
using _ScriptableObjects.Classes;
using _Scripts.ItemsLogic.Items;
using _Scripts.ItemsLogic.Stats;
using TMPro;
using UnityEngine;

namespace _Scripts.ItemsLogic.InventoryScripts
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private CharacterStats _characterInitialStats;
        [SerializeField] private AllItemTemplates _allItemTemplates;
        [SerializeField] private Transform _statsParent;
        [SerializeField] private Transform _inventorySlotParent;
        [SerializeField] private TMP_Text _statViewPrefab;
        [SerializeField] private InventorySlot _inventorySlotPrefab;
        
        private Dictionary<ItemType, Item> _inventoryItems = new Dictionary<ItemType, Item>();
        private readonly Dictionary<ItemType, InventorySlot> _itemTypesToSlots = new Dictionary<ItemType, InventorySlot>();
        private readonly Dictionary<StatType, TMP_Text> _statTypesToTMPTexts = new Dictionary<StatType, TMP_Text>();
        private readonly Dictionary<StatType, int> _statValues = new Dictionary<StatType, int>();
        private InventoryPersistentData _inventoryPersistentData;

        public int GetStat(StatType type) => _statValues[type];
        
        public void AddItem(Item equippedItem)
        {
            DestroyOldItem(equippedItem);
            PutItemAtInventory(equippedItem);
            SaveItems();
            UpdatePlayerStats();
        }

        private void Start()
        {
            LoadItems();
            InitInventorySlots();
            InitPlayerStats();
        }

        private void PutItemAtInventory(Item equippedItem)
        {
            _inventoryItems.Add(equippedItem.Type, equippedItem);
            _itemTypesToSlots[equippedItem.Type].PutItemInSlot(equippedItem.Sprite);
        }

        private void DestroyOldItem(Item equippedItem)
        {
            if (_inventoryItems.TryGetValue(equippedItem.Type, out var item))
            {
                item.Destroy();
                _inventoryItems.Remove(equippedItem.Type);
            }
        }

        private void SaveItems()
        {
            _inventoryPersistentData.Save(_inventoryItems);
        }

        private void UpdatePlayerStats()
        {
            foreach (var initialStat in _characterInitialStats.InitialStats)
            {
               int statValue = initialStat.Value + GetInventoryStat(initialStat.Key); 
               _statValues[initialStat.Key] =  statValue;
               SetStatView(_statTypesToTMPTexts[initialStat.Key], initialStat, statValue);
            }
        }

        public Item GetCurrentItemOfType(ItemType randomTemplateType) 
            => _inventoryItems.TryGetValue(randomTemplateType, out var item) ? item : default;

        private void LoadItems()
        {
            _inventoryPersistentData = new InventoryPersistentData(_allItemTemplates);
            _inventoryItems.Clear();
            _inventoryItems = _inventoryPersistentData.LoadItems(_allItemTemplates);
        }

        private void InitInventorySlots()
        {
            _itemTypesToSlots.Clear();
            
            foreach (ItemTemplateData itemTemplate in _allItemTemplates.Templates)
            {
                InventorySlot viewItem = Instantiate(_inventorySlotPrefab, _inventorySlotParent);
                _itemTypesToSlots.Add(itemTemplate.Type, viewItem);
            }

            foreach (var inventoryItem in _inventoryItems)
                _itemTypesToSlots[inventoryItem.Key].PutItemInSlot(inventoryItem.Value.Sprite);
        }

        private void InitPlayerStats()
        {
            _statTypesToTMPTexts.Clear();
            _statValues.Clear();
            
            foreach (var initialStat in _characterInitialStats.InitialStats)
            {
                TMP_Text viewItem = Instantiate(_statViewPrefab, _statsParent);
                int statValue = initialStat.Value + GetInventoryStat(initialStat.Key);
                SetStatView(viewItem, initialStat, statValue);
                _statValues.Add(initialStat.Key, statValue);
                _statTypesToTMPTexts.Add(initialStat.Key, viewItem);
            }
        }

        private static void SetStatView(TMP_Text viewItem, KeyValuePair<StatType, int> initialStat, int statValue)
        {
            viewItem.text = initialStat.Key + ": " + statValue;
        }

        private int GetInventoryStat(StatType statType)
        {
            int statSum = 0;
            foreach (var item in _inventoryItems)
            {
                if (item.Value.Stats.TryGetValue(statType, out int statValue))
                    statSum += statValue;
            }

            return statSum;
        }
    }
}