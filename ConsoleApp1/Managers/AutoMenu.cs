using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1.BuiltInActions
{
    public class AutoSingleLineString : Attribute { }
    public class Wizard : Attribute { }
    public class AutoMultiLineString : Attribute { }
    internal class AutoMenu : SimpleActionProvider
    {
        private readonly ICreatable _obj;
        private readonly IManager _manager;

        public AutoMenu(ICreatable obj, IManager manager)
        {
            _obj = obj;
            _manager = manager;
        }

        public override IEnumerable<ITreeViewChoice> GetActions()
        {
            var retList = new List<ITreeViewChoice>();
            foreach (var prop in _obj.GetType().GetProperties())
            {
                if (prop.PropertyType != typeof(string))
                {
                    continue;
                }

                if (prop.GetCustomAttributes(typeof(AutoSingleLineString), false).Any())
                {
                    retList.Add(
                        new AutoSetSingleLinePropertyAction(prop, _obj, GuiManager.Instance.GetSingleLineInput, _manager));
                }
                else if (prop.GetCustomAttributes(typeof(AutoMultiLineString), false).Any())
                {
                    retList.Add(
                        new AutoSetSingleLinePropertyAction(prop, _obj, GuiManager.Instance.GetMultiLineInput, _manager));
                }
            }

            retList.Add(new AutoDeleteCreatable(_obj, _manager));
            return retList;
        }
    }
}