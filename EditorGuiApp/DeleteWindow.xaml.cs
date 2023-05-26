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

namespace EditorGuiApp
{
	/// <summary>
	/// Interaction logic for DeleteWindow.xaml
	/// </summary>
	public partial class DeleteWindow : Window
	{
		public bool ConfirmDelete { get; private set; }

		public DeleteWindow(string name_)
		{
			InitializeComponent();
			ConfirmDelete = false;
			this.name_ = name_;
		}

		private void YesButton_Click(object sender, RoutedEventArgs e)
		{
			ConfirmDelete = true;
			Close();
		}

		private void NoButton_Click(object sender, RoutedEventArgs e)
		{
			ConfirmDelete = false;
			Close();
		}
	}
}
