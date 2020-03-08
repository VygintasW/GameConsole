namespace GameConsole.Messages
{
    public sealed class CreatePlayerMessage
    {
        public CreatePlayerMessage(string playerName)
        {
            PlayerName = playerName;
        }
        public string PlayerName { get;}
    }
}
