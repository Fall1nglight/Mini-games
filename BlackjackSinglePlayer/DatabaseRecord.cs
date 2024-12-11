namespace BlackjackSinglePlayer;

public class DatabaseRecord
{
    // fields
    private List<Player> _players;

    // constructors
    public DatabaseRecord(List<Player> players)
    {
        _players = players;
    }

    // methods

    // properties
    public List<Player> Players => _players;
}
