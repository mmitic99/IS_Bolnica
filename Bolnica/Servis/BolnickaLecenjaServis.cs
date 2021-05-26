using Bolnica.Repozitorijum;
using Bolnica.Repozitorijum.XmlSkladiste;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        }
        public void RemoveByID(string jmbgPacijenta)
        {
                SkladisteBolnickihLecenjaXml.GetInstance().RemoveByID(jmbgPacijenta);  
        }
    }
}
