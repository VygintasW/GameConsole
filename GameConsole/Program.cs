using Akka.Actor;
using Akka.Configuration;
using GameConsole.Actors;
using GameConsole.Commands;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading;

namespace GameConsole
{
    class Program
    {
        private static ActorSystem System { set; get; }
        private static IActorRef PlayerCoordinator { set; get; }
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddXmlFile("hocon.xml");

            var configuration = builder.Build();

            var hoconConfig = configuration.GetSection("hocon");

            var config = ConfigurationFactory.ParseString(hoconConfig.Value);

            System = ActorSystem.Create("Game", config);

            PlayerCoordinator = System.ActorOf<PlayerCoordinatorActor>("PlayerCoordinator");

            DisplayInstructions();

            while(true)
            {
                Thread.Sleep(2000);

                var action = ReadLine();

                var playerName = action.Split(' ')[0];

                if(action.Contains("create"))
                {
                    CreatePlayer(playerName);
                }
                else if(action.Contains("hit"))
                {
                    var damage = int.Parse(action.Split(' ')[2]);
                    HitPlayer(playerName, damage);
                }
                else if (action.Contains("display"))
                {
                    DisplayPlayer(playerName);
                }
                else if(action.Contains("error"))
                {
                    ErrorPlayer(playerName);
                }
                else
                {
                    Console.WriteLine("Unknown command");
                }
            }
        }

        private static void ErrorPlayer(string playerName)
        {
            System.ActorSelection($"/user/PlayerCoordinator/{playerName}")
                  .Tell(new SimulateError());
        }

        private static void DisplayPlayer(string playerName)
        {
            System.ActorSelection($"/user/PlayerCoordinator/{playerName}")
                  .Tell(new DisplayStatus());
        }

        private static void HitPlayer(string playerName, int damage)
        {
            System.ActorSelection($"/user/PlayerCoordinator/{playerName}")
                  .Tell(new HitPlayer(damage));
        }

        private static void CreatePlayer(string playerName)
        {
            PlayerCoordinator.Tell(new CreatePlayer(playerName));
        }

        private static string ReadLine()
        {
            return Console.ReadLine();
        }

        private static void DisplayInstructions()
        {
            Thread.Sleep(2000);

            Console.WriteLine("Available commands:");
            Console.WriteLine("<PlayerName> create");
            Console.WriteLine("<PlayerName> hit");
            Console.WriteLine("<PlayerName> display");
            Console.WriteLine("<PlayerName> error");
        }
    }
}
