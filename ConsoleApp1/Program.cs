using System;
using System.Collections.Generic;
using ConsoleApp1;

public class GtkHelloWorld
{
    public static void Main()
    {
        var manager = new GuiManager();
        while (true)
        {
            var choice = manager.GetChoice(GetChoices());
            switch (choice.Result)
            {
                case UserActionResult.ResultType.ExitApp:
                    return;
                case UserActionResult.ResultType.Canceled:
                    Console.WriteLine("cancelled!");
                    break;
                case UserActionResult.ResultType.Accept:
                    foreach (var s in choice.UserChoices) Console.WriteLine(s.GetChoiceText());
                    break;
                case UserActionResult.ResultType.NoInput:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    private static IEnumerable<ITreeViewChoice> GetChoices()
    {
        var treeViewChoice = new TreeViewChoice("Settings") {Selected = true};
        treeViewChoice.CalculateScore("test");
        var c = new List<ITreeViewChoice>
        {
            new TreeViewChoice("Projectss"),
            treeViewChoice,
            new TreeViewChoice("cccc"),
            new TreeViewChoice("test")
        };
        return c;
    }
}