using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GurpsSpace.PlanetCreation;
using GurpsSpace.ViewModels;

namespace GurpsSpace.SpeciesCreation
{
    public class ViewModelSpeciesFactory : ViewModel
    {
        private SpeciesFactory speciesFactory;

        public override string SummaryType { get { return speciesFactory.Name ?? "tbc"; } }
        public ViewModelSpeciesFactory(SpeciesFactory speciesFactory)
        {
            this.speciesFactory = speciesFactory;
            traitList = new ViewModelList<ViewModelTrait>();
            UpdateTraitList();
        }
        private void UpdateTraitList()
        {
            traitList.Clear();
            foreach (Trait t in speciesFactory.Traits)
                traitList.Add(new ViewModelTrait(t));
            MemberUpdated();
        }

        private ViewModelList<ViewModelTrait> traitList;
        public ViewModelList<ViewModelTrait> TraitList { get { return traitList; } }
        public ObservableCollection<ViewModelTrait> TraitListItems { get { return traitList.Items; } }
        

        public string Name
        { 
            get { return speciesFactory.Name ?? "tbc"; }
            set 
            { 
                speciesFactory.Name = value;
                MemberUpdated();
            }
        }
        public string DietString
        {
            get
            {
                if (speciesFactory.Diet == null)
                    return "tbc";
                else
                    return speciesFactory.Diet.ToString()!;
            }
        }
        public string ConsumptionString
        {
            get
            {
                if (speciesFactory.HasTrait(eTrait.DoesntEatOrDrink))
                    return "Doesn't eat or drink";
                else if (speciesFactory.GetTraitLevel(eTrait.ReducedConsumption) > 0)
                    return "Reduced (" + speciesFactory.GetTraitLevel(eTrait.ReducedConsumption).ToString() + ")";
                else if (speciesFactory.GetTraitLevel(eTrait.IncreasedConsumption) > 0)
                    return "Increased (" + speciesFactory.GetTraitLevel(eTrait.IncreasedConsumption).ToString() + ")";
                else // so both zero
                    return "Normal";
            }
        }
        public string StartingColonyPopulationString
        {
            get
            {
                if (speciesFactory.StartingColonyPopulation == null)
                    return "tbc";
                else
                    return (speciesFactory.StartingColonyPopulation ?? 0).ToString("N0");
            }
        }
        public string AnnualGrowthRateString
        {
            get
            {
                if (speciesFactory.AnnualGrowthRate == null)
                    return "tbc";
                else
                    return ((speciesFactory.AnnualGrowthRate ?? 0) * 100).ToString("N1") + "%";
            }
        }
        public string AffinityMultiplierString
        {
            get
            {
                if (speciesFactory.AffinityMultiplier == null)
                    return "tbc";
                else
                    return (speciesFactory.AffinityMultiplier ?? 0).ToString("N2");
            }
        }
        public string LifeChemistryString
        {
            get
            {
                if (speciesFactory.LifeChemistry == null)
                    return "tbc";
                else
                    return speciesFactory.LifeChemistry.ToString()!;
            }
        }

        public void RandomParameter(string param)
        {
            speciesFactory.RandomParameter(param);
            UpdateTraitList();
        }
        public void SelectParameter(string param)
        {
            speciesFactory.SelectParameter(param);
            UpdateTraitList();
        }
        public void FullRandom()
        {
            speciesFactory.FullRandom();
            UpdateTraitList();
        }
    }
}
