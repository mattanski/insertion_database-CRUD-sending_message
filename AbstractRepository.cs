using System.Collections.Generic;
using System.Data.SqlClient;

namespace PrimoEsempio.Agenda
{
    public abstract class AbstractRepository<T>
    {

        protected string _connectionString;
        
        public abstract void Aggiungi(T entita);
        public abstract void Aggiorna(T entita);
        public AbstractRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        protected SqlDataReader EseguiQuery(SqlConnection connection,string query, List<SqlParameter> parameters,out SqlCommand sqlCommand)
        {
            sqlCommand = new SqlCommand(query, connection);

            foreach (var parameter in parameters)
            {
                sqlCommand.Parameters.Add(parameter);
            }
               
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            return sqlDataReader;
            
        }
        
        protected SqlConnection CreaConnessioneEConnettitiAlDB()
        {
            string connectionString = _connectionString;
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            return sqlConnection;
        }
        
        protected void RilasciaRisorse(SqlConnection connection, SqlCommand sqlCommand)
        {
            sqlCommand.Dispose();
            connection.Close();
            connection.Dispose();
        }
        protected void RilasciaRisorse(SqlConnection connection, SqlCommand sqlCommand,SqlDataReader sqlDataReader)
        {
            sqlDataReader.Dispose();
            RilasciaRisorse(connection,sqlCommand);
        }
        protected void EseguiNonQuery( string query, string tipoQuery,T entita,int id)
        {
            SqlConnection connection = CreaConnessioneEConnettitiAlDB();
            SqlCommand sqlCommand = CreateAndExecuteNonQueryCommand(connection,query,tipoQuery,entita,id);
            RilasciaRisorse(connection,sqlCommand);
        }
        protected SqlCommand CreateAndExecuteNonQueryCommand(SqlConnection connection, string query,string tipoDiQuery, T entita, int id)
        {
            SqlCommand sqlCommand = new SqlCommand(query, connection);
            DefinisciParametri(sqlCommand,tipoDiQuery,entita,id);
            sqlCommand.ExecuteNonQuery();
            return sqlCommand;
        }

        protected abstract void DefinisciParametri(SqlCommand sqlCommand, string tipoDiQuery, T entita, int id);

        protected abstract T MaterializzaEntita(SqlDataReader sqlDataReader);
        protected List<T> MaterializzaListaContatti(SqlDataReader sqlDataReader)
        {
            List<T> contatti = new List<T>();
            int i = 1;
            while (sqlDataReader.Read())
            {
                contatti.Add(MaterializzaEntita(sqlDataReader));
                i++;
            }

            return contatti;
        }
        
        protected abstract string Tabella { get; }

        public  void EseguiDelete(int id)
        {
            string query = @"delete from contatti where id = @id;";
            EseguiNonQuery(query,"delete",default,id);
        }
        
        public T EseguiQueryPerId(int id)
        {
            SqlConnection connection = CreaConnessioneEConnettitiAlDB();


            string query = $"select * from {Tabella} where " +
                           "id like @id";
                
            SqlDataReader sqlDataReader = EseguiQuery(connection, query, new List<SqlParameter>()
            {
                new SqlParameter("@id", id)
            }, out SqlCommand sqlCommand);

            T c = default(T);
              
            if (sqlDataReader.Read())
            {
                c = MaterializzaEntita(sqlDataReader);
            }

            RilasciaRisorse(connection,sqlCommand,sqlDataReader);
            return c;        
        }

        protected List<T> EseguiQueryGenerica(string query, List<SqlParameter> sqlParameters)
        {
            SqlConnection connection = CreaConnessioneEConnettitiAlDB();

            

            SqlDataReader sqlDataReader = EseguiQuery(connection, query, sqlParameters, out SqlCommand sqlCommand);

            List<T> list = MaterializzaListaContatti(sqlDataReader);

            RilasciaRisorse(connection,sqlCommand,sqlDataReader);

            return list;
        }

    }
}