﻿#region Licence
/*
  *  Author: Marko Devcic, madevcic@gmail.com
  *  Copyright: Marko Devcic, 2015
  *  Licence: http://creativecommons.org/licenses/by-nc-nd/4.0/
 */
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using AppsTracker.Common.Communication;
using AppsTracker.Domain;
using AppsTracker.Domain.Apps;
using AppsTracker.MVVM;

namespace AppsTracker.ViewModels
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public sealed class DailyAppUsageViewModel : ViewModelBase
    {
        private readonly Mediator mediator;
        private readonly IUseCase<AppDurationOverview> useCase;


        public override string Title
        {
            get { return "DAILY APP USAGE"; }
        }


        public object SelectedItem { get; set; }


        private readonly AsyncProperty<IEnumerable<AppDurationOverview>> appsList;

        public AsyncProperty<IEnumerable<AppDurationOverview>> AppsList
        {
            get { return appsList; }
        }


        [ImportingConstructor]
        public DailyAppUsageViewModel(IUseCase<AppDurationOverview> useCase,
                                      Mediator mediator)
        {
            this.useCase = useCase;
            this.mediator = mediator;

            appsList = new TaskRunner<IEnumerable<AppDurationOverview>>(useCase.Get, this);

            this.mediator.Register(MediatorMessages.REFRESH_LOGS, new Action(appsList.Reload));
        }
    }
}
