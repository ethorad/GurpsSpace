using System;

namespace GurpsSpace
{

    public enum eOverallType
    {
        None,
        Hostile,
        Barren,
        Garden
    }

    public enum eSize
    {
        None,
        AsteroidBelt,
        Tiny,
        Small,
        Standard,
        Large
    }

    public enum eSubtype
    {
        None,
        AsteroidBelt,
        Ammonia,
        Chthonian,
        Garden,
        Greenhouse,
        Hadean,
        Ice,
        Ocean,
        Rock,
        Sulphur
    }

    [Flags]
    public enum fAtmosphericConditions
    {
        None = 0,
        Suffocating = 1,
        MildlyToxic = 2,
        HighlyToxic = 4,
        LethallyToxic = 8,
        Corrosive = 16,
        Marginal = 32
    }

    public enum eLiquid
    {
        None,
        Hydrocarbons,
        Ammonia,
        Water,
        Acidic
    }

    public enum eClimateType
    {
        None,
        Frozen,
        VeryCold,
        Cold,
        Chilly,
        Cool,
        Normal,
        Warm,
        Tropical,
        Hot,
        VeryHot,
        Infernal
    }

    public enum eCoreType
    {
        None,
        Icy,
        SmallIron,
        LargeIron
    }

    public enum ePressureCategory
    {
        None,
        Trace,
        VeryThin,
        Thin,
        Standard,
        Dense,
        VeryDense,
        Superdense
    }

    public enum eResourceValueCategory
    {
        Worthless = -5,
        VeryScant = -4,
        Scant = -3,
        VeryPoor = -2,
        Poor = -1,
        Average = 0,
        Abundant = 1,
        VeryAbundant = 2,
        Rich = 3,
        VeryRich = 4,
        Motherlode = 5
    }

    public enum eSettlementType
    {
        None,
        Outpost,
        Colony,
        Homeworld
    }

    public enum eTechLevelRelativity
    {
        Delayed = -1,
        Normal = 0,
        Advanced = 1
    }

    public enum eWorldUnityLevel
    {
        Diffuse,
        Factionalised,
        Coalition,
        WorldGovernment
    }

    [Flags]
    public enum fGovernmentSpecialConditions
    {
        None = 0,
        Bureaucracy = 1,
        Colony = 2,
        Cyberocracy = 4,
        Genderarchy = 8,
        Meritocracy = 16,
        MilitaryGovernment = 32,
        Oligarchy = 64,
        Sanctuary = 128,
        Socialist = 256,
        Subjugated = 512,
        Utopia = 1024
    }

    public enum eSettingSocietyType
    {
        Alliance,
        Anarchy,
        Federation,
        CorporateState,
        Empire
    }

    public enum eSocietyType
    {
        Anarchy,
        AthenianDemocracy,
        Caste,
        ClanTribal,
        Corporate,
        Dictatorship,
        Feudal,
        Technocracy,
        Theocracy,
        RepresentativeDemocracy
    }

    public enum eWealthLevel
    {
        DeadBroke,
        Poor,
        Struggling,
        Average,
        Comfortable
    }

    public enum eSpeciesDiet
    {
        Herbivore,
        Omnivore,
        Carnivore
    }

    public enum eLifeChemistry
    {
        Hydrogen,
        Ammonia,
        Hydrocarbon,
        Water,
        Chlorine,
        SiliconSulphuricAcid,
        SiliconLiquidSulphur,
        SiliconLiquidRock,
        Plasma,
        Exotica
    }

    public enum eTrait
    {
        IncreasedConsumption,
        ReducedConsumption,
        DoesntEatOrDrink,
        DecreasedTimeRate,
        ReducedBasicSpeed,
        IncreasedBasicSpeed
    }
}
