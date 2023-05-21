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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Uniza.Namedays;

namespace EditorGuiApp
{
    /// <summary>
    /// Interaction logic for CalendarUserControl.xaml
    /// </summary>
    public partial class CalendarUserControl : UserControl
    {
        public NamedayCalendar? Namedays { get; set; }

        public CalendarUserControl()
        {
            InitializeComponent();
            
        }



        private void BtnToday_Click(object sender, RoutedEventArgs e)
        {
              DisplayNamedays(DateTime.Today);
        }

        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            var calendar = sender as Calendar;

            if (calendar.SelectedDate.HasValue)
            {
                DisplayNamedays(calendar.SelectedDate.Value);
            }
        }

        public void DisplayNamedays(DateTime date)
        {
            lstNames.Items.Clear();
            selectedDateText.Text = $"{date.ToShortDateString()} celebrates:";

            if (Namedays != null)
            {
                var dateNamedays = string.Join(", ", Namedays[date]);
                lstNames.Items.Add(dateNamedays);
            }
        }
    }
}



