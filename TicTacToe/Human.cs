using System;

namespace TicTacToe
{
    public class Human : Player
    {
        public Human(string name, int number) : base(name, number)
        {
        }

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

