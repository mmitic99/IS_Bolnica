using Bolnica.model;
using Repozitorijum;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Tables;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bolnica.Servis
{
    class IzvestajServis
    {
        private ISkladisteZaTermine skladisteZaTermine;

        public IzvestajServis()
        {
            skladisteZaTermine = Repozitorijum.XmlSkladiste.SkladisteZaTermineXml.getInstance();
        }
        internal void KreirajIzvestajOPregledimaIOperacijama(string jmbgPacijenta)
        {
            using (PdfDocument Document = new PdfDocument())
            {
                PdfPage Page = Document.Pages.Add();
                PdfLightTable pdfLightTable = new PdfLightTable();
                DataTable table = new DataTable();
                table.TableName = "Prikaz pregleda i operacija u proteklih mesec dana";
                table.Columns.Add("Datum");
                table.Columns.Add("Tip termina");
                table.Columns.Add("Lekar");
                table.Columns.Add("Trajanje termina");
                table.Rows.Add(new string[] { "Datum i vreme početka", "Tip termina", "Lekar", "Trajanje termina" });
                foreach (Termin termin in skladisteZaTermine.GetByJmbgPacijenta(jmbgPacijenta))
                {
                    if (termin.DatumIVremeTermina > DateTime.Today.AddMonths(-1) && termin.DatumIVremeTermina<DateTime.Today)
                    {
                        table.Rows.Add(new string[] { termin.DatumIVremeTermina.ToString("dd.MM.yyyy HH:mm"), termin.VrstaTermina.ToString(), termin.lekar, termin.TrajanjeTermina.ToString() });
                    }
                }
                pdfLightTable.DataSource = table;
                pdfLightTable.Draw(Page, new PointF(0, 0));
                Document.Save("..\\..\\SkladistePodataka\\IzvestajPregleda - pregledi.pdf");
                Document.Close(true);
            }
        }
    }
}
