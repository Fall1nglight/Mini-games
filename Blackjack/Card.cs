using Blackjack.Enums;

namespace Blackjack;

public class Card
{
    // fields
    private CardSuit _suit;
    private CardRank _rank;

    // constructors
    public Card(CardSuit suit, CardRank rank) { }

    // methods
    public override string ToString()
    {
        return $"{_suit} {_rank}, value: {Value}";
    }

    // properties
    public int Value => (int)_rank;

    public CardSuit Suit => _suit;

    public CardRank Rank => _rank;
}
