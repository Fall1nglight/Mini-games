namespace BlackjackSinglePlayer;

public class Menu
{
    // fields
    private readonly string _label;
    private List<MenuItem> _items;

    // constructors
    public Menu(string label)
    {
        _label = label;
        _items = new List<MenuItem>();
    }

    // methods
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

            if (key == ConsoleKey.DownArrow && choice < _items.Count - 1)
                choice++;
        } while (key != ConsoleKey.Enter);

        Console.SetCursorPosition(0, _items.Count + 1);

        return _items[choice].Id;
    }

    private void DisplayItems()
    {
        Console.WriteLine($"=== [{_label}] ===");

        foreach (MenuItem menuItem in _items)
        {
            Console.WriteLine($"[ ] {menuItem.Label}");
        }
    }

    public void SetItems(List<MenuItem> items)
    {
        _items = items;
    }
}
