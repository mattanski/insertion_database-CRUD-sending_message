using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using PrimoEsempio.Agenda;

namespace PrimoEsempio
{
    public class AgendaInterface
    {
        private Agenda.Agenda agenda = new Agenda.Agenda();
        private Contatto _current;
        public  bool VuoiContinuareAdUtilizzareLAgenda()
        {
            Console.WriteLine("Vuoi continuare ad utilizzare l'agenda? (S/N)");
            string result = Console.ReadLine();
            return result == "S";
        }
        public void CreaNuovoContatto(ContattoDto dto)
        {
            agenda.AggiungiContatto(dto);
        }
        
        
        public void CercaEMostraContatti(string nome, string cognome)
        {
            _current = null;
            IReadOnlyList<Contatto> contattiTrovati = agenda.CercaContatto(nome, cognome);

            foreach (var contatto in contattiTrovati)
            {
                Console.WriteLine(contatto.ToString());
            }
        }

        public ContattoDto RichiediDatiContatto()
        {
            Console.WriteLine("Dammi il nome");
            string nome = Console.ReadLine();
            Console.WriteLine("Dammi il cognome");
            string cognome = Console.ReadLine();
            Console.WriteLine("Dammi la mail");
            string mail = Console.ReadLine();
            Console.WriteLine("Dammi il telefono");
            string telefono = Console.ReadLine();

            return new ContattoDto()
            {
                Cognome = cognome,
                Nome = nome,
                Mail = mail,
                Tel = telefono
            };
        }

        public void SelezionaContatto()
        {
            Console.WriteLine("Seleziona un contatto per codice");
            string codice = Console.ReadLine();

            
            _current = agenda.OttieniContattoPerCodice(codice);
            List<Messaggio> messaggi = agenda.OttieniMessaggiPerContatto(codice);
            
            
            Console.WriteLine($"Hai selezionato il contatto {_current.ToString()}");

            foreach (var messaggio in messaggi)
            {
                Console.WriteLine(messaggio.ToString());
            }
        }

        public void EliminaContatto()
        {
            agenda.EliminaContatto(_current.Codice);
        }

        public void AggiornaContatto()
        {
            ContattoDto contattoAggiornato = this.RichiediDatiContatto();
            _current.RinnovaInformazioni(contattoAggiornato.Nome,
                contattoAggiornato.Cognome,
                contattoAggiornato.Mail,
                contattoAggiornato.Tel);
            
            agenda.AggiornaContatto(_current);
            
        }

        public void InviaMessaggio()
        {
            Console.WriteLine("Inserisci Il tipo di messaggio da inviare (sms, mail, whatsapp)");
            string tipo = Console.ReadLine();
            Messaggio messaggio = null;
            switch (tipo)
            {
                case "sms":
                    messaggio = ScriviSms();
                    break;
                case "mail":
                    messaggio = ScriviMail();
                    break;
                case "whatsapp":
                    messaggio = ScriviWhatsapp();
                    break;
            }
            //_current.Invia(messaggio);
            agenda.InviaMessaggio(_current, messaggio);
        }

        private Messaggio ScriviWhatsapp()
        {
            Console.WriteLine("Inserisci il numero del mittente");
            string mittente = Console.ReadLine();
            Console.WriteLine("Inserisci il testo del messaggio");
            string testo = Console.ReadLine();
            return new Messaggio(testo, mittente, "","whatsapp");
        }

        private Messaggio ScriviMail()
        {
            Console.WriteLine("Inserisci il numero del mittente");
            string mittente = Console.ReadLine();
            Console.WriteLine("Inserisci il testo del messaggio");
            string testo = Console.ReadLine();
            Console.WriteLine("Inserisci il testo del messaggio");
            string oggetto = Console.ReadLine();
            return new Messaggio(testo, mittente, oggetto,"mail");
        }

        private Messaggio ScriviSms()
        {
            Console.WriteLine("Inserisci il numero del mittente");
            string mittente = Console.ReadLine();
            Console.WriteLine("Inserisci il testo del messaggio");
            string testo = Console.ReadLine();
            return new Messaggio(testo, mittente, "","sms");
        }
    }
}