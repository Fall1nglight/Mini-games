using BlackjackSinglePlayer.Enums;

namespace BlackjackSinglePlayer;

public class Card
{
    // fields
    private readonly CardSuit _suit;
    private readonly CardRank _rank;
    private readonly string _readableFormat;

    // constructors
    public Card(CardSuit suit, CardRank rank)
    {
        _suit = suit;
        _rank = rank;
        _readableFormat = GetReadableFormat();
    }

    // methods
    /// <summary>
    /// Formats card rank and suit into readable format
    /// </summary>
    /// <returns>The readable format</returns>
    private string GetReadableFormat()
    {
        string rank = $"{(int)_rank}";

        if (_rank is CardRank.Jack or CardRank.Queen or CardRank.King or CardRank.Ace)
        {
            rank = $"{_rank}";
        }

        char suit = char.MinValue;

        suit = _suit switch
        {
            CardSuit.Clubs => '♣',
            CardSuit.Diamonds => '♦',
            CardSuit.Hearts => '♥',
            CardSuit.Spades => '♠',
            _ => suit,
        };

        return $"{suit} {rank}";
    }

    public override string ToString()
    {
        return _readableFormat;
    }

    // properties
    public int Value => (int)_rank;

    public CardSuit Suit => _suit;

    public CardRank Rank => _rank;
}
