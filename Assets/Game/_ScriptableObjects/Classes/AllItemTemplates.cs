using System.Collections.Generic;
using _Scripts.ItemsLogic.InventoryScripts;
using UnityEngine;

namespace _ScriptableObjects.Classes
{
    [CreateAssetMenu(menuName = "ISN/Create AllItemTemplates", fileName = "AllItemTemplates", order = 0)]
    public class AllItemTemplates  : ScriptableObject
    {
        [SerializeField] private List<ItemTemplateData> _allTemplates;
        [SerializeField] private int _similarItemsInRowMaxCount = 3;
        
        private ItemType _lastTemplateType;
        private int _similarTemplateCount;

        public IReadOnlyCollection<ItemTemplateData> Templates => _allTemplates;

        public ItemTemplateData GetRandomTemplate()
        {
            ItemTemplateData template = _allTemplates[Random.Range(0, _allTemplates.Count)];
            IncreaseSimilarTemplatesCount(template);
            template = TryGetDifferentTemplate(template);
            template.Index = _allTemplates.IndexOf(template);
            return template;
        }

        private void IncreaseSimilarTemplatesCount(ItemTemplateData template)
        {
            if (_lastTemplateType == template.Type)
                _similarTemplateCount += 1;
            else
                _similarTemplateCount = 0;
            
            _lastTemplateType = template.Type;
        }

        private ItemTemplateData TryGetDifferentTemplate(ItemTemplateData template)
        {
            if (_similarTemplateCount > _similarItemsInRowMaxCount)
                template = GetDifferentTemplate(_allTemplates.IndexOf(template));
            return template;
        }

        private ItemTemplateData GetDifferentTemplate(int indexOfSimilarTemplate)
        {
            List<int> indexes = new List<int>(_allTemplates.Count);
            for (int i = 0; i < _allTemplates.Count; i++)
            {
                indexes.Add(i);
            }
            indexes.Remove(indexOfSimilarTemplate);
            int newIndex = indexes[Random.Range(0, indexes.Count)];
            return _allTemplates[newIndex];
        }
    }
}