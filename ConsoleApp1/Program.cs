using System;
using ConsoleApp1;

public class GtkHelloWorld
{
    private static readonly string _fileName = "settings2.txt";
    private static readonly string _folderName = "c:\\jpgtree";
    private static JpgActionManager _jpgActionManager;

    public static void Main()
    {
        _jpgActionManager = new JpgActionManager();
        InitialiseSettingsClass();
        GuiManager.Instance.AcceptCallback = HandleInput;
        GuiManager.Instance.GetReady(_jpgActionManager, "What do you want to do?");
        GuiManager.Instance.Go();
    }

    private static bool HandleInput(UserActionResult choice)
    {
        if (_jpgActionManager.CurrentActionProvider.HandleUserAction(choice)
            == ActionProviderResult.ProcessingFinished)
            return Next();
        switch (choice.Result)
        {
            case UserActionResult.ResultType.Canceled:
                JpgActionManager.UnrollActionContext();
                break;
            case UserActionResult.ResultType.Accept:
                foreach (var s in choice.UserChoices)
                {
                    Console.WriteLine(s.Text);
                    s.OnAcceptCallback(choice);
                }

                break;
            case UserActionResult.ResultType.Save:
                foreach (var s in choice.UserChoices) s.OnSaveCallback(choice);

                break;
            case UserActionResult.ResultType.NoInput:
            default:
                throw new ArgumentOutOfRangeException();
        }

        return Next();
    }

    private static bool Next()
    {
        GuiManager.Instance.GetUserInput(_jpgActionManager, "What do you want to do?");
        return true;
    }

    private static void InitialiseSettingsClass()
    {
        ProgramSettingsClass.Start(_folderName, _fileName, GetPassword());
    }

    private static string GetPassword()
    {
        return "asdf";
    }
}