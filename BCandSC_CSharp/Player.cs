namespace BCandSC_CSharp
{
    public class Player
    {
        public int Id { get; set; } = -1;
        public string Name { get; set; } = "";
        public PlayerPosition Position { get; set; } = PlayerPosition.Unknwon;

        public enum PlayerPosition
        {
            Goal = 0,
            Defense = 1,
            Midfield = 2,
            Striker = 3,
            Unknwon = 4
        }

    }
}
