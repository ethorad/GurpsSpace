
namespace GurpsSpace.PlanetCreation
{
    internal class Installation
    {
        private string name; public string Name { get { return name; } }
        private int pr; public int PR { get { return pr; } }

        public Installation(string name, int pr)
        {
            this.name = name;
            this.pr = pr;
        }   
    }
}
