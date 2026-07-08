using Gameplay.Input;
using UnityEngine;
using VContainer;

namespace Gameplay.Player
{
    public class PlayerAimController : MonoBehaviour
    {
        [SerializeField] private Camera aimCamera;
        [SerializeField] private Transform bodyToRotate;

        private IInputProvider _input;

        [Inject]
        public void Construct(IInputProvider input)
        {
            _input = input;
        }

        private void Awake()
        {
            if (aimCamera == null)
                aimCamera = Camera.main;

            if (bodyToRotate == null)
                bodyToRotate = transform;
        }

        private void Update()
        {
            if (!TryGetAimPoint(out Vector3 aimPoint))
                return;

            Vector3 direction = aimPoint - bodyToRotate.position;
            direction.z = 0f;

            if (direction.sqrMagnitude < 0.0001f)
                return;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            bodyToRotate.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        private bool TryGetAimPoint(out Vector3 aimPoint)
        {
            aimPoint = default;

            if (aimCamera == null)
                return false;

            float cameraDistance = Mathf.Abs(aimCamera.transform.position.z - bodyToRotate.position.z);
            Vector3 screenPoint = new Vector3(_input.LookInput.x, _input.LookInput.y, cameraDistance);
            aimPoint = aimCamera.ScreenToWorldPoint(screenPoint);
            aimPoint.z = bodyToRotate.position.z;
            return true;
        }
    }
}