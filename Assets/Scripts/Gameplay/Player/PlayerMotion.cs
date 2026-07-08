using Gameplay.Input;
using UnityEngine;
using VContainer;

namespace Gameplay.Player
{
    public class PlayerMotion : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 6f;

        private IInputProvider _input;

        [Inject]
        public void Construct(IInputProvider input)
        {
            _input = input;
        }

        private void Update()
        {
            Vector2 move = _input.MoveInput;
            Vector3 direction = new Vector3(move.x, move.y, 0f);

            if (direction.sqrMagnitude > 1f)
                direction.Normalize();

            Vector3 delta = direction * (moveSpeed * Time.deltaTime);
            
            transform.position += delta;
        }
    }
}