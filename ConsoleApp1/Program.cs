using System;
using ConsoleApp1;

public class GtkHelloWorld
{
    private static readonly string _fileName = "c:\\jpgtree\\settings.txt";

    public static void Main()
    {
        InitialiseSettingsClass();
        var actionManager = new JpgActionManager();
        while (true)
        {
            var choice = GuiManager.Instance.GetUserInput(actionManager, "What do you want to do?");
            if (actionManager.CurrentActionProvider.HandleUserAction(choice)
                == ActionProviderResult.ProcessingFinished)
                continue;
            switch (choice.Result)
            {
                case UserActionResult.ResultType.Canceled:
                    JpgActionManager.UnrollActionContext();
                    break;
                case UserActionResult.ResultType.Accept:
                    foreach (var s in choice.UserChoices) s.OnAcceptCallback(choice);
                    break;
                case UserActionResult.ResultType.Save:
                    foreach (var s in choice.UserChoices) s.OnSaveCallback(choice);

                    break;
                case UserActionResult.ResultType.NoInput:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    private static void InitialiseSettingsClass()
    {
        ProgramSettingsClass.Start(_fileName, GetPassword());
    }

    private static string GetPassword()
    {
        var password = GuiManager.Instance.GetPassword().Result;
        return password ?? "";
    }
}