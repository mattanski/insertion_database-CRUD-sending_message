using System;
using System.Linq.Expressions;
using PrimoEsempio.Agenda;

namespace PrimoEsempio
{
    public class Messaggio
    {
        public string Testo { get; private set; }
        public string Mittente { get; private set; }
        public string Oggetto { get; private set; }
        public string Tipo { get; private set; }
        
        public Messaggio(string testo, string mittente, string oggetto, string tipo)
        {
            Testo = testo;
            Mittente = mittente;
            Oggetto = oggetto;
            if (tipo!="sms" && tipo!="mail" && tipo!="whatsapp")
            {
                throw new Exception("Il tipo deve essere sms, mail o whatsapp");
            }
            Tipo = tipo; 
        }

        public void Invia(Contatto contatto)
        {
            switch (Tipo)
            {
                case "sms":
                    Console.WriteLine($"Invio sms a {contatto.Tel} con testo {Testo}");
                    break;
                case "mail":
                    Console.WriteLine($"Invio mail a {contatto.Mail} con testo {Testo} e oggetto {Oggetto}");
                    break;
                case "whatsapp":
                    Console.WriteLine($"Invio whatsapp a {contatto.Tel} con testo {Testo}");
                    break;
            }
        }
    }
}