using System;
using System.Collections.Generic;
using ConsoleApp1;

public class GtkHelloWorld
{
    public static void Main()
    {
        SettingsClass.Start("c:\\jpgtree\\settings.txt");
        while (true)
        {
            var choice = GuiManager.Instance.GetChoice(GetChoices(), "What do you want to do?");
            switch (choice.Result)
            {
                case UserActionResult.ResultType.Canceled:
                    Console.WriteLine("cancelled!");
                    break;
                case UserActionResult.ResultType.Accept:
                    foreach (var s in choice.UserChoices)
                    {
                        Console.WriteLine(s.GetChoiceText());
                        s.OnAcceptCallback(choice);
                    }
                    break;
                case UserActionResult.ResultType.Save:
                    foreach (var s in choice.UserChoices)
                    {
                        Console.WriteLine(s.GetChoiceText());
                        s.OnSaveCallback(choice);
                    }
                    break;
                case UserActionResult.ResultType.NoInput:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    private static IEnumerable<ITreeViewChoice> GetChoices()
    {
        return ActionManager.GetActions();
    }
}