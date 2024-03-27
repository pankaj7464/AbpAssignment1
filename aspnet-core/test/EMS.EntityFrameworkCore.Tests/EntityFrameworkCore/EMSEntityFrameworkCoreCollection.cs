using Xunit;

namespace EMS.EntityFrameworkCore;

[CollectionDefinition(EMSTestConsts.CollectionDefinitionName)]
public class EMSEntityFrameworkCoreCollection : ICollectionFixture<EMSEntityFrameworkCoreFixture>
{

}
