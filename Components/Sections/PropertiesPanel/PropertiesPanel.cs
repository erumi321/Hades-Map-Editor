using Hades_Map_Editor.Data;
using Hades_Map_Editor.Managers;
using Hades_Map_Editor.PropertiesSection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using Hades_Map_Editor.MapSection;

namespace Hades_Map_Editor.Sections
{
    public class PropertiesPanel: Panel, IComponent, Focusable
    {
        private ProjectData data;
        private Label noSelectionLabel;
        private Panel attributePanel;
        private Obstacle currentObstacle;
        PropertyCheckbox activateAtRange, active, allowMovementReaction, causesOcculsion, clutter, collision, 
            createsShadows, drawVfxOnTop, flipHorizontal, flipVertical, ignoreGridManager, invert, stopsLight,
            useBoundsForSortArea;
        PropertyDouble activationRange, ambient, angle, hue, offsetZ, parallaxAmount, saturation, scale, 
            skewAngle, skewScale, tallness, value;
        PropertyInt helpTextId, id, sortIndex;
        PropertyTextbox comments, dataType, name;
        PropertyTitle obstacleTitle, metadataTitle;

        PropertyID attachToID;
        PropertyLocation location;
        PropertyAttachedIDs attachedIDs;
        //PropertyGroup groupNames;
        PropertyColor color;
        //PropertyPoints points;

        MapPanel mapPanel;
        public PropertiesPanel(ProjectData data, MapPanel mapPanel)
        {
            this.data = data;
            this.mapPanel = mapPanel;
            Initialize();
            Populate();
            //properties = new ThingTextProperties(this, panel);
        }
        public void Initialize()
        {
            this.Size = new System.Drawing.Size(400, 500);
            BackColor = System.Drawing.Color.DarkGray;
            //AutoScroll = true;
            //AutoSize = true;
            Dock = DockStyle.Fill;

            noSelectionLabel = new Label();
            noSelectionLabel.Text = "No Selection";
            noSelectionLabel.AutoSize = true;
            noSelectionLabel.Left = (ClientSize.Width - noSelectionLabel.Width) / 2;
            noSelectionLabel.Top = (ClientSize.Height - noSelectionLabel.Height) / 2;
            //noSelectionLable.Dock = DockStyle.Cente;
            noSelectionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            attributePanel = new TableLayoutPanel();
            attributePanel.AutoScroll = true;
            //attributePanel.Size = new System.Drawing.Size(280,500);
            //attributePanel.AutoSize = true;
            attributePanel.Dock = DockStyle.Fill;
            attributePanel.BackColor = System.Drawing.Color.Black;
            attributePanel.Visible = true;

            obstacleTitle = new PropertyTitle("Obstacles Data");
            activateAtRange = new PropertyCheckbox("Activate At Range", false);
            activationRange = new PropertyDouble("Activation Range", false);
            active = new PropertyCheckbox("Active", true, (bool v) =>
            {
                if (v == currentObstacle.Active)
                {
                    return;
                }

                currentObstacle.Active = v;
                mapPanel.RefreshObstacle(currentObstacle.Id);
            });
            allowMovementReaction = new PropertyCheckbox("Allow Movement Reaction", true, (bool v) => { currentObstacle.AllowMovementReaction = v; });
            ambient = new PropertyDouble("Ambient", true, (double v) => { currentObstacle.Ambient = v; });
            angle = new PropertyDouble("Angle", true, (double v) => {
                if (v == currentObstacle.Angle)
                {
                    return;
                }
                currentObstacle.Angle = v; 
                mapPanel.RefreshObstacle(currentObstacle.Id);
            });
            attachToID = new PropertyID("Attached To ID");
            attachedIDs = new PropertyAttachedIDs("Attached IDs");
            causesOcculsion = new PropertyCheckbox("Causes Occulsion", true, (bool v) => { currentObstacle.CausesOcculsion = v; });
            clutter = new PropertyCheckbox("Clutter", true, (bool v) => { currentObstacle.Clutter = v; });
            collision = new PropertyCheckbox("Collision", true, (bool v) => { currentObstacle.Collision = v; });
            color = new PropertyColor("Color", true, (Color c) =>
            {
                if (c.R == currentObstacle.Color.R && c.B == currentObstacle.Color.B && c.G == currentObstacle.Color.G && c.A == currentObstacle.Color.A)
                {
                    return;
                }
                Obstacle.JsonColor t = new Obstacle.JsonColor();
                t.R = c.R;
                t.G = c.G;
                t.B = c.B;
                t.A = c.A;
                currentObstacle.Color = t;

                mapPanel.RefreshObstacle(currentObstacle.Id);
            });
            comments = new PropertyTextbox("Comments", true, (string v) => { currentObstacle.Comments = v; });
            createsShadows = new PropertyCheckbox("Creates Shadows", true, (bool v) => { currentObstacle.CreatesShadows = v; });
            dataType = new PropertyTextbox("Data Type", true);
            drawVfxOnTop = new PropertyCheckbox("Draw Vfx On Top", true, (bool v) => { currentObstacle.DrawVfxOnTop = v; });
            flipHorizontal = new PropertyCheckbox("Flip Horizontal", true, (bool v) => {
                if (v == currentObstacle.FlipHorizontal)
                {
                    return;
                }
                currentObstacle.FlipHorizontal = v;
                mapPanel.RefreshObstacle(currentObstacle.Id);
            });
            flipVertical = new PropertyCheckbox("Flip Vertical", true, (bool v) => {
                if (v == currentObstacle.FlipVertical)
                {
                    return;
                }
                currentObstacle.FlipVertical = v;
                mapPanel.RefreshObstacle(currentObstacle.Id);
            });
            //groupNames = 
            //helpTextId = new PropertyInt("Help Text Id");
            hue = new PropertyDouble("Hue", true, (double v) =>
            {
                if (v == currentObstacle.Hue)
                {
                    return;
                }
                currentObstacle.Hue = v;
                mapPanel.RefreshObstacle(currentObstacle.Id);
            });
            id = new PropertyInt("Id", false);
            ignoreGridManager = new PropertyCheckbox("Ignore Grid Manager", false);
            invert = new PropertyCheckbox("Invert", true, (bool v) => {
                if (v == currentObstacle.Invert)
                {
                    return;
                }
                currentObstacle.Invert = v;
                mapPanel.RefreshObstacle(currentObstacle.Id);
            });
            location = new PropertyLocation("Location", true, (Point p) =>
            {
                if (p.X == currentObstacle.Location.X && p.Y == currentObstacle.Location.Y)
                {
                    return;
                }
                Obstacle.JsonPoint t = new Obstacle.JsonPoint();
                t.X = p.X;
                t.Y = p.Y;
                currentObstacle.Location = t;

                mapPanel.RefreshObstacle(currentObstacle.Id);
                mapPanel.FocusOn(currentObstacle.Id);
            });
            name = new PropertyTextbox("Name", true, (string v) => {
                if (v == currentObstacle.Name)
                {
                    return;
                }
                currentObstacle.Name = v;
                mapPanel.RefreshObstacle(currentObstacle.Id);
            });
            offsetZ = new PropertyDouble("Offset Z", true, (double v) => {
                if (v == currentObstacle.OffsetZ)
                {
                    return;
                }
                currentObstacle.OffsetZ = v;
                mapPanel.RefreshObstacle(currentObstacle.Id);
            });
            parallaxAmount = new PropertyDouble("Parallax Amount", true);
            //points =
            saturation = new PropertyDouble("Saturation", true, (double v) =>
            {
                if (v == currentObstacle.Saturation)
                {
                    return;
                }
                currentObstacle.Saturation = v;
                mapPanel.RefreshObstacle(currentObstacle.Id);
            });
            scale = new PropertyDouble("Scale", true, (double v) =>
            {
                if (v == currentObstacle.Scale)
                {
                    return;
                }
                currentObstacle.Scale = v;
                mapPanel.RefreshObstacle(currentObstacle.Id);
            });

            //math is very complicated, not currently supported
            skewAngle = new PropertyDouble("Skew Angle", false);
            skewScale = new PropertyDouble("Skew Scale", false);

            //also not ucrrently supported, indexing not fully figured out yet
            sortIndex = new PropertyInt("Sort Index", false);

            stopsLight = new PropertyCheckbox("Stops Light", true, (bool v) => { currentObstacle.StopsLight = v; });
            tallness = new PropertyDouble("Tallness", true, (double v) => { currentObstacle.Tallness = v; });

            //Not currently supported, unsure of what this does
            useBoundsForSortArea = new PropertyCheckbox("Use Bounds For Sort Area", false);

            value = new PropertyDouble("Value", true, (double v) =>
            {
                if (v == currentObstacle.Value)
                {
                    return;
                }
                currentObstacle.Value = v;
                mapPanel.RefreshObstacle(currentObstacle.Id);
            });


            attributePanel.Controls.Add(obstacleTitle);
            attributePanel.Controls.Add(activateAtRange);
            attributePanel.Controls.Add(activationRange);
            attributePanel.Controls.Add(active);
            attributePanel.Controls.Add(allowMovementReaction);
            attributePanel.Controls.Add(ambient);
            attributePanel.Controls.Add(angle);
            attributePanel.Controls.Add(attachToID);
            attributePanel.Controls.Add(attachedIDs);
            attributePanel.Controls.Add(causesOcculsion);
            attributePanel.Controls.Add(clutter);
            attributePanel.Controls.Add(collision);
            attributePanel.Controls.Add(color);
            attributePanel.Controls.Add(comments);
            attributePanel.Controls.Add(createsShadows);
            attributePanel.Controls.Add(dataType);
            attributePanel.Controls.Add(drawVfxOnTop);
            attributePanel.Controls.Add(flipHorizontal);
            attributePanel.Controls.Add(flipVertical);
            //attributePanel.Controls.Add(groupNames);
            //attributePanel.Controls.Add(helpTextId);
            attributePanel.Controls.Add(hue);
            attributePanel.Controls.Add(id);
            attributePanel.Controls.Add(ignoreGridManager);
            attributePanel.Controls.Add(invert);
            attributePanel.Controls.Add(location);
            attributePanel.Controls.Add(name);
            attributePanel.Controls.Add(offsetZ);
            attributePanel.Controls.Add(parallaxAmount);
            //attributePanel.Controls.Add(points);
            attributePanel.Controls.Add(saturation);
            attributePanel.Controls.Add(scale);
            attributePanel.Controls.Add(skewAngle);
            attributePanel.Controls.Add(skewScale);
            attributePanel.Controls.Add(sortIndex);
            attributePanel.Controls.Add(stopsLight);
            attributePanel.Controls.Add(tallness);
            attributePanel.Controls.Add(useBoundsForSortArea);
            attributePanel.Controls.Add(value);
            //attributePanel.Visible = false;

            Controls.Add(attributePanel);
            Controls.Add(noSelectionLabel);
        }
        public void Populate()
        {
            UnFocus();
        }
        public void UnFocus()
        {
            noSelectionLabel.Visible = true;
            attributePanel.Visible = false;
        }
        public void FocusOn(int obsID)
        {
            currentObstacle = data.mapData.GetFromId(obsID);
            activateAtRange.Update(currentObstacle.ActivateAtRange);
            activationRange.Update(currentObstacle.ActivationRange);
            active.Update(currentObstacle.Active);
            allowMovementReaction.Update(currentObstacle.AllowMovementReaction);
            ambient.Update(currentObstacle.Ambient);
            angle.Update(currentObstacle.Angle);
            attachToID.Update(currentObstacle.AttachToID);
            attachedIDs.Update(currentObstacle.AttachedIDs.ToArray());
            causesOcculsion.Update(currentObstacle.CausesOcculsion);
            clutter.Update(currentObstacle.Clutter);
            collision.Update(currentObstacle.Collision);
            color.Update(currentObstacle.GetColor());
            comments.Update((string)currentObstacle.Comments);
            //createsShadows.Update((bool)currentObstacle.CreatesShadows);
            dataType.Update(currentObstacle.DataType);
            drawVfxOnTop.Update(currentObstacle.DrawVfxOnTop);
            flipHorizontal.Update(currentObstacle.FlipHorizontal);
            flipVertical.Update(currentObstacle.FlipVertical);
            //groupNames.Update(currentObstacle.GroupNames);
            //helpTextId.Update((int)currentObstacle.HelpTextID);
            hue.Update(currentObstacle.Hue);
            id.Update(currentObstacle.Id);
            ignoreGridManager.Update(currentObstacle.IgnoreGridManager);
            invert.Update(currentObstacle.Invert);
            location.Update(currentObstacle.GetLocation());
            name.Update(currentObstacle.Name);
            offsetZ.Update(currentObstacle.OffsetZ);
            parallaxAmount.Update(currentObstacle.ParallaxAmount);
            //points.Update(currentObstacle.Points);
            saturation.Update(currentObstacle.Saturation);
            scale.Update(currentObstacle.Scale);
            skewAngle.Update(currentObstacle.SkewAngle);
            skewScale.Update(currentObstacle.SkewScale);
            sortIndex.Update(currentObstacle.SortIndex);
            if(currentObstacle.StopsLight == null)
            {
                stopsLight.Update(false);
            }
            else
            {
                stopsLight.Update((bool)currentObstacle.StopsLight);
            }
            tallness.Update(currentObstacle.Tallness);
            useBoundsForSortArea.Update(currentObstacle.UseBoundsForSortArea);
            value.Update(currentObstacle.Value);

            noSelectionLabel.Visible = false;
            attributePanel.Visible = true;

        }
    }
}
