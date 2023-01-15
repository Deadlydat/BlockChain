namespace BCandSC_CSharp
{
    public class Gamelogic
    {
        private User user1;
        private User user2;
        private int matchDay;
        public Gamelogic(User User1, User User2, int matchDay)
        {
            user1 = User1;
            user2 = User2;
            this.matchDay = matchDay;
        }



        public void GetResultsForMatch()
        {
            Database db = new();
            Dictionary<int, int> MatchDayResults = db.GetResultsFormApi(matchDay);

            int pointsForTeamUser1 = GetPointsForEachTeam(user1, MatchDayResults);
            int pointsForTeamUser2 = GetPointsForEachTeam(user2, MatchDayResults);


            if (pointsForTeamUser1> pointsForTeamUser2)
            {
            
                Console.WriteLine("User1 wins");
            }
            else
            {
                Console.WriteLine("User2 wins");

            }



        }

        public int GetPointsForEachTeam(User user, Dictionary<int, int> MatchDayResults)
        {
            int points = 0;
            user.Team.Players.ForEach(element =>
            {
                MatchDayResults.TryGetValue(element.Id, out int value);
                points += value;
        

            });

            
            return points;
        }









    }

}
