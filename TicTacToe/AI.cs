using System;
using System.Collections.Generic;
using System.Threading;

namespace TicTacToe
{
    public abstract class AI : Player
    {
        protected static Random rnd = new Random();
        protected List<byte[]> emptyCells = new List<byte[]>();
        protected List<byte[]> emptyCorners = new List<byte[]>();

        protected static void SimulateThinking()
        {
            Thread.Sleep(rnd.Next(50, 150));
        }

        protected bool FindEmptyCorners(out List<byte[]> emptyCorners)
        {
            var empty = emptyCells.FindAll(x => x[0] == x[1] && x[0] != 1);
            emptyCorners = empty;
            if (empty.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        protected void FindEmptyCells()
        {
            emptyCells.Clear();
            for (byte i = 0; i < 3; i++)
            {
                for (byte j = 0; j < 3; j++)
                {
                    if (Game.gameMap[i, j] == 0)
                    {
                        emptyCells.Add(new byte[] { i, j });
                    }
                }
            }
        }

        public AI(string name, int number) : base(name, number)
        {
        }
    }
}

