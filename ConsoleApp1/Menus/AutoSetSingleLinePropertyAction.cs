using System;
using System.Reflection;

namespace ConsoleApp1.BuiltInActions
{
    internal class AutoSetSingleLinePropertyAction : SimpleTreeViewChoice
    {
        private readonly PropertyInfo _property;
        private readonly ICreatable _obj;
        private readonly Func<CancellableObj<string>> _getInputFunction;
        private readonly IManager _manager;

        public AutoSetSingleLinePropertyAction(PropertyInfo property, ICreatable obj,
            Func<CancellableObj<string>> getSingleLineInput, IManager manager) 
            : base("Set " + property.Name)
        {
            _property = property;
            _obj = obj;
            _getInputFunction = getSingleLineInput;
            _manager = manager;
            AcceptHandler = Set;
            SelectHandler = Select;
        }

        private void Select(JpgTreeView obj)
        {
            MainWindow.Instance.SetInputText((string)_property.GetValue(_obj));
        }

        private void Set(UserActionResult obj)
        {
            var newValue = _getInputFunction.Invoke();
            if (newValue.ResponseType == UserActionResult.ResultType.Accept)
            {
                _property.SetValue(_obj, newValue.Result);
                _manager.Save();
            }
        }
    }
}