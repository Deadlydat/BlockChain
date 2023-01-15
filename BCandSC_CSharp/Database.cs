using System.Data;
using System.Data.SqlClient;
using System.Numerics;
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

            if (reader.Read() == true)
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

            while (reader.Read() == true)
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



        public Dictionary<int, int> GetResultsFormApi(int matchDay)
        {
            Dictionary<int, int> MatchDayResults = new Dictionary<int, int>();
            SqlCommand command = new SqlCommand("SELECT * FROM PlayerPoints WHERE matchday =@matchDay");

            SqlParameter param = new SqlParameter
            {
                ParameterName = "@matchDay",
                Value = matchDay,
                SqlDbType = SqlDbType.Int
            };
            command.Parameters.Add(param);

            conn.Open();
            command.Connection = conn;
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read() == true)
            {
                int playerID = reader.GetInt32("player_id");
                int points = reader.GetInt32("points");

                MatchDayResults.Add(playerID, points);

            }
            reader.Close();
            conn.Close();
            Console.WriteLine("Got Results From Api");

            return MatchDayResults;
        }




        public Team GetSavedTeam(int teamID)
        {
            Team team = new Team();
            List<Player> players = new List<Player>();

            SqlCommand command = new SqlCommand("SELECT PlayerTeam.player_id, PlayerTeam.team_id, Team.matchday," +
                " Team.name AS team_name, Player.name AS player_name, Player.position" +
                " FROM PlayerTeam " +
                "INNER JOIN Team ON PlayerTeam.team_id =Team.team_id " +
                "INNER JOIN Player ON PlayerTeam.player_id = Player.player_id" +
                " WHERE PlayerTeam.team_id=@teamID");

            SqlParameter param = new SqlParameter
            {
                ParameterName = "@teamID",
                Value = teamID,
                SqlDbType = SqlDbType.Int
            };
            command.Parameters.Add(param);

            conn.Open();
            command.Connection = conn;
            SqlDataReader reader = command.ExecuteReader();


            if (reader.Read() == true)
            {
                team.TeamId = reader.GetInt32("team_id");
                team.Name = reader.GetString("team_name");

                while (reader.Read() == true)
                {
                    Player player = new Player();
                    player.Id = reader.GetInt32("player_id");
                    player.Name = reader.GetString("player_name");
                    player.Position = (Player.PlayerPosition)reader.GetInt32("position");
                    players.Add(player);

                }

                team.Players = players;
            }

            reader.Close();
            conn.Close();
           
            return team;
        }




        public Database()
        {
            conn = new SqlConnection();
            conn.ConnectionString = "Data Source=public.emmel-it.de, 1533;User ID=BCandSC;Password=FHWS2022;";
        }
    }
}
