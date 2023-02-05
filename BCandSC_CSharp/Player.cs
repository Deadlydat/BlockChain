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
        public int Points { get; set; } = 0;
        public int Marktwert { get; set; } = 0;
        public int Nummer { get; set; } = 0;
        public int Einsaetze { get; set; } = 0;
        public int Tore { get; set; } = 0;
        public string Mannschaft { get; set; } = "";

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
                player.Marktwert = reader.GetInt32("marktwert");
                player.Einsaetze = reader.GetInt32("einsaetze");
                player.Tore = reader.GetInt32("tore");
                player.Mannschaft = reader.GetString("verein");
                player.Position = (Player.PlayerPosition)reader.GetInt32("position");
            }
            reader.Close();
            db.conn.Close();

            return player;
        }

        public void SetPlayerToTeam(int playerID, int teamID)
        {
            Database db = new();
            SqlCommand command = new SqlCommand("INSERT INTO PlayerTeam (player_id,team_id) VALUES (@player_id,@team_id)");
            command.Parameters.Add(new SqlParameter { ParameterName = "@player_id", Value = playerID, SqlDbType = SqlDbType.Int });
            command.Parameters.Add(new SqlParameter { ParameterName = "@team_id", Value = teamID, SqlDbType = SqlDbType.Int });

            db.conn.Open();
            command.Connection = db.conn;
            command.ExecuteNonQuery();

            db.conn.Close();
        }

        public void RemovePlayerFromTeam(int playerID, int teamID)
        {
            Database db = new();
            SqlCommand command = new SqlCommand("DELETE FROM PlayerTeam WHERE (player_id = @player_id) AND (team_id = @team_id)");
            command.Parameters.Add(new SqlParameter { ParameterName = "@player_id", Value = playerID, SqlDbType = SqlDbType.Int });
            command.Parameters.Add(new SqlParameter { ParameterName = "@team_id", Value = teamID, SqlDbType = SqlDbType.Int });

            db.conn.Open();
            command.Connection = db.conn;
            command.ExecuteNonQuery();

            db.conn.Close();
        }

        public List<Player> GetPlayers(PlayerPosition position)
        {
            Database db = new();
            List<Player> players = new();
            SqlCommand command = new SqlCommand("SELECT TOP (1000) [player_id],[name],[verein],[position],[marktwert],[einsaetze],[tore] FROM Player WHERE (position = @position) ORDER BY marktwert DESC");
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
                player.Marktwert = reader.GetInt32("marktwert");
                player.Einsaetze = reader.GetInt32("einsaetze");
                player.Tore = reader.GetInt32("tore");
                player.Mannschaft = reader.GetString("verein");
                players.Add(player);
            }
            reader.Close();
            db.conn.Close();

            return players;
        }

    }
    
}
