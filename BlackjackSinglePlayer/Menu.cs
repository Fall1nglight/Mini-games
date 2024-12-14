namespace BlackjackSinglePlayer;

public class Menu
{
    // fields
    private string _label;
    private List<MenuItem> _items;
    private bool _isMainMenu;

    // constructors
    public Menu(string label)
    {
        _label = label;
        _items = new List<MenuItem>();
    }

    // methods
    /// <summary>
    /// Prompts the player to choose from the stored menu items
    /// </summary>
    /// <returns>The choosen items' id</returns>
    public int GetChoosenItem()
    {
        int choice = 0;
        ConsoleKey key;

        do
        {
            Console.Clear();
            DisplayItems();
            Console.SetCursorPosition(1, choice + 1);
            Console.Write('X');
            key = Console.ReadKey().Key;

            if (key == ConsoleKey.UpArrow && choice > 0)
                choice--;

            if (key == ConsoleKey.DownArrow && choice < _items.Count)
                choice++;
        } while (key != ConsoleKey.Enter);

        Console.SetCursorPosition(0, _items.Count + 1);

        return choice == _items.Count ? -1 : _items[choice].Id;
    }

    /// <summary>
    /// Display stored menu items
    /// </summary>
    private void DisplayItems()
    {
        Console.WriteLine($"=== [{_label}] ===");

        foreach (MenuItem menuItem in _items)
        {
            Console.WriteLine($"[ ] {menuItem.Label}");
        }

        if (!_isMainMenu)
            Console.WriteLine("[ ] Back");
    }

    /// <summary>
    /// Replaces the stored menu items with the given ones
    /// </summary>
    /// <param name="items">Menu items to be stored</param>
    public void SetItems(List<MenuItem> items)
    {
        _items = items;
    }

    /// <summary>
    /// Sets current menu as main
    /// </summary>
    public void SetAsMainMenu()
    {
        _isMainMenu = true;
    }

    // properties
    public string Label
    {
        get => _label;
        set => _label = value ?? throw new ArgumentNullException(nameof(value));
    }
}
