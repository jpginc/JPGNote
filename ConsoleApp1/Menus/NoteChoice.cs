using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1.BuiltInActions
{
    internal class NoteChoice : SimpleTreeViewChoice
    {
        public readonly INote Note;

        public NoteChoice(INote note) : base(note.Name)
        {
            Note = note;
            AcceptHandler = (a) =>
            {
                JpgActionManager.PushActionContext(note);
            };
            SelectHandler = PreviewValues;

        }
        private void PreviewValues(JpgTreeView obj)
        {
            var val = "";
            val += GetPreviewFromNote(Note);
            foreach (var subThing in NewNotesManager.GetChildren(Note))
            {
                val += GetPreviewFromNote(subThing);
            }
            MainWindow.Instance.SetInputText(val);
        }

        private string GetPreviewFromNote(INote obj)
        {
            var val = "";
            val += $"{obj.GetType().Name}: {obj.Name}\n";
            val += GetContents(obj);
            val += GetTags(obj);
            return val;
        }

        private static string GetContents(INote obj)
        {
            if (obj.HasContents && obj.Contents.Length > 1)
            {
                return $"{obj.Contents}\n";
            }

            return "";
        }

        private static string GetTags(INote obj)
        {
            if (obj.GetType() != typeof(Tag))
            {
                var content = string.Join(", ", obj.TagsUniqueIds
                    .Select(NewNotesManager.GetNote)
                    .Where(note => note != null)
                    .Select(notNullNote => notNullNote.Name)
                    .ToArray());
                if (content.Length > 1)
                {
                    return content + "\n";
                }
            }

            return "";
        }
    }
}