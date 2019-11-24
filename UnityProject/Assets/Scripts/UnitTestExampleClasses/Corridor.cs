using Assets.Scripts.UnitTestExampleDeps;
using Assets.Scripts.UnitTestExampleDeps2;

namespace Assets.Scripts.UnitTestExampleClasses
{
    public class Corridor
    {
        private readonly ISomeDependency _someDependency;
        private readonly ISomeDependency _someDependency2;
        private readonly ISomeOther _someOther;

        public Corridor(ISomeDependency someDependency, ISomeDependency someDependency2, ISomeOther someOther)
        {
            _someDependency = someDependency;
            _someDependency2 = someDependency2;
            _someOther = someOther;
        }

        public int CorridorMoney()
        {
            return _someDependency.GetMoney();
        }

        public void JustDoIt()
        {

        }

        public bool IsTheTruth()
        {
            return !_someOther.IsOther();
        }
    }
}