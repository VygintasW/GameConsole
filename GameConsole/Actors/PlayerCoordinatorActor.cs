using Akka.Actor;
using Akka.Persistence;
using GameConsole.Commands;
using GameConsole.Events;
using System;

namespace GameConsole.Actors
{
    public class PlayerCoordinatorActor : ReceivePersistentActor
    {
        private const int DefaultStartingHealth = 100;
        public override string PersistenceId => $"player-coordinator";

        public PlayerCoordinatorActor()
        {
            Command<CreatePlayer>(command =>
            {
                Console.WriteLine($"PlayerCoordinatorActor received CreatePlayer for {command.PlayerName}");

                var eventReceived = new PlayerCreated(command.PlayerName);

                Persist(eventReceived, playerCreatedEvent =>
                {
                    Console.WriteLine($"PlayerCoordinatorActor persisted a PlayerCreated for {playerCreatedEvent.PlayerName}");

                    CreatePlayer(playerCreatedEvent);
                });
            });

            Recover<PlayerCreated>((playerCreatedEvent =>
            {
                Console.WriteLine($"PlayerCoordinator replaying a PlayerCreated {playerCreatedEvent} from journal");
                CreatePlayer(playerCreatedEvent);
            }));
        }

        private static void CreatePlayer(PlayerCreated playerEvent)
        {
            Context.ActorOf(Props.Create(() => new PlayerActor(playerEvent.PlayerName, DefaultStartingHealth)), playerEvent.PlayerName);
        }
    }
}
