namespace BlackjackSinglePlayer;

public class Game
{
    // fields
    private readonly Deck _deck;
    private Player _player;
    private readonly Dealer _dealer;
    private readonly Database _database;
    private bool _isGameOver;
    private int _wage;
    private Menu _mainMenu;
    private Menu _playerSectionMenu;
    private PlayerMenu _playerMenu;
    private List<MenuItem> _playerSectionMenuLimited;
    private List<MenuItem> _playerSectionMenuAll;

    // constructors
    public Game()
    {
        _deck = new Deck();
        _dealer = new Dealer();
        _database = new Database("db.json");
        _mainMenu = CreateMainMenu();
        _playerSectionMenu = CreatePlayerMenu();

        _playerSectionMenuLimited = new List<MenuItem> { new MenuItem(2, "Add player") };

        _playerSectionMenuAll = new List<MenuItem>
        {
            new MenuItem(1, "View players"),
            new MenuItem(2, "Add player"),
            new MenuItem(3, "Edit player"),
            new MenuItem(4, "Delete player"),
        };

        _playerMenu = new PlayerMenu();
    }

    // methods
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

    private void ResetBeforeRound()
    {
        _wage = 0;
        _player.Reset();
        _dealer.Reset();
    }

    private void DealCards()
    {
        DealCardsPerRound();
        DealCardsPerRound();
    }

    private void DealCardsPerRound()
    {
        _player.DrawCard(_deck);
        _dealer.DrawCard(_deck);
    }

    private void DisplayPlayerHand(Player player)
    {
        Console.WriteLine(
            $"{player.Name} (balance: {player.Balance}, wage: {_wage}), you have the following cards"
        );
        Console.WriteLine($"{string.Join(", ", player.Hand)}");
        Console.WriteLine($"Your score is: {player.Score}");
        Console.WriteLine();
    }

    private void DisplayDealerHand()
    {
        Console.WriteLine("The dealer has the following cards");
        Console.WriteLine($"{_dealer.Hand[0]}, ???");
        Console.WriteLine();
    }

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

    private void DeterminateWinner()
    {
        Console.WriteLine($"\nYour score: {_player.Score}");
        Console.WriteLine($"Dealer score: {_dealer.Score}");
        Console.WriteLine();

        if (_player.IsBusted)
        {
            Console.WriteLine($"You are busted! You lost {_wage} tokens!");
            UpdatePlayerBalance(-_wage);
            return;
        }

        if (_dealer.IsBusted)
        {
            Console.WriteLine($"The dealer is busted! You won {_wage * 2} tokens!");
            UpdatePlayerBalance(_wage);
            return;
        }

        if (_player.HasBlackjack)
        {
            Console.WriteLine($"You got blackjack! You won {(int)Math.Round(_wage * 2.5)} tokens!");
            UpdatePlayerBalance((int)Math.Round(_wage * 1.5));
            return;
        }

        if (_player.Score == _dealer.Score)
        {
            Console.WriteLine("Both scores are equal. Push!");
            return;
        }

        if (_player.Score > _dealer.Score)
        {
            Console.WriteLine($"You won {_wage * 2} tokens!");
            UpdatePlayerBalance(_wage);
        }
        else
        {
            Console.WriteLine($"You lost {_wage} tokens!");
            UpdatePlayerBalance(-_wage);
        }
    }

    private void UpdatePlayerBalance(int amount)
    {
        _player.Balance += amount;
        _database.EditPlayer(_player);
    }

    private void PlayerTurn()
    {
        if (_player.HasBlackjack)
        {
            DisplayPlayerHand(_player);
            Console.WriteLine("You got blackjack!");
            return;
        }

        do
        {
            Console.Clear();
            DisplayPlayerHand(_player);
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

    private char GetPlayerHitOrStandChoice()
    {
        Console.Write("Would you like to hit or stand? [h / s]: ");
        char choice = Console.ReadKey().KeyChar;
        Console.WriteLine();
        return choice;
    }

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

    private void InitMenu()
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
                HandlePlayerMenu();
                break;

            case 3:
                HandleStartGame();
                break;

            case 4:
                ShowStatistics();
                break;
        }

        InitMenu(); // Recursively call InitMenu
    }

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
        return mainMenu;
    }

    private void HandlePlayerMenu()
    {
        _playerSectionMenu.SetItems(
            !_database.HasPlayers ? _playerSectionMenuLimited : _playerSectionMenuAll
        );

        int playerMenuChoice = _playerSectionMenu.GetChoosenItem();

        Console.WriteLine();

        switch (playerMenuChoice)
        {
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

        Console.ReadLine();
    }

    private Menu CreatePlayerMenu()
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

    private void ViewPlayers()
    {
        Console.WriteLine("Players stored locally");
        foreach (Player player in _database.Players)
        {
            Console.WriteLine($"- {player.Name}, balance: {player.Balance}");
        }
    }

    private void AddPlayer()
    {
        Console.Write("Please enter your username: ");
        string username = Console.ReadLine()!;
        Player playerToAdd = new Player(username);
        _database.AddPlayer(playerToAdd);
    }

    private void EditPlayer()
    {
        _playerMenu.Label = "Edit Player";
        _playerMenu.SetPlayers(_database.Players);

        Player playerToEdit = _playerMenu.GetChoosenPlayer();

        Menu editPlayerDetailsMenu = new Menu(playerToEdit.Name);
        editPlayerDetailsMenu.SetItems(
            new List<MenuItem>
            {
                new MenuItem(1, "Change name"),
                new MenuItem(2, $"Change balance (current: {playerToEdit.Balance})"),
            }
        );

        int editPlayerDetailsChoice = editPlayerDetailsMenu.GetChoosenItem();
        switch (editPlayerDetailsChoice)
        {
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
    }

    private void DeletePlayer()
    {
        _playerMenu.Label = "Delete player";
        _playerMenu.SetPlayers(_database.Players);
        _database.RemovePlayer(_playerMenu.GetChoosenPlayer());
    }

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

    private void ShowStatistics()
    {
        // Implement the statistics logic here
    }

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
        _player = _playerMenu.GetChoosenPlayer();

        PlayRound();
    }

    public void Run()
    {
        InitMenu();
    }
}
