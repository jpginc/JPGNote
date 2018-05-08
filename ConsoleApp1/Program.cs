using System;
using System.Collections.Generic;
using ConsoleApp1;

public class GtkHelloWorld
{
    public static void Main()
    {
        while (true)
        {
            var choice = GuiManager.Instance.GetChoices(GetChoices(), "What do you want to do?");
            switch (choice.Result)
            {
                case UserActionResult.ResultType.Canceled:
                    Console.WriteLine("cancelled!");
                    break;
                case UserActionResult.ResultType.Accept:
                    foreach (var s in choice.UserChoices)
                    {
                        Console.WriteLine(s.GetChoiceText());
                        s.OnAcceptCallback();
                    }
                    break;
                case UserActionResult.ResultType.Save:
                    break;
                case UserActionResult.ResultType.NoInput:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    private static IEnumerable<ITreeViewChoice> GetChoices()
    {
        var c = new List<ITreeViewChoice>
        {
            new SimpleTreeViewChoice("New Note"),
            new SimpleTreeViewChoice("Settings"),
            new SimpleTreeViewChoice("Exit")
        };
        return c;
    }
}