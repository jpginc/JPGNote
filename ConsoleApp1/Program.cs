using System;
using System.Collections.Generic;
using ConsoleApp1;

public class GtkHelloWorld
{
    public static void Main()
    {
        var manager = new GuiManager();
        var choice = manager.GetChoice(GetChoices());
        if (choice.Result == UserActionResult.ResultType.Accept)
        {
            foreach (var s in choice.UserChoices) Console.WriteLine(s.GetChoiceText());
            ;
        }

        manager.GetChoices(GetChoices());
        if (choice.Result == UserActionResult.ResultType.Accept)
        {
            foreach (var s in choice.UserChoices) Console.WriteLine(s.GetChoiceText());
            ;
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