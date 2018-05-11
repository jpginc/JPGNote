using System;

namespace ConsoleApp1.BuiltInActions
{
    internal class ChoiceToCallback : SimpleTreeViewChoice
    {
        private readonly Func<bool> _callback;

        public ChoiceToCallback(Func<bool> callback, string text) : base(text)
        {
            _callback = callback;
            AcceptHandler = CallCallback;
        }

        private void CallCallback(UserActionResult obj)
        {
            _callback.Invoke();
        }
    }
}