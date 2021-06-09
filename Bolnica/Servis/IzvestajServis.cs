using Bolnica.model;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Tables;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bolnica.Repozitorijum.ISkladista;
using Bolnica.Repozitorijum.XmlSkladiste;
using Servis;
using Syncfusion.Pdf.Graphics;

namespace Bolnica.Servis
{
    class IzvestajServis
    {
        private TerminServis TerminServis;
        public IzvestajServis()
        {
            TerminServis = new TerminServis();
        }
        internal void KreirajIzvestajOPregledimaIOperacijama(DateTime pocetakIntervala, DateTime krajIntervala, string jmbgPacijenta)
        {
            using (PdfDocument Document = new PdfDocument())
            {
                PdfPage Page = Document.Pages.Add();
                PdfGraphics graphics = Page.Graphics;
                PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 12);
                string title = "Uvid u zakazane operacije i preglede od " + pocetakIntervala.ToString("dd.MM.yyyy.") + " do " + krajIntervala.ToString("dd.MM.yyyy.");
                graphics.DrawString(title, font, PdfBrushes.Black, new PointF(0,0));
                PdfLightTable pdfLightTable = new PdfLightTable();
                DataTable table = new DataTable();
                table.TableName = "Prikaz pregleda i operacija u proteklih mesec dana";
                table.Columns.Add("Datum");
                table.Columns.Add("Tip termina");
                table.Columns.Add("Lekar");
                table.Columns.Add("Trajanje termina");
                table.Rows.Add(new string[] { "Datum i vreme početka", "Tip termina", "Lekar", "Trajanje termina" });
                foreach (Termin termin in TerminServis.GetByJmbgPacijentaVremenskiPeriod(pocetakIntervala, krajIntervala, jmbgPacijenta) )
                {
                   table.Rows.Add(new string[] { termin.DatumIVremeTermina.ToString("dd.MM.yyyy HH:mm"), termin.VrstaTermina.ToString(), termin.lekar, termin.TrajanjeTermina.ToString() });        
                }
                pdfLightTable.DataSource = table;
                pdfLightTable.Draw(Page, new PointF(0, 20));
                Document.Save("..\\..\\SkladistePodataka\\IzvestajPregleda - pregledi.pdf");
                Document.Close(true);
            }
        }
    }
}
