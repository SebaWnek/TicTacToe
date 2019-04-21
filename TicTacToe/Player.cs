namespace TicTacToe
{
    /// <summary>
    /// Abstract class for Player object, definies common properties for human and computer players
    /// </summary>
    public abstract class Player
    {
        public int NumericValue { get; set; } = 1;
        public string Name { get; set; }
        public int Score { get; set; }
        public abstract byte[] NextMove();
        /// <summary>
        /// Constructor for Player object, takes values from inheriting classes
        /// </summary>
        /// <param name="name">Put on board when printing and in comments</param>
        /// <param name="number">Used for calculations to distinguish from other player</param>
        protected Player(string name, int number)
        {
            Name = name;
            NumericValue = number;
        }
    }
}

