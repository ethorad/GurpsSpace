using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace GurpsSpace.PlanetCreation
{
    public class PlanetFactory
    {
        private Planet planet;
        public Planet Planet { get { return planet; } }
        private PlanetParameters? parameters;

        private IPlanetCreator randomiser;
        private IPlanetCreator userInput;

        public string? Name { get { return planet.Name; } set {  planet.Name = value; } }
        public eSize? Size { get { return planet.Size; } }
        public eSubtype? Subtype { get { return planet.Subtype; } }
        public eOverallType? OverallType { get { return planet.OverallType; } }
        public eResourceValueCategory? ResourceValueCategory { get { return planet.ResourceValueCategory; } }
        public int? ResourceValueModifier { get { return planet.ResourceValueModifier; } }
        public string? Description { get { return planet.Description; } set { planet.Description = value; } }

        public bool? HasAtmosphere { get { return (parameters == null) ? null : parameters.HasAtmosphere; } }
        public double? AtmosphericMass 
        { 
            get 
            { 
                return planet.AtmosphericMass; 
            } 
            set
            {
                if (value < 0)
                    planet.AtmosphericMass = 0;
                else
                    planet.AtmosphericMass = value;
            }
        }
        public fAtmosphericConditions? AtmosphericConditions { get { return planet.AtmosphericConditions; } }
        public bool? HasAtmosphericOptions { get { return (parameters == null) ? null : (parameters.AtmosphereANumber < 18); } }
        public string? AtmosphericDescription { get { return planet.AtmosphericDescription; } }
        public double? AtmosphericPressure { get { return planet.AtmosphericPressure; } }
        public ePressureCategory? AtmosphericPressureCategory { get { return planet.AtmosphericPressureCategory; } }

        public bool? HasLiquid
        {
            get 
            {
                return (parameters == null) ? null : (parameters.Liquid != eLiquid.None);
            }
        }
        public double? MinimumHydrographicCoverage { get { return (parameters == null) ? null : parameters.HydroMin; } }
        public double? MaximumHydrographicCoverage { get { return (parameters == null) ? null : parameters.HydroMax; } }
        public double? HydrographicCoverage
        {
            get
            {
                return planet.HydrographicCoverage;
            }
            set
            {
                if (value != null)
                {
                    if (value > 1) value = 1;
                    if (value < 0) value = 0;
                }
                planet.HydrographicCoverage = value;
            }
        }


        public List<Installation> Installations { get { return planet.Installations; } }

        internal PlanetFactory(Setting s, IPlanetCreator rnd, IPlanetCreator usr)
        {
            planet = new Planet(s);
            randomiser = rnd;
            userInput = usr;
        }
        internal PlanetFactory(Planet p, IPlanetCreator rnd, IPlanetCreator usr)
        {
            planet = p;
            randomiser = rnd;
            userInput = usr;
        }

        public void SelectInstallation(string instType)
        {
            List<Installation>? lst = userInput.GetInstallation(planet, instType);
            if (lst != null)
            {
                clearInstallations(instType);
                addInstallations(lst);
            }
        }
        public void RandomInstallation(string instType)
        {
            List<Installation>? lst = randomiser.GetInstallation(planet, instType);
            if (lst != null)
            {
                clearInstallations(instType);
                addInstallations(lst);
            }
        }
        private void clearInstallations(string instType)
        {
            for (int i = planet.Installations.Count - 1; i >= 0; i--)
                if (planet.Installations[i].Type == instType)
                    planet.Installations.RemoveAt(i);
        }
        private void addInstallations(List<Installation> newInst)
        {
            foreach (Installation inst in newInst)
                planet.Installations.Add(inst);
        }

        public void SelectParameter(string param)
        {
            SetParameter(param, userInput);
        }
        public void RandomParameter(string param)
        {
            SetParameter(param, randomiser);
        }

        private void SetParameter(string param, IPlanetCreator pc)
        {
            switch (param)
            {
                case "Name":
                    SetName(pc);
                    break;

                case "Type":
                    SetType(pc);
                    break;

                case "ResourceValueCategory":
                    SetResourceValueCategory(pc);
                    break;

                case "AtmosphericMass":
                    SetAtmosphericMass(pc);
                    break;

                case "AtmosphericConditions":
                    SetAtmosphericConditions(pc);
                    break;

                case "HydrographicCoverage":
                    SetHydrographicCoverage(pc);
                    break;

                case "AverageSurfaceTempK":
                    int? tempK = pc.GetAverageSurfaceTempK(planet);
                    if (tempK != null)
                        planet.AverageSurfaceTempK = tempK ?? 0;
                    break;

                case "Density":
                    double? density = pc.GetDensity(planet);
                    if (density != null)
                        planet.Density = density ?? 0;
                    break;

                case "Gravity":
                    double? grav = pc.GetGravity(planet);
                    if (grav != null)
                        planet.Gravity = grav ?? 0;
                    break;

                case "SettlementType":
                    eSettlementType? settType;
                    int? colonyAge;
                    bool? interstellar;
                    (settType, colonyAge, interstellar) = pc.GetSettlementType(planet);
                    if (settType != null)
                    {
                        planet.SettlementType = settType ?? eSettlementType.None;
                        planet.ColonyAge = colonyAge ?? 0;
                        planet.Interstellar = interstellar ?? true;
                    }
                    break;

                case "Species":
                    Species? s = pc.GetLocalSpecies(planet);
                    if (s != null)
                        planet.LocalSpecies = s;
                    break;

                case "TechLevel":
                    int? tl;
                    eTechLevelRelativity? adj;
                    (tl, adj) = pc.GetLocalTechLevel(planet);
                    if (tl != null)
                        planet.LocalTechLevel = tl ?? 0;
                    if (adj != null)
                        planet.LocalTechLevelRelativity = adj ?? eTechLevelRelativity.Normal;
                    break;

                case "Population":
                    double? pop = pc.GetPopulation(planet);
                    if (pop != null)
                        planet.Population = pop ?? 0;
                    break;

                case "WorldGovernance":
                    eWorldUnityLevel? unity;
                    fGovernmentSpecialConditions? specCond;
                    (unity, specCond) = pc.GetWorldGovernance(planet);
                    if (unity != null)
                        planet.WorldUnityLevel = unity ?? eWorldUnityLevel.Diffuse;
                    if (specCond != null)
                        planet.GovernmentSpecialConditions = specCond ?? fGovernmentSpecialConditions.None;
                    break;

                case "SocietyType":
                    eSocietyType? soc = pc.GetSocietyType(planet);
                    if (soc != null)
                        planet.SocietyType = soc ?? eSocietyType.Anarchy;
                    break;
                case "ControlRating":
                    int? cr = pc.GetControlRating(planet);
                    if (cr != null)
                        planet.ControlRating = cr ?? 0;
                    break;
                case "TradeVolume":
                    double? trade = pc.GetTradeVolume(planet);
                    if (trade != null)
                        planet.TradeVolume = trade ?? 0;
                    break;
                case "SpaceportClass":
                    int? spacepostClass = pc.GetSpaceportClass(planet);
                    if (spacepostClass != null)
                        planet.SpaceportClass = spacepostClass ?? 0;
                    break;
            }
        }

        private void SetName(IPlanetCreator pc)
        {
            string? name = pc.GetName(planet);
            if (name != null)
                planet.Name = name;
        }

        private void SetType(IPlanetCreator pc)
        {
            eSize? size;
            eSubtype? subtype;
            (size, subtype) = pc.GetSizeAndSubtype(planet);
            bool change = false;
            if (size != null)
            {
                if (planet.Size != size)
                    change = true;
                planet.Size = size;
            }
            if (subtype != null)
            {
                if (planet.Subtype != subtype)
                    change = true;
                planet.Subtype = subtype;
            }
            if (change)
                PlanetTypeChanged();
        }
        private void PlanetTypeChanged()
        {

            if (RuleBook.PlanetParams.ContainsKey((planet.SizeVal, planet.SubtypeVal)))
                parameters = RuleBook.PlanetParams[(planet.SizeVal, planet.SubtypeVal)];
            else
                parameters = null;

            // want to basically refresh almost everything
            // rather than going through everything nulling it
            // just save the few parameters we want to keep and then get a new planet instance

            string? name = planet.Name;
            string? desc = planet.Description;
            eSize? size = planet.Size;
            eSubtype? subtype = planet.Subtype;

            planet = new Planet(planet.Setting);
            planet.Name = name;
            planet.Description = desc;
            planet.Size = size;
            planet.Subtype = subtype;

            // then check for any parameters where there's no choices
            // or set numerics to the mid-point
            if (parameters!=null)
            {
                if (HasAtmosphericOptions!=null && HasAtmosphericOptions == false)
                    (planet.AtmosphericConditions, planet.AtmosphericDescription) = RuleBook.PlanetParams[(planet.SizeVal, planet.SubtypeVal)].AtmosphereA;
                if (HasAtmosphere == true)
                    planet.AtmosphericMass = 1;

                if (HasLiquid == true)
                    planet.HydrographicCoverage = (MinimumHydrographicCoverage + MaximumHydrographicCoverage) / 2;
            }

        }

        private void CheckRanges()
        {
            if (planet.HydrographicCoverage < MinimumHydrographicCoverage)
                planet.HydrographicCoverage = MinimumHydrographicCoverage;
            if (planet.HydrographicCoverage > MaximumHydrographicCoverage)
                planet.HydrographicCoverage = MaximumHydrographicCoverage;
        }

        private void SetResourceValueCategory(IPlanetCreator pc)
        {
            eResourceValueCategory? res = pc.GetResourceValueCategory(planet);
            if (res != null)
                planet.ResourceValueCategory = res;
        }

        private void SetAtmosphericMass(IPlanetCreator pc)
        {
            double? atmMass = pc.GetAtmosphericMass(planet);
            if (atmMass != null)
                planet.AtmosphericMass = atmMass;
        }

        private void SetAtmosphericConditions(IPlanetCreator pc)
        {
            fAtmosphericConditions? cond;
            string? condDesc;
            (cond, condDesc) = pc.GetAtmosphericConditions(planet);
            if (cond != null)
                planet.AtmosphericConditions = cond;
            if (condDesc != null)
                planet.AtmosphericDescription = condDesc;
        }

        private void SetHydrographicCoverage(IPlanetCreator pc)
        {
            double? hydro = pc.GetHydrographicCoverage(planet);
            if (hydro != null)
                planet.HydrographicCoverage = hydro;
            CheckRanges();
        }


    }
}
