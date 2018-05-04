using System;
using Gtk;

namespace ConsoleApp1
{
    internal class MainWindow : Window
    {
        public MainWindow(string title) : base(title)
        {
            SetSize();
            SetCloseOnExit();
            PopulateGui();
            ShowAll();
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
                BorderWidth = 15
            };

            AddLeftElements(leftContainer);
            AddRightElements(rightContainer);
            container.Attach(leftContainer, 1, 1, 3, 1);
            container.AttachNextTo(rightContainer, leftContainer, PositionType.Right, 7, 1);
            Add(container);
        }

        private void SetCloseOnExit()
        {
            DeleteEvent += delegate { Application.Quit(); };
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
            container.Attach(label, 2, 1, 1, 1);
            container.Attach(sw, 2, 2, 1, 8);
            container.Attach(save, 2, 10, 1, 1);
        }

        private void AddLeftElements(Grid container)
        {
            var label = new Label("Search: ");
            var search = new Entry() {Expand = false};
            var sw = new ScrolledWindow
            {
                ShadowType = ShadowType.EtchedIn,
                Expand = true
            };
            var choices = new TextView();
            sw.Add(choices);

            var accept = new Button("Accept");
            var back = new Button("Back");
            var exit = new Button("Exit");

            container.Attach(label, 1, 1, 1, 1);
            container.AttachNextTo(search, label, PositionType.Bottom, 1, 1);
            container.AttachNextTo(sw, search, PositionType.Bottom, 1, 5);
            container.AttachNextTo(accept, sw, PositionType.Bottom, 1, 1);
            container.AttachNextTo(back, accept, PositionType.Bottom,1,1);  
            container.AttachNextTo(exit, back, PositionType.Bottom,1,1);  
        }

        private void OnChanged(object sender, EventArgs e)
        {
            var search = (Entry) sender;
            Console.WriteLine(search.Text);
        }
    }
}