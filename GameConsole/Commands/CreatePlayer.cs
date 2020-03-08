namespace GameConsole.Commands
{
    public sealed class CreatePlayer
    {
        public CreatePlayer(string playerName)
        {
            PlayerName = playerName;
        }
        public string PlayerName { get;}
    }
}
