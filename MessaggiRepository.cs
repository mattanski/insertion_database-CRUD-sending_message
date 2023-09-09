using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace PrimoEsempio.Agenda
{
    public class MessaggiRepository:
        AbstractRepository<Messaggio>
    {
        public MessaggiRepository(string connectionString) : base(connectionString)
        {
        }

        public override void Aggiungi(Messaggio entita)
        {
            string query = @"insert into Messaggi 
                        (CodiceContatto, Testo, Oggetto, Mittente, 
                         Tipo) values ( @codiceContatto, @testo ,     
                        @oggetto, @mittente, @tipo);";
            EseguiNonQuery(query,"add",entita,0);
        }

        public override void Aggiorna(Messaggio entita)
        {
            string query = @"update Messaggi set 
                    CodiceContatto = @codiceContatto,
                    Testo = @testo, 
                    Oggetto = @oggetto, 
                    Mittente = @mittente,
                    Tipo = @tipo
                    where id = @id";
            EseguiNonQuery(query,"update",entita,0);
        }

        protected override void DefinisciParametri(SqlCommand sqlCommand, string tipoDiQuery, Messaggio entita, int id)
        {
            if (tipoDiQuery.Equals("delete"))
            {
                sqlCommand.Parameters.AddWithValue("@id", id);
            }else if(tipoDiQuery.Equals("update"))
            {
                sqlCommand.Parameters.AddWithValue("@codiceContatto", entita.CodiceContatto);
                sqlCommand.Parameters.AddWithValue("@testo", entita.Testo);
                sqlCommand.Parameters.AddWithValue("@oggetto", entita.Oggetto);
                sqlCommand.Parameters.AddWithValue("@mittente", entita.Mittente);
                sqlCommand.Parameters.AddWithValue("@tipo", entita.Tipo);
                sqlCommand.Parameters.AddWithValue("@id", entita.Codice);
            }
            else
            {
                sqlCommand.Parameters.AddWithValue("@codiceContatto", entita.CodiceContatto);
                sqlCommand.Parameters.AddWithValue("@testo", entita.Testo);
                sqlCommand.Parameters.AddWithValue("@oggetto", entita.Oggetto);
                sqlCommand.Parameters.AddWithValue("@mittente", entita.Mittente);
                sqlCommand.Parameters.AddWithValue("@tipo", entita.Tipo);
            }
        }

        protected override Messaggio MaterializzaEntita(SqlDataReader sqlDataReader)
        {
            Messaggio c = new Messaggio(
                sqlDataReader["Testo"].ToString(),
                sqlDataReader["Mittente"].ToString(),
                sqlDataReader["Oggetto"].ToString(),
                sqlDataReader["Tipo"].ToString());
            
            int id = Convert.ToInt32(sqlDataReader["Id"].ToString());
            c.Codice = id;
            int idContatto = Convert.ToInt32(sqlDataReader["CodiceContatto"]);
            c.CodiceContatto = idContatto;
            return c;
        }

        protected override string Tabella => "messaggi";

        public List<Messaggio> EseguiQueryPerCodiceContatto(int codiceInt)
        {
            string query = "select * from Messaggi where " +
                           "CodiceContatto = @codice";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@codice", codiceInt)
            };
            return EseguiQueryGenerica(query, parameters);
        }
    }
}