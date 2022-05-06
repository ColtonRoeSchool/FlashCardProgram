using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace FlashCardProgram
{
    // This class will display one deck of cards
    public partial class CardViewer : Window
    {
        private readonly Deck deck;
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
            DisplayCard();
        }

        public void DisplayCardImage(string path)
        {
            // Image is not needed:
            if (cardSide == front && path == "")
            {
                // Show no image if there is no path for front
                CardImage.Source = null;
            }
            else if (cardSide == back && path == "")
            {
                // Show the front image if there is no path for back
            }
            else
            {
                // Image is needed:
                try
                {
                    CardImage.Source = new BitmapImage(new Uri(Deck.Img_Directory + path));
                }
                catch (Exception ex)
                {
                    CardImage.Source = new BitmapImage(new Uri(Deck.Img_Directory + "/error.png"));
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public void DisplayCard()
        {
            if (cardSide == front)
            {
                cardSide = front;
                CardText.Text = deck.get(cardIndex).FrontText;
                DisplayCardImage(deck.get(cardIndex).FrontImage);

                ShowButton.Content = "Show Back";
            }
            else if (cardSide == back)
            {
                cardSide = back;
                CardText.Text = deck.get(cardIndex).BackText;
                DisplayCardImage(deck.get(cardIndex).BackImage);

                ShowButton.Content = "Show Front";
            }
        }

        private void ShowButton_Click(object sender, RoutedEventArgs e)
        {
            cardSide = !cardSide;
            DisplayCard();
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            cardIndex++;
            cardSide = front;
            // Check if index is out of bounds
            if (cardIndex >= deck.cards.Count) cardIndex = 0;
            DisplayCard();
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            cardIndex--;
            cardSide = front;
            // Check if index is out of bounds
            if (cardIndex < 0) cardIndex = deck.cards.Count - 1;
            DisplayCard();
        }
    }
}
