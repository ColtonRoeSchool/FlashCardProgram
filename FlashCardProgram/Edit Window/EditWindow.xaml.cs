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
using System.Diagnostics;


namespace FlashCardProgram
{
    public partial class EditWindow : Window
    {
        private Deck deck;
        private int cardIndex;
        bool showingBack;

        public EditWindow(string path)
        {
            InitializeComponent();
            cardIndex = 0;
            deck = Deck.readFromFile(path);
            displayCardFront();
        }

        public EditWindow(Deck pDeck)
        {
            InitializeComponent();
            cardIndex = 0;
            deck = pDeck;
            displayCardFront();
        }

        public void displayCardImage(string path)
        {
            // Image is not needed:
            if (!showingBack && path == "")
            {
                // Show no image if there is no path for front
                cardImage.Source = null;
                return;
            }
            else if (showingBack && path == "")
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

        public void displayCardFront()
        {
            showingBack = false;
            cardText.Text = deck.get(cardIndex).FrontText;
            displayCardImage(deck.get(cardIndex).FrontImage);

            showButton.Content = "Edit Back";
        }

        public void displayCardBack()
        {
            showingBack = true;
            cardText.Text = deck.get(cardIndex).BackText;
            displayCardImage(deck.get(cardIndex).BackImage);

            showButton.Content = "Edit Front";
        }

        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            cardIndex++;
            // Check if index is out of bounds
            if (cardIndex >= deck.cards.Count) cardIndex = 0;
            displayCardFront();
        }

        private void previousButton_Click(object sender, RoutedEventArgs e)
        {
            cardIndex--;
            // Check if index is out of bounds
            if (cardIndex < 0) cardIndex = deck.cards.Count - 1;
            displayCardFront();
        }

        private void showButton_Click(object sender, RoutedEventArgs e)
        {
            if (showingBack)
            {
                displayCardFront();
            }
            else
            {
                displayCardBack();
            }
        }

        private void editTextButton_Click(object sender, RoutedEventArgs e)
        {
            Card card = deck.get(cardIndex);

            TextDialog textDialog = new TextDialog(cardText.Text);
            textDialog.ShowDialog();

            // Check if cancelled
            if(textDialog.cancelled)
            {
                return;
            }
            else
            {
                string result = textDialog.userInput;
                // Font or back of card
                if (!showingBack)
                {
                    card.FrontText = result;
                    displayCardFront();
                }
                else if (showingBack)
                {
                    card.BackText = result;
                    displayCardBack();
                }
            }
        }
        private void editImageButton_Click(object sender, RoutedEventArgs e)
        {
            Card card = deck.get(cardIndex);

            Microsoft.Win32.OpenFileDialog imageDialogue = new Microsoft.Win32.OpenFileDialog();
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
                } catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, ex.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                Debug.WriteLine(imageDialogue.FileName);
                // Front or back of card
                if (!showingBack)
                {
                    card.FrontImage = fileName;
                    displayCardFront();
                }
                else if (showingBack)
                {
                    card.BackImage = fileName;
                    displayCardBack();
                }
            }
        }


        private void removeImageButton_Click(object sender, RoutedEventArgs e)
        {
            Card card = deck.get(cardIndex);

            if (!showingBack)
            {
                card.FrontImage = "";
                displayCardFront();
            }
            else if (showingBack)
            {
                card.BackImage = "";
                displayCardBack();
            }
        }

        private void submitButton_Click(object sender, RoutedEventArgs e)
        {
            Deck.saveToFile(deck);
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.populateListBox();
            this.Close();
        }

        private void newCardButton_Click(object sender, RoutedEventArgs e)
        {
            deck.insert(cardIndex + 1,new Card());
            cardIndex++;
            displayCardFront();
        }

        private void removeCardButton_Click(object sender, RoutedEventArgs e)
        {
            if (deck.cards.Count > 1)
            {
                deck.remove(deck.get(cardIndex));
                cardIndex--;
                // Check if index is out of bounds
                if (cardIndex < 0) cardIndex = deck.cards.Count - 1;
                displayCardFront();
            }
            else
            {
                MessageBox.Show("You cannot delete the last card in the Deck", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void renameDeckButton_Click(object sender, RoutedEventArgs e)
        {
            Card card = deck.get(cardIndex);

            TextDialog textDialog = new TextDialog(deck.name);
            textDialog.ShowDialog();

            // Check if cancelled
            if (textDialog.cancelled)
            {
                return;
            }
            else
            {
                string result = textDialog.userInput;
                File.Delete(Deck.Deck_Directory + "/" + deck.name + ".txt");
                deck.name = result;
                Deck.saveToFile(deck);
                ((MainWindow)Application.Current.MainWindow).populateListBox();
            }
        }
    }
}
