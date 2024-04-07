﻿using System.ComponentModel;
using System.Runtime.CompilerServices;

using System.ComponentModel;

namespace VUTIS2.BL.Models;

public class ModelBase : INotifyPropertyChanged, IModel
{
    public Guid Id { get; set; }
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
