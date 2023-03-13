namespace Gump.Data.Tests.Repositories;

public class AdvertRepositoryTests : RepositoryTestsBase, IClassFixture<RepositoryTestsBase>
{
	private readonly RepositoryTestsBase fixture;

	public AdvertRepositoryTests(RepositoryTestsBase fixture)
	{
		this.fixture = fixture;
	}

	[Fact]
	public void GetById_Works()
	{
		// Arrange & Act
		AdvertModel advert = Get<AdvertModel>();
		fixture.AdvertRepository.Create(advert);
		AdvertModel advert2 = fixture.AdvertRepository.GetById(advert.Id);

		// Assert
		Assert.Equal(advert.Id, advert2.Id);
		Assert.Equal(advert.Title, advert2.Title);
	}
}
