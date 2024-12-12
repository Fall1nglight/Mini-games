using BlackjackSinglePlayer.Enums;

namespace BlackjackSinglePlayer;

public class Statistics
{
    // fields
    private int _playerWins;
    private int _playerLoses;
    private int _numOfPushes;
    private int _biggestPrize;
    private int _biggestLoss;
    private int _totalWon;
    private int _totalLoss;
    private int _totalWaged;
    private int _roundsPlayed;

    // methods
    public void UpdateStats(GameOutcome outcome, int amount)
    {
        if (outcome == GameOutcome.Win)
        {
            _playerWins++;
            _totalWon += amount;

            // check biggest prize
            if (amount > _biggestPrize)
                _biggestPrize = amount;
        }
        else if (outcome == GameOutcome.Lose)
        {
            _playerLoses++;
            _totalLoss += amount;

            // check biggest loss
            if (amount > _biggestLoss)
                _biggestLoss = amount;
        }
        else
        {
            _numOfPushes++;
        }

        _totalWaged += amount;
        _roundsPlayed++;
    }

    // properties
    public int PlayerWins
    {
        get => _playerWins;
        set => _playerWins = value;
    }

    public int PlayerLoses
    {
        get => _playerLoses;
        set => _playerLoses = value;
    }

    public int NumOfPushes
    {
        get => _numOfPushes;
        set => _numOfPushes = value;
    }

    public int BiggestPrize
    {
        get => _biggestPrize;
        set => _biggestPrize = value;
    }

    public int BiggestLoss
    {
        get => _biggestLoss;
        set => _biggestLoss = value;
    }

    public int TotalWon
    {
        get => _totalWon;
        set => _totalWon = value;
    }

    public int TotalLoss
    {
        get => _totalLoss;
        set => _totalLoss = value;
    }

    public int TotalWaged
    {
        get => _totalWaged;
        set => _totalWaged = value;
    }

    public int RoundsPlayed
    {
        get => _roundsPlayed;
        set => _roundsPlayed = value;
    }
}
