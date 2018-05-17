﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ConsoleApp1.BuiltInActions
{
    [DataContract]
    [KnownType(typeof(UserAction))]
    internal class UserActionManager : Manager
    {
        [IgnoreDataMember] public static UserActionManager Instance { get; set; }
        [IgnoreDataMember] public override string ManageText => "Manage Actions";
        [IgnoreDataMember] public override string CreateChoiceText => "Create New Action";
        [IgnoreDataMember] public override string DeleteChoiceText => "Delete Actions";

        public void ManageUserActions()
        {
            JpgActionManager.PushActionContext(new UserActionManagerMenu());
        }

        public IEnumerable<ITreeViewChoice> GetUserActionChoices()
        {
            return Creatables.Select(u => new AutoAction(u, this));
        }

        public void CreateNewUserAction(UserActionResult obj)
        {
            var userAction = new UserAction();
            if (CreatableWizard.GetRequiredFields(userAction))
            {
                Creatables.Add(userAction);
                Save();
                JpgActionManager.PushActionContext(new AutoMenu(userAction, this));
            }
        }

        public override void New(UserActionResult obj)
        {
            CreateNewUserAction(obj);
        }

        public UserAction GetAction(string name)
        {
            foreach (var a in Creatables)
            {
                //Console.WriteLine("Here " + a.EditChoiceText + " and " + name);
                if(((UserAction) a).Name.Equals(name))
                    return (UserAction)a;
            }
            return null;
        }

        public IEnumerable<ITreeViewChoice> GetPortCommands()
        {
            return Creatables.Where(c => ((UserAction) c).Command.Contains("{{PORT}}"))
                .Select(c => new AutoAction(c, this));
        }
    }
}