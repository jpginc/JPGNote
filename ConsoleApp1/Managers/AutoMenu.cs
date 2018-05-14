using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1.BuiltInActions
{
    internal class AutoFolderPicker : Attribute { }
    internal class AutoFilePicker : Attribute { }
    public class AutoSingleLineString : Attribute { }
    public class Wizard : Attribute { }
    public class AutoMultiLineString : Attribute { }
    internal class AutoMenu : SimpleActionProvider
    {
        private readonly ICreatable _obj;
        private readonly IManager _manager;

        public AutoMenu(ICreatable obj)
        {
            _obj = obj;
            _manager = obj.Manager;
        }

        public override IEnumerable<ITreeViewChoice> GetActions()
        {
            var retList = new List<ITreeViewChoice>();
            foreach (var prop in _obj.GetType().GetProperties())
            {
                if (prop.PropertyType == typeof(string))
                {
                    Func<CancellableObj<string>> setValueFunction =
                        CreatableWizard.GetInputFunction(prop, "Set " + prop.Name, (string) prop.GetValue(_obj));
                    if (setValueFunction != null)
                    {
                        retList.Add(new AutoSetSingleLinePropertyAction(prop, _obj, setValueFunction, _manager));
                    }
                } else if (prop.PropertyType == typeof(IEnumerable<ICreatable>))
                {
                    //todo is the _manager correct?
                    var val = (IEnumerable<ICreatable>) prop.GetValue(_obj);
                    retList.AddRange(val.Select(v => new AutoAction(v, v.Manager)));
                }
            }

            retList.Add(new AutoDeleteCreatable(_obj, _manager));
            return retList;
        }
    }
}