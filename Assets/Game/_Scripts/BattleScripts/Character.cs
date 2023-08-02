using System;
using System.Collections;
using _Scripts.Utils;
using UnityEditor;
using UnityEngine;

namespace _Scripts.BattleScripts
{
    public class Character : MonoBehaviour, IDamageTaker
    {
        public event Action OnBattleEnded;
        
        [SerializeField] protected Animator _animator;
        [SerializeField] private HealthBar _healthBar;
        
        private Coroutine _battleRoutine;
        private readonly float _preBattleDelay = 0.75f;
        private readonly float _delayAfterHit = 0.25f;
        private readonly int _idle = Animator.StringToHash("Idle");
        private readonly int _hit = Animator.StringToHash("Hit");
        private readonly int _die = Animator.StringToHash("Die");
        private IDamageTaker _target;
        private bool _waitAttack;
        
        protected float Health { get; set; }
        protected float Damage { get; set; }
        protected float Armor { get; set; }

        public bool IsAlive => Health > 0;
        public void AttackAction() => _waitAttack = false;

        public virtual void StartBattleBehaviour(IDamageTaker damageTaker)
        {
            _healthBar.Init(Health);
            _target = damageTaker;
            StopBattleRoutine();
            _battleRoutine = StartCoroutine(BattleRoutine());
        }

        public void TakeDamage(float damageAmount)
        {
            float finalDamage = Mathf.Clamp(damageAmount - Armor, 0, float.MaxValue);
            Health -= finalDamage;
            _healthBar.ChangeValue(Health, 0.45f);
            CheckDeath();
        }

        private void CheckDeath()
        {
            if (IsAlive == false)
            {
                StopBattleRoutine();
                PlayDie();
                _healthBar.Hide();
                
                StartCoroutine(Helper.WaitCoroutine(1.0f, () =>
                {
                    OnBattleEnded?.Invoke();
                }));
            }
        }

        private IEnumerator BattleRoutine()
        {
            PlayIdle();
            yield return Helper.GetWait(_preBattleDelay);

            while (true)
            {
                PlayHit();
                
                _waitAttack = true;
                while (_waitAttack)
                    yield return null;
            
                _target.TakeDamage(Damage);
                PlayIdle();
                
                if (_target.IsAlive == false)
                {
                    yield return Helper.GetWait(1.5f);
                    OnBattleEnded?.Invoke();
                    _healthBar.Hide();
                    PlayIdle();
                    yield break;
                }
                
                yield return Helper.GetWait(_delayAfterHit);
            }
        }

        private void StopBattleRoutine()
        {
            if (_battleRoutine != null)
            {
                StopCoroutine(_battleRoutine);
                _battleRoutine = null;
            }
        }

        public void PlayIdle() => _animator.CrossFade(_idle, 0.1f, 0);
        private void PlayHit() => _animator.CrossFade(_hit, 0.15f, 0);
        private void PlayDie() => _animator.CrossFade(_die, 0.15f, 0);
    }
}