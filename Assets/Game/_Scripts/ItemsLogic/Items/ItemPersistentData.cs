using System.Collections.Generic;
using _ScriptableObjects.Classes;
using _Scripts.ItemsLogic.Items;
using _Scripts.ItemsLogic.Stats;
using _Scripts.Utils.SavableValues;

namespace _Scripts.ItemsLogic.InventoryScripts
{
    public class ItemPersistentData
    {
        private Dictionary<StatType, IntDataValueSavable> _statsSaveData;

        public ItemPersistentData(ItemTemplateData itemTemplateData)
        {
            _statsSaveData = new Dictionary<StatType, IntDataValueSavable>(itemTemplateData.AllTemplateStats.Count);
            foreach (StatType stat in itemTemplateData.AllTemplateStats)
            {
                string saveKey = itemTemplateData.Type + "_" + stat;
                _statsSaveData.Add(stat, new IntDataValueSavable(saveKey));
            }
        }
        
        public void Save(Item item)
        {
            foreach (var saveDataPair in _statsSaveData)
            {
                saveDataPair.Value.Value = item.Stats[saveDataPair.Key];
                saveDataPair.Value.Save();
            }
        }

        public Dictionary<StatType, int> Load()
        {
            Dictionary<StatType, int> itemData = new Dictionary<StatType, int>();
            foreach (var dataValueSavable in _statsSaveData)
            {
                if (dataValueSavable.Value.HasSaving())
                    itemData.Add(dataValueSavable.Key, dataValueSavable.Value.Value);
            }

            return itemData;
        }
    }
}