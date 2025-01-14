// See https://aka.ms/new-console-template for more information
using StringMatrix;

var matrix = new List<string>
{
   "abcdc",
   "fgwio",
   "chill",
   "pqnsd",
   "uvdxy"
};

var wordFinder = new WordFinder(matrix);

var wordStream = new List<string>
{
   "chill",
   "cold",
   "wind"
};

var result = wordFinder.Find(wordStream);

foreach (var word in result)
    Console.WriteLine(word);