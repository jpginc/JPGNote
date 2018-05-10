using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ConsoleApp1.BuiltInActions
{
    internal class AutoSetPropertyProvider
    {
        public static IEnumerable<ITreeViewChoice> GetActions(object obj)
        {
            var retList = new List<ITreeViewChoice>();
            foreach (var prop in obj.GetType().GetProperties())
            {
                var value = prop.GetValue(obj);
                var type = prop.PropertyType;
                if (type == typeof(AutoSingleLineString))
                {
                    retList.Add(
                        new AutoSetSingleLinePropertyAction((IGetAndSetString)value, prop.Name));
                } else if (type == typeof(AutoMultiLineString))
                {
                    retList.Add(
                        new AutoSetMultiLinePropertyAction((IGetAndSetString)value, prop.Name));
                }
            }

            return retList;
        }
    }

    internal class AutoSetMultiLinePropertyAction : SimpleTreeViewChoice
    {
        private IGetAndSetString _obj;

        public AutoSetMultiLinePropertyAction(IGetAndSetString obj, string propName)
            : base("Set " + propName) 
        {
            _obj = obj;
            AcceptHandler = Set;
        }

        private void Set(UserActionResult obj)
        {
            var newValue = GuiManager.Instance.GetMultiLineInput("Set New Value", _obj.Get());
            if (newValue.ResponseType == UserActionResult.ResultType.Accept)
            {
                _obj.Set(newValue.Result);
            }
        }
 
    }

    [DataContract]
    public class AutoMultiLineString : AutoSingleLineString 
    {
        public AutoMultiLineString(string s) : base(s)
        {
        }
    }

    internal class AutoSetSingleLinePropertyAction : SimpleTreeViewChoice
    {
        private readonly IGetAndSetString _obj;

        public AutoSetSingleLinePropertyAction(IGetAndSetString obj, string propertyName) 
            : base("Set " + propertyName) 
        {
            _obj = obj;
            AcceptHandler = Set;
        }

        private void Set(UserActionResult obj)
        {
            var newValue = GuiManager.Instance.GetSingleLineInput("Set New Value", _obj.Get() ?? "");
            if (newValue.ResponseType == UserActionResult.ResultType.Accept)
            {
                _obj.Set(newValue.Result);
            }
        }
    }

    [DataContract]
    public class AutoSingleLineString : IGetAndSetString
    {
        [DataMember] public string S { get; set; }

        public AutoSingleLineString(string s)
        {
            S = s;
        }

        public string Get()
        {
            return S;
        }

        public void Set(string s)
        {
            S = s;
        }
    }

    internal interface IGetAndSetString
    {
        string Get();
        void Set(string s);

    }
}