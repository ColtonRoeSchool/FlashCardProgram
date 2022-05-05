using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;

namespace FlashCardProgram
{
    // This class will display one deck of cards
    public partial class CardViewer : Window
    {
        private Deck deck;
        private int cardIndex;
        bool cardSide;

        private const bool front = false;
        private const bool back = true;

        public CardViewer(string path)
        {
            InitializeComponent();
            cardIndex = 0;
            cardSide = front;
            deck = Deck.readFromFile(path);
            displayCard();
        }

        public void displayCardImage(string path)
        {
            // Image is not needed:
            if(cardSide == front && path == "")
            {
                // Show no image if there is no path for front
                cardImage.Source = null;
                return;
            }
            else if (cardSide == back && path == "")
            {
                // Show the front image if there is no path for back
                return;
            }

            // Image is needed:
            try
            {
                cardImage.Source = new BitmapImage(new Uri(Deck.Img_Directory + path));
            }
            catch (Exception ex)
            {
                // TODO: Create image that informs user image could not be loaded
                cardImage.Source = null;
                Console.WriteLine(ex.Message);
            }
        }

        public void displayCard()
        {
            if (cardSide == front)
            {
                cardSide = front;
                cardText.Text = deck.get(cardIndex).FrontText;
                displayCardImage(deck.get(cardIndex).FrontImage);

                showButton.Content = "Show Back";
            }
            else if (cardSide == back)
            {
                cardSide = back;
                cardText.Text = deck.get(cardIndex).BackText;
                displayCardImage(deck.get(cardIndex).BackImage);

                showButton.Content = "Show Front";
            }
        }

        private void showButton_Click(object sender, RoutedEventArgs e)
        {
            cardSide = !cardSide;
            displayCard();
        }

        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            cardIndex++;
            cardSide = front;
            // Check if index is out of bounds
            if (cardIndex >= deck.cards.Count) cardIndex = 0;
            displayCard();
        }

        private void previousButton_Click(object sender, RoutedEventArgs e)
        {
            cardIndex--;
            cardSide = front;
            // Check if index is out of bounds
            if (cardIndex < 0) cardIndex = deck.cards.Count - 1;
            displayCard();
        }
    }
}
