using GurpsSpace.PlanetCreation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GurpsSpace.ViewModels
{
    internal class ViewModelSetting : ViewModel
    {
        private Setting setting;
        private ViewModelPlanetList planetList;
        public ViewModelPlanetList PlanetList { get { return planetList; } }

        public List<Species> SpeciesList { get { return setting.Species; } }
        public Species MainSpecies { get { return setting.MainSpecies; } }

        public string Name
        {
            get { return setting.Name; }
            set
            {
                setting.Name = value;
                MemberUpdated();
            }
        }
        public int TechLevel
        {
            get { return setting.TechLevel; }
            set
            {
                setting.TechLevel = value;
                MemberUpdated();
            }
        }
        public eSettingSocietyType SocietyType
        {
            get { return setting.SocietyType; }
            set
            {
                setting.SocietyType = value;
                MemberUpdated();
            }
        }

        public string TechLevelDescription { get { return RuleBook.TechLevelParams[TechLevel].Description; } }

        public ViewModelSetting(Setting setting)
        {
            this.setting = setting;
            planetList = new ViewModelPlanetList(setting.Planets);
        }

        public void Add(Planet p)
        {
            setting.Planets.Add(p);
            planetList.Add(p);
            MemberUpdated();
        }
        public void Add(Species s)
        {
            setting.Species.Add(s);
            MemberUpdated();
        }
        public void Remove(Planet p)
        {
            for (int i=0; i<planetList.Count; i++)
            {
                if (planetList.Planets[i].Planet == p)
                {
                    setting.Planets.RemoveAt(i);
                    planetList.Planets.RemoveAt(i);
                    MemberUpdated();
                    break;
                }
            }
        }

    }
}
