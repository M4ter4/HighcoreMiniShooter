using UnityEngine;

namespace Gameplay.View
{
    public class WorldSpaceFollower : MonoBehaviour
    {
        private Transform _parentTransform;
        private Vector3 _localOffset;
        private Quaternion _initialRotation;

        protected virtual void Start()
        {
            _parentTransform = transform.parent;

            if (_parentTransform != null)
            {
                _localOffset = transform.localPosition;
                _initialRotation = transform.rotation;
            }
        }

        protected virtual void LateUpdate()
        {
            if (_parentTransform == null)
                return;

            float direction = Mathf.Sign(_parentTransform.localScale.x);
            Vector3 correctedOffset = new Vector3(_localOffset.x * direction, _localOffset.y, _localOffset.z);

            transform.position = _parentTransform.position + correctedOffset;
            transform.rotation = _initialRotation;

            Vector3 currentScale = transform.localScale;
            transform.localScale = new Vector3(Mathf.Abs(currentScale.x), currentScale.y, currentScale.z);
        }
    }
}