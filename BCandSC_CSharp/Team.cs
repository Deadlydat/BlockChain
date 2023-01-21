using System.Data.SqlClient;
using System.Data;

namespace BCandSC_CSharp
{
    public class Team
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; } = "";
        public List<Player> Players { get; set; } = new List<Player>();


        public void SaveTeam(int userid, List<Player> players, string name, int matchday)
        {
            Database db = new();
            try
            {
                SqlCommand command = new SqlCommand("INSERT INTO Team (user_id, matchday, name) VALUES (@user_id, @matchday, @name)");
                command.Parameters.Add(new SqlParameter { ParameterName = "@user_id", Value = userid, SqlDbType = SqlDbType.Int });
                command.Parameters.Add(new SqlParameter { ParameterName = "@matchday", Value = matchday, SqlDbType = SqlDbType.Int });
                command.Parameters.Add(new SqlParameter { ParameterName = "@name", Value = name, SqlDbType = SqlDbType.NVarChar, Size = 50 });

                db.conn.Open();
                command.Connection = db.conn;
                command.ExecuteNonQuery();

                Id = GetTeam(userid, matchday).Id;

                string comtext = "";
                foreach(Player p in players)
                {
                    comtext += $"INSERT INTO PlayerTeam (player_id, team_id) VALUES (CAST({p.Id} AS int), CAST({Id} AS int));";
                }
                command = new SqlCommand(comtext);
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

        public Team GetTeam(int userid, int matchday)
        {
            Database db = new();
            Team team = new();
            SqlCommand command = new SqlCommand("SELECT * FROM Team WHERE (user_id = @user_id) AND (matchday = @matchday)");
            command.Parameters.Add(new SqlParameter { ParameterName = "@user_id", Value = userid, SqlDbType = SqlDbType.Int });
            command.Parameters.Add(new SqlParameter { ParameterName = "@matchday", Value = matchday, SqlDbType = SqlDbType.Int });

            db.conn.Open();
            command.Connection = db.conn;
            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                team.Id = reader.GetInt32("team_id");
            }
            reader.Close();
            db.conn.Close();

            return team;
        }


        public Team GetSavedTeam(int teamID)
        {
            Database db = new();
            Team team = new();
            List<Player> players = new();

            SqlCommand command = new SqlCommand("SELECT PlayerTeam.player_id, PlayerTeam.team_id, Team.matchday," +
                " Team.name AS team_name, Player.name AS player_name, Player.position" +
                " FROM PlayerTeam " +
                "INNER JOIN Team ON PlayerTeam.team_id = Team.team_id " +
                "INNER JOIN Player ON PlayerTeam.player_id = Player.player_id" +
                " WHERE PlayerTeam.team_id=@teamID");

            SqlParameter param = new SqlParameter
            {
                ParameterName = "@teamID",
                Value = teamID,
                SqlDbType = SqlDbType.Int
            };
            command.Parameters.Add(param);

            db.conn.Open();
            command.Connection = db.conn;
            SqlDataReader reader = command.ExecuteReader();


            if (reader.Read() == true)
            {
                team.Id = reader.GetInt32("team_id");
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
            db.conn.Close();

            return team;
        }

    }

}
