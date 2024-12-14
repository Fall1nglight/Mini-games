using BlackjackSinglePlayer.Enums;

namespace BlackjackSinglePlayer;

public class Game
{
    // fields
    private readonly Menu _mainMenu;
    private readonly Menu _editPlayerDetailsMenu;
    private readonly Menu _playerSectionMenu;
    private readonly PlayerMenu _playerMenu;
    private readonly List<MenuItem> _playerSectionMenuLimited;
    private readonly List<MenuItem> _playerSectionMenuAll;
    private readonly Deck _deck;
    private Player _player;
    private readonly Dealer _dealer;
    private readonly Database _database;
    private bool _isGameOver;
    private int _wage;

    // constructors
    public Game()
    {
        _deck = new Deck();
        _dealer = new Dealer();
        _database = new Database("db.json");
        _mainMenu = CreateMainMenu();
        _editPlayerDetailsMenu = new Menu(string.Empty);

        _playerSectionMenu = CreatePlayerSectionMenu();

        _playerSectionMenuLimited = new List<MenuItem> { new MenuItem(2, "Add player") };

        _playerSectionMenuAll = new List<MenuItem>
        {
            new MenuItem(1, "View players"),
            new MenuItem(2, "Add player"),
            new MenuItem(3, "Edit player"),
            new MenuItem(4, "Delete player"),
        };

        _playerMenu = new PlayerMenu();

        // necessary if we would like to show special characters
        Console.OutputEncoding = System.Text.Encoding.UTF8;
    }

    // methods
    /// <summary>
    /// Displays the main menu
    /// </summary>
    public void DisplayMainMenu()
    {
        int choice = _mainMenu.GetChoosenItem();

        if (choice == 5)
            return;

        Console.Clear();

        switch (choice)
        {
            case 1:
                ShowRules();
                Console.ReadLine();
                break;

            case 2:
                HandlePlayerSectionMenu();
                break;

            case 3:
                HandleStartGame();
                break;

            case 4:
                ShowStatistics();
                break;
        }

        DisplayMainMenu(); // Recursively call DisplayMainMenu
    }

    /// <summary>
    /// Creates the main menu
    /// </summary>
    /// <returns>The main menu</returns>
    private Menu CreateMainMenu()
    {
        Menu mainMenu = new Menu("Main Menu");

        mainMenu.SetItems(
            new List<MenuItem>
            {
                new MenuItem(1, "Show rules"),
                new MenuItem(2, "Player actions (view/add/edit/remove)"),
                new MenuItem(3, "Start game"),
                new MenuItem(4, "Show statistics"),
                new MenuItem(5, "Quit"),
            }
        );

        mainMenu.SetAsMainMenu();

        return mainMenu;
    }

    /// <summary>
    /// Shows the rules to the player
    /// </summary>
    private void ShowRules()
    {
        Console.WriteLine("=== [Rules] ===");
        Console.WriteLine("Hit: Draw a card from the deck.");
        Console.WriteLine("Stand: Keep your current total and end your turn.");
        Console.WriteLine();
        Console.WriteLine("At the start of the game, you draw 2 cards.");
        Console.WriteLine(
            "The dealer also draws 2 cards but reveals only one of them, while the other remains hidden."
        );
        Console.WriteLine(
            "You can decide whether to Hit (draw more cards) or Stand (keep your current hand)."
        );
        Console.WriteLine(
            "If you choose to Hit, you draw a card from the deck. You can Hit multiple times."
        );
        Console.WriteLine("If you choose to Stand, your turn ends, and the dealer plays.");
        Console.WriteLine(
            "The dealer must continue Hitting until their total score is at least 17."
        );
        Console.WriteLine(
            "If you or the dealer exceed 21 points, it’s a bust, and you lose the round."
        );
        Console.WriteLine("The goal is to get as close to 21 as possible without going over.");
        Console.WriteLine("If your total is closer to 21 than the dealer's, you win!");
        Console.WriteLine("If the dealer's total is closer to 21, the dealer wins.");
        Console.WriteLine(
            "In case of a tie (both have the same total score), it’s a push, and your bet is returned."
        );
        Console.WriteLine();
        Console.WriteLine("Good luck and have fun!");
    }

    /// <summary>
    /// Displays the player section menu where users can be added/edited/deleted
    /// </summary>
    private void HandlePlayerSectionMenu()
    {
        _playerSectionMenu.SetItems(
            _database.HasPlayers ? _playerSectionMenuAll : _playerSectionMenuLimited
        );

        int playerMenuChoice = _playerSectionMenu.GetChoosenItem();

        Console.WriteLine();

        switch (playerMenuChoice)
        {
            case -1:
            {
                break;
            }

            case 1:
                ViewPlayers();
                break;

            case 2:
                AddPlayer();
                break;

            case 3:
                EditPlayer();
                break;

            case 4:
                DeletePlayer();
                break;
        }
    }

    /// <summary>
    /// Creates the player section menu
    /// </summary>
    /// <returns>The player section menu</returns>
    private Menu CreatePlayerSectionMenu()
    {
        Menu playerMenu = new Menu("Player menu");
        List<MenuItem> playerMenuItems = new List<MenuItem>
        {
            new MenuItem(1, "View players"),
            new MenuItem(2, "Add player"),
            new MenuItem(3, "Edit player"),
            new MenuItem(4, "Delete player"),
        };

        playerMenu.SetItems(playerMenuItems);
        return playerMenu;
    }

    /// <summary>
    /// Displays players present in the database
    /// </summary>
    private void ViewPlayers()
    {
        Console.WriteLine("Players stored locally");
        foreach (Player player in _database.Players)
        {
            Console.WriteLine($"- {player.Name}, balance: {player.Balance}");
        }

        Console.ReadLine();
        HandlePlayerSectionMenu();
    }

    /// <summary>
    /// Propmts username from player, then adds a new player to the database
    /// </summary>
    private void AddPlayer()
    {
        Console.Write("Please enter your username: ");
        string username = Console.ReadLine()!;
        Player playerToAdd = new Player(username);
        _database.AddPlayer(playerToAdd);

        Console.ReadLine();
        HandlePlayerSectionMenu();
    }

    /// <summary>
    /// Displays player menu, and the choosen player gets edited and saved to the database depending on the user's choice
    /// </summary>
    private void EditPlayer()
    {
        _playerMenu.Label = "Select player";
        _playerMenu.SetPlayers(_database.Players);

        Player? playerToEdit = _playerMenu.GetChoosenPlayer();

        if (playerToEdit == null)
        {
            HandlePlayerSectionMenu();
            return;
        }

        _editPlayerDetailsMenu.Label = playerToEdit.Name;
        _editPlayerDetailsMenu.SetItems(
            new List<MenuItem>
            {
                new MenuItem(1, "Change name"),
                new MenuItem(2, $"Change balance (current: {playerToEdit.Balance})"),
            }
        );

        int editPlayerDetailsChoice = _editPlayerDetailsMenu.GetChoosenItem();

        switch (editPlayerDetailsChoice)
        {
            case -1:
            {
                EditPlayer();
                return;
            }

            case 1:
                Console.Write("Enter a new username: ");
                playerToEdit.Name = Console.ReadLine()!;
                break;

            case 2:
                Console.Write("Enter new balance: ");
                playerToEdit.Balance = int.Parse(Console.ReadLine()!);
                break;
        }

        _database.EditPlayer(playerToEdit);
        Console.WriteLine(
            $"{playerToEdit.Name} has been successfully edited. (id: {playerToEdit.Id})"
        );

        Console.ReadLine();
    }

    /// <summary>
    /// Displays player menu, and the choosen player gets deleted from the database
    /// </summary>
    private void DeletePlayer()
    {
        _playerMenu.Label = "Delete player";
        _playerMenu.SetPlayers(_database.Players);
        Player? playerToRemove = _playerMenu.GetChoosenPlayer();

        if (playerToRemove == null)
        {
            HandlePlayerSectionMenu();
            return;
        }

        _database.RemovePlayer(playerToRemove);
        Console.ReadLine();
    }

    /// <summary>
    /// Resets the state of game status, then starts the game
    /// </summary>
    private void HandleStartGame()
    {
        if (!_database.HasPlayers)
        {
            Console.WriteLine("There are no users created to play with.");
            return;
        }

        _isGameOver = false;
        Startgame();

        if (!_isGameOver)
            Console.ReadLine();
    }

    /// <summary>
    /// Displays player menu, and the choosen player's stats get displayed
    /// </summary>
    private void ShowStatistics()
    {
        _playerMenu.Label = "Select player";
        _playerMenu.SetPlayers(_database.Players);
        Player? tmpPlayer = _playerMenu.GetChoosenPlayer();

        if (tmpPlayer == null)
            return;

        Console.WriteLine($"- Wins: {tmpPlayer.Statistics.PlayerWins}");
        Console.WriteLine($"- Loses: {tmpPlayer.Statistics.PlayerLoses}");
        Console.WriteLine($"- Pushes: {tmpPlayer.Statistics.NumOfPushes}");
        Console.WriteLine($"- Biggest prize won: {tmpPlayer.Statistics.BiggestPrize}");
        Console.WriteLine($"- Biggest bet lost: {tmpPlayer.Statistics.BiggestLoss}");
        Console.WriteLine($"- Total prize won: {tmpPlayer.Statistics.TotalWon}");
        Console.WriteLine($"- Total loss: {tmpPlayer.Statistics.TotalLoss}");
        Console.WriteLine($"- Total waged: {tmpPlayer.Statistics.TotalWaged}");
        Console.WriteLine($"- Rounds played: {tmpPlayer.Statistics.RoundsPlayed}");

        Console.ReadLine();
    }

    /// <summary>
    /// Displays players with positive balance, then the selected player starts the round
    /// </summary>
    private void Startgame()
    {
        List<Player> posBalancePlayers = _database
            .Players.Where(player => player.Balance > 0)
            .ToList();

        if (posBalancePlayers.Count == 0)
        {
            _isGameOver = true;
            return;
        }

        _playerMenu.Label = "Select Player";
        _playerMenu.SetPlayers(posBalancePlayers);
        Player? selectedPlayer = _playerMenu.GetChoosenPlayer();

        if (selectedPlayer == null)
        {
            _isGameOver = true;
            return;
        }

        _player = selectedPlayer;

        PlayRound();
    }

    /// <summary>
    /// Promps and validates wage from player for the current round
    /// </summary>
    /// <returns>The prompted wage</returns>
    private int GetWageFromPlayer()
    {
        int tmpWage;
        do
        {
            Console.Clear();
            Console.WriteLine($"Your balance: {_player.Balance}");
            Console.Write("Enter wage: ");

            if (
                !int.TryParse(Console.ReadLine(), out tmpWage)
                || tmpWage <= 0
                || tmpWage > _player.Balance
            )
            {
                Console.WriteLine("Wrong wage! Try again!");
                Console.ReadLine();
            }
        } while (tmpWage <= 0 || tmpWage > _player.Balance);

        return tmpWage;
    }

    /// <summary>
    /// Prompts the player whether they would like to hit or stand
    /// </summary>
    /// <returns>The prompted char</returns>
    private char GetPlayerHitOrStandChoice()
    {
        Console.Write("Would you like to hit or stand? [h / s]: ");
        char choice = Console.ReadKey().KeyChar;
        Console.WriteLine();
        return choice;
    }

    /// <summary>
    /// Reset player, dealer hand and score, furthermore the wage
    /// </summary>
    private void ResetBeforeRound()
    {
        _wage = 0;
        _player.Reset();
        _dealer.Reset();
    }

    /// <summary>
    /// Simulates card drawing for 2 rounds
    /// </summary>
    private void DealCards()
    {
        DealCardsPerRound();
        DealCardsPerRound();
    }

    /// <summary>
    /// Draws a card for the player, dealer from the deck
    /// </summary>
    private void DealCardsPerRound()
    {
        _player.DrawCard(_deck);
        _dealer.DrawCard(_deck);
    }

    /// <summary>
    /// Shows all cards from player's hand
    /// </summary>
    private void DisplayPlayerHand()
    {
        Console.WriteLine(
            $"{_player.Name} (balance: {_player.Balance}, wage: {_wage}), you have the following cards"
        );
        Console.WriteLine($"{string.Join(", ", _player.Hand)}");
        Console.WriteLine($"Your score is: {_player.Score}");
        Console.WriteLine();
    }

    /// <summary>
    /// Shows only one card from dealer's hand
    /// </summary>
    private void DisplayDealerHand()
    {
        Console.WriteLine("The dealer has the following cards");
        Console.WriteLine($"{_dealer.Hand[0]}, ???");
        Console.WriteLine();
    }

    /// <summary>
    /// Simulates the player's turn
    /// </summary>
    private void PlayerTurn()
    {
        if (_player.HasBlackjack)
        {
            DisplayPlayerHand();
            Console.WriteLine("You got blackjack!");
            return;
        }

        do
        {
            Console.Clear();
            DisplayPlayerHand();
            DisplayDealerHand();

            char choice = GetPlayerHitOrStandChoice();

            if (choice == 's')
                break;

            if (choice == 'h')
            {
                _player.DrawCard(_deck);
                Console.WriteLine($"\nYou have drawn the following card: {_player.Hand[^1]}");
                Thread.Sleep(1500); // Pause after drawing a card
            }
            else
            {
                Console.WriteLine("Invalid input! Try again!");
            }
        } while (!_player.IsBusted);
    }

    /// <summary>
    /// Simulates the dealer's turn
    /// </summary>
    private void DealerTurn()
    {
        Console.WriteLine();
        Console.Write("Now the dealer draws");

        for (int i = 0; i < 5; i++)
        {
            Console.Write('.');
            Thread.Sleep(350);
        }

        while (_dealer.Score < 17)
        {
            _dealer.DrawCard(_deck);
        }

        Console.WriteLine($"\nDealer's cards: {string.Join(", ", _dealer.Hand)}");
    }

    /// <summary>
    /// Determinates who won the current round
    /// </summary>
    private void DeterminateWinner()
    {
        Console.WriteLine($"\nYour score: {_player.Score}");
        Console.WriteLine($"Dealer score: {_dealer.Score}");
        Console.WriteLine();

        if (_player.IsBusted)
        {
            Console.WriteLine($"You are busted! You lost {_wage} tokens!");
            UpdatePlayer(GameOutcome.Lose, -_wage);
            return;
        }

        if (_dealer.IsBusted)
        {
            Console.WriteLine($"The dealer is busted! You won {_wage * 2} tokens!");
            UpdatePlayer(GameOutcome.Win, _wage);
            return;
        }

        if (_player.HasBlackjack)
        {
            Console.WriteLine($"You got blackjack! You won {(int)Math.Round(_wage * 2.5)} tokens!");
            UpdatePlayer(GameOutcome.Win, (int)Math.Round(_wage * 1.5));
            return;
        }

        if (_player.Score == _dealer.Score)
        {
            Console.WriteLine("Both scores are equal. Push!");
            UpdatePlayer(GameOutcome.Push, 0);
            return;
        }

        if (_player.Score > _dealer.Score)
        {
            Console.WriteLine($"You won {_wage * 2} tokens!");
            UpdatePlayer(GameOutcome.Win, _wage);
        }
        else
        {
            Console.WriteLine($"You lost {_wage} tokens!");
            UpdatePlayer(GameOutcome.Lose, -_wage);
        }
    }

    /// <summary>
    /// Updates player balance and stats
    /// </summary>
    /// <param name="outcome">The outcome of the current round</param>
    /// <param name="amount">The won or lost prize of the current round</param>
    private void UpdatePlayer(GameOutcome outcome, int amount)
    {
        _player.Balance += amount;
        _player.Statistics.UpdateStats(outcome, Math.Abs(amount));
        _database.EditPlayer(_player);
    }

    /// <summary>
    /// Prompts the player whether they would like to play another round
    /// </summary>
    /// <returns></returns>
    private bool AskToPlayAnotherRound()
    {
        do
        {
            Console.Write("\nWould you like to play another round? [y/n] ");
            char nextRoundChoice = Console.ReadKey().KeyChar;
            Console.WriteLine();

            if (nextRoundChoice == 'n')
                return false;

            if (nextRoundChoice == 'y')
                return true;

            Console.WriteLine("Wrong input! Try again!");
        } while (true);
    }

    /// <summary>
    /// Simulates a round until the player's balance is 0 or below or the player chooses to end the game
    /// </summary>
    private void PlayRound()
    {
        while (!_isGameOver && _player.Balance > 0)
        {
            ResetBeforeRound();
            _wage = GetWageFromPlayer();

            DealCards();
            PlayerTurn();

            if (!_player.IsBusted)
                DealerTurn();

            DeterminateWinner();

            if (_player.Balance <= 0)
                return;

            if (!AskToPlayAnotherRound())
                _isGameOver = true;
        }
    }
}
