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
        private SearchableTreeView _searchableThing;

        private readonly UserActionResult _userActionResult =
            new UserActionResult {Result = UserActionResult.ResultType.NoInput};

        private Label _messageDialog;
        private TextView _inputWidget;
        private readonly AccelGroup _accelGroup = new AccelGroup();

        private MainWindow(string title) : base(title)
        {
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

        private void AddNotificationElement(Grid container)
        {
            _messageDialog = new Label("This is a notificationt thing!!");

            container.Add(_messageDialog);
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
                return "500";
            if (name.Equals("width")) return "900";
            throw new Exception("don't actually check settings yet");
        }

        private void AddRightElements(Grid container)
        {
            var label = new Label("Output <ctrl + o> ");
            var sw = new ScrolledWindow
            {
                ShadowType = ShadowType.EtchedIn,
                Expand = true
            };
            //keyboard shortcut setup in the OnKeyPressEvent override
            _inputWidget = new TextView();
            sw.Add(_inputWidget);
            var save = new Button("_Save");
            save.Clicked += OnSaveClick;
            save.AddAccelerator("activate", _accelGroup,
                new AccelKey(Key.a, ModifierType.Mod1Mask, AccelFlags.Visible));
            container.Add(label);
            container.AttachNextTo(sw, label, PositionType.Bottom, 1, 8);
            container.AttachNextTo(save, sw, PositionType.Bottom, 1, 1);
        }

        private void OnSaveClick(object sender, EventArgs e)
        {
            Callback(UserActionResult.ResultType.Save, _searchableThing.GetSelectedItems());
        }

        private void AddLeftElements(Grid container)
        {
            _searchableThing = new SearchableTreeView();

            AddAccelGroup(_accelGroup);

            var accept = new Button("_Accept");
            accept.Clicked += OnAcceptClick;
            accept.AddAccelerator("activate", _accelGroup,
                new AccelKey(Key.a, ModifierType.Mod1Mask, AccelFlags.Visible));
            var back = new Button("_Back");
            back.Clicked += OnBackClick;
            back.AddAccelerator("activate", _accelGroup,
                new AccelKey(Key.b, ModifierType.Mod1Mask, AccelFlags.Visible));
            var exit = new Button("E_xit");
            exit.Clicked += Exit;
            exit.AddAccelerator("activate", _accelGroup,
                new AccelKey(Key.x, ModifierType.Mod1Mask, AccelFlags.Visible));

            container.Add(_searchableThing);
            container.AttachNextTo(accept, _searchableThing, PositionType.Bottom, 1, 1);
            container.AttachNextTo(back, accept, PositionType.Bottom, 1, 1);
            container.AttachNextTo(exit, back, PositionType.Bottom, 1, 1);
        }

        private void Callback(UserActionResult.ResultType t, IEnumerable<ITreeViewChoice> c)
        {
            _userActionResult.Result = t;
            _userActionResult.UserChoices = c;
            _userActionResult.MultiLineInput = _inputWidget.Buffer.Text;
            _userActionResult.SingleLineInput = _searchableThing.GetSearchValue();
            UserActionCallback?.Invoke();
        }

        private void Exit(object sender, EventArgs e)
        {
            //Application.Quit();
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

        public MainWindow Reset(bool doReset)
        {
            if (doReset)
            {
                _searchableThing.Reset();
                _inputWidget.Buffer.Text = "";
                UserNotify("Double clicking or hitting Return twice will activate an item");
            }

            return this;
        }

        public void UserNotify(string message)
        {
            _messageDialog.Text = message;
            var color = new Color();
            Color.Parse("lightblue", ref color);
            _messageDialog.ModifyBg(StateType.Normal, color);
        }

        public void Error(string message)
        {
            _messageDialog.Text = message;

            var color = new Color();
            Color.Parse("red", ref color);
            _messageDialog.ModifyBg(StateType.Normal, color);
        }

        public MainWindow SetInputText(string noteContents)
        {
            _inputWidget.Buffer.Text = noteContents;
            return this;
        }
        protected override bool OnKeyPressEvent(EventKey evnt)
        {
            if (evnt.Key == Key.o && evnt.State == ModifierType.ControlMask)
            {
                _inputWidget.GrabFocus();
                return true;
            } else if (evnt.Key == Key.i && evnt.State == ModifierType.ControlMask)
            {
                _searchableThing.FocusInput();
                return true;
            }
            return base.OnKeyPressEvent(evnt);
        }
    }
}