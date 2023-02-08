using System.Data.SqlClient;
using System.Data;
using static BCandSC_CSharp.Player;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace BCandSC_CSharp
{
    public class User
    {
        public int Id { get; set; } = -1;
        public string Name { get; set; } = "";
        public string Address { get; set; } = "";
        public string PrivateKey { get; set; } = "";


        public User GetUser(string name, string password)
        {
            User user = new();
            Database db = new();

            SqlCommand command = new SqlCommand("SELECT * FROM [User] WHERE ([name] = @name) AND ([password] = @password)");
            SqlParameter param1 = new SqlParameter { ParameterName = "@name", Value = name, SqlDbType = SqlDbType.NVarChar, Size = 30 };
            SqlParameter param2 = new SqlParameter { ParameterName = "@password", Value = password, SqlDbType = SqlDbType.NVarChar, Size = 50 };


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
                    user.Address = RemoveWhitespace(reader.GetString("address"));
                    user.PrivateKey = RemoveWhitespace(reader.GetString("private_key"));
                }
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }
            finally
            {
                db.conn.Close();
            }

            return user;
        }
        public void SetUserBalance(int userId, decimal balance)
        {
            Database db = new();
            try
            {
                SqlCommand command = new SqlCommand("UPDATE [User] SET account_balance = @balance  WHERE ([user_id] = @user_id)");
                SqlParameter param1 = new SqlParameter { ParameterName = "@user_id", Value = userId, SqlDbType = SqlDbType.Int };
                SqlParameter param2 = new SqlParameter { ParameterName = "@balance", Value = balance, SqlDbType = SqlDbType.Decimal };


                command.Parameters.Add(param1);
                command.Parameters.Add(param2);

                db.conn.Open();
                command.Connection = db.conn;
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message+"bal");
            }
            finally
            {
                db.conn.Close();
            }
        }

        public decimal GetUserBalance(int userId)
        {
            decimal result = 0;
            Database db = new();

            SqlCommand command = new SqlCommand("SELECT * FROM [User] WHERE [user_id] = @user_id");
            SqlParameter param1 = new SqlParameter { ParameterName = "@user_id", Value = userId, SqlDbType = SqlDbType.Int };



            command.Parameters.Add(param1);


            db.conn.Open();
            command.Connection = db.conn;

            try
            {
                using SqlDataReader reader = command.ExecuteReader();
                if (reader.Read() == true)
                {
                    result = reader.GetDecimal("account_balance");
                }
            }
            catch (Exception e)
            {
                Console.Write(e.Message+"hier");
            }
            finally
            {
                db.conn.Close();
            }

            return result;
        }


        public int GetUserTransaction(int userId, int matchday)
        {
            int result = 0;
            Database db = new();

            SqlCommand command = new SqlCommand("SELECT * FROM [UserTransaction] WHERE ([user_id] = @user_id) AND ([matchday] = @matchday)");
            SqlParameter param1 = new SqlParameter { ParameterName = "@user_id", Value = userId, SqlDbType = SqlDbType.Int };
            SqlParameter param2 = new SqlParameter { ParameterName = "@matchday", Value = matchday, SqlDbType = SqlDbType.Int };


            command.Parameters.Add(param1);
            command.Parameters.Add(param2);

            db.conn.Open();
            command.Connection = db.conn;

            try
            {
                using SqlDataReader reader = command.ExecuteReader();
                if (reader.Read() == true)
                {
                    result = reader.GetInt32("amount");
                }
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }
            finally
            {
                db.conn.Close();
            }

            return result;
        }

        public void SetUserTransaction(int userId, int matchday, int transactionValue)
        {
            Database db = new();
            try
            {
                SqlCommand command = new SqlCommand("INSERT INTO [UserTransaction] ([user_id], [matchday], amount) VALUES (@user_id, @matchday, @amount)");
                SqlParameter param1 = new SqlParameter { ParameterName = "@user_id", Value = userId, SqlDbType = SqlDbType.Int };
                SqlParameter param2 = new SqlParameter { ParameterName = "@matchday", Value = matchday, SqlDbType = SqlDbType.Int };
                SqlParameter param3 = new SqlParameter { ParameterName = "@amount", Value = transactionValue, SqlDbType = SqlDbType.Int };

                command.Parameters.Add(param1);
                command.Parameters.Add(param2);
                command.Parameters.Add(param3);
                db.conn.Open();
                command.Connection = db.conn;
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                db.conn.Close();
            }
        }

        public User GetUser(int userId)
        {
            User user = new();
            Database db = new();

            SqlCommand command = new SqlCommand("SELECT * FROM [User] WHERE (user_id = @user_id)");
            SqlParameter param1 = new SqlParameter { ParameterName = "@user_id", Value = userId, SqlDbType = SqlDbType.Int };

            command.Parameters.Add(param1);

            db.conn.Open();
            command.Connection = db.conn;

            try
            {
                using SqlDataReader reader = command.ExecuteReader();
                if (reader.Read() == true)
                {
                    user.Id = reader.GetInt32("user_id");
                    user.Name = reader.GetString("name");
                    user.Address = RemoveWhitespace(reader.GetString("address"));
                    user.PrivateKey = RemoveWhitespace(reader.GetString("private_key"));
                }
            }
            catch (Exception e)
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

        private static string RemoveWhitespace(string input)
        {
            return Regex.Replace(input, @"\s+", "");
        }



    }

}
