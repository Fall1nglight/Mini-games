using System.Text.Json;

namespace BlackjackSinglePlayer;

public class Database
{
    // fields
    private List<Player> _storedPlayers;
    private string _path;
    private JsonSerializerOptions _options;

    // constructors
    public Database(string path)
    {
        _storedPlayers = new List<Player>();
        _path = path;
        _options = new JsonSerializerOptions { WriteIndented = true };
        Init();
    }

    private void Init()
    {
        // todo : should check if the json file is formatted correctly

        if (File.Exists(_path))
        {
            LoadFromJson();
            return;
        }

        CreateDb();
    }

    private void CreateDb()
    {
        DatabaseRecord dbRecord = new DatabaseRecord(_storedPlayers);
        string jsonString = JsonSerializer.Serialize(dbRecord, _options);
        File.WriteAllText(_path, jsonString);
    }

    // methods
    public void AddPlayer(Player player)
    {
        _storedPlayers.Add(player);
        SyncToJson();
        Console.WriteLine($"{_storedPlayers[^1].Name} has been added to the database.");
    }

    public void RemovePlayer(int playerId)
    {
        Player toRemove = _storedPlayers[playerId];
        _storedPlayers.RemoveAt(playerId);
        SyncToJson();
        Console.WriteLine($"{toRemove.Name} has been removed from the database.");
    }

    public void EditPlayer(int playerId, Player player)
    {
        _storedPlayers[playerId] = player;
        SyncToJson();
        Console.WriteLine($"Player with id: {playerId} has been edited.");
    }

    public void SyncToJson()
    {
        DatabaseRecord dbRecord = new DatabaseRecord(_storedPlayers);
        string jsonString = JsonSerializer.Serialize(dbRecord, _options);
        File.WriteAllText(_path, jsonString);
    }

    public void LoadFromJson()
    {
        string jsonString = File.ReadAllText(_path);
        DatabaseRecord? dbRecord = null;

        try
        {
            dbRecord = JsonSerializer.Deserialize<DatabaseRecord>(jsonString);
        }
        catch (Exception e)
        {
            CreateDb();
        }

        if (dbRecord != null)
            _storedPlayers = dbRecord.Players;
    }

    // properties
    public List<Player> Players => _storedPlayers;

    public bool HasPlayers => _storedPlayers.Count > 0;
}
