namespace Core.Damage
{
    public readonly struct DamageInfo
    {
        public readonly int Amount;
        
        public readonly object Source;
 
        public DamageInfo(int amount, object source = null)
        {
            Amount = amount;
            Source = source;
        }
    }

}