using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using System.Collections.Specialized;
using Org.BouncyCastle.Asn1.X509;

namespace BCandSC_CSharp
{
    public class Gamelogic
    {

        private int matchDay;
        public Gamelogic(int matchDay)
        {
            this.matchDay = matchDay;
        }

        private List<Team> GetTotalPointsForTeam()
        {
            List<Team> PointsForTeam = new();
            Database db = new();

            SqlCommand command = new SqlCommand("SELECT TotalPoints, team_id, name, user_id FROM UserMatchdayPoints WHERE matchday=@matchday");

            SqlParameter param = new SqlParameter
            {
                ParameterName = "@matchDay",
                Value = matchDay,
                SqlDbType = SqlDbType.Int
            };
            command.Parameters.Add(param);

            db.conn.Open();
            command.Connection = db.conn;
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read() == true)
            {
                Team team = new Team();
                team.Id = reader.GetInt32("team_id");
                team.Name = reader.GetString("name");
                team.TotalPoints = reader.GetInt32("TotalPoints");


                PointsForTeam.Add(team);

            }
            reader.Close();
            db.conn.Close();

            return PointsForTeam;
        }





        public bool CheckCurrentMatchday()
        {
            bool done = false;
            Database db = new();

            SqlCommand command = new SqlCommand("SELECT done FROM UserMatchdayPoints WHERE matchday=@matchday");

            SqlParameter param = new SqlParameter
            {
                ParameterName = "@matchDay",
                Value = matchDay,
                SqlDbType = SqlDbType.Int
            };
            command.Parameters.Add(param);

            db.conn.Open();
            command.Connection = db.conn;
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read() == true)
            {

                done = reader.GetBoolean("done");

            }
            reader.Close();
            db.conn.Close();

            return done;
        }




        public Team GetResultsForMatch()
        {
            List<Team> MatchDayResults = GetTotalPointsForTeam();

            List<Team> SortedList = MatchDayResults.OrderByDescending(o => o.TotalPoints).ToList();

            return SortedList.First();
        }

    }

}
