﻿using AppsTracker.Data.Models;

namespace AppsTracker.Tracking.Limits
{
    internal interface IShutdownService
    {
        void Shutdown(string appName);
    }
}