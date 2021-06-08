/*
    Stoyan Dimitrov
    
    May 2016
*/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Linq;

namespace MyCalendar.Calendar.TestApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            List<string> months = new List<string> {"January", "February", "March", "April", "May","June", "July", "August", "September", "October", "November", "December"};
            cboMonth.ItemsSource = months;

            for (int i = -50; i < 50; i++)
            {
                cboYear.Items.Add(DateTime.Today.AddYears(i).Year);
            }

            cboMonth.SelectedItem = months.FirstOrDefault(w => w == DateTime.Today.ToString("MMMM"));
            cboYear.SelectedItem = DateTime.Today.Year;

            cboMonth.SelectionChanged += (o, e) => RefreshCalendar();
            cboYear.SelectionChanged += (o, e) => RefreshCalendar();            

            getNotesFromDb();
        }
        
        private void getNotesFromDb()
        {
            DaysDBEntities ctx = new DaysDBEntities();

            var results = (from d in ctx.Days select d);

            List<Days> days = results.ToList();

            foreach (Days dbDay in days)
            {
                foreach (Day calendarDay in Calendar.Days)
                {
                    if (calendarDay.Date == dbDay.date)
                    {
                        calendarDay.Notes = dbDay.notes;
                    }
                }
            }
        }

        private void RefreshCalendar()
        {
            if (cboYear.SelectedItem == null) return;
            if (cboMonth.SelectedItem == null) return;

            int year = (int)cboYear.SelectedItem;
            
            int month = cboMonth.SelectedIndex + 1;
            
            DateTime targetDate = new DateTime(year,month,1);
            
            Calendar.BuildCalendar(targetDate);

            getNotesFromDb();
        }
        
        private void Calendar_DayChanged(object sender, DayChangedEventArgs e)
        {
            DaysDBEntities ctx = new DaysDBEntities();

            var results = (from d in ctx.Days where d.date == e.Day.Date select d);

            if (results.Count() <= 0)
            {
                Days newDay = new Days();
                newDay.date = e.Day.Date;
                newDay.notes = e.Day.Notes;

                ctx.Days.Add(newDay);
                ctx.SaveChanges();
            }
            else
            {
                Days oldDay = results.FirstOrDefault();
                oldDay.notes = e.Day.Notes;
                ctx.SaveChanges();
            }
        }
    }
}