using System;
using System.IO;
using System.Media;
using CyberSecurityChatbot.Models;
using CyberSecurityChatbot.Services;

namespace CyberSecurityChatbot
{
    class Program
    {
        static void Main()
        {
            // Play voice greeting
            try
            {
                SoundPlayer player = new SoundPlayer("Assets/welcome.wav");
                player.Play();
            }
            catch
            {
                Console.WriteLine("Voice greeting file not found.");
            }

            // ASCII Art
            Console.WriteLine(@"
   _____                 _                 
  / ____|               | |                
 | |     ___  _ __   ___| |__   ___ _ __   
 | |    / _ \| '_ \ / __| '_ \ / _ \ '__|  
 | |___| (_) | | | | (__| | | |  __/ |     
  \_____\___/|_| |_|\___|_| |_|\___|_|     
");

            Console.WriteLine("=== Cybersecurity Awareness Chatbot ===");

            Console.Write("Enter your name: ");
            string name = Console.ReadLine();

            User user = new User
            {
                Name = name,
                InteractionCount = 0
            };

            Chatbot bot = new Chatbot();

            Console.WriteLine($"\nHello {user.Name}, I'm your Cybersecurity Assistant.");
            Console.WriteLine("Type 'exit' anytime to quit.\n");

            while (true)
            {
                Console.WriteLine("\nMenu:");
                Console.WriteLine("1. Ask a cybersecurity question");
                Console.WriteLine("2. Check password strength");
                Console.WriteLine("3. Exit");
                Console.Write("Choose option: ");

                string choice = Console.ReadLine();

                if (choice == "3")
                    break;

                switch (choice)
                {
                    case "1":
                        Console.Write("Ask: ");
                        string input = Console.ReadLine();

                        if (input.ToLower() == "exit")
                            return;

                        user.InteractionCount++;

                        string response = bot.GetResponse(input);
                        Console.WriteLine("Bot: " + response);

                        Log(user.Name, input);
                        break;

                    case "2":
                        Console.Write("Enter password: ");
                        string password = Console.ReadLine();

                        SecurityTools tools = new SecurityTools();
                        Console.WriteLine("Result: " + tools.CheckPassword(password));

                        Log(user.Name, "Checked password");
                        break;

                    default:
                        Console.WriteLine("Invalid option. Try again.");
                        break;
                }
            }

            Console.WriteLine($"\nYou asked {user.InteractionCount} questions. Stay safe online!");
        }

        static void Log(string user, string action)
        {
            Directory.CreateDirectory("Logs");

            File.AppendAllText("Logs/log.txt",
                $"[{DateTime.Now}] {user}: {action}\n");
        }
    }
}