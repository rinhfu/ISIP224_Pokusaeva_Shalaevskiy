using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISIP224_Pokusaeva_Shalaevskiy
{
    internal class Program
    {
        static void Main(string[] args)
        {

        }
    }

    enum EnemyType { Goblin, Skeleton, Mage};
    enum BossTytpe { VVG, Kovalsky, ArchmageCpp, PestovCmm }
    enum ItemType { Potion, Weapon, Armor}
    enum PlayerAction { Attack, Defend }

    abstract class Entity
    {
        public string Name { get; protected set; }
        public int MaxHealth { get; protected set; }
        public int Health { get; protected set; }
        public bool IsAlive => Health > 0;

        public Entity(string name, int maxHealth)
        {
            Name = name;
            MaxHealth = maxHealth;
            Health = maxHealth;
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;
            if (Health < 0) Health = 0;
        }

        public void HealFull()
        {
            Health = MaxHealth;
        }
    }

    class Item
    {
        public string Name { get; set; }
        public ItemType Type { get; set; }
        public int AttackBonus { get; set; }
        public int DefenseBonus { get; set; }

        public Item(string name, ItemType type, int attackBonus = 0, int defenseBonus = 0)
        {
            Name = name;
            Type = type;
            AttackBonus = attackBonus;
            DefenseBonus = defenseBonus;
        }

        public override string ToString()
        {
            if (Type == ItemType.Weapon)
                return $"{Name} (Оружие, Атака +{AttackBonus})";
            if (Type == ItemType.Armor)
                return $"{Name} (Доспех, Защита +{DefenseBonus})";
            return Name;
        }
    }

    class Player : Entity
    {
        public int BaseAttack { get; set; } = 5;
        public int BaseDefense { get; set; } = 2;
        public Item Weapon { get; set; }
        public Item Armor { get; set; }

        public int TotalAttack => BaseAttack + (Weapon?.AttackBonus ?? 0);
        public int TotalDefense => BaseDefense + (Armor?.DefenseBonus ?? 0);

        public bool IsFrozen { get; set; } = false;
        public bool IsDefending { get; set; } = false;

        public Player(string name, int health) : base(name, health) { }

        public void ShowStats()
        {
            Console.WriteLine($"\n--- {Name} ---");
            Console.WriteLine($"HP: {Health}/{MaxHealth}");
            Console.WriteLine($"Атака: {TotalAttack} (база {BaseAttack} + оружие {Weapon?.AttackBonus ?? 0})");
            Console.WriteLine($"Защита: {TotalDefense} (база {BaseDefense} + доспех {Armor?.DefenseBonus ?? 0})");
            Console.WriteLine($"Оружие: {(Weapon != null ? Weapon.ToString() : "нет")}");
            Console.WriteLine($"Доспех: {(Armor != null ? Armor.ToString() : "нет")}");
        }
    }

    abstract class Enemy : Entity
    {
        public int Attack { get; protected set; }
        public int Defense { get; protected set; }
        public Random Rnd { get; } = new Random();

        public Enemy(string name, int health, int attack, int defense) : base(name, health)
        {
            Attack = attack;
            Defense = defense;
        }

        public abstract int CalculateDamage(Player player);

        public virtual void ShowStats()
        {
            Console.WriteLine($"\n--- {Name} ---");
            Console.WriteLine($"HP: {Health}/{MaxHealth}");
            Console.WriteLine($"Атака: {Attack}, Защита: {Defense}");
        }
    }

    class Goblin : Enemy
    {
        public double CritChance { get; protected set; } = 0.2;

        public Goblin(string name, int health, int attack, int defense, double critChance = 0.2)
            : base(name, health, attack, defense)
        {
            CritChance = critChance;
        }

        public override int CalculateDamage(Player player)
        {
            int damage = Math.Max(1, Attack - player.TotalDefense);
            if (Rnd.NextDouble() < CritChance)
            {
                Console.WriteLine($"{Name} наносит КРИТИЧЕСКИЙ удар!");
                damage *= 2;
            }
            return damage;
        }
    }

    class Skeleton : Enemy
    {
        public Skeleton(string name, int health, int attack, int defense)
            : base(name, health, attack, defense) { }

        public override int CalculateDamage(Player player)
        {
            Console.WriteLine($"{Name} игнорирует вашу защиту!");
            return Attack;
        }
    }

    class Mage : Enemy
    {
        public double FreezeChance { get; protected set; } = 0.25;

        public Mage(string name, int health, int attack, int defense, double freezeChance = 0.25)
            : base(name, health, attack, defense)
        {
            FreezeChance = freezeChance;
        }

        public override int CalculateDamage(Player player)
        {
            int damage = Math.Max(1, Attack - player.TotalDefense);
            if (Rnd.NextDouble() < FreezeChance)
            {
                Console.WriteLine($"{Name} накладывает ЗАМОРОЗКУ! Вы пропустите следующий ход.");
                player.IsFrozen = true;
            }
            return damage;
        }
    }

    class VVG : Goblin
    {
        public VVG() : base("ВВГ (Босс-Гоблин)", (int)(30 * 2.0), (int)(8 * 1.5), (int)(3 * 1.2), 0.2 + 0.1) { }
    }

    class Kovalsky : Skeleton
    {
        public Kovalsky() : base("Ковальский (Босс-Скелет)", (int)(25 * 2.5), (int)(7 * 1.3), (int)(2 * 1.4)) { }
    }

    class ArchmageCpp : Mage
    {
        public ArchmageCpp() : base("Архимаг C++ (Босс-Маг)", (int)(20 * 1.8), (int)(6 * 1.6), (int)(2 * 1.1), 0.25 + 0.1) { }
    }

    class PestovCmm : Skeleton
    {
        public double FreezeChance { get; } = 0.25 + 0.15;
        private Random rnd = new Random();

        public PestovCmm() : base("Пестов С-- (Босс-Скелет)", (int)(25 * 1.3), (int)(7 * 1.8), (int)(2 * 0.6)) { }

        public override int CalculateDamage(Player player)
        {
            Console.WriteLine($"{Name} игнорирует вашу защиту!");
            if (rnd.NextDouble() < FreezeChance)
            {
                Console.WriteLine($"{Name} накладывает ЗАМОРОЗКУ! Вы пропустите следующий ход.");
                player.IsFrozen = true;
            }
            return Attack;
        }
    }

    static class EnemyFactory
    {
        private static Random rnd = new Random();

        public static Enemy CreateRandomEnemy()
        {
            int type = rnd.Next(3);
            switch (type)
            {
                case 0: return new Goblin("Гоблин", 30, 8, 3);
                case 1: return new Skeleton("Скелет", 25, 7, 2);
                default: return new Mage("Маг", 20, 6, 2);
            }
        }

        public static Enemy CreateRandomBoss()
        {
            int type = rnd.Next(4);
            switch (type)
            {
                case 0: return new VVG();
                case 1: return new Kovalsky();
                case 2: return new ArchmageCpp();
                default: return new PestovCmm();
            }
        }
    }

    static class ItemFactory
    {
        private static Random rnd = new Random();

        public static Item CreateRandomItem()
        {
            int type = rnd.Next(3);
            switch (type)
            {
                case 0:
                    return new Item("Лечебное зелье", ItemType.Potion);
                case 1:
                    return new Item("Меч", ItemType.Weapon, attackBonus: rnd.Next(2, 6));
                default:
                    return new Item("Щит", ItemType.Armor, defenseBonus: rnd.Next(1, 4));
            }
        }
    }

    class Game
    {
        private Player player;
        private Random rnd = new Random();
        private int turnCounter = 0;

        public Game()
        {
            Console.WriteLine("=== Добро пожаловать в текстовый рогалик! ===");
            Console.Write("Введите имя вашего героя: ");
            string name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name)) name = "Герой";
            player = new Player(name, 100);
        }

        public void Run()
        {
            while (player.IsAlive)
            {
                turnCounter++;
                Console.WriteLine($"\n========== Ход {turnCounter} ==========");
                player.ShowStats();

                if (turnCounter % 10 == 0)
                {
                    Console.WriteLine("\n!!! ВАС ВСТРЕЧАЕТ БОСС !!!");
                    Enemy boss = EnemyFactory.CreateRandomBoss();
                    Fight(boss);
                }
                else
                {
                    if (rnd.Next(2) == 0)
                    {
                        Console.WriteLine("\nВы нашли сундук!");
                        OpenChest();
                    }
                    else
                    {
                        Console.WriteLine("\nВас атакует враг!");
                        Enemy enemy = EnemyFactory.CreateRandomEnemy();
                        Fight(enemy);
                    }
                }

                if (!player.IsAlive) break;

                Console.WriteLine("\nНажмите Enter для продолжения...");
                Console.ReadLine();
            }

            Console.WriteLine("\n========================================");
            Console.WriteLine($"Игра окончена! Вы продержались {turnCounter} ходов.");
            Console.WriteLine("========================================");
        }

        private void OpenChest()
        {
            Item item = ItemFactory.CreateRandomItem();
            Console.WriteLine($"В сундуке: {item}");

            if (item.Type == ItemType.Potion)
            {
                player.HealFull();
                Console.WriteLine("Вы выпили зелье и полностью восстановили здоровье!");
                return;
            }

            Console.WriteLine("\n--- Сравнение ---");
            if (item.Type == ItemType.Weapon)
            {
                Console.WriteLine($"Текущее оружие: {(player.Weapon != null ? player.Weapon.ToString() : "нет")}");
                Console.WriteLine($"Новое оружие: {item}");
            }
            else
            {
                Console.WriteLine($"Текущий доспех: {(player.Armor != null ? player.Armor.ToString() : "нет")}");
                Console.WriteLine($"Новый доспех: {item}");
            }

            Console.Write("Взять новый предмет? (y/n): ");
            string choice = Console.ReadLine()?.Trim().ToLower();
            if (choice == "y" || choice == "yes" || choice == "д" || choice == "да")
            {
                if (item.Type == ItemType.Weapon)
                    player.Weapon = item;
                else
                    player.Armor = item;
                Console.WriteLine("Вы взяли предмет!");
            }
            else
            {
                Console.WriteLine("Вы выбросили предмет.");
            }
        }

        private void Fight(Enemy enemy)
        {
            Console.WriteLine("\n--- НАЧАЛО БОЯ ---");
            enemy.ShowStats();
            Console.WriteLine("\n--- Ваши характеристики ---");
            player.ShowStats();

            while (player.IsAlive && enemy.IsAlive)
            {
                if (player.IsFrozen)
                {
                    Console.WriteLine("\nВы заморожены и пропускаете ход!");
                    player.IsFrozen = false;
                }
                else
                {
                    PlayerTurn(enemy);
                }

                if (!enemy.IsAlive)
                {
                    Console.WriteLine($"\n{enemy.Name} побеждён!");
                    break;
                }

                EnemyTurn(enemy);

                if (!player.IsAlive)
                {
                    Console.WriteLine("\nВы погибли...");
                    break;
                }

                Console.WriteLine($"\nВаше HP: {player.Health}/{player.MaxHealth} | HP врага: {enemy.Health}/{enemy.MaxHealth}");
            }
        }

        private void PlayerTurn(Enemy enemy)
        {
            Console.WriteLine("\n--- Ваш ход ---");
            Console.WriteLine("1. Атака");
            Console.WriteLine("2. Защита");
            Console.Write("Выбор: ");

            string input = Console.ReadLine()?.Trim();
            PlayerAction action;

            if (input == "2")
                action = PlayerAction.Defend;
            else
                action = PlayerAction.Attack;

            player.IsDefending = false;

            if (action == PlayerAction.Attack)
            {
                int damage = Math.Max(1, player.TotalAttack - enemy.Defense);
                enemy.TakeDamage(damage);
                Console.WriteLine($"Вы атакуете и наносите {damage} урона!");
            }
            else
            {
                if (rnd.NextDouble() < 0.4)
                {
                    Console.WriteLine("Вы готовитесь к защите... и полностью уклоняетесь от следующей атаки!");
                    player.IsDefending = true;
                }
                else
                {
                    double blockPercent = 0.7 + rnd.NextDouble() * 0.3;
                    int blockAmount = (int)(player.TotalDefense * blockPercent);
                    Console.WriteLine($"Вы блокируете! Следующая атака будет ослаблена на {blockAmount} (блок {blockPercent:P0} от защиты {player.TotalDefense}).");
                    player.BaseDefense += blockAmount;
                    player.IsDefending = false;
                    pendingBlock = blockAmount;
                }
            }
        }

        private int pendingBlock = 0;

        private void EnemyTurn(Enemy enemy)
        {
            Console.WriteLine($"\n--- Ход {enemy.Name} ---");

            if (player.IsDefending)
            {
                Console.WriteLine($"Вы полностью уклонились от атаки {enemy.Name}!");
                player.IsDefending = false;
                return;
            }

            int damage = enemy.CalculateDamage(player);

            if (pendingBlock > 0)
            {
                damage = Math.Max(0, damage - pendingBlock);
                player.BaseDefense -= pendingBlock;
                pendingBlock = 0;
                Console.WriteLine($"Блок поглотил часть урона!");
            }

            player.TakeDamage(damage);
            Console.WriteLine($"{enemy.Name} наносит {damage} урона!");
        }
    }
}