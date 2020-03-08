namespace GameConsole.Events
{
    public sealed class PlayerHit
    {
        public PlayerHit(int damageTaken)
        {
            DamageTaken = damageTaken;
        }

        public int DamageTaken { get; }
    }
}
