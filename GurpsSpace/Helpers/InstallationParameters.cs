using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace GurpsSpace
{
    internal class InstallationParameters
    {
        public readonly IndexedList<string> Names;
        public string Name { get { return Names[offset]; } }

        // for target number
        private int targetBase;
        private int targetPrMult;
        private int targetCrMult;

        // for checking if a valid installation
        private int minPR;
        private int minTL;

        // for installation population
        private int populationDice;
        public int PopulationDice { get { return populationDice; } }
        public readonly IndexedList<int> PopulationAdjs;
        public int PopulationAdj
        {
            get
            {
                if (PopulationAdjs == null || PopulationAdjs.Count == 0)
                    return 0;
                else
                    return PopulationAdjs[offset];
            }
        }
        private int populationRangeMin; public int PopulationRangeMin { get { return populationRangeMin; } }
        private int populationRangeMax; public int PopulationRangeMax { get { return populationRangeMax; } }

        // for where an installation goes with a spaceport
        private int withSpaceportLevel;

        // where there may be multiple options (currently just names and populations)
        private int offset;
        public int Offset { get { return offset; } }
        public int MaxOffset { get { return Names.Count; } }
        private IndexedList<int> weights;
        public int MaxWeight
        {
            get
            {
                int totw = 0;
                for (int i=0;i< weights.Count;i++)
                    totw += weights[i];
                return totw;
            }
        }
        // list is: weight, name, extrapopadj

        // for multiple installations of the same type
        private int maxCount;
        public int MaxCount { get { return maxCount; } }
        private int targetAdjPerInstallation;
        //public int TargetAdjPerInstallation { get { return targetAdjPerInstallation;} }
        private InstallationParameters? secondInstallation;
        public InstallationParameters? SecondInstallation { get { return secondInstallation; } }
        public bool HasSecond { get { return (secondInstallation != null); } }

        private InstallationParameters(string name, int targetBase, int targetPrMult, int targetCrMult)
        {
            Names = new IndexedList<string>();
            PopulationAdjs = new IndexedList<int>();
            weights = new IndexedList<int>();

            Names.Add(name);
            this.targetBase = targetBase;
            this.targetPrMult = targetPrMult;
            this.targetCrMult = targetCrMult;
            this.maxCount = 1;
            weights.Add(1);
        }

        public static InstallationParameters Create(string name, int targetBase, int targetPrMult, int targetCrMult)
        {
            return new InstallationParameters(name, targetBase, targetPrMult, targetCrMult);
        }
        public InstallationParameters SetMinPR(int minPR)
        {
            this.minPR = minPR;
            return this;
        }
        public InstallationParameters SetMinTL(int minTL)
        {
            this.minTL = minTL;
            return this;
        }
        public InstallationParameters SetPopDice(int popDice, int popAdj)
        {
            PopulationAdjs.Clear();
            populationDice = popDice;
            PopulationAdjs.Add(popAdj);
            return this;
        }
        public InstallationParameters SetPopRange(int popMin, int  popMax)
        {
            populationRangeMin = popMin;
            populationRangeMax = popMax;
            return this;
        }
        public InstallationParameters SetWithSpaceportLevel(int lvl)
        {
            withSpaceportLevel = lvl;
            return this;
        }
        public InstallationParameters HasOptions()
        {
            Names.Clear();
            PopulationAdjs.Clear();
            weights.Clear();
            return this;
        }
        public InstallationParameters AddOption(int weight, string name, int popAdj)
        {
            Names.Add(name);
            PopulationAdjs.Add(popAdj);
            weights.Add(weight);
            return this;
        }
        public InstallationParameters SetMultipleTargetAdj(int maxCount, int targetAdjPerInstallation)
        {
            this.maxCount = maxCount;
            this.targetAdjPerInstallation = targetAdjPerInstallation;
            return this;
        }
        public InstallationParameters SetSecondInstallation(InstallationParameters secondInstall)
        {
            this.secondInstallation = secondInstall;
            return this;
        }

        public bool IsValidInstallation(Planet p)
        {
            return ((p.PopulationRating >= minPR) && (p.LocalTechLevel >= minTL));
        }
        public int TargetNumber(Planet p, int count)
        {
            int targ = targetBase + targetCrMult * p.ControlRating + targetPrMult * p.PopulationRating;
            targ += count * targetAdjPerInstallation;

            return targ;
        }
        public int MinimumPopulationRating()
        {
            // use the smallest populationAdj
            int minAdj = int.MaxValue;
            for (int i = 0; i < PopulationAdjs.Count; i++)
                if (PopulationAdjs[i] < minAdj)
                    minAdj = PopulationAdjs[i];
            return populationDice + minAdj + populationRangeMin;
        }
        public int MaximumPopulationRating()
        {
            // use the largest populationAdj
            int maxAdj = 0;
            for (int i = 0; i < PopulationAdjs.Count; i++)
                if (PopulationAdjs[i] > maxAdj)
                    maxAdj = PopulationAdjs[i];
            return 6 * populationDice + maxAdj + populationRangeMax;
        }
        public void SetOffset(int i)
        {
            if (i < 0)
                i = 0;
            if (i >= Names.Count)
                i = Names.Count - 1;
            offset = i;
        }
        public void SetWeight(int w)
        {
            if (w < 0)
                w = 0;
            if (w > MaxWeight)
                w = MaxWeight;

            int i = 0;
            while (w > weights[i])
            {
                w -= weights[i];
                i++;
            }
            offset = i;
        }
    }
}
