using System;
using _Scripts.ItemsLogic.InventoryScripts;
using _Scripts.ItemsLogic.Stats;
using UnityEngine;

namespace _Scripts.BattleScripts
{
    public class PlayerCharacter : Character
    {
        public event Action OnDig;
        
        [SerializeField] private Inventory _playerInventory;
        private readonly int _dig = Animator.StringToHash("Dig");

        public void DigEvent() => OnDig?.Invoke();
        
        public override void StartBattleBehaviour(IDamageTaker damageTaker)
        {
            Health = _playerInventory.GetStat(StatType.Health);
            Damage = _playerInventory.GetStat(StatType.Attack);
            Armor = _playerInventory.GetStat(StatType.Defense);
            
            base.StartBattleBehaviour(damageTaker);
        }
        
        public void PlayDig() => _animator.CrossFade(_dig, 0.15f, 0);
    }
}