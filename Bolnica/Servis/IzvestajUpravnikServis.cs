using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Bolnica.model;
using Bolnica.Repozitorijum.XmlSkladiste;
using Bolnica.Template_class;
using Bolnica.Validacije;
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
    class IzvestajUpravnikServis : DefaultIzvestaj
    {
        public override void GenerisiDokument(DateTime pocetak, DateTime kraj)
        {
            SacuvajIzvestajPreraspodela(pocetak,kraj);
            SacuvajIzvestajRenoviranja(pocetak, kraj);
            SacuvajIzvestajNaprednihRenoviranja(pocetak, kraj);
            SacuvajIzvestajTermina(pocetak, kraj);
    
        }

        public override void PrikaziObavestenje()
        {
            ValidacijaContext validacija = new ValidacijaContext(new SelekcijaStrategy());
            validacija.IspisiGresku(18);
        }
        private void SacuvajIzvestajPreraspodela(DateTime pocetak, DateTime kraj)
        {
            using (PdfDocument Document = new PdfDocument())
            {
                PdfPage Page = Document.Pages.Add();
                PdfGraphics Graphics = Page.Graphics;
                PdfFont Font = new PdfStandardFont(PdfFontFamily.Helvetica, 16);
                Graphics.DrawString("Izvestaj zauzetosti prostorije - PRERASPODELE OPREME", Font, PdfBrushes.Black, new PointF(58, 20));
                PdfLightTable PdfLightTable = new PdfLightTable();
                DataTable Table = new DataTable();
                Table.Columns.Add("Prostrija iz koje se prenosi oprema");
                Table.Columns.Add("Prostorija u koju se prenosi oprema");
                Table.Columns.Add("Datum i vreme početka preraspodele");
                Table.Columns.Add("Trajanje preraspodele");
                Table.Rows.Add(new string[] { "Prostorija iz koje se prenosi oprema", "Prostorija iz koje se prenosi oprema", "Datum i vreme početka preraspodele", "Trajanje preraspodele" });
                foreach (ZakazanaPreraspodelaStatickeOpreme preraspodela in SkladisteZaZakazanuPreraspodeluStatickeOpremeXml.GetInstance().GetAll())
                {
                    if (DateTime.Compare(preraspodela.DatumIVremePreraspodele.Date, pocetak) > 0 && DateTime.Compare(preraspodela.DatumIVremePreraspodele.Date, kraj) <= 0)
                        Table.Rows.Add(new string[] { preraspodela.BrojProstorijeIzKojeSePrenosiOprema, preraspodela.BrojProstorijeUKojuSePrenosiOprema, preraspodela.DatumIVremePreraspodele.ToString(), "60" });
                }
                PdfLightTable.DataSource = Table;
                PdfLightTable.Draw(Page, new PointF(0, 70));
                Document.Save("..\\..\\SkladistePodataka\\Izvestaj o zauzetosti prostorija - preraspodele.pdf");
                Document.Close(true);
            }
        }

        private void SacuvajIzvestajRenoviranja(DateTime pocetak, DateTime kraj)
        {
            using (PdfDocument Document = new PdfDocument())
            {
                PdfPage Page = Document.Pages.Add();
                PdfGraphics Graphics = Page.Graphics;
                PdfFont Font = new PdfStandardFont(PdfFontFamily.Helvetica, 16);
                Graphics.DrawString("Izvestaj zauzetosti prostorije - RENOVIRANJA", Font, PdfBrushes.Black, new PointF(93, 20));
                PdfLightTable PdfLightTable = new PdfLightTable();
                DataTable Table = new DataTable();
                Table.Columns.Add("Prostorija");
                Table.Columns.Add("Sprat");
                Table.Columns.Add("Datum i vreme početka renoviranja");
                Table.Columns.Add("Datum i vreme završetka renoviranja");
                Table.Rows.Add(new string[] { "Prostorija", "Sprat", "Datum i vreme početka renoviranja", "Datum i vreme kraja renoviranja" });
                foreach (Renoviranje renoviranje in SkladisteZaRenoviranjaXml.GetInstance().GetAll())
                {
                    if (DateTime.Compare(renoviranje.DatumPocetkaRenoviranja.Date, pocetak) > 0 && DateTime.Compare(renoviranje.DatumPocetkaRenoviranja.Date, kraj) <= 0)
                        Table.Rows.Add(new string[] { renoviranje.BrojProstorije, renoviranje.Sprat.ToString(), renoviranje.DatumPocetkaRenoviranja.ToString(), renoviranje.DatumZavrsetkaRenoviranja.ToString() });
                    if (DateTime.Compare(renoviranje.DatumZavrsetkaRenoviranja.Date, pocetak) > 0 && DateTime.Compare(renoviranje.DatumZavrsetkaRenoviranja.Date, kraj) <= 0)
                        Table.Rows.Add(new string[] { renoviranje.BrojProstorije, renoviranje.Sprat.ToString(), renoviranje.DatumPocetkaRenoviranja.ToString(), renoviranje.DatumZavrsetkaRenoviranja.ToString() });
                }
                PdfLightTable.DataSource = Table;
                PdfLightTable.Draw(Page, new PointF(0, 70));
                Document.Save("..\\..\\SkladistePodataka\\Izvestaj o zauzetosti prostorija - renoviranja.pdf");
                Document.Close(true);
            }
        }

        private void SacuvajIzvestajNaprednihRenoviranja(DateTime pocetak, DateTime kraj)
        {
            using (PdfDocument Document = new PdfDocument())
            {
                PdfPage Page = Document.Pages.Add();
                PdfGraphics Graphics = Page.Graphics;
                PdfFont Font = new PdfStandardFont(PdfFontFamily.Helvetica, 16);
                Graphics.DrawString("Izvestaj zauzetosti prostorije - NAPREDNA RENOVIRANJA", Font, PdfBrushes.Black, new PointF(40, 20));
                PdfLightTable PdfLightTable = new PdfLightTable();
                DataTable Table = new DataTable();
                Table.Columns.Add("Glavna prostorija");
                Table.Columns.Add("Prostorija 1");
                Table.Columns.Add("Prostorija 2");
                Table.Columns.Add("Datum i vreme početka renoviranja");
                Table.Columns.Add("Datum i vreme završetka renoviranja");
                Table.Rows.Add(new string[] { "Glavna prostorija", "Prostorija 1", "Prostorija 2", "Datum i vreme početka renoviranja", "Datum i vreme kraja renoviranja" });
                foreach (NaprednoRenoviranje renoviranje in SkladisteZaNaprednaRenoviranjaXml.GetInstance().GetAll())
                {
                    if (DateTime.Compare(renoviranje.DatumPocetkaRenoviranja.Date, pocetak) > 0 && DateTime.Compare(renoviranje.DatumPocetkaRenoviranja.Date, kraj) <= 0)
                        Table.Rows.Add(new string[] { renoviranje.BrojGlavneProstorije, renoviranje.BrojProstorije1, renoviranje.BrojProstorije2, renoviranje.DatumPocetkaRenoviranja.ToString(), renoviranje.DatumZavrsetkaRenoviranja.ToString() });
                    if (DateTime.Compare(renoviranje.DatumZavrsetkaRenoviranja.Date, pocetak) > 0 && DateTime.Compare(renoviranje.DatumZavrsetkaRenoviranja.Date, kraj) <= 0)
                        Table.Rows.Add(new string[] { renoviranje.BrojGlavneProstorije, renoviranje.BrojProstorije1, renoviranje.BrojProstorije2, renoviranje.DatumPocetkaRenoviranja.ToString(), renoviranje.DatumZavrsetkaRenoviranja.ToString() });
                }
                PdfLightTable.DataSource = Table;
                PdfLightTable.Draw(Page, new PointF(0, 70));
                Document.Save("..\\..\\SkladistePodataka\\Izvestaj o zauzetosti prostorija - napredna renoviranja.pdf");
                Document.Close(true);
            }
        }
        private void SacuvajIzvestajTermina(DateTime pocetak, DateTime kraj)
        {
            using (PdfDocument Document = new PdfDocument())
            {
                PdfPage Page = Document.Pages.Add();
                PdfLightTable PdfLightTable = new PdfLightTable();
                DataTable Table = new DataTable();
                Table.TableName = "TABELA ZAUZETOSTI PROSTORIJA ZA NAREDNIH 7 DANA - TERMINI";
                Table.Columns.Add("Prostorija");
                Table.Columns.Add("Datum i vreme početka termina");
                Table.Columns.Add("Trajanje termina");
                Table.Rows.Add(new string[] { "Prostorija", "Datum i vreme pocetka termina", "Trajanje termina" });
                foreach (Termin termin in SkladisteZaTermineXml.getInstance().GetAll())
                {
                    if (DateTime.Compare(termin.DatumIVremeTermina.Date, pocetak) > 0 && DateTime.Compare(termin.DatumIVremeTermina.Date, kraj) <= 0)
                        Table.Rows.Add(new string[] { termin.brojSobe, termin.DatumIVremeTermina.ToString(), "30" });
                }
                PdfLightTable.DataSource = Table;
                PdfLightTable.Draw(Page, new PointF(0, 0));
                Document.Save("..\\..\\SkladistePodataka\\Izvestaj o zauzetosti prostorija - termini.pdf");
                Document.Close(true);
            }
        }
    }
}
