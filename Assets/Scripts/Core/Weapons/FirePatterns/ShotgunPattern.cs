using UnityEngine;

namespace Core.Weapons.FirePatterns
{
    public class ShotgunPattern : IFirePattern
    {
        public void Fire(FireContext context)
        {
            int pellets = Mathf.Max(1, context.Data.PelletsPerShot);
            float spread = context.Data.Spread;

            for (int i = 0; i < pellets; i++)
            {
                Vector3 direction = ApplySpread(context.Direction, spread);
                context.SpawnProjectile(context.Origin, direction);
            }
        }

        private static Vector3 ApplySpread(Vector3 direction, float spreadDegrees)
        {
            if (spreadDegrees <= 0f)
                return direction;

            float angleOffset = Random.Range(-spreadDegrees, spreadDegrees);
            Quaternion rotation = Quaternion.Euler(0f, 0f, angleOffset);

            return rotation * direction;
        }
    }
}