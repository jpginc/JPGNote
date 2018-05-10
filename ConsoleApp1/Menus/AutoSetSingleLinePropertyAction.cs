using System;
using System.Reflection;

namespace ConsoleApp1.BuiltInActions
{
    internal class AutoSetSingleLinePropertyAction : SimpleTreeViewChoice
    {
        private readonly PropertyInfo _property;
        private readonly object _obj;
        private Func<string, string, CancellableObj<string>> _getInputFunction;

        public AutoSetSingleLinePropertyAction(PropertyInfo property, object obj, 
            Func<string, string, CancellableObj<string>> getSingleLineInput) 
            : base("Set " + property.Name)
        {
            _property = property;
            _obj = obj;
            _getInputFunction = getSingleLineInput;
            AcceptHandler = Set;
            SelectHandler = Select;
        }

        private void Select(JpgTreeView obj)
        {
            MainWindow.Instance.SetInputText((string)_property.GetValue(_obj));
        }

        private void Set(UserActionResult obj)
        {
            var newValue = _getInputFunction("Set New Value", 
                ((string)_property.GetValue(_obj)) ?? "");
            if (newValue.ResponseType == UserActionResult.ResultType.Accept)
            {
                _property.SetValue(_obj, newValue.Result);
            }
        }
    }
}