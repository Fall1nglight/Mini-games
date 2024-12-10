using Blackjack.Enums;

namespace Blackjack;

public class Dealer
{
    // fields
    private List<Card> _hand;
    private int _score;

    // constructors
    public Dealer()
    {
        _hand = new List<Card>();
    }

    // methods
    public void DrawCard(Deck deck)
    {
        _hand.Add(deck.Draw());
    }

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
}
