using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hades_Map_Editor
{
    public class PagingComponent : StatusStrip, IComponent
    {
        ToolStripButton dotButton1, dotButton2, firstButton, lastButton;
        ToolStripButton numButton1, numButton2, numButton3;
        //ToolStripLabel label1, label2;
        int currentPage, maxPage = 0;
        IPaging parent;

        public PagingComponent(IPaging parent)
        {
            this.parent = parent;
            Initialize();
            Populate();
        }
        public void Initialize()
        {
            dotButton1 = new ToolStripButton();
            dotButton1.Text = "...";
            dotButton2 = new ToolStripButton();
            dotButton2.Text = "...";
            firstButton = new ToolStripButton();
            firstButton.Text = "1";
            lastButton = new ToolStripButton();
            lastButton.Text = "X";
            numButton1 = new ToolStripButton();
            numButton1.Text = "A";
            numButton2 = new ToolStripButton();
            numButton2.Text = "B";
            numButton3 = new ToolStripButton();
            numButton3.Text = "C";
            Items.AddRange(new ToolStripItem[] { firstButton, dotButton1, numButton1, numButton2, numButton3, dotButton2, lastButton });
        }

        public void Populate()
        {
            firstButton.Click += Action_GoToFirstPage;
            lastButton.Click += Action_GoToLastPage;
            numButton1.Click += Action_GoToPreviousPage;
            numButton3.Click += Action_GoToNextPage;
            SetupPaging(0);
        }
        public void SetupPaging(int maxPage)
        {
            this.maxPage = maxPage;
            dotButton1.Visible = false;
            dotButton2.Visible = false;
            firstButton.Visible = false;
            lastButton.Visible = false;
            numButton1.Visible = false;
            numButton2.Visible = false;
            numButton3.Visible = false;
            if (maxPage >= 1)
            {
                firstButton.Visible = true;
            }
            if (maxPage > 1)
            {
                lastButton.Visible = true;
                lastButton.Text = this.maxPage.ToString();
            }
            GoToPage(1);
        }
        public void GoToPage(int currentPage)
        {
            this.currentPage = currentPage;
            dotButton1.Visible = !(currentPage <= 3);
            dotButton2.Visible = !(currentPage > (maxPage-3));
            numButton1.Visible = (currentPage >= 3);
            numButton1.Text = (currentPage - 1).ToString();
            numButton2.Visible = (currentPage >= 2 && currentPage <= (maxPage - 1));
            numButton2.Text = currentPage.ToString();
            numButton3.Visible = (currentPage <= (maxPage - 2));
            numButton3.Text = (currentPage + 1).ToString();
            if (currentPage == 1)
            {
                firstButton.Font = new Font(numButton2.Font, FontStyle.Bold);
                lastButton.Font = new Font(numButton2.Font, FontStyle.Regular);
                numButton2.Font = new Font(numButton2.Font, FontStyle.Regular);
            }
            else if (currentPage == maxPage)
            {
                firstButton.Font = new Font(numButton2.Font, FontStyle.Regular);
                lastButton.Font = new Font(numButton2.Font, FontStyle.Bold);
                numButton2.Font = new Font(numButton2.Font, FontStyle.Regular);
            }
            else
            {
                firstButton.Font = new Font(numButton2.Font, FontStyle.Regular);
                lastButton.Font = new Font(numButton2.Font, FontStyle.Regular);
                numButton2.Font = new Font(numButton2.Font, FontStyle.Bold);
            }
        }
        private void Action_GoToFirstPage(object sender, EventArgs e)
        {
            if(currentPage != 1)
            {
                parent.GoToPage(1);
                GoToPage(1);
            }
        }
        private void Action_GoToLastPage(object sender, EventArgs e)
        {
            if (currentPage != maxPage)
            {
                parent.GoToPage(maxPage);
                GoToPage(maxPage);
            }
        }
        private void Action_GoToPreviousPage(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                parent.GoToPage(currentPage-1);
                GoToPage(currentPage-1);
            }
        }
        private void Action_GoToNextPage(object sender, EventArgs e)
        {
            if (currentPage < maxPage)
            {
                parent.GoToPage(currentPage+1);
                GoToPage(currentPage+1);
            }
        }
    }
}
