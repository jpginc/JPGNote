using System.Collections.Generic;
using ConsoleApp1;
using Gtk;

public class GtkHelloWorld
{
    public static void Main()
    {
        var manager = new GuiManager();
        manager.GetChoice(GetChoices());
        manager.GetChoices(GetChoices());
    }

    private static IEnumerable<ITreeViewChoice> GetChoices()
    {
        var treeViewChoice = new TreeViewChoice("Settings") { Selected = true };
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