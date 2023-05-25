
namespace GurpsSpace.PlanetCreation
{
    public class Installation
    {
        private string name; public string Name { get { return name; } }
        private int pr; public int PR { get { return pr; } }
        public string PRstring { get { return (PR == 0) ? "n/a" : PR.ToString("N0"); } }


        public Installation(string name, int pr)
        {
            this.name = name;
            this.pr = pr;
        }   
    }
}
