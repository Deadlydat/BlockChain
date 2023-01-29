using System.Data.SqlClient;
using System.Data;

namespace BCandSC_CSharp
{
    public class Team
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; } = "";
        public List<Player> Players { get; set; } = new List<Player>();
        public int TotalPoints { get; set; } = 0;

        public void CreateTeam(int userid, string name, int matchday)
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

        public void SaveTeam(int userid, List<Player> players, int matchday)
        {
            Database db = new();
            try
            {
                Id = GetTeam(userid, matchday).Id;                             
                string comtext = "";
                foreach(Player p in players)
                {
                    comtext += $"INSERT INTO PlayerTeam (player_id, team_id) VALUES (CAST({p.Id} AS int), CAST({Id} AS int));";
                }

                db.conn.Open();
                SqlCommand command = new SqlCommand(comtext);
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

        public Team GetTeam(int userid, int matchday, bool withPoints = false)
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

            if(withPoints == true)
            {
                db = new();
                command = new SqlCommand("SELECT * FROM UserMatchdayPoints WHERE (user_id = @user_id) AND (matchday = @matchday)");
                command.Parameters.Add(new SqlParameter { ParameterName = "@user_id", Value = userid, SqlDbType = SqlDbType.Int });
                command.Parameters.Add(new SqlParameter { ParameterName = "@matchday", Value = matchday, SqlDbType = SqlDbType.Int });

                db.conn.Open();
                command.Connection = db.conn;
                reader = command.ExecuteReader();

                if (reader.Read())
                {
                    team.TotalPoints = reader.GetInt32("TotalPoints");
                }
                reader.Close();
                db.conn.Close();

                db = new();
                command = new SqlCommand("SELECT * FROM PlayerTeam WHERE (team_id = @team_id)");
                command.Parameters.Add(new SqlParameter { ParameterName = "@team_id", Value = team.Id, SqlDbType = SqlDbType.Int });

                db.conn.Open();
                command.Connection = db.conn;
                reader = command.ExecuteReader();

                Player p = new();
                while (reader.Read())
                {
                    team.Players.Add(p.GetPlayer(reader.GetInt32("player_id")));
                }

                team.Players = team.Players.OrderBy(x => x.Position).ToList();

                reader.Close();
                db.conn.Close();
            }

            return team;
        }

    }

}
