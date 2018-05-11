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
            //return _manager.GetCreatables().Select(c => new AutoAction(c, _manager));

            IEnumerable<ITreeViewChoice> existingCreatables = _manager.Creatables.Select(c => new AutoAction(c, _manager));
            ITreeViewChoice createCreatable = new AutoCreate(_manager);
            //todo delete
            return existingCreatables;
        }

        public ActionProviderResult HandleUserAction(UserActionResult res)
        {
            return ActionProviderResult.PassToTreeViewChoices;
        }
    }

    internal class AutoCreate : SimpleTreeViewChoice
    {
        public AutoCreate(IManager manager) : base(manager.CreateChoiceText)
        {
            AcceptHandler = manager.New;
        }
    }

    internal class AutoAction : SimpleTreeViewChoice
    {
        private readonly ICreatable _creatable;
        private readonly IManager _manager;

        public AutoAction(ICreatable creatable, IManager manager) : base(creatable.EditChoiceText)
        {
            _creatable = creatable;
            _manager = manager;
            AcceptHandler = SetContext;
        }

        private void SetContext(UserActionResult obj)
        {
            JpgActionManager.PushActionContext(new AutoMenu(_creatable, _manager));
        }
    }
}