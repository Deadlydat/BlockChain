using System.Data.SqlClient;
using System.Data;

namespace BCandSC_CSharp
{
    public class Enviroment
    {
        public int Matchday { get; set; }
        public int Coins { get; set; }

        public static Enviroment GetEnviroment()
        {
            Database db = new();
            Enviroment enviroment = new();
            SqlCommand command = new SqlCommand("SELECT * FROM Enviroment");

            db.conn.Open();
            command.Connection = db.conn;
            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                enviroment.Matchday = reader.GetInt32("matchday");
                enviroment.Coins = reader.GetInt32("coins");
            }
            reader.Close();
            db.conn.Close();

            return enviroment;
        }
    }
}
