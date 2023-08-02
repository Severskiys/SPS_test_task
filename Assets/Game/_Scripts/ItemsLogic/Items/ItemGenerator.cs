using System.Collections.Generic;
using _ScriptableObjects.Classes;
using _Scripts.BattleScripts;
using _Scripts.ItemsLogic.InventoryScripts;
using _Scripts.ItemsLogic.Stats;
using _Scripts.Utils;
using UnityEngine;

namespace _Scripts.ItemsLogic.Items
{
    public class ItemGenerator : MonoBehaviour
    {
        [SerializeField] private ButtonDecorator _button;
        [SerializeField] private ItemPopUp _itemPopUp;
        [SerializeField] private AllItemTemplates _allItemTemplates;
        [SerializeField] private Inventory _inventory;
        [SerializeField] private PlayerCharacter _playerCharacter;
        
        private void OnEnable()
        {
            _button.OnClick += DigItem;
            _itemPopUp.OnItemEquipped += AddItemToInventory;
            _itemPopUp.OnItemDropped += DestroyItem;
        }

        private void OnDisable()
        {
            _button.OnClick -= DigItem;
            _itemPopUp.OnItemEquipped -= AddItemToInventory;
            _itemPopUp.OnItemDropped -= DestroyItem;
        }

        private void AddItemToInventory(Item item)
        {
            _button.SetInteractable(true);
            _inventory.AddItem(item);
        }

        private void DestroyItem(Item item)
        {
            _button.SetInteractable(true);
            item.Destroy();
        }

        private void DigItem()
        {
            _button.SetInteractable(false);
            _playerCharacter.OnDig += OpenItemPopUp;
            _playerCharacter.PlayDig();
        }

        private void OpenItemPopUp()
        {
            _playerCharacter.OnDig -= OpenItemPopUp;
            ItemTemplateData randomTemplate = _allItemTemplates.GetRandomTemplate();
            Item newItem = GenerateItem(randomTemplate);
            Item currentItemOfType = _inventory.GetCurrentItemOfType(randomTemplate.Type);
            _itemPopUp.Open(newItem, currentItemOfType);
        }

        private Item GenerateItem(ItemTemplateData randomTemplate)
        {
            Dictionary<StatType, int> itemStats = new Dictionary<StatType, int>();

            foreach (var statType in randomTemplate.AllTemplateStats)
            {
                int statValue = randomTemplate.GetRandomStat(statType);
                itemStats.Add(statType, statValue);
            }
            
            Item item = new Item(randomTemplate.Type, randomTemplate.Sprite, itemStats);

            return item;
        }
    }
}