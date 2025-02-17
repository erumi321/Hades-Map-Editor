using Hades_Map_Editor.AssetsSection;
using Hades_Map_Editor.Data;
using Hades_Map_Editor.Managers;
using Hades_Map_Editor.PropertiesSection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hades_Map_Editor.Sections
{
    public class GroupsPanel : Panel, IComponent
    {
        private Label groupLabel;
        private Button selectAllButton, unselectAllButton;
        private ProjectData data;
        private TableLayoutPanel topControlPanel;
        private TableLayoutPanel groupNamePanel;
        private MapPanel mapPanel;
        private ElementsPanel elementsPanel;
        List<GroupCheckbox> groupBoxes;

        public GroupsPanel(ProjectData data, MapPanel mapPanel, ElementsPanel elementsPanel)
        {
            this.data = data;
            this.mapPanel = mapPanel;
            this.elementsPanel = elementsPanel;
            groupBoxes = new List<GroupCheckbox>();
            Initialize();
            Populate();
        }
        public void Initialize()
        {
            this.Size = new System.Drawing.Size(400, 500);
            BackColor = System.Drawing.Color.Beige;
            Dock = DockStyle.Fill;


            topControlPanel = new TableLayoutPanel();
            topControlPanel.Dock = DockStyle.Fill;
            topControlPanel.Size = new System.Drawing.Size(this.Size.Width, 24);
            //topControlPanel.AutoScroll = false;
            //attributePanel.Size = new System.Drawing.Size(280,500);
            topControlPanel.AutoSize = true;
            topControlPanel.ColumnCount = 3;
            topControlPanel.Visible = true;
            topControlPanel.BackColor = System.Drawing.Color.Beige;
            topControlPanel.Margin = new Padding(0);
            topControlPanel.Padding = new Padding(0);

            groupLabel = new Label();
            groupLabel.Text = "Group Visibility";
            //groupLabel.AutoSize = true;
            groupLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            groupLabel.Dock = DockStyle.Fill;
            groupLabel.Margin = new Padding(0);
            groupLabel.Size = new System.Drawing.Size(200, 25);
            groupLabel.Padding = new Padding(8, 0, 0, 0);
            groupLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            groupLabel.ForeColor = System.Drawing.Color.Black;
            groupLabel.BackColor = System.Drawing.Color.Beige;

            selectAllButton = new Button();
            selectAllButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            selectAllButton.ForeColor = System.Drawing.Color.Black;
            selectAllButton.BackColor = System.Drawing.Color.LightGreen;
            selectAllButton.FlatStyle = FlatStyle.Flat;
            selectAllButton.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            selectAllButton.FlatAppearance.BorderSize = 1;
            selectAllButton.Size = new System.Drawing.Size(150, 25);
            selectAllButton.Text = "Select All";
            selectAllButton.Click += SelectAll;

            unselectAllButton = new Button();
            unselectAllButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            unselectAllButton.Size = new System.Drawing.Size(150, 25);
            unselectAllButton.ForeColor = System.Drawing.Color.Black;
            unselectAllButton.BackColor = System.Drawing.Color.Orange;
            unselectAllButton.FlatStyle = FlatStyle.Flat;
            unselectAllButton.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            unselectAllButton.FlatAppearance.BorderSize = 1;
            unselectAllButton.Text = "Unselect All";
            unselectAllButton.Click += UnselectAll;

            topControlPanel.Controls.Add(groupLabel);
            topControlPanel.Controls.Add(selectAllButton);
            topControlPanel.Controls.Add(unselectAllButton);

            groupNamePanel = new TableLayoutPanel();
            groupNamePanel.AutoScroll = true;
            groupNamePanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            //attributePanel.Size = new System.Drawing.Size(280,500);
            //attributePanel.AutoSize = true;
            groupNamePanel.Dock = DockStyle.Fill;
            groupNamePanel.ColumnCount = 1;
            groupNamePanel.Visible = true;
            groupNamePanel.BackColor = System.Drawing.Color.Beige;
            
            Controls.Add(groupNamePanel);
            groupNamePanel.Controls.Add(topControlPanel);

        }

        public void SelectAll(object sender, EventArgs e)
        {
            for (int i = 0; i < groupBoxes.Count(); i++) {
                groupBoxes[i].Update(true, i == groupBoxes.Count() - 1 ? true : false);
            }
        }
        public void UnselectAll(object sender, EventArgs e)
        {
            for (int i = 0; i < groupBoxes.Count(); i++)
            {
                groupBoxes[i].Update(false, i == groupBoxes.Count() - 1? true : false);
            }
            mapPanel.FocusOff();
        }


        public void Populate()
        {
            groupBoxes.Clear();
            foreach (ThingGroup group in data.mapTextData.ThingGroups)
            {
                GroupCheckbox check = new GroupCheckbox(group.Id.Name, true, (bool val, bool update) =>
                {
                    if (!val && !data.hiddenGroups.Contains(group.Id.Name))
                    {
                        data.hiddenGroups.Add(group.Id.Name);
                    }else if (val && data.hiddenGroups.Contains(group.Id.Name))
                    {
                        data.hiddenGroups.Remove(group.Id.Name);
                    }

                    if (update)
                    {
                        if (val == false)
                        {
                            mapPanel.TryFocusOff(group.Id.Name);
                        }

                        mapPanel.Refresh();
                        elementsPanel.RefreshData();
                        elementsPanel.Refresh();
                    }

                });
                
                if (data.hiddenGroups.Contains(group.Id.Name))
                {
                    check.Update(false, false);
                }

                groupNamePanel.Controls.Add(check);
                groupBoxes.Add(check);
            }
        }
    }
}
