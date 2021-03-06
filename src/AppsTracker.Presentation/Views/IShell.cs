﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsTracker.Views
{
    public interface IShell 
    {
        event CancelEventHandler Closing;

        double Left { get; set; }

        double Top { get; set; }
        
        double Width { get; set; }
        
        double Height { get; set; }

        bool IsLoaded { get; }

        void Show();

        bool Activate();

        bool? ShowDialog();

        void Close();

        object ViewArgument { get; set; }

        System.Windows.Window Owner { get; set; }
    }
}
