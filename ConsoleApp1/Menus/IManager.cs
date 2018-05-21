﻿using System;
using System.Collections.Generic;

namespace ConsoleApp1.BuiltInActions
{
    public interface IManager 
    {
        string ManageText { get; }
        string CreateChoiceText { get; }
        string DeleteChoiceText { get; }
        List<ICreatable> Creatables { get; set; }
        void Save();
        void Delete(ICreatable creatable);
        void New(UserActionResult obj);
        bool HasChildren();
        IEnumerable<ICreatable> GetChildren(ICreatable parent);
    }

    public interface ICreatable : IWizardable
    {
        string EditChoiceText { get; }
    }
}