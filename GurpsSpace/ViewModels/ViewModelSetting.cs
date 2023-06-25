using GurpsSpace.PlanetCreation;
using GurpsSpace.SpeciesCreation;
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
        private ViewModelList<ViewModelPlanet> planetList;
        public ViewModelList<ViewModelPlanet> PlanetList { get { return  planetList; } }

        private ViewModelList<ViewModelSpecies> speciesList;
        public ViewModelList<ViewModelSpecies> SpeciesList { get { return speciesList; } }

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
            planetList = new ViewModelList<ViewModelPlanet>();
            foreach (Planet p in setting.Planets)
                planetList.Add(new ViewModelPlanet(p));
            speciesList = new ViewModelList<ViewModelSpecies>();
            foreach(Species s in setting.Species)
                speciesList.Add(new ViewModelSpecies(s));
        }

        public void Add(Planet p)
        {
            setting.Planets.Add(p);
            planetList.Add(new ViewModelPlanet(p));
            MemberUpdated();
        }
        public void Add(Species s)
        {
            setting.Species.Add(s);
            speciesList.Add(new ViewModelSpecies(s));
            MemberUpdated();
        }
        public void Remove(Planet p)
        {
            for (int i=0; i<planetList.Count; i++)
            {
                if (planetList.Items[i].Planet == p)
                {
                    setting.Planets.RemoveAt(i);
                    planetList.Items.RemoveAt(i);
                    MemberUpdated();
                    break;
                }
            }
        }
        public void Remove(Species s)
        {
            for (int i=0;i<speciesList.Count;i++)
            {
                if (speciesList.Items[i].Species == s)
                {
                    setting.Species.RemoveAt(i);
                    speciesList.Items.RemoveAt(i);
                    MemberUpdated();
                    break;
                }
            }
        }

    }
}
