using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GurpsSpace
{
    internal class ViewModel
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
