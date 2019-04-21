using System.Linq;

namespace TicTacToe
{
    public class AIEasy : AI
    {
        public AIEasy(string name, int number) : base(name, number)
        {
        }

        public override byte[] NextMove()
        {
            FindEmptyCells();
            SimulateThinking();
            return emptyCells[rnd.Next(0, emptyCells.Count())];
        }
    }
}

