using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Uniza.Namedays;

namespace EditorGuiApp
{
	/// <summary>
	/// Interaction logic for AddWindow.xaml
	/// </summary>
	public partial class AddWindow : Window
	{
		public NamedayCalendar? Namedays { get; set; }

		public AddWindow(NamedayCalendar? namedays)
		{
			InitializeComponent();
			Namedays = namedays;
		}

		private void OK_Button_Click(object sender, RoutedEventArgs e)
		{
			Namedays.Add(new Nameday(Name.Text, new DayMonth( Date.SelectedDate.Value.Day, Date.SelectedDate.Value.Month)));
			this.Close();
		}

		private void Cancel_Button_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}
	}
}
