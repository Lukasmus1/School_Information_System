// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
//

using CommunityToolkit.Mvvm.Messaging;

namespace VUTIS2.App.Services;

public interface IMessengerService
{
    IMessenger Messenger { get; }

    void Send<TMessage>(TMessage message)
        where TMessage : class;
}
