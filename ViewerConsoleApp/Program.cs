using System.Collections;
using System.Globalization;
using Uniza.Namedays;

namespace Uniza.Namedays.ViewerConsoleApp
{
    internal class Program
    {
        /// <param name="load">When passed, the program will ask for a file path to load the nameday calendar data from. If not provided, the default file path will be used.</param>

        private static void Main(bool load = false)
        {
            
            NamedayCalendar calendar = new();

            if (load)
            {
                LoadCalendar(calendar);
            }
            else
            {
                FileInfo csvFile = new(@"..\..\..\..\namedays-sk.csv");
                calendar.Load(csvFile);
            }
            DisplayWelcomeMessage(calendar);
            ShowConsoleMenu(calendar);
        }

        private static void ShowConsoleMenu(NamedayCalendar calendar)
        {
            int choice;
            do
            {
                Console.WriteLine("\nMenu:");
                Console.WriteLine("1. Načítať kalendár");
                Console.WriteLine("2. Zobraziť štatistiky");
                Console.WriteLine("3. Vyhľadať mená");
                Console.WriteLine("4. Vyhľadať mená podľa dátumu");
                Console.WriteLine("5. Zobraziť kalendár mien v mesiaci");
                Console.WriteLine("6. Koniec");

                Console.Write("Zvoľte možnosť: ");
                if(!int.TryParse(Console.ReadLine(), out choice)) choice = 0;

                switch (choice)
                {
                    case 1:
                        LoadCalendar(calendar);
                        break;
                    case 2:
                        ShowStatistics(calendar);
                        break;
                    case 3:
                        SearchNames(calendar);
                        break;
                    case 4:
                        SearchNamesByDate(calendar);
                        break;
                    case 5:
                        ShowNamedaysInMonth(calendar);
                        break;
                    case 6:
                        Console.WriteLine("Ukončujem...");
                        break;
                    default:
                        Console.WriteLine("Neplatná voľba. Skúste znova.");
                        break;
                }
            } while (choice != 6);
            
        }

        private static void ShowNamedaysInMonth(NamedayCalendar calendar)
        {
            Console.WriteLine("KALENDÁR MENIN");
            DateTime currentDate = DateTime.Now;
            DateTime displayDate = currentDate;

            while (true)
            {
                Console.Clear();
                Console.WriteLine($"{NamedayCalendar.MonthNumberToSlovakName(displayDate.Month)} {displayDate.Year}:");

                for (int day = 1; day <= DateTime.DaysInMonth(displayDate.Year, displayDate.Month); day++)
                {
                    DateTime currentDisplayDate = new DateTime(displayDate.Year, displayDate.Month, day);
                    DayOfWeek dayOfWeek = currentDisplayDate.DayOfWeek;
                    string names = string.Join(", ", calendar[day, displayDate.Month]);

                    if (currentDisplayDate.Date == currentDate.Date)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    else if (dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    else
                    {
                        Console.ResetColor();
                    }

                    Console.WriteLine($"  {day}.{displayDate.Month} {currentDisplayDate.ToString("ddd", new CultureInfo("sk-SK"))} {names}");
                }

                Console.ResetColor();
                Console.WriteLine("\nŠípka doľava / doprava: mesiac dozadu / dopredu.");
                Console.WriteLine("Šípka dole / hore - rok dozadu / dopredu");
				Console.WriteLine("Kláves Home alebo D - aktuálny deň");
				Console.WriteLine("Pre ukončenie stlačte Enter.");


				ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.LeftArrow)
                {
                    displayDate = displayDate.AddMonths(-1);
                }
                else if (keyInfo.Key == ConsoleKey.RightArrow)
                {
                    displayDate = displayDate.AddMonths(1);
                }
                else if (keyInfo.Key == ConsoleKey.UpArrow)
                {
                    displayDate = displayDate.AddYears(-1);
                }
                else if (keyInfo.Key == ConsoleKey.DownArrow)
                {
                    displayDate = displayDate.AddYears(1);
                }
                else if (keyInfo.Key == ConsoleKey.Home || keyInfo.Key == ConsoleKey.D)
                {
                    displayDate = currentDate;
                }
                else if (keyInfo.Key == ConsoleKey.Enter)
                {
                    break;
                }
            }

        }

        private static void SearchNamesByDate(NamedayCalendar calendar)
        {
            Console.WriteLine("VYHĽADAVANIE MIEN PODĽA DÁTUMU");
            while (true)
            {
                Console.WriteLine("Pre ukončenie stlačte Enter.");
                Console.Write("Zadajte deň a mesiac: ");

                string? input = Console.ReadLine();

                if (string.IsNullOrEmpty(input))
                {
                    break;
                }

                string[] dateParts = input.Split('.');
                if (dateParts.Length != 2)
                {
                    Console.WriteLine("Zadajte dátum v správnom formáte (deň.mesiac).");
                    continue;
                }

                if (!int.TryParse(dateParts[0], out int day) || !int.TryParse(dateParts[1], out int month))
                {
                    Console.WriteLine("Neplatný vstup. Zadajte dátum v správnom formáte (deň.mesiac).");
                    continue;
                }

                string[] names = calendar[day, month];

                if (names.Length == 0)
                {
                    Console.WriteLine("Neboli nájdené žiadne mená.");
                }
                else
                {
                    Console.WriteLine($"Mená pre dátum {day}.{month}:");
                    int i = 0;
                    foreach (string name in names)
                    {
                        Console.WriteLine($"{i}  {name}");
                        ++i;
                    }
                }
            }
        }

        private static void SearchNames(NamedayCalendar calendar)
        {
            Console.WriteLine("VYHĽADAVANIE MIEN");
            while (true)
            {
                Console.WriteLine("Pre ukončenie stlačte Enter.");
                Console.Write("Zadajte meno (regulárny výraz): ");
                string? pattern = Console.ReadLine();

                if (string.IsNullOrEmpty(pattern))
                {
                    return;
                }

                var matchingNamedays = calendar.GetNamedays(pattern);

                if (!matchingNamedays.Any())
                {
                    Console.WriteLine("Neboli nájdené žiadne mená.");
                }
                else
                {
                    Console.WriteLine("Nájdené mená:");
                    for (int i = 0; i < matchingNamedays.Count(); i++)
                    {
                        Nameday nameday = matchingNamedays.ElementAt(i);
                        Console.WriteLine($"  {i + 1}. {nameday.Name} ({nameday.DayMonth.Day}.{nameday.DayMonth.Month})");
                    }

                }
            }
        }

        private static void ShowStatistics(NamedayCalendar calendar)
        {
            Console.WriteLine("šTATISTIKA");
            Console.WriteLine($"Celkový počet mien v kalendári: {calendar.NameCount}");
            Console.WriteLine($"Celkový počet dní obsahujúci mená v kalendári: {calendar.DayCount}");
            Console.WriteLine("Celkový počet mien v jednotlivých mesiacoch:");
            IDictionary<string, int> monthlyNameCount = calendar.GetMonthlyNameCount();
            foreach (var item in monthlyNameCount)
            {
                Console.WriteLine($"  {item.Key}: {item.Value}");
            }
            Console.WriteLine($"Počet mien podľa začiatočných písmen:");
            IDictionary<char, int> nameCountsByFirstLetter = calendar.GetNameCountsByFirstLetter();
            foreach (var item in nameCountsByFirstLetter)
            {
                Console.WriteLine($"  {item.Key}: {item.Value}");
            }
            Console.WriteLine($"Počet mien podľa dlžky znakov:");
            IDictionary<int, int> nameCountsByCharacterLength = calendar.GetNameCountsByCharacterLength();
            foreach (var item in nameCountsByCharacterLength)
            {
                Console.WriteLine($"  {item.Key}: {item.Value}");
            }
            Console.WriteLine("Pre ukončenie stlačte Enter");
            Console.ReadLine();
        }

        private static void LoadCalendar(NamedayCalendar calendar)
        {
            Console.WriteLine("OTVORENIE");
            Console.WriteLine("Zadajte cestu k súboru kalendára mien stlačte Enter pre ukončenie.");
            string? filePath;

            while (true)
            {
                try
                {
                    Console.Write("Zadajte cestu k CSV súboru: ");
                    filePath = Console.ReadLine();
                
                    FileInfo csvFile = new(filePath ?? "");
                    if (!csvFile.Exists)
                    {
                        Console.WriteLine($"Zadaný súbor {filePath ?? ""} neexistuje");
                    }
                    else if (csvFile.Extension.ToLower() != ".csv")
                    {
                        Console.WriteLine($"Zadaný súbor {filePath} nie je typu CSV");
                    }
                     else 
                    {
                        calendar.Load(csvFile);
                        Console.WriteLine("Kalendár bol úspešne načítaný.");
                        Console.WriteLine("Pre pokračovanie stlačte Enter.");
                        Console.ReadLine();
                        break;
                    }
                  
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Chyba pri načítavaní kalendára: {ex.Message}");
                }
            }
        }

        private static void DisplayWelcomeMessage(NamedayCalendar calendar)
        {
            Console.WriteLine("KALENDÁR MIEN");
            Console.WriteLine($"Dnes {DateTime.Now}, ma meniny: {string.Join(", ",calendar[DateTime.Today])} ");
            Console.WriteLine($"Zajtra má meniny: {string.Join(", ", calendar[DateTime.Today.AddDays(1)])}");
        }
    }
}