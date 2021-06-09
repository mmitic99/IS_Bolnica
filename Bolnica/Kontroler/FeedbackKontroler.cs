using System;
using System.Collections.Generic;
using Bolnica.DTOs;
using Bolnica.model;
using Bolnica.Servis;
using Model;

namespace Bolnica.Kontroler
{
    public class FeedbackKontroler
    {
        private FeedbackServis FeedbackServis;

        public FeedbackKontroler()
        {
            FeedbackServis = new FeedbackServis();
        }

        public List<FeedbackDTO> GetAll()
        {
            List<FeedbackDTO> feedbacks = new List<FeedbackDTO>();
            foreach (Feedback feedback in FeedbackServis.GetAll())
            {
                feedbacks.Add(new FeedbackDTO()
                {
                    DatumIVreme = feedback.DatumIVreme,
                    JmbgKorisnika = feedback.JmbgKorisnika,
                    Ocena = feedback.Ocena,
                    Sadrzaj = feedback.Sadrzaj
                });
            }

            return feedbacks;
        }

        public bool Save(FeedbackDTO feedback)
        {
            bool uspesno = false;
            if (DaLiJePopunjenFeedback(feedback))
            {
                uspesno = FeedbackServis.Save(new Feedback()
                {
                    DatumIVreme = feedback.DatumIVreme,
                    JmbgKorisnika = feedback.JmbgKorisnika,
                    Ocena = feedback.Ocena,
                    Sadrzaj = feedback.Sadrzaj
                });
            }
            return uspesno;
        }

        private bool DaLiJePopunjenFeedback(FeedbackDTO feedback)
        {
            return feedback.Ocena != -1;
        }

        public void SaveAll(List<FeedbackDTO> feedbacksDto)
        {
            List<Feedback> feedbacks = new List<Feedback>();
            foreach (FeedbackDTO feedback in feedbacksDto)
            {
                feedbacks.Add(new Feedback()
                {
                    DatumIVreme = feedback.DatumIVreme,
                    JmbgKorisnika = feedback.JmbgKorisnika,
                    Ocena = feedback.Ocena,
                    Sadrzaj = feedback.Sadrzaj
                });
            }
            FeedbackServis.SaveAll(feedbacks);
        }
    }
}