/*
 * This Higher or Lower game has been designed to take an integer value for the number of decks used in the game (numberOfDeck), and an integer value for the number of cards per suit (newSuitSize), 
 * before selecting a random card for the player and opponent, and declaring a winner.
*/

// If you would like to include the debugging comments that print the encoding and decoding results to the console, un-comment the #define debug variable below.
//#define debug

using System;

namespace Question2
{
    class Question2
    {
        private static readonly int numberOfDecks = 1;
        private static readonly int newSuitSize = 13;

        static void Main(string[] args)
        {
            Deck newDeck = new(numberOfDecks, newSuitSize);
            HighCard.Play(newDeck);
        }
    }

    class Deck
    {
        private  readonly List<KeyValuePair<string, int>> tempDeck;
        private readonly string[] suits = { "Spades", "Clubs", "Hearts", "Diamonds" };
        private readonly int suitSize;
        private readonly int deckSize;
        private readonly int deckNum;

        public Deck(int numberOfDecks, int newSuitSize)
        {
            suitSize = newSuitSize;
            deckSize = newSuitSize * suits.Length + 1;
            deckNum = numberOfDecks;
#if debug
            Console.WriteLine("[DEBUG] The size of the game's deck is: " + (deckNum * deckSize) + "\n");
#endif
            tempDeck = new List<KeyValuePair<string, int>>(deckNum * deckSize);


            for (int i = 0; i < deckNum; i++) { BuildDeck(); }

            tempDeck.Add(new KeyValuePair<string, int>("Joker", tempDeck.Count + 1));
#if debug
            Console.WriteLine("[DEBUG] The cards in the deck:");
            foreach (KeyValuePair<string, int> card in tempDeck) { Console.WriteLine(card); }
            Console.WriteLine("\n");
#endif
        }

        private void BuildDeck()
        {
            int value = 1;

            for (int j = 0; j < suitSize; j++)
            {
                foreach (string suit in suits)
                {
                    int cardValue = j % suitSize + 1;
                    switch (cardValue)
                    {
                        case 1:
                            tempDeck.Add(new KeyValuePair<string, int>("Ace of " + suit, value++));
                            break;
                        case 11:
                            tempDeck.Add(new KeyValuePair<string, int>("Jack of " + suit, value++));
                            break;
                        case 12:
                            tempDeck.Add(new KeyValuePair<string, int>("Queen of " + suit, value++));
                            break;
                        case 13:
                            tempDeck.Add(new KeyValuePair<string, int>("King of " + suit, value++));
                            break;
                        default:
                            tempDeck.Add(new KeyValuePair<string, int>(cardValue + " of " + suit, value++));
                            break;
                    }
                }
            }
        }

        public List<KeyValuePair<string, int>> GetDeck()
        {
            return tempDeck;
        }
    }

    class HighCard
    {
        public static void Play(Deck newDeck)
        {
            List<KeyValuePair<string, int>> gameDeck = newDeck.GetDeck();
            Random rnd = new();

            KeyValuePair<string, int> playerCard = gameDeck[rnd.Next(1, gameDeck.Count) ];
            KeyValuePair<string, int> opponentCard = gameDeck[rnd.Next(1, gameDeck.Count) ];

            Console.WriteLine("Player has " + playerCard.Key + ". \nOpponent has " + opponentCard.Key + ".\n");

            if (playerCard.Value > opponentCard.Value) { Console.WriteLine("Player 1 Wins."); }
            else if (playerCard.Value < opponentCard.Value) { Console.WriteLine("Opponent Wins."); }
            else if (playerCard.Value == opponentCard.Value)
            {
                Console.WriteLine("It's a draw. \n\nNew cards will be dealt.");
                Play(newDeck);
            }
            else { Console.WriteLine("An error has occured, please close the game."); }
#if debug
            Console.WriteLine("\n[DEBUG] Player 1 has " + playerCard.Key + " with a value of " + playerCard.Value + ", and oppenent has " + opponentCard.Key + " with a value of " + opponentCard.Value + ".");
#endif
        }
    }
}


