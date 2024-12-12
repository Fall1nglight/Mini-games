using BlackjackSinglePlayer.Enums;

namespace BlackjackSinglePlayer;

public class Dealer
{
    // fields
    private readonly List<Card> _hand;
    private int _score;

    // constructors
    public Dealer()
    {
        _hand = new List<Card>();
    }

    // methods
    /// <summary>
    /// Draws a card from the current deck then calculates the dealers's score.
    /// </summary>
    /// <param name="deck">Represents the deck class, must be same for all players.</param>
    public void DrawCard(Deck deck)
    {
        _hand.Add(deck.Draw());
        CalculateScore();
    }

    /// <summary>
    /// Calculates score of the dealer's hand depending on their cards.
    /// </summary>
    private void CalculateScore()
    {
        // ace must count as 1 if score is over 10
        if (_score > 10 && _hand[^1].Rank == CardRank.Ace)
        {
            _score += 1;
            return;
        }

        _score += _hand[^1].Value;
    }

    /// <summary>
    /// Removes cards from dealer's hand and resets score to 0
    /// </summary>
    public void Reset()
    {
        _hand.Clear();
        _score = 0;
    }

    // properties
    public bool IsBusted => _score > 21;

    public int Score => _score;

    public List<Card> Hand => _hand;
}
