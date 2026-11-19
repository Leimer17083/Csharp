using System;

namespace HunterGame
{
    class Hunter
    {
        public string Name { get; private set; }
        public int Health { get; private set; }
        public int Gold { get; private set; }
        public int Arrows { get; private set; }
        public int PreyCount { get; private set; }

        public Hunter(string name, int health, int gold, int arrows)
        {
            Name = name;
            Health = health;
            Gold = gold;
            Arrows = arrows;
            PreyCount = 0;
        }

        public void ShowInfo()
        {
            Console.WriteLine($"[{Name}] Здоров'я: {Health} | Золото: {Gold} | Стріли: {Arrows} | Здобич: {PreyCount}");
        }

        public void BuyArrows(int amount, int costPerArrow)
        {
            int totalCost = amount * costPerArrow;
            if (Gold >= totalCost)
            {
                Gold -= totalCost;
                Arrows += amount;
                Console.WriteLine($"{Name} купує {amount} стріл за {totalCost} золота.");
            }
            else
            {
                Console.WriteLine($"{Name} не має достатньо золота для покупки!");
            }
        }

        public void Hunt(Random rnd)
        {
            if (Health <= 0)
            {
                Console.WriteLine($"{Name} занадто виснажений для полювання.");
                return;
            }

            if (Arrows <= 0)
            {
                Console.WriteLine($"{Name} не має стріл для полювання.");
                return;
            }

            Arrows--;
            int healthLoss = rnd.Next(5, 16);
            Health -= healthLoss;

            bool success = rnd.Next(1, 101) <= 65;

            if (success)
            {
                PreyCount++;
                int goldReward = rnd.Next(15, 31);
                Gold += goldReward;
                Console.WriteLine($"{Name} влучає! Здобуто тушку та {goldReward} монет. Втрачено {healthLoss} здоров'я.");
            }
            else
            {
                Console.WriteLine($"{Name} промахується. Втрачено {healthLoss} здоров'я.");
            }
        }
    }

    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Random rnd = new Random();

            Hunter player = new Hunter("Гравець", 100, 20, 5);
            Hunter bot = new Hunter("Суперник", 100, 20, 5);

            Console.WriteLine("=== ПОЧАТОК ЗМАГАННЯ ===");
            player.ShowInfo();
            bot.ShowInfo();
            Console.WriteLine("========================\n");

            bool isPlaying = true;

            while (isPlaying && player.Health > 0 && bot.Health > 0)
            {
                Console.WriteLine("Оберіть дію:");
                Console.WriteLine("1 - Піти на полювання");
                Console.WriteLine("2 - Купити 1 стрілу (Ціна: 4 золота)");
                Console.WriteLine("3 - Показати статус обох мисливців");
                Console.WriteLine("0 - Завершити змагання та підбити підсумки");
                Console.Write("Ваш вибір: ");

                string choice = Console.ReadLine();
                Console.WriteLine();

                bool turnTaken = false;

                switch (choice)
                {
                    case "1":
                        player.Hunt(rnd);
                        turnTaken = true;
                        break;
                    case "2":
                        player.BuyArrows(1, 4);
                        turnTaken = true;
                        break;
                    case "3":
                        player.ShowInfo();
                        bot.ShowInfo();
                        break;
                    case "0":
                        isPlaying = false;
                        break;
                    default:
                        Console.WriteLine("Невідома команда. Спробуйте ще раз.");
                        break;
                }

                if (turnTaken && isPlaying && bot.Health > 0)
                {
                    if (bot.Arrows <= 0 && bot.Gold >= 4)
                    {
                        bot.BuyArrows(1, 4);
                    }
                    else
                    {
                        bot.Hunt(rnd);
                    }
                    Console.WriteLine();
                }
            }

            Console.WriteLine("=== РЕЗУЛЬТАТИ ЗМАГАННЯ ===");
            player.ShowInfo();
            bot.ShowInfo();
            Console.WriteLine("===========================\n");

            if (player.PreyCount > bot.PreyCount)
            {
                Console.WriteLine($"Переможець: {player.Name}!");
            }
            else if (bot.PreyCount > player.PreyCount)
            {
                Console.WriteLine($"Переможець: {bot.Name}!");
            }
            else
            {
                if (player.Gold > bot.Gold)
                {
                    Console.WriteLine($"Нічия за здобиччю. Переможець за золотом: {player.Name}!");
                }
                else if (bot.Gold > player.Gold)
                {
                    Console.WriteLine($"Нічия за здобиччю. Переможець за золотом: {bot.Name}!");
                }
                else
                {
                    Console.WriteLine("Абсолютна нічия!");
                }
            }
        }
    }
}