using Volo.Abp.Modularity;

namespace EMS;

/* Inherit from this class for your domain layer tests. */
public abstract class EMSDomainTestBase<TStartupModule> : EMSTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
