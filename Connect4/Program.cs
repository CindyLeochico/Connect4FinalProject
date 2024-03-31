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

    }

    private void SwitchPlayer()
    {

    }

    private bool CheckGameOver()
    {
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
}
