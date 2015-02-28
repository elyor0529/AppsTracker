﻿#region Licence
/*
  *  Author: Marko Devcic, madevcic@gmail.com
  *  Copyright: Marko Devcic, 2014
  *  Licence: http://creativecommons.org/licenses/by-nc-nd/4.0/
 */
#endregion

using System;

namespace AppsTracker.Data.Models
{
    public abstract class Selectable
    {
        protected bool _isSelected;
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                SelectedChanging();
            }
        }

        protected void SelectedChanging()
        {
            var handler = IsSelectedChanging;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }
        public event EventHandler IsSelectedChanging;
    }
    public abstract class TopModel : Selectable
    {
        public double Usage { get; set; }
        public long Duration { get; set; }
        public bool IsRequested { get; set; }
    }

    public class TopAppsModel : TopModel
    {
        public string AppName { get; set; }
        public string Date { get; set; }
        public DateTime DateTime { get; set; }

    }

    public class TopWindowsModel : TopModel
    {
        public string Title { get; set; }

    }

    public class DayViewModel
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string DateCreated { get; set; }
        public string DateEnded { get; set; }
        public long Duration { get; set; }
        public bool IsRequested { get; set; }
        public bool IsSelected { get; set; }

    }
}