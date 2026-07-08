namespace Core.Weapons.FirePatterns
{
    public class SingleShotPattern : IFirePattern
    {
        public void Fire(FireContext context)
        {
            context.SpawnProjectile(context.Origin, context.Direction);
        }
    }
}