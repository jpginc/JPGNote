using System;
using System.Collections.Generic;
using Gdk;
using Gtk;
using Key = Gdk.Key;
using Window = Gtk.Window;

namespace ConsoleApp1
{
    internal class MainWindow : Window
    {
        public static MainWindow Instance { get; } = new MainWindow("JPG Tree");
        public Func<bool> UserActionCallback;
        private SearchableTreeView _searchableTreeView;

        private readonly UserActionResult _userActionResult =
            new UserActionResult {Result = UserActionResult.ResultType.NoInput};

        private Label _notificationLabel;
        private TextView _multiLineInputWidget;
        private readonly AccelGroup _buttonKeyboardShortcutAccelGroup = new AccelGroup();

        private MainWindow(string title) : base(title)
        {
            SetSize();
            SetCloseOnExit();
            PopulateGui();
            ShowAll();
        }

        public MainWindow SetChoices(IEnumerable<ITreeViewChoice> choices, string label)
        {
            _searchableTreeView.SetChoices(choices);
            _searchableTreeView.SetLabelText(label);
            _userActionResult.Result = UserActionResult.ResultType.NoInput;
            _userActionResult.UserChoices = null;
            return this;
        }

        private void PopulateGui()
        {
            var leftContainer = new Grid();
            var rightContainer = new Grid();
            var topContainer = new Grid();
            var container = new Grid
            {
                ColumnHomogeneous = false,
                RowHomogeneous = false,
                ColumnSpacing = 10,
                BorderWidth = 15
            };

            AddNotificationElement(topContainer);
            AddLeftElements(leftContainer);
            AddRightElements(rightContainer);
            container.Attach(topContainer, 1, 0, 10, 1);
            container.Attach(leftContainer, 1, 1, 3, 1);
            container.AttachNextTo(rightContainer, leftContainer, PositionType.Right, 7, 1);
            Add(container);
        }

        private void AddNotificationElement(Container container)
        {
            _notificationLabel = new Label("This is a notificationt thing!!");
            container.Add(_notificationLabel);
        }

        private void SetCloseOnExit()
        {
            DeleteEvent += Exit;
        }

        private void SetSize()
        {
            SetDefaultSize(900, 500);
            SetPosition(WindowPosition.Center);
        }

        private void AddRightElements(Grid container)
        {
            var label = new Label("Output <ctrl+o>");
            var sw = new ScrolledWindow
            {
                ShadowType = ShadowType.EtchedIn,
                Expand = true
            };
            //keyboard shortcut setup in the OnKeyPressEvent override
            _multiLineInputWidget = new TextView();
            sw.Add(_multiLineInputWidget);
            var save = new Button("_Save");
            save.Clicked += OnSaveClick;
            save.AddAccelerator("activate", _buttonKeyboardShortcutAccelGroup,
                new AccelKey(Key.a, ModifierType.Mod1Mask, AccelFlags.Visible));
            container.Add(label);
            container.AttachNextTo(sw, label, PositionType.Bottom, 1, 8);
            container.AttachNextTo(save, sw, PositionType.Bottom, 1, 1);
        }

        private void OnSaveClick(object sender, EventArgs e)
        {
            Callback(UserActionResult.ResultType.Save, _searchableTreeView.GetSelectedItems());
        }

        private void AddLeftElements(Grid container)
        {
            _searchableTreeView = new SearchableTreeView();

            AddAccelGroup(_buttonKeyboardShortcutAccelGroup);

            var accept = new Button("_Accept");
            accept.Clicked += OnAcceptClick;
            accept.AddAccelerator("activate", _buttonKeyboardShortcutAccelGroup,
                new AccelKey(Key.a, ModifierType.Mod1Mask, AccelFlags.Visible));
            var back = new Button("_Back");
            back.Clicked += OnBackClick;
            back.AddAccelerator("activate", _buttonKeyboardShortcutAccelGroup,
                new AccelKey(Key.b, ModifierType.Mod1Mask, AccelFlags.Visible));
            var exit = new Button("E_xit");
            exit.Clicked += Exit;
            exit.AddAccelerator("activate", _buttonKeyboardShortcutAccelGroup,
                new AccelKey(Key.x, ModifierType.Mod1Mask, AccelFlags.Visible));

            container.Add(_searchableTreeView);
            container.AttachNextTo(accept, _searchableTreeView, PositionType.Bottom, 1, 1);
            container.AttachNextTo(back, accept, PositionType.Bottom, 1, 1);
            container.AttachNextTo(exit, back, PositionType.Bottom, 1, 1);
        }

        private void Callback(UserActionResult.ResultType t, IEnumerable<ITreeViewChoice> c)
        {
            _userActionResult.Result = t;
            _userActionResult.UserChoices = c;
            _userActionResult.MultiLineInput = _multiLineInputWidget.Buffer.Text;
            _userActionResult.SingleLineInput = _searchableTreeView.GetSearchValue();
            UserActionCallback?.Invoke();
        }

        private static void Exit(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void OnBackClick(object sender, EventArgs e)
        {
            Callback(UserActionResult.ResultType.Canceled, null);
        }

        public MainWindow Accept()
        {
            OnAcceptClick(null, null);
            return this;
        }

        private void OnAcceptClick(object sender, EventArgs e)
        {
            Callback(UserActionResult.ResultType.Accept, _searchableTreeView.GetSelectedItems());
        }

        public MainWindow SetMultiSelect(bool b)
        {
            _searchableTreeView.SetMultiSelect(b);
            return this;
        }

        public UserActionResult GetUserActionResult()
        {
            return _userActionResult;
        }

        public MainWindow Reset(bool doReset)
        {
            if (doReset)
            {
                _searchableTreeView.Reset();
                GuiThread.DontWait(() => { _multiLineInputWidget.Buffer.Text = ""; });
                UserNotify("Double clicking or hitting Return twice will activate an item");
            }

            return this;
        }

        public void UserNotify(string message)
        {
            GuiThread.DontWait(() =>
            {
                _notificationLabel.Text = message;
                var color = new Color();
                Color.Parse("lightblue", ref color);
                _notificationLabel.ModifyBg(StateType.Normal, color);
            });
        }

        public void Error(string message)
        {
            GuiThread.DontWait(() =>
            {
                _notificationLabel.Text = message;
                var color = new Color();
                Color.Parse("red", ref color);
                _notificationLabel.ModifyBg(StateType.Normal, color);
            });
        }

        public MainWindow SetInputText(string noteContents)
        {
            _multiLineInputWidget.Buffer.Text = noteContents;
            return this;
        }

        protected override bool OnKeyPressEvent(EventKey evnt)
        {
            if (evnt.Key == Key.o && evnt.State == ModifierType.ControlMask)
            {
                _multiLineInputWidget.GrabFocus();
                return true;
            }

            if (evnt.Key == Key.i && evnt.State == ModifierType.ControlMask)
            {
                _searchableTreeView.FocusInput();
                return true;
            }

            return base.OnKeyPressEvent(evnt);
        }

        public void Accept(JpgTreeView obj)
        {
            Accept();
        }
    }
}