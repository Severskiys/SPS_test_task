using System.Collections.Generic;
using _Scripts.Utils;
using _Scripts.Utils.SavableValues;
using UnityEngine;

namespace _Scripts.BattleScripts
{
    public class BattleOrganizer : MonoBehaviour
    {
        [SerializeField] private ButtonDecorator _button;
        [SerializeField] private List<EnemyCharacter> _enemyPrefabs;
        [SerializeField] private PlayerCharacter _playerCharacter;
        [SerializeField] private Transform _playerBattlePoint;
        [SerializeField] private Transform _enemyBattlePoint;
        [SerializeField] private CanvasGroup _inventoryCanvasGroup;
        [SerializeField] private CameraMover _cameraMover;

        private IntDataValueSavable _enemyIndex = new IntDataValueSavable("enemyIndex");
        private Vector3 _playerOriginalPosition;
        private EnemyCharacter _enemy;

        private void OnEnable()
        {
            _button.SetInteractable(true);
            _button.OnClick += StartBattle;
        }

        private void OnDisable()
        {
            _button.OnClick -= StartBattle;
        }

        private void StartBattle()
        {
            HideHubUi();
            SpawnEnemy();
            MovePlayerToBattle();
            _cameraMover.MoveToBattle(1.3f);
        }

        private void SpawnEnemy()
        {
            _enemy = Instantiate(_enemyPrefabs[_enemyIndex.Value], _enemyBattlePoint);
            _enemy.StartBattleBehaviour(_playerCharacter);

            _enemyIndex.Value += 1;
            if (_enemyIndex.Value >= _enemyPrefabs.Count)
                _enemyIndex.Value = 0;
            _enemyIndex.Save();
        }

        private void MovePlayerToBattle()
        {
            _playerOriginalPosition = _playerCharacter.transform.position;
            _playerCharacter.transform.position = _playerBattlePoint.position;
            _playerCharacter.StartBattleBehaviour(_enemy);
            _playerCharacter.OnBattleEnded += ProcessBattleEnd;
            
        }

        private void HideHubUi()
        {
            _inventoryCanvasGroup.Hide(0.1f);
            _button.Hide();
        }
        
        private void ProcessBattleEnd()
        {
            _playerCharacter.OnBattleEnded -= ProcessBattleEnd;
            _playerCharacter.transform.position = _playerOriginalPosition;
            _playerCharacter.PlayIdle();
            Destroy(_enemy.gameObject);
            
            StartCoroutine(Helper.WaitCoroutine(1.25f, () =>
            {
                _inventoryCanvasGroup.Show(0.1f);
                _button.Show();
            }));
            _cameraMover.MoveToHub(1.0f);
        }
    }
}