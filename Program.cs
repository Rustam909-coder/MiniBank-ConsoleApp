using System;
using System.Collections.Generic;
using System.IO;

namespace MiniBankApp
{
    class Program
    {
        private static string balanceFile = "balance.txt";
        private static string historyFile = "history.txt";

        private static decimal balance = 0;
        private static List<string> history = new List<string>();

        static void Main(string[] args)
        {
            LoadData();
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("=== Добро пожаловать в систему Т-Банк Старт ===");
            
            Console.Write("Введите номер аккаунта: ");
            Console.ReadLine();
            Console.Write("Введите ПИН-код: ");
            Console.ReadLine();
            Console.WriteLine("\nАвторизация успешна!\n");

            bool isRunning = true;
            while (isRunning)
            {
                Console.WriteLine("--- Меню обслуживания ---");
                Console.WriteLine("1. Проверить баланс");
                Console.WriteLine("2. Пополнить счёт");
                Console.WriteLine("3. Снять наличные");
                Console.WriteLine("4. Показать историю операций");
                Console.WriteLine("5. Выйти из системы");
                Console.Write("\nВыберите действие (1-5): ");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        Console.WriteLine($"\nТекущий баланс: {balance} руб.\n");
                        break;
                    case "2":
                        ProcessDeposit();
                        break;
                    case "3":
                        ProcessWithdrawal();
                        break;
                    case "4":
                        ShowHistory();
                        break;
                    case "5":
                        SaveData();
                        Console.WriteLine("\nСпасибо за использование системы Т-Банк! До свидания.");
                        isRunning = false;
                        break;
                    default:
                        Console.WriteLine("\nОшибка: Неверный пункт меню. Попробуйте снова.\n");
                        break;
                }
            }
        }

        private static void ProcessDeposit()
        {
            Console.Write("\nВведите сумму для пополнения: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal amount) && amount > 0)
            {
                balance += amount;
                string record = $"{DateTime.Now}: Пополнение на +{amount} руб.";
                history.Add(record);
                SaveData();
                Console.WriteLine($"\nСчёт успешно пополнен. Текущий баланс: {balance} руб.\n");
            }
            else
            {
                Console.WriteLine("\nОшибка: Введена неверная сумма.\n");
            }
        }

        private static void ProcessWithdrawal()
        {
            Console.Write("\nВведите сумму для снятия: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal amount) && amount > 0)
            {
                if (amount <= balance)
                {
                    balance -= amount;
                    string record = $"{DateTime.Now}: Снятие -{amount} руб.";
                    history.Add(record);
                    SaveData();
                    Console.WriteLine($"\nУспешно снято {amount} руб. Остаток: {balance} руб.\n");
                }
                else
                {
                    Console.WriteLine("\nОшибка: Недостаточно средств на счёте.\n");
                }
            }
            else
            {
                Console.WriteLine("\nОшибка: Введена неверная сумма.\n");
            }
        }

        private static void ProcessTransfer()
        {
            Console.Write("\nВведите номер карты получателя: ");
            string targetCard = Console.ReadLine();
            Console.Write("Введите сумму перевода: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal amount) && amount > 0)
            {
                if (amount <= balance)
                {
                    balance -= amount;
                    string record = $"{DateTime.Now}: Перевод {amount} руб. на карту {targetCard}";
                    history.Add(record);
                    SaveData();
                    Console.WriteLine($"\nПеревод успешно выполнен. Остаток: {balance} руб.\n");
                }
                else
                {
                    Console.WriteLine("\nОшибка: Недостаточно средств.\n");
                }
            }
            else
            {
                Console.WriteLine("\nОшибка: Неверная сумма.\n");
            }
        }

        private static void ShowHistory()
        {
            Console.WriteLine("\n--- История ваших операций ---");
            if (history.Count == 0)
            {
                Console.WriteLine("Операций пока не обнаружено.");
            }
            else
            {
                foreach (var record in history)
                {
                    Console.WriteLine(record);
                }
            }
            Console.WriteLine();
        }

        private static void SaveData()
        {
            try
            {
                File.WriteAllText(balanceFile, balance.ToString());
                File.WriteAllLines(historyFile, history);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка сохранения: {ex.Message}");
            }
        }

        private static void LoadData()
        {
            try
            {
                if (File.Exists(balanceFile))
                {
                    decimal.TryParse(File.ReadAllText(balanceFile), out balance);
                }
                if (File.Exists(historyFile))
                {
                    history = new List<string>(File.ReadAllLines(historyFile));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки: {ex.Message}");
            }
        }
    }
}
