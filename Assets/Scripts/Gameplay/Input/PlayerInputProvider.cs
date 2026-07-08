using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay.Input
{
    public class PlayerInputProvider : MonoBehaviour, IInputProvider
    {
        private const int MaxWeaponSlots = 10;
        private static readonly string[] SlotKeys = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };

        private InputActionMap _actionMap;
        private InputAction _moveAction;
        private InputAction _lookAction;
        private InputAction _fireAction;
        private InputAction _reloadAction;
        private InputAction[] _weaponSlotActions = Array.Empty<InputAction>();

        private bool _configured;

        public Vector2 MoveInput { get; private set; }
        public Vector2 LookInput { get; private set; }
        public bool FirePressed { get; private set; }
        public bool FireHeld { get; private set; }
        public bool ReloadPressed { get; private set; }
        public int WeaponSwitchIndex { get; private set; } = -1;

        private void Awake()
        {
            _actionMap = new InputActionMap("Player");

            _moveAction = _actionMap.AddAction("Move", InputActionType.Value);
            
            _moveAction.AddCompositeBinding("2DVector")
                .With("Up", "<Keyboard>/w")
                .With("Down", "<Keyboard>/s")
                .With("Left", "<Keyboard>/a")
                .With("Right", "<Keyboard>/d");
            
            _moveAction.AddCompositeBinding("2DVector")
                .With("Up", "<Keyboard>/upArrow")
                .With("Down", "<Keyboard>/downArrow")
                .With("Left", "<Keyboard>/leftArrow")
                .With("Right", "<Keyboard>/rightArrow");

            _lookAction = _actionMap.AddAction("Look", InputActionType.Value, "<Mouse>/position");

            _fireAction = _actionMap.AddAction("Fire", InputActionType.Button, "<Mouse>/leftButton");
            _reloadAction = _actionMap.AddAction("Reload", InputActionType.Button, "<Keyboard>/r");
        }
        
        public void ConfigureWeaponSlots(int weaponCount)
        {
            if (_configured)
            {
                Debug.LogWarning("[PlayerInputProvider] ConfigureWeaponSlots вызван повторно, игнорирую.");
                return;
            }

            if (weaponCount > MaxWeaponSlots)
                Debug.LogWarning($"[PlayerInputProvider] Кнопки кончились: запрошено {weaponCount} оружий, доступно только {MaxWeaponSlots} слотов (1-9, 0).");

            int slotCount = Mathf.Min(weaponCount, MaxWeaponSlots);
            _weaponSlotActions = new InputAction[slotCount];

            for (int i = 0; i < slotCount; i++)
                _weaponSlotActions[i] = _actionMap.AddAction($"WeaponSlot{i + 1}", InputActionType.Button, $"<Keyboard>/{SlotKeys[i]}");

            _configured = true;
            _actionMap.Enable();
        }
        
        private void OnEnable()
        {
            if (_configured)
                _actionMap.Enable();
        }

        private void OnDisable()
        {
            if (_configured)
                _actionMap.Disable();
        }

        private void OnDestroy()
        {
            _actionMap.Dispose();
        }

        private void Update()
        {
            if (!_configured)
                return;

            MoveInput = _moveAction.ReadValue<Vector2>();
            LookInput = _lookAction.ReadValue<Vector2>();

            FirePressed = _fireAction.WasPressedThisFrame();
            FireHeld = _fireAction.IsPressed();
            ReloadPressed = _reloadAction.WasPressedThisFrame();

            WeaponSwitchIndex = -1;
            for (int i = 0; i < _weaponSlotActions.Length; i++)
            {
                if (_weaponSlotActions[i].WasPressedThisFrame())
                {
                    WeaponSwitchIndex = i;
                    break;
                }
            }
            
            print($"{MoveInput} {LookInput} {FirePressed} {FireHeld} {ReloadPressed}");
        }
    }
}