namespace BlackjackSinglePlayer;

public class DatabaseRecord
{
    // fields
    private readonly List<Player> _players;

    // constructors
    public DatabaseRecord(List<Player> players)
    {
        _players = players;
    }

    // properties
    public List<Player> Players => _players;
}
