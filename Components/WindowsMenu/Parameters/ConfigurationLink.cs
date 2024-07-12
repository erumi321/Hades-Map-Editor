using Hades_Map_Editor.Managers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hades_Map_Editor.Components.Dialogs
{
    class ConfigurationLink : Panel, IComponent
    {
        string defaultLabelText;
        Label pathLabel;
        TextBox pathTextbox;
        public Button saveButton, browseButton;
        public ConfigurationLink(string labelText, Button saveButton)
        {
            this.saveButton = saveButton;
            defaultLabelText = labelText;
            Initialize();
            Populate();
        }
        public void Initialize()
        {
            //Dock = DockStyle.Fill;
            Dock = DockStyle.Top;
            
            pathLabel = new Label();
            pathLabel.Text = defaultLabelText;
            pathLabel.BackColor = Color.LightGray;
            pathLabel.Dock = DockStyle.Top;

            Panel panel = new Panel();
            panel.Dock = DockStyle.Top;

            pathTextbox = new TextBox();
            pathTextbox.Text = "";
            pathTextbox.Dock = DockStyle.Fill;

            browseButton = new Button();
            browseButton.Image = Image.FromFile(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + @"\Res\browse_button.png");
            browseButton.Dock = DockStyle.Right;

            panel.Controls.Add(pathTextbox);
            panel.Controls.Add(browseButton);
            panel.Controls.Add(saveButton);

            Controls.Add(panel);
            Controls.Add(pathLabel);
        }

        public void Populate()
        {
            pathTextbox.TextChanged += (s, e) => Change_TextBox(s, e);
            // Nothing
        }
        public void SetText(string path)
        {
            pathTextbox.Text = path;
        }
        public string GetText()
        {
            return pathTextbox.Text;
        }
        private void Change_TextBox(object sender, EventArgs e)
        {
            saveButton.Enabled = true;
            TextBox textbox = (TextBox)sender;
            Console.WriteLine(textbox.Text);
        }
    }
}
