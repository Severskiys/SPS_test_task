using _Scripts.Utils;
using DG.Tweening;
using UnityEngine;

namespace _Scripts.BattleScripts
{
    public class CameraMover : MonoBehaviour
    {
        [SerializeField] private Transform _initialCameraTarget;
        [SerializeField] private Transform _battleCameraTarget;
        
        private Camera _camera;

        private void Awake()
        {
            _camera = Helper.Camera;
            _camera.transform.position = _initialCameraTarget.position;
        }

        public void MoveToBattle(float timer) => _camera.transform.DOMove(_battleCameraTarget.position, timer).SetEase(Ease.InOutQuad).SetUpdate(UpdateType.Late);
        public void MoveToHub(float timer) => _camera.transform.DOMove(_initialCameraTarget.position, timer).SetEase(Ease.InOutQuad).SetUpdate(UpdateType.Late);
    }
}