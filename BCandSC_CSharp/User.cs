using System.Data.SqlClient;
using System.Data;
using static BCandSC_CSharp.Player;

namespace BCandSC_CSharp
{
    public class User
    {
        public int Id { get; set; } = -1;
        public string Name { get; set; } = "";

        public User GetUser(string name, string password)
        {
            User user = new();
            Database db = new();

            SqlCommand command = new SqlCommand("SELECT * FROM [User] WHERE ([name] = @name) AND ([password] = @password)");
            SqlParameter param1 = new SqlParameter {ParameterName = "@name", Value = name, SqlDbType = SqlDbType.NVarChar, Size = 30};
            SqlParameter param2 = new SqlParameter {ParameterName = "@password", Value = password, SqlDbType = SqlDbType.NVarChar, Size = 50};

            command.Parameters.Add(param1);
            command.Parameters.Add(param2);
            db.conn.Open();
            command.Connection = db.conn;

            try
            {
                using SqlDataReader reader = command.ExecuteReader();
                if (reader.Read() == true)
                {
                    user.Id = reader.GetInt32("user_id");
                    user.Name = reader.GetString("name");
                }
            }
            catch(Exception e)
            {
                Console.Write(e.Message);
            }
            finally
            {
                db.conn.Close();
            }

            return user;
        }

        public User SetUser(string name, string password)
        {
            User user = new();
            Database db = new();

            try
            {
                SqlCommand command = new SqlCommand("INSERT INTO [User] ([name], [password]) VALUES (@name, @password)");
                SqlParameter param1 = new SqlParameter { ParameterName = "@name", Value = name, SqlDbType = SqlDbType.NVarChar, Size = 30 };
                SqlParameter param2 = new SqlParameter { ParameterName = "@password", Value = password, SqlDbType = SqlDbType.NVarChar, Size = 50 };

                command.Parameters.Add(param1);
                command.Parameters.Add(param2);
                db.conn.Open();
                command.Connection = db.conn;
                command.ExecuteNonQuery();
                user = GetUser(name, password);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                db.conn.Close();
            }

            return user;
        }
    }

}
