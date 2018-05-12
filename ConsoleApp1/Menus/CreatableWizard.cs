using System;
using System.Linq;
using System.Reflection;

namespace ConsoleApp1.BuiltInActions
{
    internal static class CreatableWizard
    {
        public static bool GetRequiredFields(ICreatable obj)
        {
            foreach (var prop in obj.GetType().GetProperties())
            {
                if (prop.GetCustomAttributes(typeof(Wizard), false).Any())
                {
                    var prompt = "Enter value for " + prop.Name;
                    var inputFunc = GetInputFunction(prop, prompt, (string) prop.GetValue(obj));

                    CancellableObj<string> result = inputFunc.Invoke();
                    if (result.ResponseType == UserActionResult.ResultType.Canceled)
                    {
                        return false;
                    }

                    prop.SetValue(obj, result.Result);
                }
            }
            return true;
        }

        public static Func<CancellableObj<string>> GetInputFunction(PropertyInfo prop, string prompt, 
            string currentValue)
        {
            Func<CancellableObj<string>> inputFunc = null;
            currentValue = currentValue ?? "";
            if (prop.GetCustomAttributes(typeof(AutoSingleLineString), false).Any())
            {
                inputFunc = () => GuiManager.Instance.GetSingleLineInput(prompt, currentValue);
            }
            else if (prop.GetCustomAttributes(typeof(AutoMultiLineString), false).Any())
            {
                inputFunc = () => GuiManager.Instance.GetMultiLineInput(prompt, currentValue);
            }
            else if (prop.GetCustomAttributes(typeof(AutoFolderPicker), false).Any())
            {
                inputFunc = () => GuiManager.Instance.GetFolder(prompt);
            }
            else if (prop.GetCustomAttributes(typeof(AutoFilePicker), false).Any())
            {
                inputFunc = () => GuiManager.Instance.GetFile(prompt);
            }
            return inputFunc;
        }
    }
}