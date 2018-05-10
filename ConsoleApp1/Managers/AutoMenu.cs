using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1.BuiltInActions
{
    public class AutoSingleLineString : Attribute { }
    public class AutoMultiLineString : Attribute { }
    internal class AutoMenu : SimpleActionProvider
    {
        private readonly object _obj;

        public AutoMenu(object obj)
        {
            _obj = obj;
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
                        new AutoSetSingleLinePropertyAction(prop, _obj, GuiManager.Instance.GetSingleLineInput));
                }
                else if (prop.GetCustomAttributes(typeof(AutoMultiLineString), false).Any())
                {
                    retList.Add(
                        new AutoSetSingleLinePropertyAction(prop, _obj, GuiManager.Instance.GetMultiLineInput));
                }
            }

            return retList;
        }
    }
}