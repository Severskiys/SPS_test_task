using System;
using _Scripts.Utils;
using UnityEditor;
using UnityEngine;

namespace _Scripts.ItemsLogic.Items
{
    public class ItemPopUp : MonoBehaviour
    {
        public event Action<Item> OnItemEquipped;
        public event Action<Item> OnItemDropped;

        [SerializeField] private ButtonDecorator _equipButton;
        [SerializeField] private ButtonDecorator _dropButton;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private ItemCardWithDifference _newItemCard;
        [SerializeField] private ItemCard _oldItemCard;
        private Item _newItem;

        public void Open(Item newItem, Item currentItemOfType)
        {
            _newItem = newItem;
            _canvasGroup.Show();

            FillNewItemStats(newItem);

            if (currentItemOfType != default)
            {
                FillCurrentItemStats(currentItemOfType);
                FillDifferenceInStats(currentItemOfType);
            }
            else
            {
                ClearCurrentItemCard();
            }
        }

        private void ClearCurrentItemCard()
        {
            _oldItemCard.ClearCard();
            _newItemCard.ClearDifference();
        }

        private void FillNewItemStats(Item newItem) => _newItemCard.FillCard(newItem);
        private void FillCurrentItemStats(Item currentItemOfType) => _oldItemCard.FillCard(currentItemOfType);
        private void FillDifferenceInStats(Item currentItemOfType) => _newItemCard.FillItemDifference(currentItemOfType);

        private void OnEnable()
        {
            _equipButton.OnClick += EquipNewItem;
            _dropButton.OnClick += DropNewItem;
        }

        private void OnDisable()
        {
            _equipButton.OnClick -= EquipNewItem;
            _dropButton.OnClick -= DropNewItem;
        }

        private void EquipNewItem()
        {
            _canvasGroup.Hide();
            OnItemEquipped?.Invoke(_newItem);
        }

        private void DropNewItem()
        {
            _canvasGroup.Hide();
            OnItemDropped?.Invoke(_newItem);
        }
        
    }
}