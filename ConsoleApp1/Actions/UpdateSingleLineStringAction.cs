using System;

namespace ConsoleApp1.BuiltInActions
{
    internal class UpdateSingleLineStringAction : SimpleTreeViewChoice
    {
        protected readonly Action<string> _action;
        protected readonly string _currentValue;

        public UpdateSingleLineStringAction(Action<string> action, string propertyName, string currentValue) : base(
            "Set " + propertyName)
        {
            _action = action;
            _currentValue = currentValue ?? "";
            AcceptHandler = UpdateProperty;
        }

        protected virtual void UpdateProperty(UserActionResult obj)
        {
            var newValue = GuiManager.Instance.GetSingleLineInput("Set New Value", _currentValue);
            if (newValue.ResponseType == UserActionResult.ResultType.Accept) _action(newValue.Result);
        }
    }
}