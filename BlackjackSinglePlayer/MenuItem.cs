namespace BlackjackSinglePlayer;

public class MenuItem
{
    // fields
    private int _id;
    private string _label;

    // constructors
    public MenuItem(int id, string label)
    {
        _id = id;
        _label = label;
    }

    // properties
    public string Label => _label;
    public int Id => _id;
}
