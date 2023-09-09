using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace PrimoEsempio.Agenda
{
    public class Agenda
    {
        private const string _connectionString = @"Server=DESKTOP-CFI2N9C\SQLEXPRESS;
                                               Database=ProgrammatoriSenzaMani;
                                               Trusted_Connection=True;";
        
        private ContattiRepository _repository = new ContattiRepository(
            _connectionString);
        
        private MessaggiRepository _repositoryMessaggi = new MessaggiRepository(
            _connectionString);
        public Agenda()
        {
            
        }

        
        public void AggiungiContatto(ContattoDto dto)
        {
            bool duplicato = CercaDuplicato(dto.Nome, dto.Cognome);
            Contatto c = new Contatto(dto.Nome, dto.Cognome, dto.Mail, dto.Tel);
            
            //_contatti.Add(c);
            _repository.Aggiungi(c);
        }

        private bool CercaDuplicato(string nome, string cognome)
        {
            var ricerca = _repository.EseguiQueryPerNomeECognome(nome, cognome);
            return ricerca.Any();
        }

     
        public IReadOnlyList<Contatto> CercaContatto(string nome, string cognome)
        {
            if (string.IsNullOrEmpty(nome) && string.IsNullOrEmpty(cognome))
                return _repository.EseguiQueryPerNomeECognome("", "");
            
            return _repository.EseguiQueryPerNomeECognome(nome, cognome).ToList().AsReadOnly();
              
        }


        public Contatto OttieniContattoPerCodice(string codice)
        {
            bool isInInt = int.TryParse(codice, out int codiceInt);
            if (!isInInt)
                throw new ArgumentException("Codice non corretto");
            return _repository.EseguiQueryPerId(codiceInt);
        }

        public void EliminaContatto(int currentCodice)
        {
            _repository.EseguiDelete(currentCodice);
        }

        public void AggiornaContatto(Contatto current)
        {
            _repository.Aggiorna(current);
        }

        public void InviaMessaggio(Contatto current, Messaggio messaggio)
        {
            messaggio.CodiceContatto = current.Codice;
            current.Invia(messaggio);
            _repositoryMessaggi.Aggiungi(messaggio);
        }

        public List<Messaggio> OttieniMessaggiPerContatto(string codice)
        {
            bool isInInt = int.TryParse(codice, out int codiceInt);
            if (!isInInt)
                throw new ArgumentException("Codice non corretto");
            return _repositoryMessaggi.EseguiQueryPerCodiceContatto(codiceInt).ToList();
        }
    }
}