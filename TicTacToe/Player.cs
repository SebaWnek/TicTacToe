namespace TicTacToe
{
    public abstract class Player
    {
        public int NumericValue { get; set; } = 1;
        public string Name { get; set; }
        public int Score { get; set; }
        public abstract byte[] NextMove();

        public Player(string name, int number)
        {
            Name = name;
            NumericValue = number;
        }
    }
}

