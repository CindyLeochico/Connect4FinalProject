namespace ConenctFourGame
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Create a new game controller instance and start the game.
            GameController gameController = new GameController();
            gameController.StartGame();
        }
    }
    public class GameController
    {
        private Board board; // Manages the game board's state
        private Player currentPlayer; // Tracks the current player for turn management
        private HumanPlayer player1; // Represents player 1 using 'X' as their symbol
        private HumanPlayer player2; // Represents player 2 using 'O' as their symbol
        private GameView view; // Manages game-related displays and user interaction

        public GameController()
        {
            // Initialize the game components and set the starting player.
            board = new Board();
            player1 = new HumanPlayer('X');
            player2 = new HumanPlayer('O');
            currentPlayer = player1; // Start the game with player 1
            view = new GameView();
        }

        public void StartGame()
        {
            view.DisplayTeamName(); // Introduce the game and the team behind it

            bool gameRunning = true;
            while (gameRunning) // Main game loop to handle turns and game state
            {
                
                view.DisplayBoard(board); // Display the current state of the board              
                view.DisplayTurn(currentPlayer); // Announce the current player's turn

                // Execute a turn and update the board
                int columnChoice = currentPlayer.MakeMove(board);
                bool moveSuccessful = board.DropDisc(columnChoice, currentPlayer.Symbol);

                if (moveSuccessful)
                {
                    // Check for a win or a draw
                    if (board.IsWinningMove(columnChoice, currentPlayer.Symbol))
                    {
                        view.DisplayWinner(currentPlayer); // Announce the winner
                        RestartOrExitGame(); // Offer a restart or exit option
                        return;
                    }
                    else if (board.IsFull())
                    {
                        view.DisplayDraw(); // Announce a draw if the board is full
                        RestartOrExitGame(); // Offer a restart or exit option
                        return; 
                    }
                    else
                    {                    
                        currentPlayer = (currentPlayer == player1) ? player2 : player1; // Switch the current player if the game is still going
                    }
                }
                else
                {
                    view.DisplayInvalidMove(); // Prompt for a valid move if the attempted move is invalid
                }
            }
        }

        public void PlayTurn()
        {
            bool moveMade = false;
            while (!moveMade)
            {
                int column = currentPlayer.MakeMove(board); // Get column from player

                if (!board.DropDisc(column, currentPlayer.Symbol))
                {
                    view.DisplayInvalidMove(); // Show error message for invalid move
                    // The loop will continue, prompting the player for a new move
                }
                else
                {
                    moveMade = true; // Valid move was made, exit the loop
                }
            }
        }

        private void SwitchPlayer()
        {
            currentPlayer = (currentPlayer == player1) ? player2 : player1; // Toggle between player 1 and player 2 to switch turns.
        }

    private void RestartOrExitGame() {

        // Prompt the user to decide whether to restart the game or exit.
        view.DisplayRestartPrompt();
        string input = Console.ReadLine()!;
        switch (input) {
            case "1":
            case "yes":
            case "Yes":
            case "Y":
            case "y":
                // Clear the board and reset the game state
                board.Reset();
                currentPlayer = player1;
                StartGame(); // Restart the game
                break;
            case "0":
            case "no":
            case "No":
            case "N":
            case "n":
                view.DisplayExitMessage(); // This would display a goodbye message.
                Environment.Exit(0); // Exit the game
                break;
            default:
                view.DisplayInvalidMove();
                RestartOrExitGame(); // Recursively call itself to handle invalid input
                break;
        }
    }

        private bool CheckGameOver()
        {
            // Check all win conditions for the current player
            if (board.HasWon(currentPlayer.Symbol))
            {
                view.DisplayWinner(currentPlayer);
                return true;
            }

            // Check if the board is full and therefore the game is a draw
            if (board.IsFull())
            {
                view.DisplayDraw();
                return true;
            }

            // If neither condition is met, the game is not over
            return false;
        }

    }

    public class Board
    {
        private char[,] grid;
        public static readonly int Rows = 6;
        public static readonly int Columns = 7;

        public Board()
        {
            grid = new char[Rows, Columns];
            // Initialize the grid with an empty value
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    grid[i, j] = '\0';
                }
            }
        }

        // Attempt to drop a disc into the specified column
        public bool DropDisc(int column, char symbol)
        {
            if (column < 0 || column >= Columns)
            {
                return false; // Column out of bounds
            }

            for (int row = Rows - 1; row >= 0; row--)
            {
                if (grid[row, column] == '\0')
                {
                    grid[row, column] = symbol;
                    return true; // Successfully placed the disc
                }
            }

            return false; // The column is full, disc not placed
        }


        public bool IsWinningMove(int column, char symbol)
        {
            // Find the row index of the topmost disc in the column
            int row = -1;
            for (int i = 0; i < Rows; i++)
            {
                if (grid[i, column] == symbol)
                {
                    row = i;
                    break;
                }
            }

            // If no disc is found in the column for the symbol, return false
            if (row == -1) return false;

            // Check for a horizontal, vertical, or diagonal win starting from the position (row, column)
            return CheckHorizontalWin(row, column, symbol) ||
                   CheckVerticalWin(column, symbol) ||
                   CheckDiagonalWin(row, column, symbol);
        }


        private int FindRowForColumn(int column, char symbol)
        {
            for (int i = 0; i < Rows; i++)
            {
                if (grid[i, column] == symbol)
                {
                    return i;
                }
            }
            return -1; // No disc found in this column for the symbol, or the column is out of bounds
        }

        private bool CheckVerticalWin(int startColumn, char symbol)
        {
            // Starting from the bottom of the column where the last disc was placed
            // and moving up, check if there are 4 in a row.
            int consecutiveCount = 0;
            for (int row = Rows - 1; row >= 0; row--)
            {
                if (grid[row, startColumn] == symbol)
                {
                    consecutiveCount++;
                    if (consecutiveCount == 4)
                    {
                        return true;
                    }
                }
                else
                {
                    consecutiveCount = 0; // Reset the count if the sequence is broken
                }
            }
            return false;
        }

        private bool CheckHorizontalWin(int row, int column, char symbol)
        {
            // Check left and right from the column
            int count = 0;
            for (int i = column; i >= 0 && grid[row, i] == symbol; i--) count++;
            for (int i = column + 1; i < Columns && grid[row, i] == symbol; i++) count++;
            return count >= 4;
        }

        private bool CheckDiagonalWin(int row, int column, char symbol)
        {
            // Check for a diagonal win in both directions
            int count = 0;
            // Check diagonal (bottom-left to top-right)
            for (int i = 0; i < 4; i++)
            {
                if (row - i < 0 || column + i >= Columns || grid[row - i, column + i] != symbol) break;
                count++;
            }
            if (count >= 4) return true;

            count = 0;
            // Check diagonal (top-left to bottom-right)
            for (int i = 0; i < 4; i++)
            {
                if (row + i >= Rows || column + i >= Columns || grid[row + i, column + i] != symbol) break;
                count++;
            }
            return count >= 4;
        }

        public bool HasWon(char symbol)
        {
            // Check horizontal lines for a win
            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col <= Columns - 4; col++)
                {
                    if (grid[row, col] == symbol &&
                        grid[row, col + 1] == symbol &&
                        grid[row, col + 2] == symbol &&
                        grid[row, col + 3] == symbol)
                    {
                        return true;
                    }
                }
            }

            // Check vertical lines for a win
            for (int col = 0; col < Columns; col++)
            {
                for (int row = 0; row <= Rows - 4; row++)
                {
                    if (grid[row, col] == symbol &&
                        grid[row + 1, col] == symbol &&
                        grid[row + 2, col] == symbol &&
                        grid[row + 3, col] == symbol)
                    {
                        return true;
                    }
                }
            }

            // Check diagonal lines (bottom-left to top-right) for a win
            for (int row = 3; row < Rows; row++)
            {
                for (int col = 0; col <= Columns - 4; col++)
                {
                    if (grid[row, col] == symbol &&
                        grid[row - 1, col + 1] == symbol &&
                        grid[row - 2, col + 2] == symbol &&
                        grid[row - 3, col + 3] == symbol)
                    {
                        return true;
                    }
                }
            }

            // Check diagonal lines (top-left to bottom-right) for a win
            for (int row = 0; row <= Rows - 4; row++)
            {
                for (int col = 0; col <= Columns - 4; col++)
                {
                    if (grid[row, col] == symbol &&
                        grid[row + 1, col + 1] == symbol &&
                        grid[row + 2, col + 2] == symbol &&
                        grid[row + 3, col + 3] == symbol)
                    {
                        return true;
                    }
                }
            }

            return false;
        }


        public bool IsFull()
        {
            // Check if all columns in the top row (the entry row for discs) are filled
            for (int col = 0; col < grid.GetLength(1); col++)
            {
                if (grid[0, col] == '\0')
                {
                    return false; // Found an empty space, so the board is not full
                }
            }
            return true; // No empty spaces found in the top row, so the board is full
        }

        private bool CheckHorizontalWin(char symbol)
        {
            // Loop through each row of the game board
            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Columns - 3; col++)
                {
                    if (grid[row, col] == symbol &&
                        grid[row, col + 1] == symbol &&
                        grid[row, col + 2] == symbol &&
                        grid[row, col + 3] == symbol)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool CheckVerticalWin(char symbol)
        {
            // Loop through each column of the board. The outer loop iterates over each column to check for vertical wins.
            for (int col = 0; col < Columns; col++)
            {
                for (int row = 0; row < Rows - 3; row++)
                {
                    if (grid[row, col] == symbol &&
                        grid[row + 1, col] == symbol &&
                        grid[row + 2, col] == symbol &&
                        grid[row + 3, col] == symbol)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool CheckDiagonalWin(char symbol)
        {
            // Check for diagonals that go from the top-left to the bottom-right
            for (int row = 0; row < Rows - 3; row++)
            {
                for (int col = 0; col < Columns - 3; col++)
                {
                    if (grid[row, col] == symbol &&
                        grid[row + 1, col + 1] == symbol &&
                        grid[row + 2, col + 2] == symbol &&
                        grid[row + 3, col + 3] == symbol)
                    {
                        return true;
                    }
                }
            }

            // Check for diagonals that go from the bottom-left to the top-right
            for (int row = 3; row < Rows; row++)
            {
                for (int col = 0; col < Columns - 3; col++)
                {
                    if (grid[row, col] == symbol &&
                        grid[row - 1, col + 1] == symbol &&
                        grid[row - 2, col + 2] == symbol &&
                        grid[row - 3, col + 3] == symbol)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool CanPlaceDisc(int column)
        {
            // Check if a disc can be placed in the specified column.
            return grid[0, column] == '\0';
        }

        public char GetCell(int row, int col)
        {
            // Retrieve the symbol from a specific cell in the grid.
            return grid[row, col];
        }

        public void Reset()
        {
            // Iterate over each row of the grid
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    grid[i, j] = '\0';
                }
            }
        }
    }

    public abstract class Player
    {
        // Protected member 'symbol' holds the character that represents the player's discs on the board.
        protected char symbol;

        protected Player(char symbol)
        {
            this.symbol = symbol;
        }

        public char Symbol
        {
            get { return symbol; }
        }

        public abstract int MakeMove(Board board);
    }

    // HumanPlayer class inherits from Player, focusing on human-specific interactions for making moves.
    public class HumanPlayer : Player
    {
        // Constructor that calls the base Player class constructor with a symbol for the human player ('X' or 'O').
        public HumanPlayer(char symbol) : base(symbol) { }

        public override int MakeMove(Board board)
        {
            bool validInput = false;
            int column = -1;

            while (!validInput)
            {
                Console.WriteLine($"Player {symbol}, enter your column choice (1-{Board.Columns}): ");
                string input = Console.ReadLine()!;

                if (int.TryParse(input, out column) && column >= 1 && column <= Board.Columns)
                {        
                    column--;
                    if (board.CanPlaceDisc(column))
                    {
                        validInput = true;
                    }
                    else
                    {
                        Console.WriteLine("That column is full. Please try a different column.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a number corresponding to a column.");
                }
            }

            return column;
        }
    }

    public class GameView
    {
        // Display initial welcome message and team information at the start of the game.
        public void DisplayTeamName()
        {
            Console.WriteLine("Welcome to our Connect Four Game! (Object-Oriented Program (OOP))");
            Console.WriteLine("Developed by: Team Fourward Thinkers (Johnson Benedict Corpus, Cindy April Leochico)\n");
        }

        // Display the current state of the game board to the console.
        public void DisplayBoard(Board board)
        {
            for (int row = 0; row < Board.Rows; row++)
            {
                for (int col = 0; col < Board.Columns; col++)
                {
                    char symbol = board.GetCell(row, col);
                    Console.Write(symbol == '\0' ? "." : symbol.ToString());
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
            Console.WriteLine("1 2 3 4 5 6 7");
            Console.WriteLine();
        }

        // Display whose turn it is in the game.
        public void DisplayTurn(Player currentPlayer)
        {
            Console.WriteLine($"It is Player {currentPlayer.Symbol}'s turn.");
        }

        // Prompt for game restart or exit after a game ends.
        public void DisplayRestartPrompt()
        {
            Console.WriteLine("Game over. Would you like to play again? Press (Y/1 or N/0)");
        }

        // Display a message thanking the player when they choose to exit the game.
        public void DisplayExitMessage()
        {
            Console.WriteLine("Thank you for playing! Bye.");
        }

        // Display a message when an invalid move is attempted.
        public void DisplayInvalidMove()
        {
            Console.WriteLine("Invalid move. Please try again.");
        }

        // Announce the winner of the game.
        public void DisplayWinner(Player currentPlayer)
        {
            Console.WriteLine($"Congratulations! Player {currentPlayer.Symbol} has won the game!");
        }

        // Inform both players that the game has ended in a draw.
        public void DisplayDraw()
        {
            Console.WriteLine("The game is a draw. There are no more moves possible.");
        }
    }
}

