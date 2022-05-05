using System;
using System.Collections.Generic;
using System.Windows;
using System.IO;

namespace FlashCardProgram
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Application.Current.MainWindow = this;

            populateListBox();
        }

        public void populateListBox()
        {
            List<string> list = new List<string>();
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
                CardViewer newWindow = new CardViewer(Deck.Deck_Directory + "/" + DeckListBox.SelectedItem.ToString() + ".txt");
                newWindow.Show();
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            EditWindow editWindow = new EditWindow(Deck.Deck_Directory + "/" + DeckListBox.SelectedItem.ToString() + ".txt");
            editWindow.Show();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            populateListBox();
        }

        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            TextDialog textDialog = new TextDialog("New Deck", "Deck name: ");
            textDialog.ShowDialog();

            if(textDialog.cancelled)
            {
                return;
            }
            else
            {
                // Check if there is already a deck with the same name
                if(DeckListBox.Items.Contains(textDialog.userInput))
                {
                    MessageBox.Show("There is already a deck with than name", "Cannot create a deck with than name", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                Deck newDeck = new Deck(textDialog.userInput);
                newDeck.add(new Card("Front", "Back"));
                EditWindow editWindow = new EditWindow(newDeck);
                editWindow.Show();
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (DeckListBox.SelectedItem != null)
            {
                File.Delete(Deck.Deck_Directory + "/" + DeckListBox.SelectedItem.ToString() + ".txt");
            }
            populateListBox();
        }
    }
}