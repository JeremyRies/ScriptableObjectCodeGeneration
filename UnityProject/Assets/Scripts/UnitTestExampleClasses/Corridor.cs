namespace MyNamespace
{


    public class Corridor
    {
        private readonly ISomeDependency _someDependency;
        private readonly ISomeDependency _someDependency2;

        public Corridor(ISomeDependency someDependency, ISomeDependency someDependency2, ISomeDependency dep3)
        {
            _someDependency = someDependency;
            _someDependency2 = someDependency2;
        }

        public int CorridorMoney()
        {
            return _someDependency.GetMoney();
        }

        public void JustDoIt()
        {

        }
    }

}