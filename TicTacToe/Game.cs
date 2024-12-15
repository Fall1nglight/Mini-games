namespace TicTacToe;

public class Game
{
    // fields
    private Board _board;
    private Player _playerOne;
    private Player _playerTwo;
    private readonly Random _rand;
    private bool _isGameOver;
    private int _numOfRounds;

    // constructors
    public Game()
    {
        _board = new Board();
        _playerOne = new Player(string.Empty, 'X');
        _playerTwo = new Player(string.Empty, 'O');
        _rand = new Random();
    }

    private void AddPlayers()
    {
        Console.Write("Player one name: ");
        _playerOne.Name = Console.ReadLine()!;
        Console.WriteLine($"{_playerOne.Name} has been added to the game!");

        Console.WriteLine();

        Console.Write("Player two name: ");
        _playerTwo.Name = Console.ReadLine()!;
        Console.WriteLine($"{_playerTwo.Name} has been added to the game!");
    }

    // methods
    /// <summary>
    /// Starts the game
    /// </summary>
    public void Start()
    {
        AddPlayers();
        SelectStartingPlayer(_playerOne, _playerTwo);
        AnnounceStartingPlayer();
        _board.Initialize();
        PromptStartRound();

        while (!_isGameOver)
        {
            if (_board.IsFull())
            {
                HandleFullBoard();
                continue;
            }

            _board.Display();

            Player tmpPlayer = GetActivePlayer(_playerOne, _playerTwo);
            Tuple<int, int> pos = tmpPlayer.GetMove(_board);

            int i = pos.Item1;
            int j = pos.Item2;

            if (_board.IsValidMove(i, j))
                _board.MakeMove(tmpPlayer.Symbol, i, j);

            if (_board.CheckWin(tmpPlayer))
            {
                HandleWinner();
                char asd = PromptAnotherRound();

                if (asd == 'n')
                {
                    _isGameOver = true;
                    break;
                }

                _board.Initialize();
            }

            SwitchPlayer();
        }

        DisplayGameSummary();
    }

    private void PromptStartRound()
    {
        Console.WriteLine("Press enter to start the round!");
        Console.ReadLine();
    }

    private void AnnounceStartingPlayer()
    {
        var startingPlayer = _playerOne.IsActive ? _playerOne.Name : _playerTwo.Name;
        Console.WriteLine($"\n{startingPlayer} starts the round.");
    }

    private void HandleFullBoard()
    {
        _numOfRounds++;
        Console.SetCursorPosition(0, 7);
        Console.WriteLine("The board is full");
        Console.ReadLine();
        _board.Initialize();
    }

    private void HandleWinner()
    {
        Console.SetCursorPosition(0, 7);

        if (_playerOne.IsActive)
        {
            _playerOne.RoundsWon++;
            Console.WriteLine($"{_playerOne.Name} won the round!");
        }
        else
        {
            _playerTwo.RoundsWon++;
            Console.WriteLine($"{_playerTwo.Name} won the round!");
        }

        _numOfRounds++;
    }

    private char PromptAnotherRound()
    {
        char answer;
        do
        {
            Console.Write("\nWould you like to play another round? [y/n] => ");
            answer = Console.ReadKey().KeyChar;

            if (answer == 'n')
            {
                _isGameOver = true;
                break;
            }
        } while (answer != 'y');

        return answer;
    }

    private void DisplayGameSummary()
    {
        Console.WriteLine($"\n\nTotal rounds played: {_numOfRounds}");
        Console.WriteLine($"{_playerOne.Name} won: {_playerOne.RoundsWon} rounds");
        Console.WriteLine($"{_playerTwo.Name} won: {_playerTwo.RoundsWon} rounds");
        Console.ReadLine();
    }

    private void SelectStartingPlayer(Player p1, Player p2)
    {
        if (_rand.Next(2) == 0)
        {
            p1.IsActive = true;
        }
        else
        {
            p2.IsActive = true;
        }
    }

    private Player GetActivePlayer(Player p1, Player p2)
    {
        if (p1.IsActive)
            return p1;

        return p2;
    }

    private void SwitchPlayer()
    {
        if (_playerOne.IsActive)
        {
            _playerOne.IsActive = false;
            _playerTwo.IsActive = true;
            return;
        }

        _playerTwo.IsActive = false;
        _playerOne.IsActive = true;
    }
}
