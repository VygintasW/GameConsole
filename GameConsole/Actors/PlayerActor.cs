using Akka.Actor;
using GameConsole.Messages;
using System;

namespace GameConsole.Actors
{
    public class PlayerActor : ReceiveActor
    {
        public PlayerActor(string playerName, int health)
        {
            PlayerName = playerName;
            Health = health;

            Console.WriteLine($"{PlayerName} created");

            Receive<HitMessage>(message => HitPlayer(message));
            Receive<DisplayStatusMessage>(message => DisplayPlayerStatus(message));
            Receive<CouseErrorMessage>(message => SimulateError());
        }

        private void SimulateError()
        {
            Console.WriteLine($"{PlayerName} received CauseErrorMessage");

            throw new ApplicationException($"Simulated exception in player: {PlayerName}");
        }

        private void DisplayPlayerStatus(DisplayStatusMessage message)
        {
            Console.WriteLine($"{PlayerName} received DisplayStatusMessage");

            Console.WriteLine($"{PlayerName} has {Health} health");
        }

        private void HitPlayer(HitMessage message)
        {
            Console.WriteLine($"{PlayerName} received HitMessage");

            Health -= message.Damage;
        }

        private string PlayerName { get; }
        private int Health { get; set; }
    }
}
