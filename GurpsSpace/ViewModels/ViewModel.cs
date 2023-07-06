using System;
using System.ComponentModel;

namespace GurpsSpace
{
    public abstract class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public abstract string SummaryType { get; }

        protected void MemberUpdated(string property="")
        {
            // by default set the property to String.Empty so that all properties count as updated
            if (property == "")
                property = String.Empty;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
