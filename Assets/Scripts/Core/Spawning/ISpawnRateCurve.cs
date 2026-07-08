namespace Core.Spawning
{
    public interface ISpawnRateCurve
    {
        public float GetInterval(float elapsedTime, int killCount);
    }
}