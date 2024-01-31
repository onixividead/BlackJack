namespace BlackJack
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Welcome to Blackjack!");

            while (true)
            {
                Console.WriteLine("\nA new game is starting...");

                List<Card> deck = CreateDeck();
                ShuffleDeck(deck);

                List<Card> playerHand = DealCards(deck, 2);
                List<Card> dealerHand = DealCards(deck, 2);

                Console.WriteLine($"Your cards: {GetHandString(playerHand)}");
                Console.WriteLine($"Dealer's cards: {dealerHand[0]} and a hidden card");

                PlayerTurn(playerHand, deck);

                DealerTurn(dealerHand, deck);

                DetermineWinner(playerHand, dealerHand);

                Console.Write("\nDo you want to play again? (y/n): ");
                if (Console.ReadLine().ToLower() != "y")
                    break;
            }

            Console.WriteLine("Thanks for playing!");

            Console.WriteLine("Press any key to close this window...");
            Console.ReadKey();
        }

        static List<Card> CreateDeck()
        {
            List<Card> deck = new List<Card>();
            string[] suits = { "Hearts", "Diamonds", "Clubs", "Spades" };
            string[] ranks = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "Jack", "Queen", "King", "Ace" };

            foreach (var suit in suits)
            {
                foreach (var rank in ranks)
                {
                    deck.Add(new Card(suit, rank));
                }
            }

            return deck;
        }

        static void ShuffleDeck(List<Card> deck)
        {
            Random random = new Random();
            int n = deck.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                Card value = deck[k];
                deck[k] = deck[n];
                deck[n] = value;
            }
        }

        static List<Card> DealCards(List<Card> deck, int numCards)
        {
            List<Card> hand = deck.Take(numCards).ToList();
            deck.RemoveRange(0, numCards);
            return hand;
        }

        static string GetHandString(List<Card> hand)
        {
            return string.Join(", ", hand);
        }

        static void PlayerTurn(List<Card> playerHand, List<Card> deck)
        {
            while (true)
            {
                Console.WriteLine($"\nYour cards: {GetHandString(playerHand)}");
                Console.WriteLine($"Your score: {GetHandValue(playerHand)}");

                Console.Write("Do you want to draw another card? (y/n): ");
                var input = Console.ReadLine().ToLower();
                if (input == "y")
                {
                    playerHand.Add(deck[0]);
                    deck.RemoveAt(0);

                    if (GetHandValue(playerHand) > 21)
                    {
                        Console.WriteLine("You busted! Too many points.");
                        return;
                    }
                }
                else if (input == "n")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid input! Please type 'y' for yes or 'n' for no.");
                }
            }
        }

        static void DealerTurn(List<Card> dealerHand, List<Card> deck)
        {
            Console.WriteLine("\nDealer's turn...");
            while (GetHandValue(dealerHand) < 17)
            {
                dealerHand.Add(deck[0]);
                deck.RemoveAt(0);
            }

            Console.WriteLine($"Dealer's cards: {GetHandString(dealerHand)}");
            Console.WriteLine($"Dealer's score: {GetHandValue(dealerHand)}");
        }

        static int GetHandValue(List<Card> hand)
        {
            int value = hand.Sum(card => GetCardValue(card));

            int numAces = hand.Count(card => card.Rank == "Ace");
            while (value > 21 && numAces > 0)
            {
                value -= 10;
                numAces--;
            }

            return value;
        }

        static int GetCardValue(Card card)
        {
            if (card.Rank == "Ace") return 11;
            if (card.Rank == "Jack" || card.Rank == "Queen" || card.Rank == "King") return 10;
            return int.Parse(card.Rank);
        }

        static void DetermineWinner(List<Card> playerHand, List<Card> dealerHand)
        {
            int playerValue = GetHandValue(playerHand);
            int dealerValue = GetHandValue(dealerHand);

            Console.WriteLine($"\nYour cards: {GetHandString(playerHand)}");
            Console.WriteLine($"Your score: {playerValue}");
            Console.WriteLine($"\nDealer's cards: {GetHandString(dealerHand)}");
            Console.WriteLine($"Dealer's score: {dealerValue}");

            if (playerValue > 21)
            {
                Console.WriteLine("You lost! Busted with too many points.");
            }
            else if (dealerValue > 21 || playerValue > dealerValue)
            {
                Console.WriteLine("Congratulations! You won!");
            }
            else if (playerValue < dealerValue)
            {
                Console.WriteLine("You lost. Dealer wins.");
            }
            else
            {
                Console.WriteLine("It's a tie! Both players have the same score.");
            }
        }
    }

    class Card
    {
        public string Suit { get; }
        public string Rank { get; }

        public Card(string suit, string rank)
        {
            Suit = suit;
            Rank = rank;
        }

        public override string ToString()
        {
            return $"{Rank} of {Suit}";
        }
    }
}
