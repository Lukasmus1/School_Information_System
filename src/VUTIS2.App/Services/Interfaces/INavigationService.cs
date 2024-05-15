// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
//

using VUTIS2.App.Models;
using VUTIS2.App.ViewModels;

namespace VUTIS2.App.Services;

public interface INavigationService
{
    IEnumerable<RouteModel> Routes { get; }

    Task GoToAsync<TViewModel>(IDictionary<string, object?> parameters)
        where TViewModel : IViewModel;

    Task GoToAsync(string route);
    bool SendBackButtonPressed();
    Task GoToAsync(string route, IDictionary<string, object?> parameters);

    Task GoToAsync<TViewModel>()
        where TViewModel : IViewModel;
}
