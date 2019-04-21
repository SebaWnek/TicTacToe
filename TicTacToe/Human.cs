using System;

namespace TicTacToe
{
    /// <summary>
    /// Class responsible for input and selecting next position when player is Human
    /// </summary>
    public class Human : Player
    {
        /// <summary>
        /// Constructor for Human player
        /// </summary>
        /// <param name="name">Put on board when printing and in comments</param>
        /// <param name="number">Used for calculations to distinguish from other player</param>
        public Human(string name, int number) : base(name, number)
        {
        }
        /// <summary>
        /// Method for getting input from human player and returning coordinates of next human move position
        /// </summary>
        /// <returns>Coordinates of next moves decided by Human player</returns>
        public override byte[] NextMove()
        {
            Console.WriteLine($"{Name} - Next Move?");
            byte[] move = new byte[2];
            string moveString = Console.ReadLine();
            if ((moveString[0] == '3' || moveString[0] == '1' || moveString[0] == '2') &&
                (moveString[1] == '3' || moveString[1] == '1' || moveString[1] == '2'))
            {
                move[1] = byte.Parse(moveString[0].ToString());
                move[0] = byte.Parse(moveString[1].ToString());
                move[0]--;
                move[1]--;
            }
            else if (moveString == "reset")
            {
                Game.PlayGame();
            }
            else if(moveString == "exit")
            {
                Game.EndGame();
            }
            else
            {
                Console.WriteLine("Wrong coordinates!");
                move = NextMove();
            }
            if (Game.gameMap[move[0], move[1]] != 0)
            {
                Console.WriteLine("Already taken!");
                move = NextMove();
            }
            return move;
        }
    }
}

