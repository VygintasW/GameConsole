namespace GameConsole.Messages
{
    public sealed class HitMessage
    {
        public HitMessage(int damage)
        {
            Damage = damage;
        }
        public int Damage { get; }
    }
}
