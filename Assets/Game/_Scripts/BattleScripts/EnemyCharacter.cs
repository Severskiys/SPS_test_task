using UnityEngine;

namespace _Scripts.BattleScripts
{
    public class EnemyCharacter : Character
    {
        [SerializeField] private int _health;
        [SerializeField] private int _attack;
        [SerializeField] private int _defense;
        
        public override void StartBattleBehaviour(IDamageTaker damageTaker)
        {
            Health = _health;
            Damage = _attack;
            Armor = _defense;
            
            base.StartBattleBehaviour(damageTaker);
        }
    }
}