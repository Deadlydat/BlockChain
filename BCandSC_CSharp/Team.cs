namespace BCandSC_CSharp
{
   
    public class Team
    {
        public Team(List<Player> players)
        {
            Players = players;
        }

        public List<Player> Players { get; set; } = new List<Player>();
    }
    
}
