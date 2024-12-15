namespace TicTacToe;

public class Board
{
    // fields
    private char[,] _board;
    private readonly Random _rand;

    // constructors
    public Board()
    {
        _board = new char[3, 3];
        _rand = new Random();
    }

    // methods
    /// <summary>
    /// Initializes an empy board
    /// </summary>
    public void Initialize()
    {
        _board = new char[3, 3];

        for (int i = 0; i < _board.GetLength(0); i++)
        {
            for (int j = 0; j < _board.GetLength(1); j++)
            {
                // _board[i, j] = _rand.Next(2) == 0 ? 'X' : 'O';
            }
        }
    }

    /// <summary>
    /// Displays the actual state of the board
    /// </summary>
    public void Display()
    {
        Console.Clear();

        Console.WriteLine("-------");
        for (int i = 0; i < _board.GetLength(0); i++)
        {
            Console.Write('-');

            for (int j = 0; j < _board.GetLength(1); j++)
            {
                if (j == 1)
                    Console.Write('-');

                Console.Write(_board[i, j] == char.MinValue ? ' ' : _board[i, j]);

                if (j == 1)
                    Console.Write('-');
            }

            Console.Write('-');
            Console.WriteLine();
        }
        Console.WriteLine("-------");
    }

    /// <summary>
    /// Checks if the
    /// </summary>
    /// <param name="row"></param>
    /// <param name="col"></param>
    /// <returns></returns>
    public bool IsValidMove(int row, int col)
    {
        return _board[row, col] == char.MinValue;
    }

    public void MakeMove(char symbol, int row, int col)
    {
        _board[row, col] = symbol;
    }

    public bool CheckWin(Player player)
    {
        char symbol = player.Symbol;

        // vízszintesen
        if (_board[0, 0] == symbol && _board[0, 1] == symbol && _board[0, 2] == symbol)
            return true;

        if (_board[1, 0] == symbol && _board[1, 1] == symbol && _board[1, 2] == symbol)
            return true;

        if (_board[2, 0] == symbol && _board[2, 1] == symbol && _board[2, 2] == symbol)
            return true;

        // függőlegesen
        if (_board[0, 0] == symbol && _board[1, 0] == symbol && _board[2, 0] == symbol)
            return true;

        if (_board[0, 1] == symbol && _board[1, 1] == symbol && _board[2, 1] == symbol)
            return true;

        if (_board[0, 2] == symbol && _board[1, 2] == symbol && _board[2, 2] == symbol)
            return true;

        // átlóba
        if (_board[0, 0] == symbol && _board[1, 1] == symbol && _board[2, 2] == symbol)
            return true;

        if (_board[0, 2] == symbol && _board[1, 1] == symbol && _board[2, 0] == symbol)
            return true;

        return false;
    }

    public bool IsFull()
    {
        bool result = true;

        for (int i = 0; i < _board.GetLength(0); i++)
        {
            for (int j = 0; j < _board.GetLength(1); j++)
            {
                if (_board[i, j] == char.MinValue)
                {
                    result = false;
                    break;
                }
            }
        }
        return result;
    }

    /// <summary>
    /// Checks if the given colId is separator ('-')
    /// </summary>
    /// <param name="left">colId</param>
    /// <returns></returns>
    public bool IsSeparator(int left)
    {
        return left is 2 or 4;
    }
}
