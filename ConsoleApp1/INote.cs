﻿using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    public interface INote : IComparable
    {
        DateTime CreateTime { get; set; }
        string NoteContents { get; set; }
        string NoteName { get; set; }
        List<string> Tags { get; set; }
    }
}