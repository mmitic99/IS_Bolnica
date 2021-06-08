using System;
using System.Collections.Generic;
using Telerik.Windows.Controls;

namespace CalendarAndViewModel
{
	public class SampleViewModel : ViewModelBase
	{
		private IEnumerable<DateTime> _SelectedDates;
		public IEnumerable<DateTime> SelectedDates
		{
			get
			{
				return this._SelectedDates;
			}
			set
			{
				if (this._SelectedDates != value)
				{
					this._SelectedDates = value;
					this.OnPropertyChanged("SelectedDates");
				}
			}
		}		
	}
}
