using System.Text;
using WordleSpoilers;

Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~");
Console.WriteLine("~Welcome to WordleSpoilers~");
Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~");
Console.WriteLine();

int wordleNo = GetWordleNo();
var game = new Game(GetAnswer(), wordleNo);
PopulateGuesses(game);
game.PrintGuesses();
Console.WriteLine(game.GenerateSpoiler());
Console.WriteLine();
Console.WriteLine("Spoiler copied to your clipboard!");
Console.WriteLine("(press any key to exit)");
Console.ReadKey();


string GetAnswer()
{
    string answer = "";
    bool validInput = false;

    while (!validInput)
    {
        Console.WriteLine("Input the correct answer:");
        answer = Console.ReadLine() ?? "";
        Console.WriteLine();

        if (!string.IsNullOrEmpty(answer) && answer.Length == 5)
        {
            validInput = true;
        }
        else
        {
            Console.WriteLine("Answer must be 5 characters.");
        }
    }

    return answer;
}

void PopulateGuesses(Game game)
{
    while (true)
    {
        if (game.Guesses?.Count() > 5)
        {
            break;
        }
        Console.WriteLine("Input a guess (blank if done):");
        string guess = Console.ReadLine() ?? "";
        Console.WriteLine();
        if (guess == "")
        {
            break;
        }
        if (!string.IsNullOrEmpty(guess) && guess.Length == 5)
        {
            string result = GenerateResultString(guess, game.CorrectAnswer);
            game.AddGuess(guess, result);
            if (game.IsCorrect(guess))
            {
                break;
            }
        }
        else
        {
            Console.WriteLine("Guess must be 5 characters, try again!");
        }
    }
}

string GenerateResultString(string guess, string correctAnswer)
{
    List<string> result = new List<string>()
    {
        ":black_large_square:", ":black_large_square:", ":black_large_square:", ":black_large_square:", ":black_large_square:"
    };

    string guess_without_greens = guess;
    foreach (var (c, i) in guess.Select((c, i) => (c, i)))
    {
        if (correctAnswer[i] == c)  // Character matches at the same index
        {
            correctAnswer = ReplaceCharacter(correctAnswer, i, '_');
            guess_without_greens = ReplaceCharacter(guess_without_greens, i, '&');
            result[i] = ":green_square:";
        }        
    }

    foreach (var (c, i) in guess_without_greens.Select((c, i) => (c, i)))
    {
        if (c == '&')
        {
            continue;
        }
        if (correctAnswer.Contains(c))  // Character exists elsewhere
        {
            correctAnswer = ReplaceCharacter(correctAnswer, correctAnswer.IndexOf(c), '_');
            result[i] = ":yellow_square:";
        }
    }

    return string.Concat(result);
}

int GetWordleNo()
{
    int ref_no = 1264;
    DateTime ref_date = new DateTime(2024, 12, 4);
    int diff = (DateTime.Today - ref_date).Days;
    int wordleNo = ref_no + diff;

    Console.WriteLine($"Wordle no. {wordleNo:N0}, enter to accept or input different number:");
    while (true)
    {
        string answer = Console.ReadLine() ?? "";
        Console.WriteLine();

        if (!string.IsNullOrEmpty(answer) && int.TryParse(answer, out int result))
        {
            return result;

        }
        else if (string.IsNullOrEmpty(answer))
        {
            return wordleNo;
        }
        else
        {
            Console.WriteLine("Numerical values (or empty) only, try again:");
        }
    }


}

string ReplaceCharacter(string input, int index, char newChar)
{
    var stringBuilder = new StringBuilder(input);
    stringBuilder[index] = newChar;  // Replace the character at the specified index
    return stringBuilder.ToString();
}