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
        public PlanetParameters? Parameters { get { return parameters; } }

        public Setting Setting { get { return planet.Setting; } }

        private IPlanetCreator randomiser;
        private IPlanetCreator userInput;

        public string? Name { get { return planet.Name; } set { planet.Name = value; } }
        public eSize? Size { get { return planet.Size; } }
        public eSubtype? Subtype { get { return planet.Subtype; } }
        public eOverallType? OverallType { get { return planet.OverallType; } }
        public eResourceValueCategory? ResourceValueCategory { get { return planet.ResourceValueCategory; } }
        public int? ResourceValueModifier { get { return planet.ResourceValueModifier; } }
        public string? Description { get { return planet.Description; } set { planet.Description = value; } }
        public bool? IsPlanet { get { return planet.IsPlanet; } }

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

        public bool? HasHydrosphere
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
        public eLiquid? LiquidType { get { return planet.LiquidType; } }

        public int? MinSurfaceTemperatureK { get { return (parameters == null) ? null : parameters.MinSurfaceTemperatureK; } }
        public int? MaxSurfaceTemperatureK { get { return (parameters == null) ? null : parameters.MaxSurfaceTemperatureK; } }
        public int? StepSurfaceTemperatureK { get { return (parameters == null) ? null : parameters.StepSurfaceTemperatureK; } }
        public int? AverageSurfaceTemperatureK { get { return planet.AverageSurfaceTemperatureK; } set { planet.AverageSurfaceTemperatureK = value; } }
        public eClimateType? ClimateType { get { return planet.ClimateType; } }
        public int? BlackbodyTemperatureK { get { return planet.BlackbodyTemperatureK; } }

        public bool? HasLithosphere
        {
            get
            {
                if (planet.Size == null || planet.Size == eSize.None)
                    return null;
                else if (planet.Size == eSize.AsteroidBelt)
                    return false;
                else
                    return true;
            }
        }
        public eCoreType? CoreType { get { return planet.CoreType; } }
        public double? MinDensity
        {
            get
            {
                switch (CoreType ?? eCoreType.None) // so falls to default if null
                {
                    case eCoreType.Icy:
                        return RuleBook.DensityIcyCore[0];
                    case eCoreType.SmallIron:
                        return RuleBook.DensitySmallIronCore[0];
                    case eCoreType.LargeIron:
                        return RuleBook.DensityLargeIronCore[0];
                    default:
                        return null;
                }
            }
        }
        public double? MaxDensity
        {
            get
            {
                switch (CoreType ?? eCoreType.None) // so falls to default if null
                {
                    case eCoreType.Icy:
                        return RuleBook.DensityIcyCore[20];
                    case eCoreType.SmallIron:
                        return RuleBook.DensitySmallIronCore[20];
                    case eCoreType.LargeIron:
                        return RuleBook.DensityLargeIronCore[20];
                    default:
                        return null;
                }
            }
        }
        public double? Density { get { return planet.Density; } set { planet.Density = value; } }
        public double? MinGravity
        {
            get
            {
                // check for values we need
                if (parameters == null || planet.BlackbodyTemperatureK == null || planet.Density == null)
                    return null;
                // so can now use ?? on all nullable values

                double minSizeFactor = (parameters == null) ? 0 : parameters.MinSizeFactor;
                double minG = Math.Sqrt((double)(planet.BlackbodyTemperatureK ?? 0) * (Density ?? 0)) * minSizeFactor;
                return Math.Round(minG, 2);
            }
        }
        public double? MaxGravity
        {
            get
            {
                // check for values we need
                if (parameters == null || planet.BlackbodyTemperatureK == null || planet.Density == null)
                    return null;
                // so can now use ?? on all nullable values

                double maxSizeFactor = (parameters == null) ? 0 : parameters.MaxSizeFactor;
                double maxG = Math.Sqrt((double)(planet.BlackbodyTemperatureK ?? 0) * (Density ?? 0)) * maxSizeFactor;
                return Math.Round(maxG, 2);
            }
        }
        public double? Gravity { get { return planet.Gravity; } set { planet.Gravity = value; } }
        public double? DiameterEarths { get { return planet.DiameterEarths; } }
        public double? DiameterMiles { get { return planet.DiameterMiles; } }
        public double? Mass { get { return planet.Mass; } }

        public eSettlementType? SettlementType
        {
            get { return planet.SettlementType; }
            set
            {
                planet.SettlementType = value;
                if (planet.SettlementType == null || planet.SettlementType == eSettlementType.None)
                {
                    planet.LocalSpecies = null;
                    planet.LocalTechLevel = null;
                    planet.LocalTechLevelRelativity = null;
                }
            }
        }
        public int? ColonyAge { get { return planet.ColonyAge; } }
        public bool? Interstellar { get { return planet.Interstellar; } }
        public bool? HasSettlement { get { return planet.HasSettlement; } }
        public Species? LocalSpecies { get { return planet.LocalSpecies; } }
        public int? Habitability { get { return planet.Habitability; } }
        public int? AffinityScore { get { return planet.AffinityScore; } }
        public int? LocalTechLevel { get { return planet.LocalTechLevel; } }
        public string? LocalTechLevelAge { get { return planet.LocalTechLevelAge; } }
        public eTechLevelRelativity? LocalTechLevelRelativity { get { return planet.LocalTechLevelRelativity; } }

        public double? CarryingCapacity { get { return planet.CarryingCapacity; } }
        public double? Population { get { return planet.Population; } }
        public int? PopulationRating { get { return planet.PopulationRating; } }

        public eWorldUnityLevel? WorldUnityLevel { get { return planet.WorldUnityLevel; } }
        public eSocietyType? SocietyType { get { return planet.SocietyType; } }
        public fGovernmentSpecialConditions? GovernmentSpecialConditions { get { return planet.GovernmentSpecialConditions; } }
        public bool HasGovernmentSpecialCondition(fGovernmentSpecialConditions cond)
        {
            return planet.HasGovernmentSpecialCondition(cond);
        }
        public int? ControlRating { get { return planet.ControlRating; } }

        public int? IncomePerCapita { get { return planet.IncomePerCapita; } }
        public eWealthLevel? WealthLevel { get { return planet.WealthLevel; } }
        public double? EconomicVolume { get { return planet.EconomicVolume; } }
        public double? TradeVolume { get { return planet.TradeVolume; } }

        public int? SpaceportClass { get { return planet.SpaceportClass; } }
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
            List<Installation>? lst = userInput.GetInstallation(this, instType);
            if (lst != null)
            {
                clearInstallations(instType);
                addInstallations(lst);
            }
        }
        public void RandomInstallation(string instType)
        {
            List<Installation>? lst = randomiser.GetInstallation(this, instType);
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
                    SetAverageSurfaceTemperatureK(pc);
                    break;

                case "Density":
                    SetDensity(pc);
                    break;

                case "Gravity":
                    SetGravity(pc);
                    break;

                case "SettlementType":
                    SetSettlementType(pc);
                    break;

                case "Species":
                    SetLocalSpecies(pc);
                    break;

                case "TechLevel":
                    SetLocalTechLevel(pc);
                    break;

                case "Population":
                    SetPopulation(pc);
                    break;

                case "WorldGovernance":
                    SetWorldGovernance(pc);
                    break;

                case "SocietyType":
                    SetSocietyType(pc);
                    break;

                case "ControlRating":
                    SetControlRating(pc);
                    break;

                case "TradeVolume":
                    SetTradeVolume(pc);
                    break;

                case "SpaceportClass":
                    SetSpaceportClass(pc);
                    break;

                case "Installations":
                    SetInstallations(pc);
                    break;
            }
        }

        private void SetName(IPlanetCreator pc)
        {
            string? name = pc.GetName(this);
            if (name != null)
                planet.Name = name;
        }

        private void SetType(IPlanetCreator pc)
        {
            eSize? size;
            eSubtype? subtype;
            (size, subtype) = pc.GetSizeAndSubtype(this);
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
                planet.CoreType = parameters.CoreType;

                if (HasAtmosphericOptions!=null && HasAtmosphericOptions == false)
                    (planet.AtmosphericConditions, planet.AtmosphericDescription) = RuleBook.PlanetParams[(planet.SizeVal, planet.SubtypeVal)].AtmosphereA;
                if (HasAtmosphere == true)
                    planet.AtmosphericMass = 1;

                if (HasHydrosphere == true)
                {
                    planet.HydrographicCoverage = (MinimumHydrographicCoverage + MaximumHydrographicCoverage) / 2;
                    planet.LiquidType = parameters.Liquid;
                }

                planet.PressureFactor = parameters.PressureFactor;

                planet.AverageSurfaceTemperatureK = (MinSurfaceTemperatureK + MaxSurfaceTemperatureK) / 2;
                planet.Density = (MinDensity+MaxDensity) / 2;
                planet.Gravity = (MaxGravity+MinGravity) / 2;
            }

        }

        private void CheckRanges()
        {
            if (planet.HydrographicCoverage < MinimumHydrographicCoverage)
                planet.HydrographicCoverage = MinimumHydrographicCoverage;
            if (planet.HydrographicCoverage > MaximumHydrographicCoverage)
                planet.HydrographicCoverage = MaximumHydrographicCoverage;

            if (planet.AverageSurfaceTemperatureK < MinSurfaceTemperatureK)
                planet.AverageSurfaceTemperatureK = MinSurfaceTemperatureK;
            if (planet.AverageSurfaceTemperatureK > MaxSurfaceTemperatureK)
                planet.AverageSurfaceTemperatureK = MaxSurfaceTemperatureK;

            if (planet.Density < MinDensity)
                planet.Density = MinDensity;
            if (planet.Density > MaxDensity)
                planet.Density = MaxDensity;

            if (planet.Gravity < MinGravity)
                planet.Gravity = MinGravity;
            if (planet.Gravity > MaxGravity)
                planet.Gravity = MaxGravity;
        }

        private void SetResourceValueCategory(IPlanetCreator pc)
        {
            eResourceValueCategory? res = pc.GetResourceValueCategory(this);
            if (res != null)
                planet.ResourceValueCategory = res;
        }

        private void SetAtmosphericMass(IPlanetCreator pc)
        {
            double? atmMass = pc.GetAtmosphericMass(this);
            if (atmMass != null)
                planet.AtmosphericMass = atmMass;
        }

        private void SetAtmosphericConditions(IPlanetCreator pc)
        {
            if (parameters == null)
                return;

            if (parameters.AtmosphereANumber > 18) // ie no choice
            {
                (planet.AtmosphericConditions, planet.AtmosphericDescription) = parameters.AtmosphereA;
                return;
            }

            fAtmosphericConditions? cond;
            string? condDesc;
            (cond, condDesc) = pc.GetAtmosphericConditions(this);
            if (cond != null)
                planet.AtmosphericConditions = cond;
            if (condDesc != null)
                planet.AtmosphericDescription = condDesc;
        }

        private void SetHydrographicCoverage(IPlanetCreator pc)
        {
            double? hydro = pc.GetHydrographicCoverage(this);
            if (hydro != null)
                planet.HydrographicCoverage = hydro;
            CheckRanges();
        }

        private void SetAverageSurfaceTemperatureK(IPlanetCreator pc)
        {
            int? tempK = pc.GetAverageSurfaceTemperatureK(this);
            if (tempK != null)
                planet.AverageSurfaceTemperatureK = tempK ?? 0;
        }

        private void SetDensity(IPlanetCreator pc)
        {
            double? density = pc.GetDensity(this);
            if (density != null)
                planet.Density = density ?? 0;
        }

        private void SetGravity(IPlanetCreator pc)
        {
            double? grav = pc.GetGravity(this);
            if (grav != null)
                planet.Gravity = grav ?? 0;
        }

        private void SetSettlementType(IPlanetCreator pc)
        {
            eSettlementType? settType;
            int? age;
            bool? stellar;
            (settType, age, stellar) = pc.GetSettlementType(this);
            if (settType != null)
            {
                bool change = false;
                if (settType != planet.SettlementType)
                    change = true;
                planet.SettlementType = settType;
                planet.ColonyAge = age;
                planet.Interstellar = stellar;

                if (change)
                {
                    planet.LocalSpecies = null;
                    planet.LocalTechLevel = null;
                    planet.LocalTechLevelRelativity = null;
                    planet.Population= null;
                    planet.WorldUnityLevel = null;
                    planet.SocietyType = null;
                    planet.GovernmentSpecialConditions = null;
                    planet.ControlRating = null;
                    Installations.Clear();

                }
            }
        }

        private void SetLocalSpecies(IPlanetCreator pc)
        {
            Species? s = pc.GetLocalSpecies(this);
            if (s != null)
                planet.LocalSpecies = s;
        }

        private void SetLocalTechLevel(IPlanetCreator pc)
        {
            int? tl;
            eTechLevelRelativity? adj;
            (tl, adj) = pc.GetLocalTechLevel(this);
            if (tl != null)
                planet.LocalTechLevel = tl ?? 0;
            if (adj != null)
                planet.LocalTechLevelRelativity = adj ?? eTechLevelRelativity.Normal;
        }

        private void SetPopulation(IPlanetCreator pc)
        {
            double? pop = pc.GetPopulation(this);
            if (pop != null)
                planet.Population = pop ?? 0;
        }

        private void SetWorldGovernance(IPlanetCreator pc)
        {
            eWorldUnityLevel? unity;
            fGovernmentSpecialConditions? specCond;
            (unity, specCond) = pc.GetWorldGovernance(this);
            if (unity != null)
                planet.WorldUnityLevel = unity ?? eWorldUnityLevel.Diffuse;
            if (specCond != null)
                planet.GovernmentSpecialConditions = specCond ?? fGovernmentSpecialConditions.None;
        }

        private void SetSocietyType(IPlanetCreator pc)
        {
            eSocietyType? soc = pc.GetSocietyType(this);
            if (soc != null)
                planet.SocietyType = soc ?? eSocietyType.Anarchy;
        }

        private void SetControlRating(IPlanetCreator pc)
        {
            int? cr = pc.GetControlRating(this);
            if (cr != null)
                planet.ControlRating = cr ?? 0;
        }

        private void SetTradeVolume(IPlanetCreator pc)
        {
            double? trade = pc.GetTradeVolume(this);
            if (trade != null)
                planet.TradeVolume = trade ?? 0;
        }

        private void SetSpaceportClass(IPlanetCreator pc)
        {
            int? spacepostClass = pc.GetSpaceportClass(this);
            if (spacepostClass != null)
                planet.SpaceportClass = spacepostClass ?? 0;
        }

        private void SetInstallations(IPlanetCreator pc)
        {
            List<Installation>? lst = pc.GetInstallations(this);
            if (lst!=null)
                planet.Installations = lst;
        }
    }
}
