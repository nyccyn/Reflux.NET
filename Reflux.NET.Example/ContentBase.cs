using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Reflux.NET.Example.Annotations;

namespace Reflux.NET.Example
{
    public abstract class ContextBase : INotifyPropertyChanged
    {        
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
