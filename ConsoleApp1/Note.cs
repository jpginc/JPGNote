﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ConsoleApp1
{
    [DataContract]
    internal class Note : IComparable<Note>, IActionProvider, ITreeViewChoice
    {
        [DataMember] public readonly string NoteName;
        [DataMember] private readonly string _uniqueId;
        [DataMember] public string NoteContents { get; set; }
        [DataMember] public DateTime CreateTime { get; set; }
        [DataMember] public List<string> Tags { get; set; } = new List<string>();

        public Note(string noteName)
        {
            NoteName = noteName;
            _uniqueId = Guid.NewGuid().ToString("N");
            NoteContents = "";
            CreateTime = DateTime.Now;
        }

        public int CompareTo(Note obj)
        {
            return string.Compare(_uniqueId, obj._uniqueId, StringComparison.Ordinal);
        }

        public IEnumerable<ITreeViewChoice> GetActions()
        {
            return new[]
            {
                new SimpleTreeViewChoice("Delete note") {AcceptHandler = Delete},
                new SimpleTreeViewChoice("Add Tags") {AcceptHandler = AddTag}
            };
        }

        private void AddTag(UserActionResult obj)
        {
            var input = GuiManager.Instance.GetNonEmptySingleLineInput("Enter tag");
            if (input.Result == UserActionResult.ResultType.Accept)
            {
                Tags.Add(input.SingleLineInput);
            }
        }

        private void Delete(UserActionResult obj)
        {
            NotesManager.Instance.Delete(this);
            if (JpgActionManager.CurrentActionProvider == this)
            {
                JpgActionManager.CurrentActionProvider = null;
            }
        }

        public string SortByText => NoteName;
        public string Text => NoteName;
        public bool OnTreeViewSelectCallback(JpgTreeView jpgTreeView)
        {
            MainWindow.Instance.SetInputText(NoteContents);
            return true;
        }

        public bool OnAcceptCallback(UserActionResult choice)
        {
            JpgActionManager.CurrentActionProvider = this;
            MainWindow.Instance.SetInputText(NoteContents);
            return true;
        }

        public bool OnSaveCallback(UserActionResult choice)
        {
            Console.WriteLine("the save data is " + choice.MultiLineInput);
            NoteContents = choice.MultiLineInput;
            NotesManager.Instance.Save();
            return true;
        }
    }
}