using BlackjackSinglePlayer.Enums;

namespace BlackjackSinglePlayer;

public class Player
{
    // fields
    private readonly Guid _id;
    private string _name;
    private readonly List<Card> _hand;
    private int _score;
    private int _balance;
    private Statistics _stats;

    // constructors
    public Player(string name)
    {
        _id = Guid.NewGuid();
        _name = name;
        _hand = new List<Card>();
        _balance = 100;
        _stats = new Statistics();
    }

    // methods
    /// <summary>
    /// Draws a card from the current deck then calculates the player's score.
    /// </summary>
    /// <param name="deck">Represents the deck class, must be same for all players.</param>
    public void DrawCard(Deck deck)
    {
        _hand.Add(deck.Draw());
        CalculateScore();
    }

    /// <summary>
    /// Calculates score of the player's hand depending on their cards.
    /// </summary>
    private void CalculateScore()
    {
        if (_score >= 11 && _hand[^1].Rank == CardRank.Ace)
        {
            _score += 1;
            return;
        }

        _score += _hand[^1].Value;
    }

    public void Reset()
    {
        _hand.Clear();
        _score = 0;
    }

    // properties
    public Guid Id => _id;

    public int Score => _score;

    public bool IsBusted => _score > 21;

    public bool HasBlackjack => _score == 21;

    public string Name
    {
        get => _name;
        set => _name = value ?? throw new ArgumentNullException(nameof(value));
    }

    public List<Card> Hand => _hand;

    public int Balance
    {
        get => _balance;
        set => _balance = value < 0 ? _balance : value;
    }

    public Statistics Statistics
    {
        get => _stats;
        set => _stats = value;
    }
}
