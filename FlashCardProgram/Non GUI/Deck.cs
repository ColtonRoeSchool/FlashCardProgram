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
        public static string Deck_Directory = Directory.GetCurrentDirectory() + "/decks/";
        public static string Img_Directory = Directory.GetCurrentDirectory() + "/images/";

        public Deck(string pName)
        {
            name = pName;
            cards = new List<Card>();
        }

        public Card get(int index)
        {
            return cards[index];
        }

        public void add(Card card)
        {
            cards.Add(card);
        }

        public void insert(int index, Card card)
        {
            cards.Insert(index, card);
        }

        public void remove(Card card)
        {
            cards.Remove(card);
        }

        // Saving and Loading
        public static void saveToFile(Deck deck)
        {
            string directory = Directory.GetCurrentDirectory();

            using (StreamWriter writer = new StreamWriter(directory + "/decks/" +  deck.name + ".txt"))
            {
                // Name and count
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
        }

        public static Deck readFromFile(string path)
        {
            Deck deck;
            using (StreamReader sr = new StreamReader(path))
            {
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

                    deck.add(new Card(frontText, backText, frontImagePath, backImagePath));
                }
            }

            return deck;
        }
    }
}