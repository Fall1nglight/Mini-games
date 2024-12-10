using BlackjackSinglePlayer.Enums;

namespace BlackjackSinglePlayer;

public class Deck
{
    // fields
    private List<Card> _cards;
    private readonly Random _rand;

    // constructors
    public Deck()
    {
        _cards = new List<Card>();
        _rand = new Random();
        Fill();
        Shuffle();
    }

    // methods
    /// <summary>
    /// Fills the deck based on card suits and ranks.
    /// </summary>
    private void Fill()
    {
        foreach (CardSuit suit in Enum.GetValues<CardSuit>())
        {
            foreach (CardRank rank in Enum.GetValues<CardRank>())
            {
                _cards.Add(new Card(suit, rank));
            }
        }
    }

    /// <summary>
    /// Shuffles the deck in a random way.
    /// </summary>
    private void Shuffle()
    {
        for (int i = 0; i < _cards.Count; i++)
        {
            int randIdx = _rand.Next(_cards.Count);
            (_cards[i], _cards[randIdx]) = (_cards[randIdx], _cards[i]);
        }
    }

    /// <summary>
    /// Draws a card from the top of the deck and removes it from the deck.
    /// </summary>
    /// <returns>The drawn card.</returns>
    public Card Draw()
    {
        if (_cards.Count == 0)
        {
            Fill();
            Shuffle();
        }

        Card drawnCard = _cards[0];
        _cards.RemoveAt(0);
        return drawnCard;
    }
}
