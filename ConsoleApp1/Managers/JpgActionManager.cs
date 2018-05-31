using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleApp1.BuiltInActions;
using GLib;

namespace ConsoleApp1
{
    public class JpgActionManager : SimpleActionProvider
    {
        public IActionProvider CurrentActionProvider => _actionProviders.Last().ActionProvider;
        public static string RestoreText { get; set; } = "";

        private static readonly List<ActionContext> _actionProviders 
            = new List<ActionContext>() {new ActionContext(new ProgramMenu(), "")}; 

        public override IEnumerable<ITreeViewChoice> GetActions()
        {
            return CurrentActionProvider.GetActions();
        }

        public static void PushActionContext(IActionProvider action)
        {
            _actionProviders.Add(new ActionContext(action, GuiManager.Instance.GetSearchText()));
            RestoreText = "";
        }

        public static void UnrollActionContext()
        {
            if (_actionProviders.Count > 1)
            {
                RestoreText = _actionProviders.Last().SearchText;
                _actionProviders.Remove(_actionProviders.Last());
            }
        }
    }

    public class ActionContext
    {

        public ActionContext(IActionProvider actionProvider, string searchText)
        {
            ActionProvider = actionProvider;
            SearchText = searchText;
        }

        public IActionProvider ActionProvider { get; set; }
        public string SearchText { get; set; }
    }
}