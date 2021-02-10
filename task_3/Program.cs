using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Linq;

namespace task_3
{
    class Program
    {
        static void Main(string[] args)
        {            
            if (args.Length % 2 == 0 || args.Length <= 1 || args.Length != args.Distinct().Count())
            {
                Console.WriteLine("Something wrong! Check all the rules and launch it again!\n" +
                    "1. Number of moves must be odd and > 1\n2. All moves must be unique\n" +
                    "Try 'dotnet run 1 2 3' or 'dotnet run rock paper scissors lizard Spock'");
            }
            else
            {
                var movenum = RandomNumberGenerator.GetInt32(args.Length);
                var comp = args[movenum];
                byte[] key = new byte[16];
                var gen = RandomNumberGenerator.Create();
                gen.GetBytes(key);
                byte[] hash = HashIt(key, Encoding.UTF8.GetBytes(comp));
                Console.WriteLine($"HMAC: {BitConverter.ToString(hash).Replace("-", "")}\nAvailable moves: ");
                List<string> menu = new List<string>() { };
                for (int i = 0; i < args.Length; i++)
                {
                    menu.Add($"{i + 1}");
                }
                string pl = LoopIt(args);
                while (!menu.Contains(pl))
                {
                    if (pl == "0")
                    {
                        Console.WriteLine("Good Bye!");
                        return;
                    }
                    Console.WriteLine("\nWrong move!\nAvailable moves: ");
                    pl = LoopIt(args);
                }
                int plMove = int.Parse(pl) - 1;
                int half = (args.Length - 1) / 2;
                List<string> win = Win(plMove - 1, half, args);
                List<string> lose = Lose(plMove + 1, half, args);
                Console.WriteLine($"Your move: {args[plMove]}\nComputer move: {comp}");
                if (win.Contains(comp))
                {
                    Console.WriteLine("You win!");
                }
                else if (lose.Contains(comp))
                {
                    Console.WriteLine("You lose!");
                }
                else
                {
                    Console.WriteLine("It's a tie!");
                }
                Console.WriteLine($"HMAC key: {BitConverter.ToString(key).Replace("-", "")}");
            }
        }

        static byte[] HashIt(byte[] key, byte[] msg)
        {
            var hash = new HMACSHA256(key);
            return hash.ComputeHash(msg);
        }

        static string LoopIt(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                Console.WriteLine($"{i + 1} - {args[i]}");
            }
            Console.Write("0 - Exit\nEnter your move: ");
            return Console.ReadLine();
        }

        static List<string> Win(int start, int half, string[] args)
        {
            List<string> win = new List<string> { };
            for (; half > 0; half--)
            {
                if (start < 0) {
                    start = args.Length - 1;
                    win.Add(args[start--]);
                } else {
                    win.Add(args[start--]);
                }
            }
            return win;
        }
        static List<string> Lose(int start, int half, string[] args)
        {
            List<string> lose = new List<string> { };
            for (; half > 0; half--)
            {
                if (start == args.Length) {
                    start = 0;
                    lose.Add(args[start++]);
                }else {
                    lose.Add(args[start++]);
                }
            }
            return lose;
        }
    }
}