using Akka.Actor;
using GameConsole.Messages;
using System;

namespace GameConsole.Actors
{
    public class PlayerCoordinatorActor : ReceiveActor
    {
        private const int DefaultStartingHealth = 100;

        public PlayerCoordinatorActor()
        {
            Receive<CreatePlayerMessage>(message =>
            {
                Console.WriteLine($"PlayerCoordinatorActor received CreatePlayerMessage for {message.PlayerName}");

                Context.ActorOf(Props.Create(() => new PlayerActor(message.PlayerName, DefaultStartingHealth)), message.PlayerName);
            });
        }
    }
}
