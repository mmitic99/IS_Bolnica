using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CalendarAndViewModel
{
	public partial class MainPage : UserControl
	{
		SampleViewModel viewModel;
		public MainPage()
		{
			InitializeComponent();
			viewModel = this.DataContext as SampleViewModel;
		}

		private void OnSelectManyClick(object sender, RoutedEventArgs e)
		{
			viewModel.SelectedDates = Enumerable.Range(1, 5).Select(i => DateTime.Today.AddDays(i));
		}

		private void OnDeselectAllClick(object sender, RoutedEventArgs e)
		{
			viewModel.SelectedDates = Enumerable.Empty<DateTime>();
		}
	}
}
