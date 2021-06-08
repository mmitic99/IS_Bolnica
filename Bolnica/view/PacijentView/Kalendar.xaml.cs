using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Linq;
using System.Windows.Controls;
using MyCalendar.Calendar;
using Kontroler;
using Bolnica.DTOs;
using Bolnica.viewActions;

namespace Bolnica.view.PacijentView
{
    /// <summary>
    /// Interaction logic for Kalendar.xaml
    /// </summary>
    public partial class Kalendar : UserControl
    {
        private TerminKontroler TerminKontroler;
        public Kalendar()
        {
            InitializeComponent();

            this.TerminKontroler = new TerminKontroler();

            List<string> months = new List<string> { "Januar", "Februar", "Mart", "April", "Maj", "Jun", "Jul", "Avgust", "Septembar", "Oktobar", "Novembar", "Decembar" };
            cboMonth.ItemsSource = months;

            for (int i = -50; i < 50; i++)
            {
                cboYear.Items.Add(DateTime.Today.AddYears(i).Year);
            }

            cboMonth.SelectedItem = months.FirstOrDefault(w => w == konverterMeseca());
            cboYear.SelectedItem = DateTime.Today.Year;

            RefreshCalendar();

            cboMonth.SelectionChanged += (o, e) => RefreshCalendar();
            cboYear.SelectionChanged += (o, e) => RefreshCalendar();

        }

        private string konverterMeseca()
        {
            String mesec = DateTime.Today.ToString("MMMM");
            String povratna = "";
            switch(mesec){
                case "January":
                    povratna = "Januar";
                    break;
                case "February":
                    povratna = "Februar";
                    break;
                case "May":
                    povratna = "Maj";
                    break;
                case "June":
                    povratna = "Jun";
                    break;

            }
            return povratna;
        }

        private void RefreshCalendar()
        {
            if (cboYear.SelectedItem == null) return;
            if (cboMonth.SelectedItem == null) return;

            int year = (int)cboYear.SelectedItem;

            int month = cboMonth.SelectedIndex + 1;

            DateTime targetDate = new DateTime(year, month, 1);
            Dictionary<DateTime, String> notesForTheMonth = TerminKontroler.getNotesForTheMonth(targetDate, MainViewModel.getInstance().JmbgPacijenta);

            Calendar.BuildCalendar(targetDate, notesForTheMonth);

        }

        private void Calendar_DayChanged(object sender, DayChangedEventArgs e)
        {

        }


    }
}
