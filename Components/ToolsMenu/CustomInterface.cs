﻿using Hades_Map_Editor.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hades_Map_Editor
{
    public interface IComponent
    {
        void Initialize();
        void Populate();
    }
    public interface IPaging
    {
        void GoToPage(int numberPage);
        int GetCurrentPage();
    }
    public interface Focusable
    {
        void FocusOn(Obstacle obs);
        void UnFocus();
    }
}
