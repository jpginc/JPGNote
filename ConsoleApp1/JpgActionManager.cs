using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleApp1.BuiltInActions;
using GLib;

namespace ConsoleApp1
{
    internal class JpgActionManager
    {
        public static IActionProvider CurrentActionProvider { get; set; }
        public static IEnumerable<ITreeViewChoice> GetActions()
        {
            if (CurrentActionProvider == null)
            {
                return BuiltInActionProvider.Instance.GetActions();
            }
            else
            {
                Console.WriteLine("getting the actions choices");
                return CurrentActionProvider.GetActions();
            }
        }
    }
}