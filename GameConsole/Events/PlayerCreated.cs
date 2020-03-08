namespace GameConsole.Events
{
    public sealed class PlayerCreated
    {
        public PlayerCreated(string playerName)
        {
            PlayerName = playerName;
        }

        public string PlayerName { get; }
    }
}
