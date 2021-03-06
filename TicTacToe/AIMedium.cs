﻿using System;
using System.Linq;

namespace TicTacToe
{
    /// <summary>
    /// Class for AIMedium 
    /// Intermediate between Easy and Hard.
    /// This class is aware of potential win on both, itself and oponent side and takes action to win or to block opponent from wining, 
    /// otherwise selects random position
    /// </summary>
    public class AIMedium : AI
    {
        private int indexOfFound;
        /// <summary>
        /// Constructor for class AIMedium
        /// </summary>
        /// <param name="name">Put on board when printing and in comments</param>
        /// <param name="number">Used for calculations to distinguish from other player</param>
        public AIMedium(string name, int number) : base(name, number)
        {
        }
        /// <summary>
        /// First part of AI, deciding about next move
        /// </summary>
        private void FindIndexOfSumsArray()
        {
            int[] mapSums = Game.MapSums();
            int EnemyNumericValue = NumericValue == 1 ? 10 : 1;

            //return 8 to indicate middle value
            if (mapSums.Sum() == 0 || (Game.gameMap[1,1] == 0) && (!mapSums.Contains(20) || !mapSums.Contains(2)))
            {
                indexOfFound = 8;
            }
            else if (mapSums.Contains(NumericValue * 2))
            {
                indexOfFound = Array.IndexOf(mapSums, NumericValue * 2);
            }
            else if (mapSums.Contains(EnemyNumericValue * 2))
            {
                indexOfFound = Array.IndexOf(mapSums, EnemyNumericValue * 2);
            }
            //return 9 to indicate random value
            else
            {
                indexOfFound = 9;
            }

        }
        /// <summary>
        /// Second part of AI executing next move based on FindIndexOfSumsArray decision 
        /// </summary>
        /// <returns>Coordinates of next moves decided by AI</returns>
        private byte[] NextFree()
        {
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
                    return new byte[] { 1, 1 };
                default:

                    return emptyCells[rnd.Next(0, emptyCells.Count())];
            }
            return new byte[] { 3, 3 };
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
            return NextFree();
        }
    }
}

