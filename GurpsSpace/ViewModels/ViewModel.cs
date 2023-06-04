using System;
using System.ComponentModel;

namespace GurpsSpace
{
    public class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public ViewModel() { }

        protected void MemberUpdated(string property="")
        {
            // by default set the property to String.Empty so that all properties count as updated
            if (property == "")
                property = String.Empty;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
