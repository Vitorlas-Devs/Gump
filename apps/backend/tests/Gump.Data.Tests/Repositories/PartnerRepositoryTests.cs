namespace Gump.Data.Tests.Repositories;

public class PartnerRepositoryTests : IClassFixture<RepositoryTestsBase<PartnerRepository>>
{
	private readonly RepositoryTestsBase<PartnerRepository> fixture;
	public PartnerRepositoryTests(RepositoryTestsBase<PartnerRepository> fixture) 
	{
		this.fixture = fixture;
	}

	[Fact]
	public void Create()
	{
		// Arrange
		const string name = "Test";
		
		PartnerModel partner = new PartnerModel
		{
			Name = name,
			ContactUrl = new Uri("http://www.g.hu"),
			ApiUrl = new Uri("http://www.g.hu")
		};

		// Act
		fixture.Repository.Create(partner);

		// Assert
		Assert.Equal(name, partner.Name);
		Assert.Throws<ArgumentException>(() => fixture.Repository.Create(partner));
	}

}
