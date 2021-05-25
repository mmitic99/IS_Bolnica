using Bolnica.DTOs;
using Bolnica.model;
using Bolnica.Repozitorijum.XmlSkladiste;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bolnica.Kontroler
{
    public class BolnickaLecenjaKontroler
    {
        private static BolnickaLecenjaKontroler instance = null;

        public static BolnickaLecenjaKontroler GetInstance()
        {
            if (instance == null)
            {
                return new BolnickaLecenjaKontroler();
            }
            else
                return instance;
        }

        public BolnickaLecenjaKontroler()
        {
            instance = this;
        }
        public List<BolnickoLecenjeDTO> GetAll()
        {
            List<BolnickoLecenje> bolnickaLecenja= SkladisteBolnickihLecenjaXml.GetInstance().GetAll();
            List<BolnickoLecenjeDTO> bolnickaLecenjaDTO = new List<BolnickoLecenjeDTO>();
            foreach (BolnickoLecenje bolnickoLecenje in bolnickaLecenja)
            {
                bolnickaLecenjaDTO.Add(new BolnickoLecenjeDTO()
                {
                    brojSobe = SkladisteZaProstorijeXml.GetInstance().GetById(bolnickoLecenje.idProstorije).BrojSobe,
                    DatumOtpustanja = bolnickoLecenje.krajBolnickogLecenja,
                    DatumPrijema = bolnickoLecenje.pocetakBolnickogLecenja,
                    imeLekara = SkladisteZaLekaraXml.GetInstance().getByJmbg(bolnickoLecenje.jmbgLekara).FullName,
                    imePacijenta = SkladistePacijentaXml.GetInstance().GetByJmbg(bolnickoLecenje.jmbgPacijenta).FullName

                } ) ; 
            }
            return bolnickaLecenjaDTO;
        }
    }
}
