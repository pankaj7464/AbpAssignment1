using EMS.Samples;
using Xunit;

namespace EMS.EntityFrameworkCore.Domains;

[Collection(EMSTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<EMSEntityFrameworkCoreTestModule>
{

}
