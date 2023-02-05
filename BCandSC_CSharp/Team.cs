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
        public string Formation { get; set; } = "";

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

        public void SetFormation(int teamid, string formation)
        {
            Database db = new();
            try
            {
                SqlCommand command = new SqlCommand("UPDATE Team SET formation = @formation WHERE team_id = @team_id");
                command.Parameters.Add(new SqlParameter { ParameterName = "@team_id", Value = teamid, SqlDbType = SqlDbType.Int });
                command.Parameters.Add(new SqlParameter { ParameterName = "@formation", Value = formation, SqlDbType = SqlDbType.NVarChar, Size = 4 });

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
                team.Formation = reader.GetString("formation");
                team.Name = reader.GetString("name");
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
