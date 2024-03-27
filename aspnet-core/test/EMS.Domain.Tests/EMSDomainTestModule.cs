using Volo.Abp.Modularity;

namespace EMS;

[DependsOn(
    typeof(EMSDomainModule),
    typeof(EMSTestBaseModule)
)]
public class EMSDomainTestModule : AbpModule
{

}
