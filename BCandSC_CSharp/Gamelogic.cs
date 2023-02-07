using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using System.Collections.Specialized;
using Org.BouncyCastle.Asn1.X509;

namespace BCandSC_CSharp
{
    public class Gamelogic
    {
        //public List<int> UsersId { get; set; } = new();

        private int matchDay;
        public Gamelogic(int matchDay)
        {

            this.matchDay = matchDay;
        }



        //public List<int> GetUsersForMatchDay()
        //{
        //    List<int> usersID = new();
        //    Database db = new();

        //    SqlCommand command = new SqlCommand("SELECT TOP user_id FROM UserMatchdayList WHERE matchday=@matchday");

        //    SqlParameter param = new SqlParameter
        //    {
        //        ParameterName = "@matchDay",
        //        Value = matchDay,
        //        SqlDbType = SqlDbType.Int
        //    };
        //    command.Parameters.Add(param);

        //    db.conn.Open();
        //    command.Connection = db.conn;
        //    SqlDataReader reader = command.ExecuteReader();

        //    while (reader.Read() == true)
        //    {
        //        int userID = reader.GetInt32("user_id");


        //        usersID.Add(userID);


        //    }
        //    reader.Close();
        //    db.conn.Close();

        //    return usersID;
        //}


        //public static void SetUserToUserMatchDayList(int userId, int matchDay)
        //{
        //    Database db = new();
        //    SqlCommand command = new SqlCommand("INSERT INTO UserMatchdayList (matchday, user_id) VALUES (@matchday, @user_id)");
        //    command.Parameters.Add(new SqlParameter { ParameterName = "@matchday", Value = matchDay, SqlDbType = SqlDbType.Int });
        //    command.Parameters.Add(new SqlParameter { ParameterName = "@user_id", Value = userId, SqlDbType = SqlDbType.Int });

        //    db.conn.Open();
        //    command.Connection = db.conn;
        //    command.ExecuteNonQuery();

        //    db.conn.Close();
        //}


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


        public Team GetResultsForMatch()
        {
            List<Team> MatchDayResults = GetTotalPointsForTeam();

            List<Team> SortedList = MatchDayResults.OrderBy(o => o.TotalPoints).ToList();

            return SortedList.First();
        }









        //public Dictionary<int, int> GetResultsFormApi(int matchDay)
        //{
        //    Database db = new();
        //    Dictionary<int, int> MatchDayResults = new Dictionary<int, int>();
        //    SqlCommand command = new SqlCommand("SELECT * FROM PlayerPoints WHERE matchday =@matchDay");

        //    SqlParameter param = new SqlParameter
        //    {
        //        ParameterName = "@matchDay",
        //        Value = matchDay,
        //        SqlDbType = SqlDbType.Int
        //    };
        //    command.Parameters.Add(param);

        //    db.conn.Open();
        //    command.Connection = db.conn;
        //    SqlDataReader reader = command.ExecuteReader();

        //    while (reader.Read() == true)
        //    {
        //        int playerID = reader.GetInt32("player_id");
        //        int points = reader.GetInt32("points");

        //        MatchDayResults.Add(playerID, points);

        //    }
        //    reader.Close();
        //    db.conn.Close();
        //    Console.WriteLine("Got Results From Api");

        //    return MatchDayResults;
        //}

    }

}
