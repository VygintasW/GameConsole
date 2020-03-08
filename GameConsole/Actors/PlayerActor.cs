using Akka.Persistence;
using GameConsole.Commands;
using GameConsole.Events;
using GameConsole.States;
using System;

namespace GameConsole.Actors
{
    public class PlayerActor : ReceivePersistentActor
    {
        public override string PersistenceId => $"player-{PlayerActorState.PlayerName}";

        private PlayerActorState PlayerActorState { get; set; }
        private int EventCount { get; set; }

        public PlayerActor(string playerName, int health)
        {
            PlayerActorState = new PlayerActorState
            {
                PlayerName = playerName,
                Health = health
            };

            Console.WriteLine($"{PlayerActorState.PlayerName} created");

            Command<HitPlayer>(message => HitPlayer(message));
            Command<DisplayStatus>(message => DisplayPlayerStatus(message));
            Command<SimulateError>(message => SimulateError());

            Recover<PlayerHit>(message =>
            {
                Console.WriteLine($"{PlayerActorState.PlayerName} replaying PlayerHit event from journal");

                PlayerActorState.Health -= message.DamageTaken;
            });

            Recover<SnapshotOffer>(offer =>
            {
                Console.WriteLine($"{PlayerActorState.PlayerName} received SnapshotOffer from snapshot store, updating state");

                PlayerActorState = offer.Snapshot as PlayerActorState;

                Console.WriteLine($"{PlayerActorState.PlayerName} state {PlayerActorState} set from snapshot");
            });
        }

        private void SimulateError()
        {
            Console.WriteLine($"{PlayerActorState.PlayerName} received SimulateError");

            throw new ApplicationException($"Simulated exception in player: {PlayerActorState.PlayerName}");
        }

        private void DisplayPlayerStatus(DisplayStatus message)
        {
            Console.WriteLine($"{PlayerActorState.PlayerName} received DisplayStatus");

            Console.WriteLine($"{PlayerActorState.PlayerName} has {PlayerActorState.Health} health");
        }

        private void HitPlayer(HitPlayer command)
        {
            Console.WriteLine($"{PlayerActorState.PlayerName} received HitPlayer");

            Console.WriteLine($"{PlayerActorState.PlayerName} persisting HitPlayer");

            var playerEvent = new PlayerHit(command.Damage);

            Persist(playerEvent, playerHit =>
            {
                Console.WriteLine($"{PlayerActorState.PlayerName} persisted PlayerHit event ok, updating actor state");

                PlayerActorState.Health -= command.Damage;
                EventCount++;

                if(EventCount == 5)
                {
                    Console.WriteLine($"{PlayerActorState.PlayerName} saving snapshot");

                    SaveSnapshot(PlayerActorState);

                    Console.WriteLine($"{PlayerActorState.PlayerName} reseting event count to 0");

                    EventCount = 0;
                }
            });

        }
    }
}
