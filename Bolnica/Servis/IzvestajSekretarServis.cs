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
using Bolnica.view.LekarView;
using Bolnica.viewActions;
using Model;
using Servis;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Tables;
using Syncfusion.XPS;

namespace Bolnica.Servis
{
    class IzvestajSekretarServis : DefaultIzvestaj
    {
        public override void GenerisiDokument(DateTime pocetak, DateTime kraj)
        {
            using (PdfDocument Document = new PdfDocument())
            {

                PdfPage Page = Document.Pages.Add();
                PdfGraphics Graphics = Page.Graphics;
                PdfFont Font = new PdfStandardFont(PdfFontFamily.Helvetica, 12);
                Graphics.DrawString("Zakazani pregledi i operacije u periodu: " + pocetak.Date.ToString("dd.MM.yyyy") +
                                             " - " + kraj.Date.ToString("dd.MM.yyyy"), Font, PdfBrushes.Black, new PointF(113, 20));
                PdfLightTable PdfLightTable = new PdfLightTable();
                DataTable Table = new DataTable();

                Table.TableName = "Zakazani pregledi i operacije u periodu";
                Table.Columns.Add("Datum i vreme početka");
                Table.Columns.Add("Vrsta");
                Table.Columns.Add("Pacijent");
                Table.Columns.Add("Lekar");
                Table.Columns.Add("Broj sobe");

                Table.Rows.Add(new string[] { "Datum i vreme početka", "Vrsta", "Pacijent", "Lekar", "Broj sobe" });

                foreach (Termin termin in GetTermineUIntervalu(pocetak, kraj))

                    Table.Rows.Add(new string[] { termin.DatumIVremeTermina.ToString("dd.MM.yyyy HH:mm"), termin.VrstaTermina.ToString(), termin.pacijent, termin.lekar, termin.brojSobe });
                PdfLightTable.DataSource = Table;
                PdfLightTable.Draw(Page, new PointF(0, 70));
                Document.Save("..\\..\\..\\IzvestajSekretar.pdf");
                Document.Close(true);

            }
        }

        public override void PrikaziObavestenje()
        {
            MessageBox.Show("Uspesno kreiran izvestaj!");
        }

        private List<Termin> GetTermineUIntervalu(DateTime datumPocetka, DateTime datumZavrsetka)
        {
            List<Termin> termini = new List<Termin>();
            foreach (Termin termin in TerminServis.getInstance().GetAll())
            {
                if (termin.DatumIVremeTermina >= datumPocetka && termin.DatumIVremeTermina <= datumZavrsetka)
                {
                    termini.Add(termin);
                }
            }
            return termini.OrderBy(termin => termin.DatumIVremeTermina).ToList();
        }
   } 
}
