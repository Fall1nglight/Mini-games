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
        DatabaseRecord dbRecord = new DatabaseRecord(_storedPlayers);
        string jsonString = JsonSerializer.Serialize(dbRecord, _options);
        File.WriteAllText(_path, jsonString);
        Console.WriteLine($"{_storedPlayers[^1].Name} has been added to the database.");
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
