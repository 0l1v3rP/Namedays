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
	/// Interaction logic for EditWindow.xaml
	/// </summary>
	public partial class EditWindow : Window
	{
		private Nameday oldNameday;
		private NamedayCalendar namedays;

		public EditWindow(Nameday nameday, NamedayCalendar namedays)
		{
			InitializeComponent();
			oldNameday = nameday;
			this.namedays = namedays;
			Name.Text = oldNameday.Name;
			Date.SelectedDate = oldNameday.DayMonth.ToDateTime();
		}

		private void OK_Button_Click(object sender, RoutedEventArgs e)
		{
			var newNameday = new Nameday(Name.Text, new DayMonth(Date.SelectedDate.Value.Day, Date.SelectedDate.Value.Month));
			namedays.UpdateNameday(oldNameday, newNameday);
			this.Close();
		}
		private void Cancel_Button_Click(object sender, RoutedEventArgs e)
		{ 
			this.Close();
		}
	}
}
