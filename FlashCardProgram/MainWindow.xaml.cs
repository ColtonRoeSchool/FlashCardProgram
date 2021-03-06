using System;
using System.Collections.Generic;
using System.Windows;
using System.IO;

namespace FlashCardProgram
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Application.Current.MainWindow = this;

            // Check that the needed directories exist
            Directory.CreateDirectory(Deck.Deck_Directory);
            Directory.CreateDirectory(Deck.Img_Directory);

            PopulateListBox();
        }

        public void PopulateListBox()
        {
            List<string> list = new();
            foreach (string str in Directory.GetFiles(Deck.Deck_Directory))
            {
                list.Add(System.IO.Path.GetFileNameWithoutExtension(str));
            }
            DeckListBox.ItemsSource = list;
        }

        private void ViewButton_Click(object sender, RoutedEventArgs e)
        {
            if (DeckListBox.SelectedItem != null)
            {
                CardViewer newWindow = new(Deck.Deck_Directory + "/" + DeckListBox.SelectedItem.ToString() + ".txt");
                newWindow.Show();
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (DeckListBox.SelectedItem != null)
            {
                EditWindow editWindow = new(Deck.Deck_Directory + "/" + DeckListBox.SelectedItem.ToString() + ".txt");
                editWindow.Show();
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            PopulateListBox();
        }

        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            TextDialog textDialog = new("New Deck", "Deck name: ");
            textDialog.ShowDialog();

            if(textDialog.cancelled == false)
            {
                // Check if there is already a deck with the same name
                if(DeckListBox.Items.Contains(textDialog.userInput))
                {
                    MessageBox.Show(
                        "There is already a deck with than name",
                        "Cannot create a deck with than name",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                    );
                }
                else
                {
                    Deck newDeck = new(textDialog.userInput);
                    newDeck.Add(new Card("Front", "Back"));

                    EditWindow editWindow = new(newDeck);
                    editWindow.Show();
                }
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (DeckListBox.SelectedItem != null)
            {
                string messageBoxText = "Are you sure you want to permanantly delete this deck?";
                string caption = "Delete Deck";
                MessageBoxButton button = MessageBoxButton.YesNo;
                MessageBoxImage icon = MessageBoxImage.Warning;
                MessageBoxResult result;

                result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);

                if(result == MessageBoxResult.Yes)
                {
                    File.Delete(Deck.Deck_Directory + "/" + DeckListBox.SelectedItem.ToString() + ".txt");
                }
            }
            PopulateListBox();
        }
    }
}