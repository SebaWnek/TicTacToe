using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    /// <summary>
    /// Class responsible for hard AI
    /// Coded based on http://www.cs.jhu.edu/~jorgev/cs106/ttt.pdf
    /// Should never lose, only win or draw
    /// </summary>
    public class AIHard : AI
    {
        private int indexOfFound;
        private int[] mapSums;
        private int EnemyNumericValue;
        /// <summary>
        /// Constructor for class AIHard
        /// </summary>
        /// <param name="name">Put on board when printing and in comments</param>
        /// <param name="number">Used for calculations to distinguish from other player</param>
        public AIHard(string name, int number) : base(name, number)
        {
            EnemyNumericValue = NumericValue == 1 ? 10 : 1;
        }
        /// <summary>
        /// First part of AI, deciding about next move
        /// </summary>
        private void FindIndexOfSumsArray()
        {
            mapSums = Game.MapSums();

            //first step
            //return 8 to indicate any corner value
            if (mapSums.Sum() == 0)
            {
                indexOfFound = 8;
            }
            //basic operations, when you have already 2 in a row - win
            else if (mapSums.Contains(NumericValue * 2))
            {
                indexOfFound = Array.IndexOf(mapSums, NumericValue * 2);
            }
            //when enemy has 2 in a row - must block
            else if (mapSums.Contains(EnemyNumericValue * 2))
            {
                indexOfFound = Array.IndexOf(mapSums, EnemyNumericValue * 2);
            }
            //second step when oponent left middle
            //return 9 to indicate middle
            else if (Game.gameMap[1, 1] == 0 && Game.gameMap.Cast<int>().Sum() == EnemyNumericValue)
            {
                indexOfFound = 9;
            }
            //second step when oponent took middle
            //return 8 to indicate any corner
            else if (mapSums.Sum() == 4 * EnemyNumericValue && Game.gameMap[1, 1] == EnemyNumericValue)
            {
                indexOfFound = 8;
            }
            //third step if oponent put in the middle
            else if (Game.gameMap.Cast<int>().Sum() == NumericValue + EnemyNumericValue && 
                (mapSums[6] == EnemyNumericValue + NumericValue || mapSums[7] == EnemyNumericValue + NumericValue))
            {
                indexOfFound = mapSums[6] == EnemyNumericValue + NumericValue ? 6 : 7;
            }
            //fourth step, if second step was middle
            //return 10 to indicate wall
            else if (Game.gameMap[1, 1] == NumericValue && Game.gameMap.Cast<int>().Sum() == EnemyNumericValue * 2 + NumericValue)
            {
                indexOfFound = 10;
            }
            //anyhing else - return 20 to indicate random value
            else
            {
                indexOfFound = 20;
            }

        }
        /// <summary>
        /// Second part of AI executing next move based on FindIndexOfSumsArray decision 
        /// </summary>
        /// <returns>Coordinates of next moves decided by AI</returns>
        private byte[] NextFree()
        {
            bool isThereFreeCorner;

            switch (indexOfFound)
            {
                case 0:
                case 1:
                case 2:
                    for (byte i = 0; i < 3; i++)
                    {
                        if (Game.gameMap[indexOfFound, i] == 0)
                        {
                            return new byte[] { (byte)indexOfFound, i };
                        }
                    }
                    break;
                case 3:
                case 4:
                case 5:
                    for (byte i = 0; i < 3; i++)
                    {
                        if (Game.gameMap[i, indexOfFound - 3] == 0)
                        {
                            return new byte[] { i, (byte)(indexOfFound - 3) };
                        }
                    }
                    break;
                case 6:
                    for (byte i = 0; i < 3; i++)
                    {
                        if (Game.gameMap[i, i] == 0)
                        {
                            return new byte[] { i, i };
                        }
                    }
                    break;
                case 7:
                    for (byte i = 0; i < 3; i++)
                    {
                        if (Game.gameMap[i, 2 - i] == 0)
                        {
                            return new byte[] { i, (byte)(2 - i) };
                        }
                    }
                    break;
                case 8:
                    FindEmptyCorners();
                    return emptyCorners[rnd.Next(emptyCorners.Count())];
                case 9:
                    return new byte[] { 1, 1 };
                case 10:
                    FindEmptyWalls();
                    return CheckNeighbours();

                default:
                    isThereFreeCorner = FindEmptyCorners();
                    if (isThereFreeCorner)
                    {
                        return emptyCorners[rnd.Next(0, emptyCorners.Count())];
                    }
                    else
                    {
                        return emptyCells[rnd.Next(0, emptyCells.Count())];
                    }
            }
            return new byte[] { 3, 3 };
        }
        /// <summary>
        /// Helper method to decide fourth move, when AI started second and first move was in the middle
        /// </summary>
        /// <returns>Coordinates of next moves decided by AI</returns>
        private byte[] CheckNeighbours()
        {
            int sumTmp = 0;
            //All enemy moves were on corners
            if (emptyWalls.Count() == 4)
            {
                return emptyWalls[rnd.Next(emptyWalls.Count())];
            }
            //One enemy move was in corner
            else if (emptyWalls.Count() == 3)
            {
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        if (i == 0)
                        {
                            sumTmp = Game.gameMap[j, 0] + Game.gameMap[j, 1] + Game.gameMap[j, 2] +
                                Game.gameMap[j + 1, 0] + Game.gameMap[j + 1, 1] + Game.gameMap[j + 1, 2];
                            if (sumTmp == EnemyNumericValue * 2 + NumericValue)
                            {
                                return new byte[] { (byte)(j * 2), 1 };
                            }
                        }
                        else
                        {
                            sumTmp = Game.gameMap[0, j] + Game.gameMap[1, j] + Game.gameMap[2, j] +
                                Game.gameMap[0, j + 1] + Game.gameMap[1, j + 1] + Game.gameMap[2, j + 1];
                            if (sumTmp == EnemyNumericValue * 2 + NumericValue)
                            {
                                return new byte[] { 1, (byte)(j * 2) };
                            }
                        }
                    }
                }
            }
            //Both enemy moves were not in corners and were in same row or column
            else if (emptyWalls.Count() == 2 && mapSums[1] == EnemyNumericValue * 2 + NumericValue ||
                mapSums[4] == EnemyNumericValue * 2 + NumericValue)
            {
                return emptyWalls[rnd.Next(emptyWalls.Count())];
            }
            //Both enemy moves were not in corners and are not in same row or column
            else
            {
                for (byte i = 0; i < 2; i++)
                {
                    for (byte j = 0; j < 2; j++)
                    {
                        sumTmp = Game.gameMap[i, j] + Game.gameMap[i, j + 1] +
                                    Game.gameMap[i + 1, j] + Game.gameMap[i + 1, j + 1];
                        if (sumTmp == EnemyNumericValue * 2 + NumericValue)
                        {
                            return new byte[] { (byte)(i * 2), (byte)(j * 2) };
                        }
                    }
                }
            }
            //Should never be invoked
            Console.WriteLine("Something went wrong!");
            return new byte[] { 0, 0 };
        }
        /// <summary>
        /// Inherited methos, calculates and sends to Game coordinates that AI decided
        /// </summary>
        /// <returns>Coordinates of next moves decided by AI</returns>
        public override byte[] NextMove()
        {
            FindEmptyCells();
            SimulateThinking();
            FindIndexOfSumsArray();
            byte[] next = NextFree();
            //Just to be sure it wont select already used position, will select new one, or loop
            if(Game.gameMap[next[0],next[1]] != 0)
            {
                next = NextMove();
            }
            return next;
        }
    }
}
