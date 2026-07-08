using Core.Weapons;
using Data;

namespace Core.ObjectManagement
{
    public interface IWeaponFactory
    {
        public IWeapon Create(WeaponData data);
    }
}