using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using Uniza.Namedays;

namespace EditorGuiApp
{
    /// <summary>
    /// Interaction logic for EditorUserControl.xaml
    /// </summary>
    public partial class EditorUserControl : UserControl
    {
        public NamedayCalendar? Namedays { get; set; }

        public EditorUserControl()
        {
            InitializeComponent();
        }

		private void MonthComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ShowNames();
		}

		private void ShowNames()
		{
			if (Namedays != null && monthComboBox.SelectedIndex != -1)
			{
				namedaysListBox.Items.Clear();

				int selectedMonth = monthComboBox.SelectedIndex + 1;

				int daysInMonth = DateTime.DaysInMonth(DateTime.Now.Year, selectedMonth);



				var filteredNamedays = Namedays.GetNamedays(regexTextBox.Text).Where(a => a.DayMonth.Month == selectedMonth);

				foreach (var nameday in filteredNamedays)
				{

					namedaysListBox.Items.Add(new NamedayListItem
					{
						Date = $"{nameday.DayMonth.Day}.{nameday.DayMonth.Month}",
						Name = nameday.Name
					});
				}
			}
		}

		private void regexTextBox_SelectionChanged(object sender, RoutedEventArgs e)
		{
			ShowNames();
		}

		private void Clear_Button_Click(object sender, RoutedEventArgs e)
		{
			regexTextBox.Clear();
			monthComboBox.SelectedIndex = -1;
			namedaysListBox.Items.Clear();

		}
		private void Add_Button_Click(object sender, RoutedEventArgs e)
		{
			
			var addWindow = new AddWindow(Namedays);
			addWindow.Show();
			ShowNames();

		}

		private void Edit_Button_Click(object sender, RoutedEventArgs e)
		{
			if (namedaysListBox.SelectedItem is NamedayListItem selectedListItem)
			{
				var parts = selectedListItem.Date.Split('.');
				if (parts.Length == 2 && int.TryParse(parts[0], out var day) && int.TryParse(parts[1], out var month))
				{
					var selectedNameday = Namedays.GetNameDay(day, month, selectedListItem.Name.ToString());
					if (selectedNameday != null)
					{
						var editWindow = new EditWindow((Nameday)selectedNameday, Namedays);
						editWindow.ShowDialog();
						ShowNames();
					}
				}
			}
		}


		private void Remove_Button_Click(object sender, RoutedEventArgs e)
		{
			if (namedaysListBox.SelectedItem is NamedayListItem selectedListItem)
			{ 
				var remWindow = new DeleteWindow(selectedListItem.Name.ToString());
				remWindow.ShowDialog();
				if (remWindow.ConfirmDelete)
				{
					{
						Namedays.Remove(selectedListItem.Name.ToString());
						ShowNames();
					}
				}
				
			}
		}
		
		private void Show_Button_Click(object sender, RoutedEventArgs e)
		{
			if (namedaysListBox.SelectedItem is NamedayListItem selectedListItem)
			{
				var parts = selectedListItem.Date.Split('.');
				if (parts.Length == 2 && int.TryParse(parts[0], out var day) && int.TryParse(parts[1], out var month))
				{
					var selectedDate = new DateTime(DateTime.Now.Year, month, day);
					MainWindow mainWindow = FindParentMainWindow();
					var calendarControl = mainWindow.calendarControl;
					calendarControl.DisplayNamedays(selectedDate);

				}
			}

		}
		private MainWindow FindParentMainWindow()
		{
			DependencyObject parent = VisualTreeHelper.GetParent(this);

			while (parent != null && !(parent is MainWindow))
			{
				parent = VisualTreeHelper.GetParent(parent);
			}

			return parent as MainWindow;
		}
	}


	internal class NamedayListItem
	{
		public string Date { get; set; }
		public object Name { get; set; }
	}
}
