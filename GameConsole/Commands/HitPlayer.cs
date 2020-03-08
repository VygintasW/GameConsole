namespace GameConsole.Commands
{
    public sealed class HitPlayer
    {
        public HitPlayer(int damage)
        {
            Damage = damage;
        }
        public int Damage { get; }
    }
}
