using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1.BuiltInActions
{
    internal class ManageableCreatable : IActionProvider
    {
        private readonly IManagerAndActionProvider _manager;

        public ManageableCreatable(IManagerAndActionProvider manager)
        {
            _manager = manager;
        }

        public InputType InputType => InputType.Single;

        public IEnumerable<ITreeViewChoice> GetActions()
        {
            return GetManageActions();
        }
        public IEnumerable<ITreeViewChoice> GetActionsWithChildren()
        {
            var existingCreatables = _manager.GetActions();
            return GetManageActions().Concat(existingCreatables);
        }

        private List<ITreeViewChoice> GetManageActions()
        {
            return new List<ITreeViewChoice>
            {
                new ChoiceToActionProvider(_manager, _manager.ManageText),
                new AutoCreateCreatable(_manager),
                new ChoiceToActionProvider(new AutoDeleteMenu(_manager), _manager.DeleteChoiceText)
            };
        }

        public ActionProviderResult HandleUserAction(UserActionResult res)
        {
            return ActionProviderResult.PassToTreeViewChoices;
        }
    }

    internal class ChoiceToActionProvider : SimpleTreeViewChoice
    {
        private readonly IActionProvider _autoDeleteMenu;

        public ChoiceToActionProvider(IActionProvider autoDeleteMenu, string text) : base(text)
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
            if (choice.Result == UserActionResult.ResultType.Accept && choice.UserChoices.Count() != 0)
                if (UserNotifier.Confirm(
                    $"Are you sure you want to delete these {choice.UserChoices.Count()} items?"))
                    foreach (var item in choice.UserChoices)
                        _manager.Delete(((AutoAction) item).Creatable);
            JpgActionManager.UnrollActionContext();
            return ActionProviderResult.ProcessingFinished;
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
        private IManager _manager;

        public AutoAction(ICreatable creatable, IManager manager) : base(creatable.EditChoiceText)
        {
            Creatable = creatable;
            _manager = manager;
            AcceptHandler = SetContext;
            SelectHandler = PreviewValues;
            DoneHandler = MarkScanItemAsDone;
            
        }

        private void MarkScanItemAsDone(JpgTreeView obj)
        {
            if (Creatable is IDoneable doneable)
            {
                Console.WriteLine("Marking something done");
                doneable.ScanItemStatus = ScanItemState.Done;
            }
        }

        private void PreviewValues(JpgTreeView obj)
        {
            var val = "";
            val += GetPreviewFromObject(Creatable);
            foreach (var subThing in _manager.GetChildren(Creatable))
            {
                val += GetPreviewFromObject(subThing);
            }
            MainWindow.Instance.SetInputText(val);
        }

        private string GetPreviewFromObject(ICreatable obj)
        {
            var val = "";
            var propertyInfos = obj.GetType().GetProperties();
            foreach (var prop in propertyInfos)
            {
                if (prop.PropertyType == typeof(string))
                {
                    val += prop.Name + ": " + (prop.GetValue(obj) ?? "") + "\n";
                }

                if (prop.PropertyType == typeof(List<ICreatable>))
                {
                    foreach (var subThing in (List<ICreatable>) prop.GetValue(obj))
                    {
                        val += GetPreviewFromObject(subThing);
                    }
                }
            }
            return val;
        }

        private void SetContext(UserActionResult obj)
        {
            JpgActionManager.PushActionContext(new AutoMenu(Creatable, _manager));
        }
    }
}