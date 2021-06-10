using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Bolnica.model;
using Bolnica.Template_class;
using Bolnica.Validacije;
using Bolnica.viewActions;
using Model;
using Servis;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Tables;

namespace Bolnica.Servis
{
    class IzvestajPacijentServis : DefaultIzvestaj
    {
        public override void GenerisiDokument(DateTime pocetak, DateTime kraj)
        {
            using (PdfDocument Document = new PdfDocument())
            {
                PdfPage Page = Document.Pages.Add();
                PdfGraphics graphics = Page.Graphics;
                PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 12);
                string title = "Uvid u zakazane operacije i preglede od " + pocetak.ToString("dd.MM.yyyy.") + " do " + kraj.ToString("dd.MM.yyyy.");
                graphics.DrawString(title, font, PdfBrushes.Black, new PointF(0, 0));
                PdfLightTable pdfLightTable = new PdfLightTable();
                DataTable table = new DataTable();
                table.TableName = "Prikaz pregleda i operacija u proteklih mesec dana";
                table.Columns.Add("Datum");
                table.Columns.Add("Tip termina");
                table.Columns.Add("Lekar");
                table.Columns.Add("Trajanje termina");
                table.Rows.Add(new string[] { "Datum i vreme početka", "Tip termina", "Lekar", "Trajanje termina" });
                Pacijent pacijent = PacijentServis.GetInstance().GetByJmbg(MainViewModel.getInstance().Pacijent.Jmbg);
                foreach (Termin termin in TerminServis.getInstance().GetByJmbgPacijentaVremenskiPeriod(pocetak, kraj, pacijent.Jmbg))
                {
                    table.Rows.Add(new string[] { termin.DatumIVremeTermina.ToString("dd.MM.yyyy HH:mm"), termin.VrstaTermina.ToString(), termin.lekar, termin.TrajanjeTermina.ToString() });
                }
                pdfLightTable.DataSource = table;
                pdfLightTable.Draw(Page, new PointF(0, 20));
                Document.Save("..\\..\\SkladistePodataka\\IzvestajPregleda - pregledi.pdf");
                Document.Close(true);
            }
        }

        public override void PrikaziObavestenje()
        {
            ValidacijaContext validacija = new ValidacijaContext(new SelekcijaStrategy());
            validacija.IspisiGresku(18);
        }
    }
}
