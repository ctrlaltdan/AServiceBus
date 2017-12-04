namespace AServiceBus.Core
{
    public interface IConfigureDependencies
    {
        IConfigureDependencies Add<TConcrete>() where TConcrete : class;
        IConfigureDependencies Add<TInstance>(TInstance instance) where TInstance : class;
        IConfigureDependencies Add<TInterface, TConcrete>() where TConcrete : TInterface;
        IConfigureDependencies AddSingleton<TConcrete>() where TConcrete : class;
        IConfigureDependencies AddSingleton<TInterface, TConcrete>() where TConcrete : TInterface;
        IResolver Build();
    }
}
