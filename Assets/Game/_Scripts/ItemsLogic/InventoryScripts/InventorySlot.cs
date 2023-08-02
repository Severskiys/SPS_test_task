using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.ItemsLogic.InventoryScripts
{
    public class InventorySlot : MonoBehaviour
    {
        [SerializeField] private Image _itemView;

        private void Awake()
        {
            _itemView.DOFade(0f, 0f);
        }

        public void PutItemInSlot(Sprite equippedItemSprite)
        {
            _itemView.sprite = equippedItemSprite;
            _itemView.SetNativeSize();
            _itemView.DOFade(1.0f, 0.15f);
        }
    }
}