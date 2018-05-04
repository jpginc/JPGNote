using System;
using ConsoleApp1;
using Gtk;

public class GtkHelloWorld
{

    public static void Main()
    {
        Application.Init();
        new MainWindow("JPG Tree");
        Application.Run();
    }
}