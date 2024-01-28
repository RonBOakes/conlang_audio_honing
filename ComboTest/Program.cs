// See https://aka.ms/new-console-template for more information
using ConlangJson;

List<int> testList = new List<int> { 1, 2, 3 };
List<List<int>> combos = ConLangUtilities.allCombinations(testList);

foreach(List<int> combo in combos)
{
    Console.Write("(");
    foreach(int item in combo)
    {
        Console.Write(item + ", ");
    }
    Console.WriteLine(")");
}

