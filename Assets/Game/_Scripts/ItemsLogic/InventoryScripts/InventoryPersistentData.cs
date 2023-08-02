using System.Collections.Generic;
using _ScriptableObjects.Classes;
using _Scripts.ItemsLogic.Items;
using _Scripts.ItemsLogic.Stats;

namespace _Scripts.ItemsLogic.InventoryScripts
{
    public class InventoryPersistentData
    {
        private Dictionary<ItemType, ItemPersistentData> _itemSaveData;

        public InventoryPersistentData(AllItemTemplates allItemTemplates)
        {
            _itemSaveData = new Dictionary<ItemType, ItemPersistentData>();
            foreach (var itemTemplateData in allItemTemplates.Templates)
                _itemSaveData.Add(itemTemplateData.Type, new ItemPersistentData(itemTemplateData));
        }
        
        public Dictionary<ItemType, Item> LoadItems(AllItemTemplates allItemTemplates)
        {
            Dictionary<ItemType, Item> itemsDictionary = new Dictionary<ItemType, Item>();
            
            foreach (ItemTemplateData itemTemplate in allItemTemplates.Templates)
            {
                Dictionary<StatType, int> loadedData = _itemSaveData[itemTemplate.Type].Load();
                if (loadedData.Count > 0)
                {
                    Item loadedItem = new Item(itemTemplate.Type, itemTemplate.Sprite, loadedData);
                    itemsDictionary.Add(loadedItem.Type, loadedItem);
                }
            }

            return itemsDictionary;
        }

        public void Save(Dictionary<ItemType, Item> inventoryItems)
        {
            foreach (KeyValuePair<ItemType, ItemPersistentData> saveData in _itemSaveData)
            {
                if (inventoryItems.TryGetValue(saveData.Key, out Item item))
                    saveData.Value.Save(item);
            }
        }
    }
}