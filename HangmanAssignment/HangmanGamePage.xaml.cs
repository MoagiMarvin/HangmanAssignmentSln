namespace HangmanAssignment
{
    public partial class HangmanGamePage : ContentPage
    {
        private readonly string[] questions = new string[]
        {
            "What animal barks and is called man's best friend?",
            "What planet do we live on?",
            "What is frozen water called?",
            "What color is a banana?",
            "What do fish use to breathe?",
            "What bright object lights up our day?"
        };

        private readonly string[] answers = new string[]
        {
            "DOG",
            "EARTH",
            "ICE",
            "YELLOW",
            "GILLS",
            "SUN"
        };

        private string currentWord;
        private int currentIndex;
        private char[] displayWord;
        private int wrongGuesses;
        private Random random;

        public HangmanGamePage()
        {
            InitializeComponent();
            random = new Random();
            StartNewGame();
        }

        private void StartNewGame()
        {
            // Select random question
            currentIndex = random.Next(questions.Length);
            currentWord = answers[currentIndex];

            // Initialize display word with underscores
            displayWord = new char[currentWord.Length];
            for (int i = 0; i < displayWord.Length; i++)
            {
                displayWord[i] = '_';
            }

            // Reset game state
            wrongGuesses = 0;

            // Reset UI
            questionLabel.Text = questions[currentIndex];
            displayWordLabel.Text = string.Join(" ", displayWord);
            messageLabel.Text = "";
            letterEntry.Text = "";
            letterEntry.IsEnabled = true;
            guessButton.IsEnabled = true;
            hangmanImage.Source = "hang1.png";
        }

        private async void OnGuessClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(letterEntry.Text))
            {
                await DisplayAlert("Error", "Please enter a letter", "OK");
                return;
            }

            char guess = char.ToUpper(letterEntry.Text[0]);
            letterEntry.Text = "";

            if (!char.IsLetter(guess))
            {
                await DisplayAlert("Error", "Please enter a valid letter", "OK");
                return;
            }

            if (displayWord.Contains(guess))
            {
                messageLabel.Text = "You already guessed that letter!";
                return;
            }

            bool foundLetter = false;
            for (int i = 0; i < currentWord.Length; i++)
            {
                if (currentWord[i] == guess)
                {
                    displayWord[i] = guess;
                    foundLetter = true;
                }
            }

            displayWordLabel.Text = string.Join(" ", displayWord);

            if (!foundLetter)
            {
                wrongGuesses++;
                hangmanImage.Source = $"hang{wrongGuesses + 1}.png";
                messageLabel.Text = "Wrong guess! Try again.";

                if (wrongGuesses >= 6)
                {
                    await GameOver(false);
                    return;
                }
            }
            else
            {
                messageLabel.Text = "Good guess!";
            }

            if (!displayWord.Contains('_'))
            {
                await GameOver(true);
            }
        }

        private async Task GameOver(bool won)
        {
            letterEntry.IsEnabled = false;
            guessButton.IsEnabled = false;

            if (won)
            {
                messageLabel.Text = "Congratulations! You won!";
            }
            else
            {
                messageLabel.Text = $"Game Over! The word was {currentWord}";
            }

            bool playAgain = await DisplayAlert(
                won ? "Victory!" : "Game Over",
                $"{messageLabel.Text}\nWould you like to play again?",
                "Yes",
                "No");

            if (playAgain)
            {
                StartNewGame();
            }
        }
    }
}