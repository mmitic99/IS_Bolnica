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

namespace Bolnica.Servis
{
    class IzvestajLekarServis : DefaultIzvestaj
    {
        public override void GenerisiDokument(DateTime pocetak, DateTime kraj)
        {
            SacuvajIzvestajPacijenta(pocetak,kraj);
            SacuvajReceptIDijagnozujPacijenta(pocetak, kraj);
        }

        public override void PrikaziObavestenje()
        {
            MessageBox.Show("Uspesno kreiran izvestaj!");
        }
        private void SacuvajIzvestajPacijenta(DateTime pocetak,DateTime kraj)
        {
            using (PdfDocument Document = new PdfDocument())
            {
                Pacijent pacijent = PacijentServis.GetInstance().GetByJmbg(PacijentInfoPage.getInstance().Jmbg);
                PdfPage Page = Document.Pages.Add();
                PdfGraphics Graphics = Page.Graphics;
                PdfFont Font = new PdfStandardFont(PdfFontFamily.Helvetica, 12);
                Graphics.DrawString(pacijent.FullName + "-Izvestaj o stanju pacijenta - anamneze", Font, PdfBrushes.Black, new PointF(113, 20));
                PdfLightTable PdfLightTable = new PdfLightTable();
                DataTable Table = new DataTable();

                Table.TableName = "IZVEŠTAJ O STANJU PACIJENTA U PROŠLIH 7 DANA.";
                Table.Columns.Add("Datum pregleda");
                Table.Columns.Add("Anamneza");
                Table.Rows.Add(new string[] { "Datum pregleda", "Anamneza" });

                foreach (Anamneza anamneza in pacijent.ZdravstveniKarton.Anamneze)
                {
                    if (DateTime.Compare(anamneza.DatumAnamneze.Date, pocetak) <= 0 && (DateTime.Compare(anamneza.DatumAnamneze.Date, kraj) >= 0))
                    {

                        Table.Rows.Add(new string[] { anamneza.DatumAnamneze.ToShortDateString(), anamneza.AnamnezaDijalog });


                    }
                }
                PdfLightTable.DataSource = Table;
                PdfLightTable.Draw(Page, new PointF(0, 70));
                Document.Save("..\\..\\SkladistePodataka\\Izvestaj o stanju pacijenta-anamneza.pdf");
                Document.Close(true);
            }
        }
            private void SacuvajReceptIDijagnozujPacijenta(DateTime pocetak, DateTime kraj)
            {
                using (PdfDocument Document = new PdfDocument())
                {
                    Pacijent pacijent = PacijentServis.GetInstance().GetByJmbg(PacijentInfoPage.getInstance().Jmbg);
                    PdfPage Page = Document.Pages.Add();
                    PdfGraphics Graphics = Page.Graphics;
                    PdfFont Font = new PdfStandardFont(PdfFontFamily.Helvetica, 12);
                    Graphics.DrawString(pacijent.FullName + "-Izvestaj o stanju pacijenta - terapija i dijagnoza", Font, PdfBrushes.Black, new PointF(113, 20));
                    PdfLightTable PdfLightTable = new PdfLightTable();
                    DataTable Table = new DataTable();

                    Table.TableName = "IZVEŠTAJ O STANJU PACIJENTA U PROŠLIH 7 DANA.";
                    Table.Columns.Add("Datum pregleda");
                    Table.Columns.Add("Dijagnoza");
                    Table.Columns.Add("Terapija");
                    Table.Rows.Add(new string[] { "Datum pregleda", "Dijagnoza", "Terapija" });
                    foreach (Izvestaj izvestaj in pacijent.ZdravstveniKarton.Izvestaj)
                        foreach (Recept recept in izvestaj.recepti)
                        {
                            if (DateTime.Compare(recept.DatumIzdavanja.Date, pocetak) <= 0 && (DateTime.Compare(recept.DatumIzdavanja.Date, kraj) >= 0))
                            {

                                Table.Rows.Add(new string[] { recept.DatumIzdavanja.ToShortDateString(), recept.Dijagnoza, recept.ImeLeka });


                            }
                        }
                    PdfLightTable.DataSource = Table;
                    PdfLightTable.Draw(Page, new PointF(0, 70));
                    Document.Save("..\\..\\SkladistePodataka\\Izvestaj o stanju pacijenta-dijagnoza i terapija.pdf");
                    Document.Close(true);
                }
            }

        
    }
}
