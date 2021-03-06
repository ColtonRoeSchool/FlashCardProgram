using System;
using System.Windows;
using System.Windows.Media.Imaging;
using System.IO;
using System.Diagnostics;

namespace FlashCardProgram
{
    public partial class EditWindow : Window
    {
        private readonly Deck deck;
        private int cardIndex;

        private bool cardSide;
        private const bool front = false;
        private const bool back  = true;

        public EditWindow(string path)
        {
            InitializeComponent();
            cardIndex = 0;
            cardSide = front;

            deck = Deck.ReadFromFile(path);

            DisplayCard();
        }

        public EditWindow(Deck pDeck)
        {
            InitializeComponent();
            cardIndex = 0;
            cardSide = front;

            deck = pDeck;

            DisplayCard();
        }

        public void DisplayCardImage(string image)
        {
            // Image is not needed:
            if (cardSide == front && image == "")
            {
                // Show no image if there is no path for front
                CardImage.Source = null;
            }
            else if (cardSide == back && image == "")
            {
                // Show the front image if there is no path for back
                string frontImage = deck.Get(cardIndex).FrontImage;
                if (frontImage != "")
                {
                    DisplayCardImage(frontImage);
                }
                else
                {
                    CardImage.Source = null;
                }
            }
            else
            {
                // Image is needed:
                try
                {
                    CardImage.Source = new BitmapImage(new Uri(Deck.Img_Directory + image));
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
                CardText.Text = deck.Get(cardIndex).FrontText;
                DisplayCardImage(deck.Get(cardIndex).FrontImage);

                ShowButton.Content = "Edit Back";
            }
            else if (cardSide == back)
            {
                Debug.WriteLine(cardIndex);   
                cardSide = back;
                CardText.Text = deck.Get(cardIndex).BackText;
                DisplayCardImage(deck.Get(cardIndex).BackImage);

                ShowButton.Content = "Edit Front";
            }
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

        private void ShowButton_Click(object sender, RoutedEventArgs e)
        {
            cardSide = !cardSide;
            DisplayCard();
        }

        private void EditTextButton_Click(object sender, RoutedEventArgs e)
        {
            Card card = deck.Get(cardIndex);

            TextDialog textDialog = new(CardText.Text);
            textDialog.ShowDialog();

            // Check if cancelled
            if(textDialog.cancelled == false)
            {
                string result = textDialog.userInput;
                // Font or back of card
                if (cardSide == front)
                {
                    card.FrontText = result;
                    DisplayCard();
                }
                else if (cardSide == back)
                {
                    card.BackText = result;
                    DisplayCard();
                }
            }
        }
        private void EditImageButton_Click(object sender, RoutedEventArgs e)
        {
            Card card = deck.Get(cardIndex);

            Microsoft.Win32.OpenFileDialog imageDialogue = new();
            imageDialogue.Filter = "Image files(*.png; *.jpeg, *.jpg)| *.png; *.jpeg; *.jpg";

            if (imageDialogue.ShowDialog() == true)
            {
                // Copy the image to the images directory and then use that copy as the image source
                string sourceFile = imageDialogue.FileName;
                string fileName = System.IO.Path.GetFileName(sourceFile);
                string targetFile = Deck.Img_Directory + "/" + fileName;
                try
                {
                    System.IO.File.Copy(sourceFile, targetFile, true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, ex.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
                    CardImage.Source = new BitmapImage(new Uri(Deck.Img_Directory + "/error.png"));
                    return;
                }
                // Front or back of card
                if (cardSide == front)
                {
                    card.FrontImage = fileName;
                    DisplayCard();
                }
                else if (cardSide == back)
                {
                    card.BackImage = fileName;
                    DisplayCard();
                }
            }
        }


        private void RemoveImageButton_Click(object sender, RoutedEventArgs e)
        {
            Card card = deck.Get(cardIndex);

            if (cardSide == front)
            {
                card.FrontImage = "";
                DisplayCard();
            }
            else if (cardSide == back)
            {
                card.BackImage = "";
                DisplayCard();
            }
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            Deck.SaveToFile(deck);
            ((MainWindow)Application.Current.MainWindow).PopulateListBox();
            this.Close();
        }

        private void NewCardButton_Click(object sender, RoutedEventArgs e)
        {
            deck.Insert(cardIndex + 1,new Card());
            cardIndex++;
            cardSide = front;
            DisplayCard();
        }

        private void RemoveCardButton_Click(object sender, RoutedEventArgs e)
        {
            if (deck.cards.Count > 1)
            {
                deck.Remove(deck.Get(cardIndex));
                cardIndex--;
                cardSide = front;
                // Check if index is out of bounds
                if (cardIndex < 0) cardIndex = deck.cards.Count - 1;
                DisplayCard();
            }
            else
            {
                MessageBox.Show("You cannot delete the last card in the Deck", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void RenameDeckButton_Click(object sender, RoutedEventArgs e)
        {
            TextDialog textDialog = new(deck.name);
            textDialog.ShowDialog();
            // Check if cancelled
            if (textDialog.cancelled == false)
            {
                string result = textDialog.userInput;
                File.Delete(Deck.Deck_Directory + "/" + deck.name + ".txt");
                deck.name = result;
                Deck.SaveToFile(deck);
                ((MainWindow)Application.Current.MainWindow).PopulateListBox();
            }
        }
    }
}
