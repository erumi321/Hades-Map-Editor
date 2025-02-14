using Hades_Map_Editor.Managers;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hades_Map_Editor.Sections
{
    internal class GroupCheckbox : FlowLayoutPanel, IComponent
    {
        string title;
        Label nameLabel;
        CheckBox checkBox;
        bool canEdit;
        private Action<bool, bool> _onChange;
        public GroupCheckbox(string label, bool edit = false, Action<bool, bool> onChange = null)
        {
            title = label;
            canEdit = edit;
            Initialize();
            Populate();

            _onChange = onChange;

            //properties = new ThingTextProperties(this, panel);
        }
        public void Initialize()
        {
            BackColor = System.Drawing.Color.Gray;
            AutoScroll = false;
            Dock = DockStyle.Fill;
            Size = new System.Drawing.Size(1, 24);
            Margin = new Padding(0, 2, 2, 0);

            FlowDirection = FlowDirection.LeftToRight;

            nameLabel = new Label();
            nameLabel.Text = title;
            nameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            nameLabel.BackColor = System.Drawing.Color.LightGreen;
            nameLabel.Padding = new Padding(10, 0, 0, 0);
            nameLabel.Margin = new Padding(0);
            //nameLabel.Size = new System.Drawing.Size(100,20);
            nameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            nameLabel.Size = new System.Drawing.Size(200, nameLabel.Size.Height);
            Controls.Add(nameLabel);

            checkBox = new CheckBox();
            checkBox.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            checkBox.Enabled = canEdit;
            checkBox.Checked = true;
            checkBox.Click += UpdateCheckbox;
            checkBox.Padding = new Padding(10, 0, 0, 0);
            //checkBox.Dock = DockStyle.Left;
            Controls.Add(checkBox);
        }
        public void Populate()
        {
        }
        public void UpdateCheckbox(object sender, EventArgs e)
        {
            Update(checkBox.Checked, true);
        }
        public void Update(bool value, bool useOnChange)
        {
            if (value)
            {
                nameLabel.BackColor = System.Drawing.Color.LightGreen;
            }
            else
            {
                nameLabel.BackColor = System.Drawing.Color.Orange;
            }
            checkBox.Checked = value;

            if (_onChange != null)
            {
                _onChange(value, useOnChange);
            }
        }
    }
}
