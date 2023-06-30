using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GurpsSpace.PlanetCreation
{
    public class PlanetFactory
    {
        private Planet planet;

        private IPlanetCreator randomiser;
        private IPlanetCreator userInput;

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
                    string? name = pc.GetName(planet);
                    if (name != null)
                        planet.Name = name;
                    break;

                case "Type":
                    eSize? size;
                    eSubtype? subtype;
                    (size, subtype) = pc.GetSizeAndSubtype(planet);
                    if (size != null)
                        planet.Size = size ?? eSize.None;
                    if (subtype != null)
                        planet.Subtype = subtype ?? eSubtype.None;
                    break;

                case "ResourceValueCategory":
                    eResourceValueCategory? res = pc.GetResourceValueCategory(planet);
                    if (res != null)
                        planet.ResourceValueCategory = res ?? eResourceValueCategory.Average;
                    break;

                case "AtmosphericMass":
                    double? atmMass = pc.GetAtmosphericMass(planet);
                    if (atmMass != null)
                        planet.AtmosphericMass = atmMass ?? 0;
                    break;

                case "AtmosphericConditions":
                    fAtmosphericConditions? cond;
                    string? condDesc;
                    (cond, condDesc) = pc.GetAtmosphericConditions(planet);
                    if (cond != null)
                        planet.AtmosphericConditions = cond ?? fAtmosphericConditions.None;
                    if (condDesc != null)
                        planet.AtmosphericDescription = condDesc ?? "tbc";
                    break;

                case "HydrographicCoverage":
                    double? hydro = pc.GetHydrographicCoverage(planet);
                    if (hydro != null)
                        planet.HydrographicCoverage = hydro ?? 0;
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


    }
}
