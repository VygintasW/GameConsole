namespace GameConsole.States
{
    public class PlayerActorState
    {
        public string PlayerName { set; get; }
        public int Health { set; get; }

        public override string ToString()
        {
            return $"[{nameof(PlayerActorState)} {PlayerName} {Health}]";
        }
    }
}
