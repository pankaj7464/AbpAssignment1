using Volo.Abp.Modularity;

namespace EMS;

public abstract class EMSApplicationTestBase<TStartupModule> : EMSTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
