﻿#region Licence
/*
  *  Author: Marko Devcic, madevcic@gmail.com
  *  Copyright: Marko Devcic, 2015
  *  Licence: http://creativecommons.org/licenses/by-nc-nd/4.0/
 */
#endregion

using System;
using System.ComponentModel.Composition;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using AppsTracker.Data.Models;
using AppsTracker.MVVM;
using AppsTracker.Data.Repository;
using AppsTracker.Common.Communication;
using AppsTracker.Tracking;

namespace AppsTracker.ViewModels
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public sealed class AppStatsViewModel : ViewModelBase
    {
        private readonly IRepository repository;
        private readonly ITrackingService trackingService;
        private readonly IMediator mediator;

        public override string Title
        {
            get { return "APPS"; }
        }


        private AppDuration selectedApp;

        public AppDuration SelectedApp
        {
            get { return selectedApp; }
            set
            {
                SetPropertyValue(ref selectedApp, value);
                dailyAppList.Reload();
            }
        }


        public object SelectedItem { get; set; }


        private readonly AsyncProperty<IEnumerable<AppDuration>> appsList;

        public AsyncProperty<IEnumerable<AppDuration>> AppsList
        {
            get { return appsList; }
        }


        private readonly AsyncProperty<IEnumerable<DailyAppDuration>> dailyAppList;

        public AsyncProperty<IEnumerable<DailyAppDuration>> DailyAppList
        {
            get { return dailyAppList; }
        }


        private ICommand returnFromDetailedViewCommand;

        public ICommand ReturnFromDetailedViewCommand
        {
            get { return returnFromDetailedViewCommand ?? (returnFromDetailedViewCommand = new DelegateCommand(ReturnFromDetailedView)); }
        }


        [ImportingConstructor]
        public AppStatsViewModel(IRepository repository,
                                 ITrackingService trackingService,
                                 IMediator mediator)
        {
            this.repository = repository;
            this.trackingService = trackingService;
            this.mediator = mediator;

            appsList = new TaskRunner<IEnumerable<AppDuration>>(GetApps, this);
            dailyAppList = new TaskRunner<IEnumerable<DailyAppDuration>>(GetDailyApp, this);

            this.mediator.Register(MediatorMessages.REFRESH_LOGS, new Action(ReloadAll));
        }


        private void ReloadAll()
        {
            appsList.Reload();
            dailyAppList.Reload();
        }


        private IEnumerable<AppDuration> GetApps()
        {
            var logs = repository.GetFiltered<Log>(l => l.Window.Application.User.UserID == trackingService.SelectedUserID
                                        && l.DateCreated >= trackingService.DateFrom
                                        && l.DateCreated <= trackingService.DateTo,
                                        l => l.Window.Application,
                                        l => l.Window.Application.User);

            var grouped = logs.GroupBy(l => l.Window.Application.Name);

            return grouped.Select(g => new AppDuration()
            {
                Name = g.Key,
                Duration = Math.Round(new TimeSpan(g.Sum(l => l.Duration)).TotalHours, 1)
            });
        }


        private IEnumerable<DailyAppDuration> GetDailyApp()
        {
            var app = selectedApp;
            if (app == null)
                return null;

            var logs = repository.GetFiltered<Log>(l => l.Window.Application.Name == app.Name
                                               && l.Window.Application.User.UserID == trackingService.SelectedUserID
                                               && l.DateCreated >= trackingService.DateFrom
                                               && l.DateCreated <= trackingService.DateTo);

            var grouped = logs.GroupBy(l => new
                                        {
                                            year = l.DateCreated.Year,
                                            month = l.DateCreated.Month,
                                            day = l.DateCreated.Day
                                        })
                                          .OrderBy(g => new DateTime(g.Key.year, g.Key.month, g.Key.day));

            return grouped.Select(g => new DailyAppDuration
            {
                Date = new DateTime(g.Key.year, g.Key.month, g.Key.day).ToShortDateString(),
                Duration = Math.Round(new TimeSpan(g.Sum(l => l.Duration)).TotalHours, 1)
            });
        }


        private void ReturnFromDetailedView()
        {
            SelectedApp = null;
        }

    }
}
