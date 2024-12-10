namespace Blackjack;

public class Game
{
    // fields
    private Deck _deck;
    private List<Player> _players;
    private Dealer _dealer;

    // constructors
    public Game()
    {
        _deck = new Deck();
        _players = new List<Player>();
        _dealer = new Dealer();
    }

    private void AddPlayers()
    {
        Console.Write("Please enter your name: ");
        string playerName = Console.ReadLine()!;
        _players.Add(new Player(playerName));

        Console.WriteLine();
    }

    private void WelcomePlayers()
    {
        Console.WriteLine("=== Welcome to the game ===");
        Console.WriteLine();
        Console.WriteLine("=== [Players] ===");

        int idx = 0;

        while (idx < _players.Count)
        {
            Console.WriteLine($"{idx + 1}. {_players[idx].Name}");
            idx++;
        }

        Console.WriteLine($"{idx + 1}. Dealer (BOT)");
        Console.WriteLine();
    }

    private void ShowRules()
    {
        Console.WriteLine("=== [Rules] ===");
        Console.WriteLine("Hit: Draw a card from the deck.");
        Console.WriteLine("Stand: Keep your current total and end your turn.");
        Console.WriteLine();
        Console.WriteLine(
            "Every player (including the dealer) draws at least 2 cards at the start of the game."
        );
        Console.WriteLine(
            "The dealer reveals only one of their cards, while the other remains hidden."
        );
        Console.WriteLine(
            "Players then take turns deciding whether to Hit (draw more cards) or Stand (keep their current hand)."
        );
        Console.WriteLine(
            "If a player chooses to Hit, they draw a card from the deck. Multiple hits are allowed."
        );
        Console.WriteLine(
            "If the player chooses to Stand, their turn ends, and the game moves to the next player or phase."
        );
        Console.WriteLine(
            "The dealer must continue Hitting until their total score is at least 17."
        );
        Console.WriteLine(
            "If a player or the dealer exceeds 21 points, they bust and lose the round."
        );
        Console.WriteLine("The goal is to get as close to 21 as possible without going over.");
        Console.WriteLine(
            "If the player's total is closer to 21 than the dealer's, the player wins."
        );
        Console.WriteLine("If the dealer's total is closer to 21, the dealer wins.");
        Console.WriteLine(
            "In case of a tie (both have the same total score), it's a push, and the bet is returned."
        );
        Console.WriteLine();
        Console.WriteLine("Good luck and have fun!");
    }

    private void DealInitCards()
    {
        foreach (Player player in _players)
        {
            player.DrawCard(_deck);
        }

        _dealer.DrawCard(_deck);

        foreach (Player player in _players)
        {
            player.DrawCard(_deck);
        }
    }

    public void Run()
    {
        AddPlayers();
        WelcomePlayers();
        ShowRules();
        DealInitCards();
    }

    // methods
}
