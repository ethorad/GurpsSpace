using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GurpsSpace.PlanetCreation
{
    internal class ViewModelPlanetList : ViewModel
    {
        private ObservableCollection<ViewModelPlanet> planets;
        public ObservableCollection<ViewModelPlanet> Planets
        { 
            get { return planets; }
        }
        public int Count { get { return planets.Count; } }
        public void Add(Planet planet)
        {
            planets.Add(new ViewModelPlanet(planet));
            MemberUpdated();
        }
        public void Add(ViewModelPlanet vmPlanet)
        {
            planets.Add(vmPlanet);
            MemberUpdated();
        }

        public ViewModelPlanetList(List<ViewModelPlanet> planetLst)
        {
            planets = new ObservableCollection<ViewModelPlanet>(planetLst);
            MemberUpdated();
        }
        public ViewModelPlanetList(List<Planet> planetLst)
        {
            planets = new ObservableCollection<ViewModelPlanet>();
            foreach (Planet planet in planetLst)
                planets.Add(new ViewModelPlanet(planet));
            MemberUpdated();
        }
        public ViewModelPlanetList()
        {
            planets = new ObservableCollection<ViewModelPlanet>();
            MemberUpdated();
        }
    }
}
