﻿using System;
using System.Collections;
using System.Collections.Generic;
using Gdk;
using Gtk;
using Window = Gtk.Window;

namespace ConsoleApp1
{
    internal class MainWindow : Window
    {
        private readonly Func<bool> _acceptCallback;
        SearchableTreeView _searchableThing;
        private UserActionResult _userActionResult = new UserActionResult {Result = UserActionResult.ResultType.NoInput};
        public MainWindow(string title, Func<bool> acceptCallback) : base(title)
        {
            _acceptCallback = acceptCallback;
            SetSize();
            SetCloseOnExit();
            PopulateGui();
            ShowAll();
        }


        public MainWindow SetChoices(IEnumerable<ITreeViewChoice> choices, string label)
        {
            _searchableThing.SetChoices(choices);
            _searchableThing.SetLabelText(label);
            _userActionResult.Result = UserActionResult.ResultType.NoInput;
            _userActionResult.UserChoices = null;
            return this;
        }

        private void PopulateGui()
        {
            var leftContainer = new Grid();
            var rightContainer = new Grid();
            var container = new Grid
            {
                ColumnHomogeneous = false,
                RowHomogeneous = false,
                ColumnSpacing = 10,
                BorderWidth = 15,
            };

            AddLeftElements(leftContainer);
            AddRightElements(rightContainer);
            container.Attach(leftContainer, 1, 1, 3, 1);
            container.AttachNextTo(rightContainer, leftContainer, PositionType.Right, 7, 1);
            Add(container);
        }

        private void SetCloseOnExit()
        {
            DeleteEvent += Exit;
        }

        private void SetSize()
        {
            SetDefaultSize(Convert.ToInt32(GetSetting("width")), Convert.ToInt32(GetSetting("height")));
            SetPosition(WindowPosition.Center);
        }

        private static string GetSetting(string name)
        {
            //todo get from settings
            if (name.Equals("height"))
            {
                return "500";
            } else if( name.Equals("width"))
            { 
                return "900";
            }
            throw new Exception("don't actually check settings yet");
        }

        private void AddRightElements(Grid container)
        {
            var label = new Label("Output: ");
            var sw = new ScrolledWindow
            {
                ShadowType = ShadowType.EtchedIn,
                Expand = true
            };
            var choices = new TextView();
            sw.Add(choices);
            var save = new Button("Save");
            container.Add(label);
            container.AttachNextTo(sw, label, PositionType.Bottom, 1, 8);
            container.AttachNextTo(save, sw, PositionType.Bottom, 1, 1);
        }

        private void AddLeftElements(Grid container)
        {
            _searchableThing = new SearchableTreeView();

            AccelGroup agr = new AccelGroup();
            AddAccelGroup(agr);

            var accept = new Button("_Accept");
            accept.Clicked += OnAcceptClick;
            accept.AddAccelerator("activate", agr, 
                new AccelKey(Gdk.Key.a, ModifierType.Mod1Mask, AccelFlags.Visible));
            var back = new Button("_Back");
            back.Clicked += OnBackClick;
            back.AddAccelerator("activate", agr, 
                new AccelKey(Gdk.Key.b, ModifierType.Mod1Mask, AccelFlags.Visible));
            var exit = new Button("E_xit");
            exit.Clicked += Exit;
            exit.AddAccelerator("activate", agr, 
                new AccelKey(Gdk.Key.x, ModifierType.Mod1Mask, AccelFlags.Visible));

            container.Add(_searchableThing);
            container.AttachNextTo(accept, _searchableThing, PositionType.Bottom, 1, 1);
            container.AttachNextTo(back, accept, PositionType.Bottom,1,1);  
            container.AttachNextTo(exit, back, PositionType.Bottom,1,1);  
        }

        private void Callback(UserActionResult.ResultType t, IEnumerable<ITreeViewChoice> c)
        {
            _userActionResult.Result = t;
            _userActionResult.UserChoices = c;
            _acceptCallback?.Invoke();
        }
        private void Exit(object sender, EventArgs e)
        {
            Callback(UserActionResult.ResultType.ExitApp, null);
            Application.Quit();
        }

        private void OnBackClick(object sender, EventArgs e)
        {
            Callback(UserActionResult.ResultType.Canceled, null);
        }

        private void OnAcceptClick(object sender, EventArgs e)
        {
            Callback(UserActionResult.ResultType.Accept, _searchableThing.GetSelectedItems());
        }

        public MainWindow SetMultiSelect(bool b)
        {
            _searchableThing.SetMultiSelect(b);
            return this;
        }

        public UserActionResult GetUserActionResult()
        {
            return _userActionResult;
        }
    }
}