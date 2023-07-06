using GurpsSpace.PlanetCreation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GurpsSpace.ViewModels
{
    public class ViewModelList<T> : ViewModel where T : ViewModel
    {
        private ObservableCollection<T> items;
        public ObservableCollection<T> Items
        {
            get { return items; }
        }
        public int Count { get { return items.Count; } }

        public override string SummaryType { get { return items.Count.ToString(); } }

        public void Add(T newItem)
        {
            items.Add(newItem);
            MemberUpdated();
        }
        public void Clear()
        {
            items.Clear();
            MemberUpdated();
        }
        public void RemoveAt(int i)
        {
            items.RemoveAt(i);
            MemberUpdated();
        }

        public ViewModelList(List<T> itemLst)
        {
            items = new ObservableCollection<T>(itemLst);
            MemberUpdated();
        }

        public ViewModelList()
        {
            items = new ObservableCollection<T>();
            MemberUpdated();
        }

        public string this[string typeToSummarise]
        {
            get
            {
                if (typeToSummarise == "all")
                    return Items.Count.ToString();

                int count = 0;

                foreach (ViewModel vm in Items)
                    if (vm.SummaryType == typeToSummarise)
                        count++;
                return count.ToString();
            }
        }

    }
}
