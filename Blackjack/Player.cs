using Blackjack.Enums;

namespace Blackjack;

public class Player
{
    // fields
    private string _name;
    private List<Card> _hand;
    private int _score;

    // constructors
    public Player(string name)
    {
        _name = name;
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
}
