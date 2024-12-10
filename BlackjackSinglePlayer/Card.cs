using BlackjackSinglePlayer.Enums;

namespace BlackjackSinglePlayer;

public class Card
{
    // fields
    private readonly CardSuit _suit;
    private readonly CardRank _rank;

    // constructors
    public Card(CardSuit suit, CardRank rank)
    {
        _suit = suit;
        _rank = rank;
    }

    // methods
    public override string ToString()
    {
        return $"{_suit} {_rank}";
    }

    // properties
    public int Value => (int)_rank;

    public CardSuit Suit => _suit;

    public CardRank Rank => _rank;
}
