using Core.Weapons;
using Core.Weapons.FirePatterns;
using Data;

namespace Core.ObjectManagement
{
    public class WeaponFactory : IWeaponFactory
    {
        public IWeapon Create(WeaponData data)
        {
            IFirePattern pattern = CreatePattern(data.FireMode);
            return new Weapon(data, pattern);
        }

        private static IFirePattern CreatePattern(WeaponType type)
        {
            switch (type)
            {
                case WeaponType.Shotgun:
                    return new ShotgunPattern();
                default:
                    return new SingleShotPattern();
            }
        }
    }
}