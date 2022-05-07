using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FlashCardProgram
{
    public class Deck
    {
        public string name;
        public List<Card> cards;

        // Do not change these values!
        public readonly static string Deck_Directory = Directory.GetCurrentDirectory() + "/decks/";
        public readonly static string Img_Directory  = Directory.GetCurrentDirectory() + "/images/";

        public Deck(string pName)
        {
            name = pName;
            cards = new List<Card>();
        }

        public Card Get(int index)
        {
            return cards[index];
        }

        public void Add(Card card)
        {
            cards.Add(card);
        }

        public void Insert(int index, Card card)
        {
            cards.Insert(index, card);
        }

        public void Remove(Card card)
        {
            cards.Remove(card);
        }

        // Saving and Loading
        public static void SaveToFile(Deck deck)
        {
            string directory = Directory.GetCurrentDirectory();

            using StreamWriter writer = new(directory + "/decks/" + deck.name + ".txt");
            writer.Write(deck.name + "," + deck.cards.Count + "\n");
            foreach(Card card in deck.cards)
            {
                writer.Write(card.FrontText + ",");
                writer.Write(card.BackText + ",");
                writer.Write(card.FrontImage + ",");
                writer.Write(card.BackImage);
                writer.Write("\n");
            }
        }

        public static Deck ReadFromFile(string path)
        {
            Deck deck;
            using StreamReader sr = new(path);
            
            string? line;
            line = sr.ReadLine();

            // If reading file failes:
            if (line == null) return new Deck("Could not read file!");

            string name = line.Split(",")[0];
            int count = Convert.ToInt32(line.Split(",")[1]);

            deck = new Deck(name);

            for (int i = 0; i < count; i++)
            {
                line = sr.ReadLine();

                // If reading file failes:
                if (line == null) return new Deck("Error while reading file!");

                string[] strlist = line.Split(",");

                string frontText = strlist[0];
                string backText = strlist[1];
                string frontImagePath = strlist[2];
                string backImagePath = strlist[3];

                deck.Add(new Card(frontText, backText, frontImagePath, backImagePath));
            }
            
            return deck;
        }
    }
}