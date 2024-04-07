public class GameController
{
    private Board board;
    private Player currentPlayer;
    private HumanPlayer player1;
    private HumanPlayer player2;

    public GameController()
    {
        board = new Board();
        player1 = new HumanPlayer('X');
        player2 = new HumanPlayer('O');
        currentPlayer = player1;
    }

    public void StartGame()
    {
        GameView view = new GameView();
        bool gameRunning = true;

        while (gameRunning)
        {
            // Display the current state of the board
            view.DisplayBoard(board);

            // Announce the current player's turn
            view.DisplayTurn(currentPlayer);

            // Get the column from the current player
            int column = currentPlayer.MakeMove(board);

            // Attempt to drop the disc into the board
            bool moveSuccessful = board.DropDisc(column, currentPlayer.Symbol);

            // If the move is successful, check for a win or draw
            if (moveSuccessful)
            {
                if (board.IsWinningMove(column, currentPlayer.Symbol))
                {
                    view.DisplayBoard(board);
                    view.DisplayWinner(currentPlayer);
                    gameRunning = false;
                }
                else if (board.IsDraw())
                {
                    view.DisplayBoard(board);
                    view.DisplayDraw();
                    gameRunning = false;
                }
                else
                {
                    // If the game hasn't been won or drawn, switch players
                    SwitchPlayer();
                }
            }
            else
            {
                // If the move wasn't successful (column full or invalid), prompt again
                view.DisplayInvalidMove();
            }
        }

        // End of game, offer restart or exit
        RestartOrExitGame();
    }


    private void SwitchPlayer()
    {
        currentPlayer = (currentPlayer == player1) ? player2 : player1;
    }

    private void RestartOrExitGame()
    {
        GameView view = new GameView();
        view.DisplayRestartPrompt(); // This would prompt the user with a message to restart or exit.

        string input = Console.ReadLine();
        switch (input)
        {
            case "1":
            case "yes":
            case "Yes":
            case "Y":
            case "y":
                // Clear the board and reset the game state
                board.Reset();
                currentPlayer = player1; // Or randomize starting player
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
                view.DisplayInvalidOption();
                RestartOrExitGame(); // Recursively call itself to handle invalid input
                break;
        }
    }

    private bool CheckGameOver()
    {
        // Check all win conditions for the current player
        if (board.HasWon(currentPlayer.Symbol))
        {
            GameView.DisplayWinner(currentPlayer);
            return true;
        }

        // Check if the board is full and therefore the game is a draw
        if (board.IsFull())
        {
            GameView.DisplayDraw();
            return true;
        }

        // If neither condition is met, the game is not over
        return false;
    }
}

public class Board
{
    private char[,] grid;

    public Board()
    {
        grid = new char[6, 7];
    }

    public bool DropDisc(int column, char symbol)
    {
        return true;
    }

    public bool IsWinningMove(int column, char symbol)
    {
        return false;
    }

    public bool HasWon(char symbol)
    {
        // Check horizontal, vertical, and diagonal lines for a win
        return CheckHorizontalWin(symbol) || CheckVerticalWin(symbol) || CheckDiagonalWin(symbol);
    }

    public bool IsFull()
    {
        // Check if all columns in the top row (the entry row for discs) are filled
        for (int col = 0; col < grid.GetLength(1); col++)
        {
            if (grid[0, col] == '\0')
            { // Assuming the default value of '\0' for empty cells
                return false; // Found an empty space, so the board is not full
            }
        }
        return true; // No empty spaces found in the top row, so the board is full
    }

    private bool CheckHorizontalWin(char symbol)
    {
        // Implement logic to check for 4 in a row horizontally
    }

    private bool CheckVerticalWin(char symbol)
    {
        // Implement logic to check for 4 in a row vertically
    }

    private bool CheckDiagonalWin(char symbol)
    {
        // Implement logic to check for 4 in a row diagonally
    }
}

public abstract class Player
{
    protected char symbol;

    protected Player(char symbol)
    {
        this.symbol = symbol;
    }

    public abstract int MakeMove(Board board);
}

public class HumanPlayer : Player
{
    public HumanPlayer(char symbol) : base(symbol) { }

    public override int MakeMove(Board board)
    {
        return 0;
    }
}

public class GameView
{
    public void DisplayBoard(Board board)
    {

    }

    public void DisplayTurn(Player player)
    {
     
    }

    public void DisplayRestartPrompt()
    {
        Console.WriteLine("Game over. Would you like to play again? (Yes/1 or No/0)");
    }

    public void DisplayExitMessage()
    {
        Console.WriteLine("Thank you for playing! Goodbye.");
    }

    public void DisplayInvalidOption()
    {
        Console.WriteLine("Invalid option, please try again.");
    }

    public static void DisplayWinner(Player player)
    {
        Console.WriteLine($"{player.Name} has won the game!");
    }

    public static void DisplayDraw()
    {
        Console.WriteLine("The game is a draw. No more moves possible.");
    }
}
