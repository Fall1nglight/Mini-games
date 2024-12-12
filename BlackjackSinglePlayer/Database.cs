using System.Text.Json;

namespace BlackjackSinglePlayer;

public class Database
{
    // fields
    private List<Player> _storedPlayers;
    private readonly string _path;
    private readonly JsonSerializerOptions _options;

    // constructors
    public Database(string path)
    {
        _storedPlayers = new List<Player>();
        _path = path;
        _options = new JsonSerializerOptions { WriteIndented = true };
        Init();
    }

    // methods
    /// <summary>
    /// Creates db file if does not exist, otherwise loads db content from file
    /// </summary>
    private void Init()
    {
        if (File.Exists(_path))
        {
            LoadFromJson();
            return;
        }

        CreateDb();
    }

    /// <summary>
    /// Creates db file then writes players' data into it
    /// </summary>
    private void CreateDb()
    {
        DatabaseRecord dbRecord = new DatabaseRecord(_storedPlayers);
        string jsonString = JsonSerializer.Serialize(dbRecord, _options);
        File.WriteAllText(_path, jsonString);
    }

    /// <summary>
    /// Adds a player to the _storedPlayers List, then syncronizes the db file
    /// </summary>
    /// <param name="player">Player to be added</param>
    public void AddPlayer(Player player)
    {
        _storedPlayers.Add(player);
        SyncToJson();
        Console.WriteLine($"{player.Name} has been added to the database.");
    }

    /// <summary>
    /// Replace player with a new one in _storedPlayers List, then syncronizes the db file
    /// </summary>
    /// <param name="player">Player to be replaced</param>
    public void EditPlayer(Player player)
    {
        int idx = 0;

        while (idx < _storedPlayers.Count && _storedPlayers[idx].Id != player.Id)
            idx++;

        _storedPlayers[idx] = player;

        SyncToJson();
    }

    /// <summary>
    /// Removes a player from the _storedPlayers List, then syncronizes the db file
    /// </summary>
    /// <param name="player">Player to be removed</param>
    public void RemovePlayer(Player player)
    {
        _storedPlayers.Remove(player);
        SyncToJson();
        Console.WriteLine($"{player.Name} has been removed from the database.");
    }

    /// <summary>
    /// Syncronizes _storedPlayers List with the db file
    /// </summary>
    private void SyncToJson()
    {
        DatabaseRecord dbRecord = new DatabaseRecord(_storedPlayers);
        string jsonString = JsonSerializer.Serialize(dbRecord, _options);
        File.WriteAllText(_path, jsonString);
    }

    /// <summary>
    /// Loads players from the db file, then stores the result in _storedPlayers List
    /// </summary>
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
            // if the db file is corrupt replace with a new db file
            CreateDb();
        }

        if (dbRecord != null)
            _storedPlayers = dbRecord.Players;
    }

    // properties
    public List<Player> Players => _storedPlayers;

    public bool HasPlayers => _storedPlayers.Count > 0;
}
