using static BCandSC_CSharp.Player;

namespace BCandSC_CSharp
{
    public class User
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public int Coins { get; set; }
        public Team? Team { get; set; }
        public User(string name, string username, int coins)
        {
            Name = name;
            Username = username;
            Coins = coins;
        
        }

    }

}
