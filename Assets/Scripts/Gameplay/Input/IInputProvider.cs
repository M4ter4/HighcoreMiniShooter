using UnityEngine;

namespace Gameplay.Input
{
    public interface IInputProvider
    {
        Vector2 MoveInput { get; }
        Vector2 LookInput { get; }
        bool FirePressed { get; }
        bool FireHeld { get; }
        bool ReloadPressed { get; }
        int WeaponSwitchIndex { get; }
    }
}