﻿namespace BlackjackSinglePlayer;

public class Game
{
    // fields
    private readonly Deck _deck;
    private Player _player;
    private readonly Dealer _dealer;
    private readonly Database _database;

    // constructors
    public Game()
    {
        _deck = new Deck();
        _dealer = new Dealer();
        _database = new Database("db.json");
    }

    // methods
    private void AddPlayer()
    {
        Console.Write("Please enter your name: ");
        string playerName = Console.ReadLine()!;
        _player = new Player(playerName);
    }

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
        Console.WriteLine($"{player.Name}, you have the following cards");
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

        for (int i = 0; i < 10; i++)
        {
            Console.Write('.');
            Thread.Sleep(300);
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

        if (_player.IsBusted)
        {
            Console.WriteLine("You are busted! You lost!");
            return;
        }

        if (_dealer.IsBusted)
        {
            Console.WriteLine("The dealer is busted! You won!");
            return;
        }

        if (_player.Score == _dealer.Score)
        {
            Console.WriteLine("Both scores are equal. Push!");
            return;
        }

        Console.WriteLine(_player.Score > _dealer.Score ? "You won!" : "You lost!");
    }

    private void PlayerTurn()
    {
        Console.ReadLine();

        if (_player.HasBlackjack)
        {
            Console.WriteLine("You got blackjack!");
            return;
        }

        char choice;
        do
        {
            Console.Clear();
            DisplayPlayerHand(_player);
            DisplayDealerHand();

            Console.Write("Would you like to hit or stand? [h / s]: ");
            choice = Console.ReadKey().KeyChar;

            if (choice == 's')
                break;

            if (choice != 'h')
            {
                Console.WriteLine("You have entered wrong input! Try again!");
                continue;
            }

            _player.DrawCard(_deck);

            Console.WriteLine($"\n\nYou have drawn the following card: {_player.Hand[^1]}");

            Thread.Sleep(1500);
        } while (choice != 's' && !_player.IsBusted);
    }

    private void InitMenu()
    {
        // todo: nested menus should only return to the previus menu
        Menu mainMenu = new Menu("Main Menu");
        MenuItem rulesItem = new MenuItem(1, "Show rules");
        MenuItem playersItem = new MenuItem(2, "Player actions (view/add/edit/remove)");
        MenuItem startItem = new MenuItem(3, "Start game");
        MenuItem statisticsItem = new MenuItem(4, "Show statistics");
        MenuItem exitItem = new MenuItem(5, "Quit");

        mainMenu.SetItems(
            new List<MenuItem>() { rulesItem, playersItem, startItem, statisticsItem, exitItem }
        );

        int choice = mainMenu.GetChoosenItem();

        if (choice != 5)
            Console.Clear();

        switch (choice)
        {
            case 1:
            {
                ShowRules();
                break;
            }

            case 2:
            {
                Menu playerMenu = new Menu("Player menu");
                MenuItem addPlayerItem = new MenuItem(2, "Add player");
                List<MenuItem> playerMenuItems = new List<MenuItem>() { addPlayerItem };

                if (_database.HasPlayers)
                {
                    MenuItem viewPlayerItem = new MenuItem(1, "View players");
                    MenuItem editPlayer = new MenuItem(3, "Edit player");
                    MenuItem deletePlayer = new MenuItem(4, "Delete player");
                    playerMenuItems.Add(viewPlayerItem);
                    playerMenuItems.Add(editPlayer);
                    playerMenuItems.Add(deletePlayer);
                }

                playerMenu.SetItems(playerMenuItems);
                int playerMenuChoice = playerMenu.GetChoosenItem();

                Console.WriteLine();

                // playerMenuSwitch
                switch (playerMenuChoice)
                {
                    // view players
                    case 1:
                    {
                        Console.WriteLine("Players stored locally");
                        foreach (Player player in _database.Players)
                        {
                            Console.WriteLine($"{player.Name}");
                        }

                        break;
                    }

                    // add players
                    case 2:
                    {
                        Console.Write("Please enter your username: ");
                        string username = Console.ReadLine()!;
                        Player playerToAdd = new Player(username);
                        _database.AddPlayer(playerToAdd);
                        break;
                    }

                    // edit player
                    case 3:
                    {
                        Menu editPlayerMenu = new Menu("Edit Player");
                        List<MenuItem> editPlayerItems = new List<MenuItem>();

                        for (var i = 0; i < _database.Players.Count; i++)
                        {
                            editPlayerItems.Add(new MenuItem(i, $"{_database.Players[i].Name}"));
                        }

                        editPlayerMenu.SetItems(editPlayerItems);
                        int editPlayerChoice = editPlayerMenu.GetChoosenItem();

                        Player playerToEdit = _database.Players[editPlayerChoice];

                        Menu editPlayerDetailsMenu = new Menu(playerToEdit.Name);
                        MenuItem nameItem = new MenuItem(1, "Change name");
                        MenuItem balanceItem = new MenuItem(
                            2,
                            $"Balance (current: {playerToEdit.Balance})"
                        );

                        editPlayerDetailsMenu.SetItems(
                            new List<MenuItem>() { nameItem, balanceItem }
                        );

                        int editPlayerDetailsChoice = editPlayerDetailsMenu.GetChoosenItem();

                        switch (editPlayerDetailsChoice)
                        {
                            case 1:
                            {
                                Console.Write("Enter a new username: ");
                                string newUsername = Console.ReadLine()!;
                                playerToEdit.Name = newUsername;
                                break;
                            }

                            case 2:
                            {
                                Console.Write("Enter new balance: ");
                                int newBalance = int.Parse(Console.ReadLine()!);
                                playerToEdit.Balance = newBalance;
                                break;
                            }
                        }

                        _database.EditPlayer(editPlayerChoice, playerToEdit);

                        break;
                    }

                    // delete player
                    case 4:
                    {
                        Menu deletePlayerMenu = new Menu("Delete player");
                        List<MenuItem> deletePlayerItems = new List<MenuItem>();

                        for (var i = 0; i < _database.Players.Count; i++)
                        {
                            deletePlayerItems.Add(new MenuItem(i, _database.Players[i].Name));
                        }

                        deletePlayerMenu.SetItems(deletePlayerItems);
                        int deletePlayerChoice = deletePlayerMenu.GetChoosenItem();
                        _database.RemovePlayer(deletePlayerChoice);

                        break;
                    }
                }
                // playerMenuSwitch
                break;
            }

            case 5:
            {
                return;
            }
        }

        Console.ReadLine();
        InitMenu();
    }

    public void Run()
    {
        InitMenu();

        // AddPlayer();
        // ShowRules();
        // DealCards();
        // PlayerTurn();
        //
        // if (!_player.IsBusted)
        //     DealerTurn();
        //
        // DeterminateWinner();
        //
        // Console.ReadLine();
    }
}
