namespace TicTacToe;

public class Player
{
    // fields
    private string _name;
    private readonly char _symbol;
    private bool _isActive;
    private int roundsWon;

    // constructors
    public Player(string name, char symbol)
    {
        _name = name;
        _symbol = symbol;
        _isActive = false;
    }

    public Player()
    {
        _name = string.Empty;
        _symbol = Char.MinValue;
    }

    // methods
    public Tuple<int, int> GetMove(Board board)
    {
        ConsoleKey key;
        int left = 1;
        int top = 1;

        DisplayMove(left, top, board);

        do
        {
            key = Console.ReadKey().Key;
            board.Display();

            if (key == ConsoleKey.DownArrow && top < 3)
                top++;

            if (key == ConsoleKey.UpArrow && top > 1)
                top--;

            if (key == ConsoleKey.LeftArrow && left > 1)
                left -= board.IsSeparator(left - 1) ? 2 : 1;

            if (key == ConsoleKey.RightArrow && left < 5)
                left += board.IsSeparator(left + 1) ? 2 : 1;

            DisplayMove(left, top, board);
        } while (key != ConsoleKey.Enter);

        // 0123456
        // ------- 0
        // -x-x-x- 1
        // -x-x-x- 2
        // -x-x-x- 3
        // ------- 4

        return GetRowAndColFromCursorPos(left, top);
    }

    private void DisplayMove(int left, int top, Board board)
    {
        ChangeSymbolColor(left, top, board);
        Console.SetCursorPosition(left, top);
        Console.Write(_symbol);
        Console.ResetColor();
        Console.SetCursorPosition(0, 7);
        Console.WriteLine($"{_name}'s turn");
    }

    private void ChangeSymbolColor(int left, int top, Board board)
    {
        Tuple<int, int> pos = GetRowAndColFromCursorPos(left, top);

        Console.ForegroundColor = board.IsValidMove(pos.Item1, pos.Item2)
            ? ConsoleColor.Green
            : ConsoleColor.Red;
    }

    private Tuple<int, int> GetRowAndColFromCursorPos(int left, int top)
    {
        int col = 0;

        if (left == 3)
            col = 1;

        if (left == 5)
            col = 2;

        return new Tuple<int, int>(top - 1, col);
    }

    // properties
    public string Name
    {
        get => _name;
        set => _name = value ?? throw new ArgumentNullException(nameof(value));
    }

    public char Symbol => _symbol;

    public bool IsActive
    {
        get => _isActive;
        set => _isActive = value;
    }

    public int RoundsWon
    {
        get => roundsWon;
        set => roundsWon = value;
    }
}
