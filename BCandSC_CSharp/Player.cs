using System.Data.SqlClient;
using System.Data;

namespace BCandSC_CSharp
{
    [Serializable]
    public class Player
    {
        public int Id { get; set; } = -1;
        public string Name { get; set; } = "";
        public PlayerPosition Position { get; set; } = PlayerPosition.Unknwon;

        public enum PlayerPosition
        {
            Goal = 0,
            Defense = 1,
            Midfield = 2,
            Striker = 3,
            Unknwon = 4
        }

        public Player GetPlayer(int playerID)
        {
            Database db = new();
            Player player = new();
            SqlCommand command = new SqlCommand("SELECT * FROM Player WHERE player_id = @player_id");
            SqlParameter param = new SqlParameter { ParameterName = "@player_id", Value = playerID, SqlDbType = SqlDbType.Int };
            command.Parameters.Add(param);

            db.conn.Open();
            command.Connection = db.conn;
            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read() == true)
            {
                player.Id = reader.GetInt32("player_id");
                player.Name = reader.GetString("name");
            }
            reader.Close();
            db.conn.Close();

            return player;
        }

        public List<Player> GetPlayers()
        {
            Database db = new();
            List<Player> players = new();
            SqlCommand command = new SqlCommand("SELECT * FROM Player");

            db.conn.Open();
            command.Connection = db.conn;
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read() == true)
            {
                Player player = new Player();
                player.Id = reader.GetInt32("player_id");
                player.Name = reader.GetString("name");
                player.Position = (Player.PlayerPosition)reader.GetInt32("position");
                players.Add(player);
            }
            reader.Close();
            db.conn.Close();

            return players;
        }

        public List<Player> GetPlayers(PlayerPosition position)
        {
            Database db = new();
            List<Player> players = new();
            SqlCommand command = new SqlCommand("SELECT * FROM Player WHERE (position = @position)");
            SqlParameter param = new SqlParameter { ParameterName = "@position", Value = (int)position, SqlDbType = SqlDbType.Int };
            command.Parameters.Add(param);

            db.conn.Open();
            command.Connection = db.conn;
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read() == true)
            {
                Player player = new Player();
                player.Id = reader.GetInt32("player_id");
                player.Name = reader.GetString("name");
                player.Position = (Player.PlayerPosition)reader.GetInt32("position");
                players.Add(player);
            }
            reader.Close();
            db.conn.Close();

            return players;
        }

    }
    
}
