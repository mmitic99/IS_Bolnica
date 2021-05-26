using Bolnica.DTOs;
using Bolnica.model;
using Bolnica.Repozitorijum.XmlSkladiste;
using Bolnica.Servis;
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
                    jmbgLekara =  bolnickoLecenje.jmbgLekara,
                    jmbgPacijenta = bolnickoLecenje.jmbgPacijenta,
                    idProstorije = bolnickoLecenje.idProstorije

                } ) ; 
            }
            return bolnickaLecenjaDTO;
        }
      
        public void Save(BolnickoLecenjeDTO bolnickoLecenjeDTO)
        {
            BolnickoLecenje bolnickoLecenje= new BolnickoLecenje()
            {
                idProstorije = bolnickoLecenjeDTO.idProstorije,
                jmbgLekara = bolnickoLecenjeDTO.jmbgLekara,
                jmbgPacijenta = bolnickoLecenjeDTO.jmbgPacijenta,
                krajBolnickogLecenja = bolnickoLecenjeDTO.DatumOtpustanja,
                pocetakBolnickogLecenja = bolnickoLecenjeDTO.DatumPrijema


            };
            SkladisteBolnickihLecenjaXml.GetInstance().Save(bolnickoLecenje);
        }
        public void SaveAll(List<BolnickoLecenjeDTO> bolnickaLecenjaDTO)
        {
            List<BolnickoLecenje> bolnickaLecenja = new List<BolnickoLecenje>();
            foreach(BolnickoLecenjeDTO bolnickoLecenjeDTO in bolnickaLecenjaDTO)
            {
                BolnickoLecenje bolnickoLecenje = new BolnickoLecenje()
                {
                    idProstorije = bolnickoLecenjeDTO.idProstorije,
                    jmbgLekara = bolnickoLecenjeDTO.jmbgLekara,
                    jmbgPacijenta = bolnickoLecenjeDTO.jmbgPacijenta,
                    krajBolnickogLecenja = bolnickoLecenjeDTO.DatumOtpustanja,
                    pocetakBolnickogLecenja = bolnickoLecenjeDTO.DatumPrijema
                };
                bolnickaLecenja.Add(bolnickoLecenje);
            }
            SkladisteBolnickihLecenjaXml.GetInstance().SaveAll(bolnickaLecenja);
        }
        public BolnickoLecenjeDTO nadjiPoJmbgPacijenta(String jmbgIzabranogPacijenta)
        {
            BolnickoLecenjeDTO novoBolnickaLecenjaDTo = new BolnickoLecenjeDTO();
            foreach (BolnickoLecenjeDTO bolnickoLecenjeDTO in BolnickaLecenjaKontroler.GetInstance().GetAll())
            {
                if (bolnickoLecenjeDTO.jmbgPacijenta != null)
                {
                    if (bolnickoLecenjeDTO.jmbgPacijenta.Equals(jmbgIzabranogPacijenta))
                    {
                        novoBolnickaLecenjaDTo = bolnickoLecenjeDTO;
                        break;
                    }
                }
            }
            return novoBolnickaLecenjaDTo;
        }
        internal void RemoveSelected(String selectedItem)
        {
           // BolnickoLecenje SelektovanoLecenje = (BolnickoLecenje)selectedItem;
            BolnickaLecenjaServis.GetInstance().RemoveByID(selectedItem);
        }
        public void OtpustanjePacijenata()
        {
            
            List<BolnickoLecenjeDTO> bolnickaLecenjaDTO = BolnickaLecenjaKontroler.GetInstance().GetAll();
            foreach (BolnickoLecenjeDTO bolnickoLecenjeDTO in bolnickaLecenjaDTO)
            {
                if (bolnickoLecenjeDTO.DatumOtpustanja.CompareTo(DateTime.Today) < 0)
                {
                    bolnickaLecenjaDTO.Remove(bolnickoLecenjeDTO);
                    BolnickaLecenjaKontroler.GetInstance().SaveAll(bolnickaLecenjaDTO);
                    break;
                }
            }
        }

    }
}
