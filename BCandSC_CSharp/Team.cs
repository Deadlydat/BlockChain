using System.Data.SqlClient;
using System.Data;
using System.Reflection.PortableExecutable;

namespace BCandSC_CSharp
{
    public class Team
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; } = "";
        public List<Player> Players { get; set; } = new List<Player>();
        public int TotalPoints { get; set; } = 0;
        public string Formation { get; set; } = "";
        public bool Done { get; set; } = false;

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

        public void SetTeamDone(bool isDone, int teamId)
        {
            Database db = new();
            try
            {
                SqlCommand command = new SqlCommand("UPDATE Team SET done = @done WHERE team_id = @team_id");
                command.Parameters.Add(new SqlParameter { ParameterName = "@team_id", Value = teamId, SqlDbType = SqlDbType.Int });
                command.Parameters.Add(new SqlParameter { ParameterName = "@done", Value = isDone, SqlDbType = SqlDbType.Bit });

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

        public List<Team> GetTeamList(int userid)
        {
            List<Team> list = new List<Team>();
            Database db = new();            
            SqlCommand command = new SqlCommand("SELECT * FROM UserMatchdayPoints WHERE (user_id = @user_id)");
            command.Parameters.Add(new SqlParameter { ParameterName = "@user_id", Value = userid, SqlDbType = SqlDbType.Int });

            db.conn.Open();
            command.Connection = db.conn;
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Team team = new();
                team.Id = reader.GetInt32("team_id");
                team.Name = reader.GetString("name");
                team.TotalPoints = reader.GetInt32("TotalPoints");
                team.Players = GetPlayersForTeam(team.Id);
                team.Done = reader.GetBoolean("done");
                list.Add(team);
            }
            reader.Close();
            db.conn.Close();

            return list;
        }

        public List<Player> GetPlayersForTeam(int teamId)
        {
            List<Player> list = new List<Player>();
            Database db = new();
            SqlCommand command = new SqlCommand("SELECT * FROM PlayerTeam WHERE (team_id = @team_id)");
            command.Parameters.Add(new SqlParameter { ParameterName = "@team_id", Value = teamId, SqlDbType = SqlDbType.Int });

            db.conn.Open();
            command.Connection = db.conn;
            SqlDataReader reader = command.ExecuteReader();

            Player p = new();
            while (reader.Read())
            {
                list.Add(p.GetPlayer(reader.GetInt32("player_id")));
            }

            list = list.OrderBy(x => x.Position).ToList();

            reader.Close();
            db.conn.Close();

            return list;
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
                team.Done = reader.GetBoolean("done");
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
