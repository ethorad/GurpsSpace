using GurpsSpace.PlanetCreation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GurpsSpace.ViewModels
{
    internal class ViewModelList<T> : ViewModel where T : ViewModel
    {
        private ObservableCollection<T> items;
        public ObservableCollection<T> Items
        {
            get { return items; }
        }
        public int Count { get { return items.Count; } }
        //public void Add(Planet planet)
        //{
        //    items.Add(new T(planet));
        //    MemberUpdated();
        //}
        public void Add(T newItem)
        {
            items.Add(newItem);
            MemberUpdated();
        }

        public ViewModelList(List<T> itemLst)
        {
            items = new ObservableCollection<T>(itemLst);
            MemberUpdated();
        }
        //public ViewModelList(List<Planet> planetLst)
        //{
        //    items = new ObservableCollection<T>();
        //    foreach (Planet planet in planetLst)
        //        items.Add(new T(planet));
        //    MemberUpdated();
        //}
        public ViewModelList()
        {
            items = new ObservableCollection<T>();
            MemberUpdated();
        }
    }
}
