namespace Gump.Data.Tests.Repositories;

public class PartnerRepositoryTests : IClassFixture<RepositoryTestsBase>
{
	private readonly RepositoryTestsBase fixture;
	public PartnerRepositoryTests(RepositoryTestsBase fixture)
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
		fixture.PartnerRepository.Create(partner);

		// Assert
		Assert.Equal(name, partner.Name);
		Assert.Throws<ArgumentException>(() => fixture.PartnerRepository.Create(partner));
	}

	[Fact]
	public void Update()
	{
		// Arrange
		const string name = "Test";

		PartnerModel partner = new PartnerModel
		{
			Name = name,
			ContactUrl = new Uri("http://www.g.hu"),
			ApiUrl = new Uri("http://www.g.hu")
		};

		fixture.PartnerRepository.Create(partner);

		// Act
		partner.Name = "Test2";
		fixture.PartnerRepository.Update(partner);

		// Assert
		Assert.Equal("Test2", partner.Name);
		Assert.Throws<ArgumentException>(() => fixture.PartnerRepository.Update(partner));
	}

	[Fact]
	public void Delete()
	{
		// Arrange
		const string name = "Test";

		PartnerModel partner = new PartnerModel
		{
			Name = name,
			ContactUrl = new Uri("http://www.g.hu"),
			ApiUrl = new Uri("http://www.g.hu"),
		};

		fixture.PartnerRepository.Create(partner);

		// Act
		fixture.PartnerRepository.Delete(partner.Id);

		// Assert
		Assert.Throws<ArgumentNullException>(() => fixture.PartnerRepository.Delete(partner.Id));
	}

}
