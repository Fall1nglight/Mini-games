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
    public void DrawCard(Deck deck)
    {
        _hand.Add(deck.Draw());
        CalculateScore();
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
