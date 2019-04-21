using System;
using System.Linq;
using System.Threading;

namespace TicTacToe
{
    /// <summary>
    /// Static class responsible for keeping game info and playing game in general
    /// </summary>
    public static class Game
    {
        /// <summary>
        /// Player types
        /// </summary>
        public enum PlayerType
        {
            human,
            easy,
            medium,
            hard
        };
        ///
        public static Player player1;
        public static Player player2;
        public static int[,] gameMap;
        private static bool seamlessPlay;
        public static int GameCounter { get; set; } = 0;
        /// <summary>
        /// Creating new player based on selected player type
        /// </summary>
        /// <param name="type">Player type to create</param>
        /// <param name="name">Put on board when printing and in comments</param>
        /// <param name="number">Used for calculations to distinguish from other player</param>
        /// <returns>New player object</returns>
        public static Player CreatePlayer(PlayerType type, string name, int number)
        {
            switch (type)
            {
                case PlayerType.human:
                    return new Human(name, number);
                case PlayerType.easy:
                    return new AIEasy(name, number);
                case PlayerType.medium:
                    return new AIMedium(name, number);
                case PlayerType.hard:
                    return new AIHard(name, number);
                default:
                    throw new Exception("Wrong player type");
            }
        }
        /// <summary>
        /// Method setting up and initializing new game
        /// </summary>
        public static void InitializeGame()
        {
            PlayerType playerType1;
            PlayerType playerType2;
            bool player1Correct, player2Correct, seamlessCorrect;
            Console.WriteLine("Player 1 - human/easy/medium/hard?");

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

            //Decides if after every round game will start new round, or ask to be run again or terminated
            Console.WriteLine("Do you want to continiue playing, or decide after every game? Or do you need help? y/n/help");

            do
            {
                string answer = Console.ReadLine();
                if (answer == "y")
                {
                    seamlessPlay = true;
                    seamlessCorrect = true;
                }
                else if(answer == "n")
                {
                    seamlessPlay = false;
                    seamlessCorrect = true;
                }
                else if (answer == "help")
                {
                    seamlessCorrect = false;
                    Console.WriteLine("To play write coordinates, first column then row.For example type 21\n" +
                        "To reset game type reset\n" +
                        "To exit game type exit\n" +
                        "AI vs AI game with continuos play cannot be terminated by command. Just exit application");
                }
                else
                {
                    seamlessCorrect = false;
                    Console.WriteLine("Wrong answer, please try again y or n");
                }
            }
            while (!seamlessCorrect);

            player1 = CreatePlayer(playerType1, "X", 1);
            player2 = CreatePlayer(playerType2, "O", 10);
            PlayGame();
        }
        /// <summary>
        /// Methid for printing game board to console
        /// </summary>
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
        /// <summary>
        /// Decides which player moves next
        /// </summary>
        /// <param name="move">Move counter based on which next player is decided</param>
        /// <returns>Next player</returns>
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
        /// <summary>
        /// Method responsible for performing whole round
        /// </summary>
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
            Console.WriteLine($"Number of games played: {GameCounter}\nCurrent score: {player1.Name}: {player1.Score}, {player2.Name}: {player2.Score}");

            if (!seamlessPlay)
            {
                NewGame(); 
            }
            else
            {
                Thread.Sleep(1000);
                PlayGame(); 
            }
        }
        /// <summary>
        /// Ending game and exiting program if ordered to
        /// </summary>
        public static void EndGame()
        {
            Console.Clear();
            Console.WriteLine($"Thank you for playing!\nFinal score:\n{player1.Name}: {player1.Score}\n{player2.Name}: {player2.Score}");
            Environment.Exit(0);
        }
        /// <summary>
        /// Deciding what to do after round has finished
        /// </summary>
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
        /// <summary>
        /// Calculating array of sums of rows/columns/diagonals that can be used to decide next move by AI 
        /// and also to decide if player has won
        /// </summary>
        /// <returns>Array of sums of rows/columns/diagonals</returns>
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
        /// <summary>
        /// Checking if player has won
        /// </summary>
        /// <param name="player">Player to check</param>
        /// <returns>Information if player has won</returns>
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
}

