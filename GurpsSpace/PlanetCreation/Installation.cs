﻿
namespace GurpsSpace.PlanetCreation
{
    public class Installation
    {
        private string type; public string Type { get { return type; } }
        private string subtype; public string Subtype { get { return subtype; } }
        public string Name { get { return type + ((Subtype == "") ? "" : " (" + subtype + ")"); } }
        private int pr; public int PR { get { return pr; } }


        public Installation(string type, string subtype, int pr)
        {
            this.type = type;
            this.subtype = subtype;
            this.pr = pr;
        }   
    }
}
