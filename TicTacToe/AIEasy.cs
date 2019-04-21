using System.Linq;

namespace TicTacToe
{
    /// <summary>
    /// Class responsible for AIEasy
    /// Made as simple as possible, just selects random empty position
    /// </summary>
    public class AIEasy : AI
    {
        /// <summary>
        /// Constructor for class AIEasy
        /// </summary>
        /// <param name="name">Put on board when printing and in comments</param>
        /// <param name="number">Used for calculations to distinguish from other player</param>
        public AIEasy(string name, int number) : base(name, number)
        {
        }
        /// <summary>
        /// Inherited methos, calculates and sends to Game coordinates that AI decided
        /// </summary>
        /// <returns>Coordinates of next moves decided by AI</returns>
        public override byte[] NextMove()
        {
            FindEmptyCells();
            SimulateThinking();
            return emptyCells[rnd.Next(0, emptyCells.Count())];
        }
    }
}

