public class Corridor
{
    private readonly ISomeDependency _someDependency;

    public Corridor(ISomeDependency someDependency)
    {
        _someDependency = someDependency;
    }
}
