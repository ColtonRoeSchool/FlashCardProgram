using System;
using System.Windows;

namespace FlashCardProgram
{
    public partial class TextDialog : Window
    {
        public string userInput;
        public bool cancelled;

        public TextDialog()
        {
            InitializeComponent();
            userInput = String.Empty;
            cancelled = true;
        }

        public TextDialog(string text) : this()
        {
            textBox.Text = text;
            textLabel.Content = "Text:";
        }

        public TextDialog(string text, string prompt) : this(text)
        {
            textLabel.Content = prompt;
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(textBox.Text))
            {
                userInput = textBox.Text;
                cancelled = false;
                this.Close();
            }
            else
            {
                MessageBox.Show("Must provide text in the textbox to submit!");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            cancelled = true;
            userInput = String.Empty;
            this.Close();
        }
    }
}
