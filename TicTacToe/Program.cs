using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TicTacToe
{
    class Program
    {
        static void Main(string[] args)
        {
            Game.InitializeGame();
        }
    }

    public static class Game
    {
        public enum PlayerType
        {
            human,
            easy,
            hard
        };

        public static Player player1;
        public static Player player2;
        public static int[,] gameMap;
        public static int GameCounter { get; set; } = 0;

        public static Player CreatePlayer(PlayerType type, string name, int number)
        {
            switch (type)
            {
                case PlayerType.human:
                    return new Human(name, number);
                case PlayerType.easy:
                    return new AIEasy(name, number);
                case PlayerType.hard:
                    return new AIHard(name, number);
                default:
                    throw new Exception("Wrong player type");
            }
        }

        public static void InitializeGame()
        {
            PlayerType playerType1;
            PlayerType playerType2;
            bool player1Correct, player2Correct;
            Console.WriteLine("Player 1 - human/easy/hard?");

            do
            {
                player1Correct = Enum.TryParse(Console.ReadLine(), out playerType1);
                if (!player1Correct)
                {
                    Console.WriteLine("Wrong player type, please try again.");
                }
            }
            while (!player1Correct);
            Console.WriteLine("Player 2 - human/easy/hard?");
            do
            {
                player2Correct = Enum.TryParse(Console.ReadLine(), out playerType2);
                if (!player2Correct)
                {
                    Console.WriteLine("Wrong player type, please try again.");
                }
            }
            while (!player2Correct);

            player1 = CreatePlayer(playerType1, "X", 1);
            player2 = CreatePlayer(playerType2, "O", 10);
            PlayGame();
        }

        public static void PrintGame()
        {
            string textTmp;
            Console.Clear();
            Console.WriteLine("  1   2   3");
            for (int i = 0; i < 3; i++)
            {
                Console.Write($"{i + 1} ");
                for (int j = 0; j < 3; j++)
                {
                    textTmp = (gameMap[i, j] == 0) ? "  " : (gameMap[i, j] == player1.NumericValue) ? $"{player1.Name} " : $"{player2.Name} ";
                    Console.Write(textTmp);
                    if (j < 2) Console.Write("| ");
                }
                Console.WriteLine();
                if (i < 2) Console.WriteLine("  ---------");
            }
        }

        private static Player SelectPlayer(int move)
        {
            if ((move + GameCounter) % 2 == 0)
            {
                return player1;
            }
            else
            {
                return player2;
            }
        }

        public static void PlayGame()
        {
            gameMap = new int[3, 3];
            PrintGame();
            int movesDone = 0;
            byte[] nextPosition;
            Player currentPlayer;
            bool won = false;

            while (movesDone < 9)
            {
                currentPlayer = SelectPlayer(movesDone);
                nextPosition = currentPlayer.NextMove();
                gameMap[nextPosition[0], nextPosition[1]] = currentPlayer.NumericValue;
                if (CheckIfWon(currentPlayer))
                {
                    PrintGame();
                    currentPlayer.Score++;
                    Console.WriteLine($"{currentPlayer.Name} has won!");
                    won = true;
                    break;
                }
                movesDone++;
                PrintGame();
            }
            if (!won)
            {
                Console.WriteLine("It's a tie!");
            }
            GameCounter++;
            Console.WriteLine($"Current score: {player1.Name}: {player1.Score}, {player2.Name}: {player2.Score}");
            NewGame();
        }

        private static void EndGame()
        {
            Console.Clear();
            Console.WriteLine($"Thank you for playing!\nFinal score:\n{player1.Name}: {player1.Score}\n{player2.Name}: {player2.Score}");
            Environment.Exit(0);
        }

        private static void NewGame()
        {
            Console.WriteLine("Do you want to play again? y/n/reset");
            string answer = Console.ReadLine();
            switch (answer)
            {
                case "y":
                    PlayGame();
                    return;
                case "n":
                    EndGame();
                    return;
                case "reset":
                    InitializeGame();
                    return;
                default:
                    Console.WriteLine("Wrong command");
                    NewGame();
                    return;
            }
        }

        public static int[] MapSums()
        {
            //1,2,3 - horizontal, 4,5,6 - vertical, 7 - descending, 8 - ascending
            int[] mapSums = new int[8];

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    mapSums[i] += gameMap[i, j];
                }
            }
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    mapSums[i + 3] += gameMap[j, i];
                }
            }
            mapSums[6] = gameMap[0, 0] + gameMap[1, 1] + gameMap[2, 2];
            mapSums[7] = gameMap[0, 2] + gameMap[1, 1] + gameMap[2, 0];

            return mapSums;
        }

        private static bool CheckIfWon(Player player)
        {
            int[] mapSums = MapSums();
            if (mapSums.Contains(3) || mapSums.Contains(30))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

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

    public abstract class AI : Player
    {
        protected static Random rnd = new Random();
        protected List<byte[]> emptyCells = new List<byte[]>();

        protected static void SimulateThinking()
        {
            Thread.Sleep(rnd.Next(50, 150));
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

    public class AIHard : AI
    {
        private int indexOfFound;
        private List<int> indexOfNotPReffered = new List<int>();

        public AIHard(string name, int number) : base(name, number)
        {
        }

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


        public override byte[] NextMove()
        {
            FindEmptyCells();
            SimulateThinking();
            FindIndexOfSumsArray();
            return NextFree();
        }
    }

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

