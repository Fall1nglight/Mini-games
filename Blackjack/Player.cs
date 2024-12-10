using Blackjack.Enums;

namespace Blackjack;

public class Player
{
    // fields
    private readonly string _name;
    private readonly List<Card> _hand;
    private int _score;

    // constructors
    public Player(string name)
    {
        _name = name;
        _hand = new List<Card>();
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
        if (_score > 11 && _hand[^1].Rank == CardRank.Ace)
        {
            _score += 1;
            return;
        }

        _score += _hand[^1].Value;
    }

    // properties
    public int Score => _score;

    public bool IsBusted => _score > 21;

    public bool HasBlackjack => _score == 21;

    public string Name => _name;

    public List<Card> Hand => _hand;
}
