using EMS.Samples;
using Xunit;

namespace EMS.EntityFrameworkCore.Applications;

[Collection(EMSTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<EMSEntityFrameworkCoreTestModule>
{

}
