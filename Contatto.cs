using System;

namespace PrimoEsempio.Agenda
{
    public class Contatto
    {
        private readonly int _codice;
        private  string _nome;
        private  string _cognome;
        private  string _mail;
        private  string _tel;

        public Contatto(int codice, string nome, string cognome, string mail, string tel)
        {
            if (codice <= 0) throw new ArgumentOutOfRangeException(nameof(codice));
            _codice = codice;
            _nome = nome ?? throw new ArgumentNullException(nameof(nome));
            _cognome = cognome ?? throw new ArgumentNullException(nameof(cognome));
            _mail = mail;
            _tel = tel;
        }
        
        public Contatto( string nome, string cognome, string mail, string tel)
        {
           
            _nome = nome ?? throw new ArgumentNullException(nameof(nome));
            _cognome = cognome ?? throw new ArgumentNullException(nameof(cognome));
            _mail = mail;
            _tel = tel;
        }

        public int Codice => _codice;

        public string Nome => _nome;

        public string Cognome => _cognome;

        public string Mail => _mail;

        public string Tel => _tel;

        public void RinnovaInformazioni(string nome, string cognome, string mail, string tel)
        {
            _nome = nome ?? throw new ArgumentNullException(nameof(nome));
            _cognome = cognome ?? throw new ArgumentNullException(nameof(cognome));
            _mail = mail;
            _tel = tel;
        }
        
        public override string ToString()
        {
            return $"Codice {this._codice} - Nome {this._nome} - " +
                   $"Cognome {this._cognome} - Mail {this._mail} - Tel {this._tel}";
        }

        public void Invia(Messaggio messaggio)
        {
            if (messaggio == null) throw new ArgumentNullException(nameof(messaggio));
            messaggio.Invia(this);
        }
    }
}