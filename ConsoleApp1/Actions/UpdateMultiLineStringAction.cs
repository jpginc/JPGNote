using System;

namespace ConsoleApp1.BuiltInActions
{
    internal class UpdateMultiLineStringAction : UpdateSingleLineStringAction
    {
        public UpdateMultiLineStringAction(Action<string> action, string propertyName, string currentValue) 
            : base(action, propertyName, currentValue)
        {
        }

        protected override void UpdateProperty(UserActionResult obj)
        {
            var newValue = GuiManager.Instance.GetMultiLineInput("Set New Value", _currentValue);
            if (newValue.ResponseType == UserActionResult.ResultType.Accept) _action(newValue.Result);
        }
    }
}