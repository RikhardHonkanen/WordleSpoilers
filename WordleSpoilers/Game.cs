using System;
using System.Collections.Generic;
using System.Text;
using TextCopy;  // Add this for Clipboard

namespace WordleSpoilers
{
    internal class Game
    {
        // Member variables
        private List<(string guess, string result)> guesses;
        private string correctAnswer;
        private int wordleNo;

        // Constructor
        public Game(string answer, int number)
        {
            correctAnswer = answer;
            wordleNo = number;
            guesses = new List<(string, string)>();
        }

        // Read-only properties
        public string CorrectAnswer
        {
            get { return correctAnswer; }
        }
        public List<(string guess, string result)> Guesses
        {
            get { return guesses; }
        }

        // Method to add a guess and its result
        public void AddGuess(string guess, string result)
        {
            if (guess.Length == 5)  // Validate the guess length
            {
                guesses.Add((guess, result));
            }
            else
            {
                Console.WriteLine("Invalid guess length.");
            }
        }

        public void PrintGuesses() { 
            guesses?.ForEach(x => Console.WriteLine(x));
        }

        public string GenerateSpoiler()
        {
            bool solved = IsCorrect(guesses.Last().guess);
            string numberOfGuesses = solved ? guesses.Count.ToString() : "X";
            StringBuilder spoiler = new StringBuilder();
            spoiler.AppendLine($"Wordle {wordleNo:N0} {numberOfGuesses}/6*");
            foreach (var (guess, result) in guesses)
            {
                spoiler.AppendLine($"{result} ||{guess.ToUpper()}||");
            }
            string spoilerString = spoiler.ToString();
            ClipboardService.SetText(spoilerString); // Copy to clipboard using TextCopy
            return spoilerString;
        }

        public bool IsCorrect(string guess)
        {
            return guess.Equals(correctAnswer, StringComparison.OrdinalIgnoreCase);
        }
    }
}