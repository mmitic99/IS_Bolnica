using Model;
using Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bolnica.DTOs
{
    class DataTransferObjects
    {
    }

    public class ParametriZaTermineUIntervaluDTO
    {
        public List<DateTime> SviDani { get; set; }
        public TimeSpan Pocetak { get; set; }
        public TimeSpan Kraj { get; set; }
        public String jmbgPacijenta { get; set; }
        public String OpisTegobe { get; set; }
        public String jmbgLekara { get; set; }
        public int Trajanje { get; set; }
        public VrstaPregleda vrstaPregleda { get; set; }
    }

    public class ParametriZaNalazenjeTerminaZaTacnoVreme
    {
        public DateTime TacnoVreme { get; set; }
        public int trajanjeUMinutama { get; set; }
        public String JmbgLekara { get; set; }
        public VrstaPregleda vrstaPregleda { get; set; }
    }

    public class ParamsToCheckAvailabilityOfRoomDTO
    {
        public String IDRoom { get; set; }
        public VrstaPregleda InterventionType { get; set; }
        public DateTime startTime { get; set; }
        public int durationInMinutes { get; set; }

        public ParamsToCheckAvailabilityOfRoomDTO(VrstaPregleda InterventionType, DateTime startTime, int duration)
        {
            this.InterventionType = InterventionType;
            this.startTime = startTime;
            this.durationInMinutes = duration;

        }
    }

    public class ParamsToCheckAvailabilityOfDoctorDTO
    {
        public String IDDoctor { get; set; }
        public DateTime startTime { get; set; }
        public int durationInMinutes { get; set; }

        public ParamsToCheckAvailabilityOfDoctorDTO(String ID, DateTime startTime, int duration)
        {
            this.IDDoctor = ID;
            this.startTime = startTime;
            this.durationInMinutes = duration;

        }
    }
    public class ParamsToCheckAvailabilityOfPatientDTO
    {
        public String Id { get; set; }
        public DateTime startTime { get; set; }
        public int durationInMinutes { get; set; }
        public ParamsToCheckAvailabilityOfPatientDTO(String ID, DateTime startTime, int duration)
        {
            this.Id = ID;
            this.startTime = startTime;
            this.durationInMinutes = duration;

        }
    }

    public class ParametriZaTrazenjeTerminaKlasifikovanoDTO
    {
        public String JmbgLekara { get; set; }
        public String JmbgPacijenta { get; set; }
        public List<DateTime> IzabraniDani { get; set; }
        public List<DateTime> SviMoguciDaniZakazivanja { get; set; }
        public TimeSpan Pocetak { get; set; }
        public TimeSpan Kraj { get; set; }
        public int Prioritet { get; set; }
        public String tegobe { get; set; }
        public Termin PrethodnoZakazaniTermin { get; set; }
        public int trajanjeUMinutama { get; set; }
        public VrstaPregleda vrstaTermina { get; set; }
    }

    public class ParamsToPickAppointmentsAppropriateForSpecificPatientDTO
    {
        public String Id { get; set; }
        public VrstaPregleda AppointmentKind { get; set; }
        public List<Termin> PossibleAppointmentsTimings { get; set; }
        public String SymptomsOfIllness { get; set; }

        public ParamsToPickAppointmentsAppropriateForSpecificPatientDTO(string id, VrstaPregleda appointmentKind, List<Termin> possibleAppointmentsTimings, string symptomsOfIllness)
        {
            Id = id;
            AppointmentKind = appointmentKind;
            PossibleAppointmentsTimings = possibleAppointmentsTimings;
            SymptomsOfIllness = symptomsOfIllness;
        }
    }

    public class ParametriZaTrazenjeMogucihTerminaDTO
    {
        public Object Pacijent { get; set; }
        public Object PrethodnoZakazaniTermin { get; set; }
        public Object IzabraniLekar { get; set; }
        public Object IzabraniDatumi { get; set; }
        public Object SviMoguciDatumi { get; set; }
        public Object PocetnaSatnica { get; set; }
        public Object PocetakMinut { get; set; }
        public Object KrajnjaSatnica { get; set; }
        public Object KrajnjiMinuti { get; set; }
        public Object NemaPrioritet { get; set; }
        public Object PrioritetLekar { get; set; }
        public Object PriotitetVreme { get; set; }
        public Object OpisTegobe { get; set; }
        public Object trajanjeUMinutama { get; set; }
        public Object vrstaTermina { get; set; }
        public Object brojTermina { get; set; }
    }

}
