using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System;


namespace Uniza.Namedays
{
	
	/// Reprezentuje deň a mesiac.
	public record struct DayMonth
	{
		/// <summary>
		/// Získava alebo nastavuje deň.
		/// </summary>
		public int Day { get; init; }

		/// <summary>
		/// Získava alebo nastavuje mesiac.
		/// </summary>
		public int Month { get; init; }

		/// <summary>
		/// Inicializuje novú inštanciu štruktúry <see cref="DayMonth"/> s aktuálnym dňom a mesiacom.
		/// </summary>
		public DayMonth() { Day = DateTime.Now.Day; Month = DateTime.Now.Month; }

		/// <summary>
		/// Inicializuje novú inštanciu štruktúry <see cref="DayMonth"/> s daným dňom a mesiacom.
		/// </summary>
		/// <param name="day">Deň.</param>
		/// <param name="month">Mesiac.</param>
		public DayMonth(int day, int month) { Day = day; Month = month; }

		/// <summary>
		/// Prevádza deň a mesiac na objekt typu <see cref="DateTime"/> s aktuálnym rokom.
		/// </summary>
		/// <returns>Objekt typu <see cref="DateTime"/> reprezentujúci deň a mesiac s aktuálnym rokom.</returns>
		public DateTime ToDateTime()
		{
			return new DateTime(DateTime.Now.Year, Month, Day);
		}
	}

	/// <summary>
	/// Reprezentuje meniny.
	/// </summary>
	public record struct Nameday
	{
		/// <summary>
		/// Získava alebo nastavuje meno.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Získava alebo nastavuje deň a mesiac.
		/// </summary>
		public DayMonth DayMonth { get; set; }

		/// <summary>
		/// Inicializuje novú inštanciu štruktúry <see cref="Nameday"/> so základnými hodnotami.
		/// </summary>
		public Nameday()
		{
			Name = "";
			DayMonth = new DayMonth();
		}

		/// <summary>
		/// Inicializuje novú inštanciu štruktúry <see cref="Nameday"/> s daným menom a dňom a mesiacom.
		/// </summary>
		/// <param name="name">Meno.</param>
		/// <param name="dayMonth">Deň a mesiac.</param>
		public Nameday(string name, DayMonth dayMonth)
		{
			Name = name;
			DayMonth = dayMonth;
		}
	}

	/// <summary>
	/// Kalendár menín.
	/// </summary>
	public record NamedayCalendar : IEnumerable<Nameday>
	{
		private readonly List<Nameday> _namedays = new();

		/// <summary>
		/// Počet záznamov menín.
		/// </summary>
		public int NameCount => _namedays.Count;

		/// <summary>
		/// Počet rôznych dní menín.
		/// </summary>
		public int DayCount => _namedays.Select(x => x.DayMonth).Distinct().Count();

		/// <summary>
		/// Indexer pre získanie menín podľa dňa a mesiaca.
		/// </summary>
		/// <param name="dayMonth">Deň a mesiac.</param>
		/// <returns>Pole menín pre daný deň a mesiac.</returns>
		public string[] this[DayMonth dayMonth]
		{
			get
			{
				return _namedays.Where(x => x.DayMonth == dayMonth).Select(x => x.Name).ToArray();
			}
		}

		/// <summary>
		/// Indexer pre získanie menín podľa dátumu (iba deň a mesiac).
		/// </summary>
		/// <param name="date">Dátum (iba deň a mesiac).</param>
		/// <returns>Pole menín pre daný dátum.</returns>
		public string[] this[DateOnly date]
		{
			get
			{
				return _namedays.Where(x => x.DayMonth.Day == date.Day && x.DayMonth.Month == date.Month).Select(x => x.Name).ToArray();
			}
		}

		/// <summary>
		/// Indexer pre získanie menín podľa dátumu.
		/// </summary>
		/// <param name="date">Dátum.</param>
		/// <returns>Pole menín pre daný dátum.</returns>
		public string[] this[DateTime date]
		{
			get
			{
				return _namedays.Where(x => x.DayMonth.Day == date.Day && x.DayMonth.Month == date.Month).Select(x => x.Name).ToArray();
			}
		}

		/// <summary>
		/// Indexer pre získanie menín podľa dňa a mesiaca.
		/// </summary>
		/// <param name="day">Deň.</param>
		/// <param name="month">Mesiac.</param>
		/// <returns>Pole menín pre daný deň a mesiac.</returns>
		public string[] this[int day, int month]
		{
			get
			{
				return _namedays.Where(x => x.DayMonth.Day == day && x.DayMonth.Month == month).Select(x => x.Name).ToArray();
			}
		}

		/// <summary>
		/// Enumerator pre prechádzanie záznamov menín.
		/// </summary>
		/// <returns>Enumerator pre záznamy menín.</returns>
		public IEnumerator<Nameday> GetEnumerator()
		{
			return _namedays.GetEnumerator();
		}

		/// <summary>
		/// Enumerator pre prechádzanie záznamov menín.
		/// </summary>
		/// <returns>Enumerator pre záznamy menín.</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
		
		/// <summary>
		/// Získanie všetkých záznamov menín.
		/// </summary>
		/// <returns>Kolekcia záznamov menín.</returns>
		public IEnumerable<Nameday> GetNamedays()
		{
			return _namedays;
		}

		/// <summary>
		/// Získanie záznamov menín pre daný mesiac.
		/// </summary>
		/// <param name="month">Mesiac.</param>
		/// <returns>Kolekcia záznamov menín pre daný mesiac.</returns>
		public IEnumerable<Nameday> GetNamedays(int month)
		{
			return _namedays.Where(x => x.DayMonth.Month == month);
		}

		/// <summary>
		/// Získanie záznamov menín, ktoré vyhovujú zadanému regulárnemu výrazu.
		/// </summary>
		/// <param name="pattern">Regulárny výraz.</param>
		/// <returns>Kolekcia záznamov menín, ktoré vyhovujú zadanému regulárnemu výrazu.</returns>
		public IEnumerable<Nameday> GetNamedays(string pattern)
		{
			Regex regex = new Regex(pattern);
			return _namedays.Where(x => regex.IsMatch(x.Name));
		}

		/// <summary>
		/// Pridanie záznamu menín.
		/// </summary>
		/// <param name="nameday">Záznam menín.</param>
		public void Add(Nameday nameday)
		{
			_namedays.Add(nameday);
		}

		/// <summary>
		/// Pridanie záznamov menín pre daný deň a mesiac a zadané mená.
		/// </summary>
		/// <param name="day">Deň.</param>
		/// <param name="month">Mesiac.</param>
		/// <param name="names">Mená.</param>
		public void Add(int day, int month, params string[] names)
		{
			foreach (string name in names)
			{
				_namedays.Add(new Nameday(name, new DayMonth(day, month)));
			}
		}

		/// <summary>
		/// Pridanie záznamov menín pre daný deň a mesiac a zadané mená.
		/// </summary>
		/// <param name="dayMonth">Deň a mesiac.</param>
		/// <param name="names">Mená.</param>
		public void Add(DayMonth dayMonth, params string[] names)
		{
			foreach (string name in names)
			{
				_namedays.Add(new Nameday(name, dayMonth));
			}
		}

		/// <summary>
		/// Odstránenie záznamu menín podľa mena.
		/// </summary>
		/// <param name="name">Meno.</param>
		/// <returns>True, ak bol záznam odstránený, inak False.</returns>
		public bool Remove(string name)
		{
			return _namedays.Remove(_namedays.FirstOrDefault(x => x.Name == name));
		}

		/// <summary>
		/// Získanie záznamu menín podľa dňa, mesiaca a mena.
		/// </summary>
		/// <param name="day">Deň.</param>
		/// <param name="month">Mesiac.</param>
		/// <param name="name">Meno.</param>
		/// <returns>Záznam menín, alebo null, ak sa nenájde zhodujúci sa záznam.</returns>
		public Nameday? GetNameDay(int day, int month, string name)
		{
			return _namedays.FirstOrDefault(nd => nd.DayMonth.Day == day && nd.DayMonth.Month == month && nd.Name == name);
		}

		/// <summary>
		/// Overenie, či záznam menín obsahuje dané meno.
		/// </summary>
		/// <param name="name">Meno.</param>
		/// <returns>True, ak záznam menín obsahuje dane meno, inak False.</returns>
		public bool Contains(string name)
		{
			return _namedays.Contains(_namedays.FirstOrDefault(x => x.Name == name));
		}

		/// <summary>
		/// Vyprázdnenie záznamov menín.
		/// </summary>
		public void Clear()
		{
			_namedays.Clear();
		}

		/// <summary>
		/// Načítanie záznamov menín zo súboru CSV.
		/// </summary>
		/// <param name="csvFile">Cesta k súboru CSV.</param>
		public void Load(FileInfo csvFile)
		{
			using (StreamReader reader = new StreamReader(csvFile.FullName))
			{
				while (!reader.EndOfStream)
				{
					string? line = reader.ReadLine();
					if (string.IsNullOrWhiteSpace(line))
					{
						continue;
					}
					string[] row = line.Split(';');

					string[] dayMonthParts = row[0].Split('.');
					int day = int.Parse(dayMonthParts[0]);
					int month = int.Parse(dayMonthParts[1]);
					for (int i = 1; i < row.Length; i++)
					{
						if (row[i] != "-" && row[i] != "")
						{
							_namedays.Add(new Nameday(row[i], new DayMonth(day, month)));
						}
						else
						{
							break;
						}
					}
				}
			}
		}

		/// <summary>
		/// Uloženie záznamov menín do súboru CSV.
		/// </summary>
		/// <param name="csvFile">Cesta k súboru CSV.</param>
		public void Save(FileInfo csvFile)
		{
			using (StreamWriter writer = new StreamWriter(csvFile.FullName))
			{
				Dictionary<DayMonth, List<string>> groupedNames = new Dictionary<DayMonth, List<string>>();

				foreach (Nameday nameday in _namedays)
				{
					if (!groupedNames.ContainsKey(nameday.DayMonth))
					{
						groupedNames[nameday.DayMonth] = new List<string>();
					}
					groupedNames[nameday.DayMonth].Add(nameday.Name);
				}

				foreach (var entry in groupedNames)
				{
					writer.WriteLine($"{entry.Key.Day}.{entry.Key.Month};{string.Join(";", entry.Value)}");
				}
			}
		}

		/// <summary>
		/// Získanie počtu menín pre každý mesiac.
		/// </summary>
		/// <returns>Slovník, kde kľúčom je názov mesiaca a hodnotou je počet menín pre daný mesiac.</returns>
		public IDictionary<string, int> GetMonthlyNameCount()
		{
			var monthlyNameCount = new Dictionary<string, int>();

			for (int month = 1; month < 13; ++month)
			{
				monthlyNameCount[MonthNumberToSlovakName(month)] = _namedays.Where(x => x.DayMonth.Month == month).Count();
			}
			return monthlyNameCount;
		}

		/// <summary>
		/// Prevod čísla mesiaca na slovenský názov mesiaca.
		/// </summary>
		/// <param name="month">Číslo mesiaca.</param>
		/// <returns>Slovenský názov mesiaca.</returns>
		public static string MonthNumberToSlovakName(int month)
		{
			string[] monthNames = {
			"Január", "Február", "Marec", "Apríl", "Máj", "Jún",
			"Júl", "August", "September", "Október", "November", "December"
			};

			if (month < 1 || month > 12) return string.Empty;
			return monthNames[month - 1];
		}

		/// <summary>
		/// Získanie počtu menín pre každé písmeno abecedy.
		/// </summary>
		/// <returns>Slovník, kde kľúčom je písmeno abecedy a hodnotou je počet menín začínajúcich na dané písmeno.</returns>
		public IDictionary<char, int> GetNameCountsByFirstLetter()
		{
			IDictionary<char, int> nameCountsByFirstLetter = new Dictionary<char, int>();

			var groupedNames = _namedays.Select(x => x.Name).GroupBy(x => x[0]).OrderBy(x => x.Key);

			foreach (var item in groupedNames)
			{
				nameCountsByFirstLetter[item.Key] = item.Count();
			}
			return nameCountsByFirstLetter;
		}

		/// <summary>
		/// Získanie počtu menín podľa počtu znakov v mene.
		/// </summary>
		/// <returns>Slovník, kde kľúčom je počet znakov v mene a hodnotou je počet menín s daným počtom znakov v mene.</returns>
		public IDictionary<int, int> GetNameCountsByCharacterLength()
		{
			IDictionary<int, int> nameCountsByCharacterLength = new Dictionary<int, int>();

			var groups = _namedays.Select(x => x.Name).GroupBy(x => x.Length).OrderBy(x => x.Key);

			foreach (var item in groups)
			{
				nameCountsByCharacterLength[item.Key] = item.Count();
			}
			return nameCountsByCharacterLength;
		}
		/// <summary>
		/// Aktualizovanie Nameday.
		/// </summary>
		public void UpdateNameday(Nameday oldNameday, Nameday newNameday)
		{
			var index = _namedays.IndexOf(oldNameday);
			if (index != -1)
			{
				_namedays[index] = newNameday;
			}
		}

	}
}