using RestWithAspNet10.IntegrationTests.Tools;

namespace RestWithAspNet10.IntegrationTests.Fixtures;

[CollectionDefinition("IntegrationTests")]
public class IntegrationTestCollection :
   ICollectionFixture<SqlServerFixture>,
   ICollectionFixture<TestDatabaseFixture>
{

}