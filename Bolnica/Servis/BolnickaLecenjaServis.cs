using System.Collections.Generic;
using Bolnica.model;
using Bolnica.Repozitorijum;
using Bolnica.Repozitorijum.XmlSkladiste;

namespace Bolnica.Servis
{
    public class BolnickaLecenjaServis
    {
        private static BolnickaLecenjaServis instance = null;


        public static BolnickaLecenjaServis GetInstance()
        {
            if (instance == null)
            {
                return new BolnickaLecenjaServis();
            }
            else
                return instance;
        }

        public BolnickaLecenjaServis()
        {
            instance = this;
            SkladisteBolnickihLecenja = SkladisteBolnickihLecenjaXml.GetInstance();

        }

        private ISkladisteBolnickihLecenja SkladisteBolnickihLecenja;

        public void RemoveByID(string jmbgPacijenta)
        {
            SkladisteBolnickihLecenja.RemoveById(jmbgPacijenta);
        }

        public List<BolnickoLecenje> GetAll()
        {
            return SkladisteBolnickihLecenja.GetAll();
        }

        public void Save(BolnickoLecenje bolnickoLecenje)
        {
            SkladisteBolnickihLecenja.Save(bolnickoLecenje);
        }

        public void SaveAll(List<BolnickoLecenje> bolnickaLecenja)
        {
            SkladisteBolnickihLecenja.SaveAll(bolnickaLecenja);
        }
    }
}
