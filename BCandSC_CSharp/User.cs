using static BCandSC_CSharp.Player;

namespace BCandSC_CSharp
{

    public class User
    {
        public Team Team { get; set; }

        public User(Team team)
        {
            Team = team;
        }
    }

}
