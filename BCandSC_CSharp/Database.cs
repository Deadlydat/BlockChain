using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace BCandSC_CSharp
{
    public class Database
    {
        SqlConnection conn;

        public Player GetPlayer(int playerID)
        {
            Player player = new Player();
            SqlCommand command = new SqlCommand("SELECT * FROM Player WHERE player_id = @player_id");
            SqlParameter param = new SqlParameter
            {
                ParameterName = "@player_id",
                Value = playerID,
                SqlDbType = SqlDbType.Int
            };
            command.Parameters.Add(param);

            conn.Open();
            command.Connection = conn;
            SqlDataReader reader = command.ExecuteReader();

            if(reader.Read() == true)
            {
                player.Id = reader.GetInt32("player_id");
                player.Name = reader.GetString("name");
            }
            reader.Close();
            conn.Close();

            return player;
        }

        public List<Player> GetPlayers()
        {
            List<Player> players = new List<Player>();
            SqlCommand command = new SqlCommand("SELECT * FROM Player");

            conn.Open();
            command.Connection = conn;
            SqlDataReader reader = command.ExecuteReader();

            while(reader.Read() == true)
            {
                Player player = new Player();
                player.Id = reader.GetInt32("player_id");
                player.Name = reader.GetString("name");
                player.Position = (Player.PlayerPosition)reader.GetInt32("position");
                players.Add(player);
            }
            reader.Close();
            conn.Close();

            return players;
        }

        public Database()
        {
            conn = new SqlConnection();
            conn.ConnectionString = "Data Source=public.emmel-it.de, 1533;User ID=BCandSC;Password=FHWS2022;";
        }
    }
}
