using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RateValue
{
    class Db
    {
        #region Data
        string connectionString;
        string Host { get; set; } //"localHost";
        string Database { get; set; } //"rateValutes";
        string User { get; set; } //"root";
        string Password { get; set; } //"Ghjcnjgfhjkm";
        int idUser;
        int idValute;
        Queue<string> Parametrs { get; set; }
        string Valute { get; set; }
        string Date { get; set; }
        public MySqlCommand command;
        public MySqlConnection connection;
        public MySqlDataReader reader;
        #endregion

        public Db(string valute, string date, Queue<string> parametrs,string host,string db,string user,string password)
        {
            Parametrs = parametrs;
            Valute = valute;
            Date = date;
            Host = host;
            Database = db;
            User = user;
            Password = password;
            connectionString = "Database=" + Database + ";Datasource=" + Host + ";User=" + User + ";Password=" + Password;
            connection = new MySqlConnection(connectionString);
            connection.Open();

        }
        public void SaveToDb()
        {
            if (ChekValuteName() && ChekValuteDate() && ChekUsers() && ChekDepends())
                Console.WriteLine("Такой пользователь уже совершал данную операцию");
            if (ChekValuteName() && ChekValuteDate() && ChekUsers() && !ChekDepends())
                InsertToDepends();

            if (!ChekValuteName() && ChekValuteDate() && ChekUsers())
            {
                InsertToValutes();
                InsertToDepends();
            }
            if (ChekValuteName() && !ChekValuteDate() && ChekUsers())
            {
                InsertToValutes();
                InsertToDepends();
            }
            if (!ChekValuteName() && !ChekValuteDate() && ChekUsers())
            {
                InsertToValutes();
                InsertToDepends();
            }
            if (!ChekValuteName() && !ChekValuteDate() && !ChekUsers())
            {
                InsertToValutes();
                InsertToUsers();
                InsertToDepends();
            }

            if (ChekValuteName() && !ChekValuteDate() && !ChekUsers())
            {
                InsertToUsers();
                InsertToDepends();
            }
            if (!ChekValuteName() && ChekValuteDate() && !ChekUsers())
            {
                InsertToUsers();
                InsertToDepends();
            }
            if (ChekValuteName() && ChekValuteDate() && !ChekUsers())
            {
                InsertToUsers();
                InsertToDepends();
            }

        }

        #region ReturnId
        public int ReturnIdValute()
        {
            command = connection.CreateCommand();
            command.CommandText = "Select * from valutes where name = \'" + Valute + "\' and datevalutes = \'" + Date + "\'";

            reader = command.ExecuteReader();

            while (reader.HasRows)
            {
                while (reader.Read())
                {
                    idValute = reader.GetInt32(0);
                }
                reader.NextResult();
            }
            reader.Close();
            return idValute;
        }
        public int ReturnIdUser()
        {
            command = connection.CreateCommand();
            command.CommandText = "Select * from users where username = \'" + User + "\'";

            reader = command.ExecuteReader();

            while (reader.HasRows)
            {
                while (reader.Read())
                {
                    idUser = reader.GetInt32(0);
                }
                reader.NextResult();
            }
            reader.Close();
            return idUser;
        }
        #endregion

        #region ChekCondition
        public bool ChekUsers()
        {
            command = connection.CreateCommand();
            command.CommandText = "Select * from users where username =\'" + User + "\'";

            reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                //Console.WriteLine("User: {0} already exists in DataBase", user);
                reader.Close();
                return true;
            }
            else
            {
                reader.Close();
                return false;
            }
        }
        public bool ChekValuteName()
        {
            command = connection.CreateCommand();
            command.CommandText = "Select * from valutes where name = \'" + Valute + "\'";

            reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                //Console.WriteLine("Valute: {0} already exists in DataBase", Valute);
                reader.Close();
                return true;
            }

            else
            {
                reader.Close();
                return false;
            }
        }

        public bool ChekValuteDate()
        {
            command = connection.CreateCommand();
            command.CommandText = "Select * from valutes where DateValutes = \'" + Date + "\'";

            reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                //Console.WriteLine("Date: {0} already exists in DataBase", Date);
                reader.Close();
                return true;
            }
            else
            {
                reader.Close();
                return false;
            }
        }
        public bool ChekDepends()
        {
            command = connection.CreateCommand();
            command.CommandText = "Select idusers from users_has_valutes where idusers = \'" + ReturnIdUser() + "\' and idvalutes = \'" + ReturnIdValute() +"\'";

            reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Close();
                return true;
            }
            else
            {
                reader.Close();
                return false;
            }
        }
        #endregion

        #region InsertIntoDb
        public int InsertToUsers()
        {
            MySqlCommand insertToUsers = connection.CreateCommand();
            insertToUsers.CommandText = "insert into users values (NULL,\'" + User + "\',\'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\')";

            int countRows = insertToUsers.ExecuteNonQuery();
            Console.WriteLine("InsertToUsers. Row Count affected = {0}", countRows);

            MySqlCommand selectFromUser = connection.CreateCommand();
            selectFromUser.CommandText = "select MAX(id) from users";

            MySqlDataReader reader = selectFromUser.ExecuteReader();
            while (reader.HasRows)
            {
                while (reader.Read())
                {
                    idUser = reader.GetInt32(0);
                }
                reader.NextResult();
            }

            reader.Close();
            return idUser;

        }
        public int InsertToValutes()
        {
            MySqlCommand insertToValutes = connection.CreateCommand();
            insertToValutes.CommandText = "insert into valutes " +
                "values(NULL,\'" + Parametrs.Dequeue() + "\',\'" + Parametrs.Dequeue() + "\',\'" + Parametrs.Dequeue() + "\',\'" + Parametrs.Dequeue() + "\'," +
                "\'" + Parametrs.Dequeue() + "\',\'" + Parametrs.Dequeue() + "\',\'" + Date + "\')";

            int countRows = insertToValutes.ExecuteNonQuery();
            Console.WriteLine("Insert to Valutes.Row Count affected = {0}", countRows);

            MySqlCommand selectFromValutes = connection.CreateCommand();
            selectFromValutes.CommandText = "select max(id) from valutes";

            MySqlDataReader reader = selectFromValutes.ExecuteReader();

            while (reader.HasRows)
            {
                while (reader.Read())
                {
                    idValute = reader.GetInt32(0);
                }
                reader.NextResult();
            }
            reader.Close();
            return idValute;
        }
        public void InsertToDepends()
        {
            MySqlCommand insertToDepends = connection.CreateCommand();
            insertToDepends.CommandText = "insert into users_has_valutes values(" + ReturnIdUser() +","+ ReturnIdValute()+ ")";

            int countRows = insertToDepends.ExecuteNonQuery();
            Console.WriteLine("Insert to Depends. Row Count affected = {0}", countRows);
        }
        #endregion
    }
}
