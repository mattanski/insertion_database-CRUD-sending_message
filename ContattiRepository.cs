using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace PrimoEsempio.Agenda
{
    public class ContattiRepository : AbstractRepository<Contatto>
    {
        public ContattiRepository(string connectionString) : base(connectionString)
        {
            
        }
        protected override string Tabella => "contatti";

        public override void Aggiungi(Contatto contatto)
        {

            string query = @"insert into Contatti 
                        (Nome, Cognome, Mail, Telefono) values ( @nome, @cognome ,
                                                                @mail, @telefono);";
            EseguiNonQuery(query,"add",contatto,0);
            
        }
          
        public List<Contatto> EseguiQueryPerNomeECognome(string nome, 
                string cognome)
            {

                string query = "select * from Contatti where " +
                               "nome like @nome and cognome like @cognome;";
                List<SqlParameter> parameters = new List<SqlParameter>()
                {
                    new SqlParameter("@nome", "%" + nome + "%"),
                    new SqlParameter("@cognome", "%" + cognome + "%")
                };
                return EseguiQueryGenerica(query, parameters);

            }
        
        public override void Aggiorna(Contatto contatto)
        {
            string query = @"update Contatti set 
                    Nome = @nome,
                    Cognome = @cognome, 
                    Mail = @mail, 
                    Telefono = @telefono
                    where id = @id;";
           EseguiNonQuery(query,"update",contatto,0);
        }

       
        protected override Contatto MaterializzaEntita(SqlDataReader sqlDataReader)
        {
            Contatto c = new Contatto(Convert.ToInt32(sqlDataReader["Id"]),
                sqlDataReader["Nome"].ToString(),
                sqlDataReader["Cognome"].ToString(),
                sqlDataReader["Mail"].ToString(),
                sqlDataReader["Telefono"].ToString()
            );
            return c;
        }

        
        protected override void DefinisciParametri(SqlCommand sqlCommand,string tipoDiQuery,Contatto contatto, int id)
        {
            if (tipoDiQuery.Equals("delete"))
            {
                sqlCommand.Parameters.AddWithValue("@id", id);
            }else if(tipoDiQuery.Equals("update"))
            {
                sqlCommand.Parameters.AddWithValue("@nome", contatto.Nome);
                sqlCommand.Parameters.AddWithValue("@cognome", contatto.Cognome);
                sqlCommand.Parameters.AddWithValue("@mail", contatto.Mail);
                sqlCommand.Parameters.AddWithValue("@telefono", contatto.Tel);
                sqlCommand.Parameters.AddWithValue("@id", contatto.Codice);
            }
            else
            {
                sqlCommand.Parameters.AddWithValue("@nome", contatto.Nome);
                sqlCommand.Parameters.AddWithValue("@cognome", contatto.Cognome);
                sqlCommand.Parameters.AddWithValue("@mail", contatto.Mail);
                sqlCommand.Parameters.AddWithValue("@telefono", contatto.Tel);
            }
        }
       
       
    }
}