namespace BlackjackSinglePlayer;

public class PlayerMenu
{
    // fields
    private string _label;
    private List<Player> _players;

    // constructors
    public PlayerMenu()
    {
        _label = string.Empty;
        _players = new List<Player>();
    }

    // methods
    public Player GetChoosenPlayer()
    {
        int choice = 0;
        ConsoleKey key;

        do
        {
            Console.Clear();
            DisplayPlayers();
            Console.SetCursorPosition(1, choice + 1);
            Console.Write('X');
            key = Console.ReadKey().Key;

            if (key == ConsoleKey.UpArrow && choice > 0)
                choice--;

            if (key == ConsoleKey.DownArrow && choice < _players.Count - 1)
                choice++;
        } while (key != ConsoleKey.Enter);

        Console.SetCursorPosition(0, _players.Count + 1);

        return _players[choice];
    }

    private void DisplayPlayers()
    {
        Console.WriteLine($"=== [{_label}] ===");

        foreach (Player player in _players)
        {
            Console.WriteLine($"[ ] {player.Name} (balance: {player.Balance})");
        }
    }

    public void SetPlayers(List<Player> players)
    {
        _players = players;
    }

    // properties
    public string Label
    {
        get => _label;
        set => _label = value ?? throw new ArgumentNullException(nameof(value));
    }
}
