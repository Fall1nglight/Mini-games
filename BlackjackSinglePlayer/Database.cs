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

    // methods
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

    public Player GetPlayerByGuid(Guid id)
    {
        return _storedPlayers.First(player => player.Id == id);
    }

    public void AddPlayer(Player player)
    {
        _storedPlayers.Add(player);
        SyncToJson();
        Console.WriteLine($"{player.Name} has been added to the database.");
    }

    public void EditPlayer(Player player)
    {
        int idx = 0;

        while (idx < _storedPlayers.Count && _storedPlayers[idx].Id != player.Id)
            idx++;

        _storedPlayers[idx] = player;

        SyncToJson();
    }

    public void RemovePlayer(Player player)
    {
        _storedPlayers.Remove(player);
        SyncToJson();
        Console.WriteLine($"{player.Name} has been removed from the database.");
    }

    private void SyncToJson()
    {
        DatabaseRecord dbRecord = new DatabaseRecord(_storedPlayers);
        string jsonString = JsonSerializer.Serialize(dbRecord, _options);
        File.WriteAllText(_path, jsonString);
    }

    private void LoadFromJson()
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
