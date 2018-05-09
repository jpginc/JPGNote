using System;
using System.Collections.Generic;
using ConsoleApp1;

public class GtkHelloWorld
{
    private static readonly string _fileName = "c:\\jpgtree\\settings.txt";

    public static void Main()
    {
        InitialiseSettingsClass();
        while (true)
        {
            var choice = GuiManager.Instance.GetChoice(GetChoices(), "What do you want to do?");
            switch (choice.Result)
            {
                case UserActionResult.ResultType.Canceled:
                    Console.WriteLine("cancelled!");
                    JpgActionManager.Instance.UnrollActionContext();
                    break;
                case UserActionResult.ResultType.Accept:
                    foreach (var s in choice.UserChoices)
                    {
                        s.OnAcceptCallback(choice);
                    }
                    break;
                case UserActionResult.ResultType.Save:
                    foreach (var s in choice.UserChoices)
                    {
                        s.OnSaveCallback(choice);
                    }

                    break;
                case UserActionResult.ResultType.NoInput:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    private static void InitialiseSettingsClass()
    {
        SettingsClass.Start(_fileName , GetPassword());
    }

    private static string GetPassword()
    {
        Console.Write("Enter password: ");
        var pwd = "";
        while (true)
        {
            ConsoleKeyInfo i = Console.ReadKey(true);
            if (i.Key == ConsoleKey.Enter)
            {
                break;
            }
            else if (i.Key == ConsoleKey.Backspace)
            {
                if (pwd.Length > 0)
                {
                    pwd = pwd.Substring(0, pwd.Length - 1);
                    Console.Write("\b \b");
                }
            }
            else
            {
                pwd += i.KeyChar;
                Console.Write("*");
            }
        }

        return pwd;
    }

    private static IEnumerable<ITreeViewChoice> GetChoices()
    {
        return JpgActionManager.Instance.GetActions();
    }
}