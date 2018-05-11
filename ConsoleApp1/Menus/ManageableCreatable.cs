using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1.BuiltInActions
{
    internal class ManageableCreatable : IActionProvider
    {
        private readonly IManager _manager;

        public ManageableCreatable(IManager manager)
        {
            _manager = manager;
        }

        public string SortByText => _manager.ManageText;
        public string Text => _manager.ManageText;
        public bool OnTreeViewSelectCallback(JpgTreeView jpgTreeView)
        {
            return true;
        }

        public bool OnAcceptCallback(UserActionResult choice)
        {
            JpgActionManager.PushActionContext(this);
            return true;
        }

        public bool OnSaveCallback(UserActionResult choice)
        {
            return true;
        }

        public InputType InputType => InputType.Single;
        public IEnumerable<ITreeViewChoice> GetActions()
        {
            //not quite right, i need to have a choice that will then set the context to an auto menu
            var a = new List<ITreeViewChoice>
            {
                new AutoManage(_manager),
                new AutoCreateCreatable(_manager),
                new ChoiceToActionProvider(new AutoDeleteMenu(_manager), _manager.DeleteChoiceText)
            };

            IEnumerable<ITreeViewChoice> existingCreatables = _manager.Creatables.Select(c => new AutoAction(c, _manager));
            return existingCreatables.Concat(a);
        }

        public ActionProviderResult HandleUserAction(UserActionResult res)
        {
            return ActionProviderResult.PassToTreeViewChoices;
        }
    }

    internal class ChoiceToActionProvider : SimpleTreeViewChoice
    {
        private readonly AutoDeleteMenu _autoDeleteMenu;

        public ChoiceToActionProvider(AutoDeleteMenu autoDeleteMenu, string text) : base(text)
        {
            _autoDeleteMenu = autoDeleteMenu;
            AcceptHandler = SetContext;
        }

        private void SetContext(UserActionResult obj)
        {
            JpgActionManager.PushActionContext(_autoDeleteMenu);
        }
    }

    internal class AutoDeleteMenu : IActionProvider
    {
        private readonly IManager _manager;

        public AutoDeleteMenu(IManager manager)
        {
            _manager = manager;
        }

        public InputType InputType => InputType.Multi;
        public IEnumerable<ITreeViewChoice> GetActions()
        {
            return _manager.Creatables.Select(c => new AutoAction(c, _manager));
        }

        public ActionProviderResult HandleUserAction(UserActionResult choice)
        {
            //todo can't be both 
            if (choice.Result == UserActionResult.ResultType.Accept && choice.UserChoices.Count() != 0)
            {
                if (UserNotifier.Confirm(
                    $"Are you sure you want to delete these {choice.UserChoices.Count()} items?"))
                {
                    foreach (ITreeViewChoice item in choice.UserChoices)
                    {
                        _manager.Delete(((AutoAction)item).Creatable);
                    }
                }
            }
            JpgActionManager.UnrollActionContext();
            return ActionProviderResult.ProcessingFinished;
        }
    }

    internal class AutoManage : SimpleTreeViewChoice
    {
        private readonly IManager _manager;

        public AutoManage(IManager manager) : base(manager.ManageText)
        {
            _manager = manager;
            AcceptHandler = SetContext;
        }

        private void SetContext(UserActionResult obj)
        {
            UserNotifier.Error("Not implemented yet!");
            //JpgActionManager.PushActionContext(_manager);
        }
    }

    internal class AutoDeleteCreatable : SimpleTreeViewChoice
    {
        private readonly ICreatable _creatable;
        private readonly IManager _manager;

        public AutoDeleteCreatable(ICreatable creatable, IManager manager) 
            : base("Delete " + creatable.EditChoiceText)
        {
            _creatable = creatable;
            _manager = manager;
            AcceptHandler = ChoseItemsToDelete;
        }

        private void ChoseItemsToDelete(UserActionResult obj)
        {
            _manager.Delete(_creatable);
            JpgActionManager.UnrollActionContext();
        }
    }

    internal class AutoCreateCreatable : SimpleTreeViewChoice
    {
        public AutoCreateCreatable(IManager manager) : base(manager.CreateChoiceText)
        {
            AcceptHandler = manager.New;
        }
    }

    internal class AutoAction : SimpleTreeViewChoice
    {
        public readonly ICreatable Creatable;
        private readonly IManager _manager;

        public AutoAction(ICreatable creatable, IManager manager) : base(creatable.EditChoiceText)
        {
            Creatable = creatable;
            _manager = manager;
            AcceptHandler = SetContext;
        }

        private void SetContext(UserActionResult obj)
        {
            JpgActionManager.PushActionContext(new AutoMenu(Creatable, _manager));
        }
    }
}