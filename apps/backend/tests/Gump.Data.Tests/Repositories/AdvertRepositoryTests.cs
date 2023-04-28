namespace Gump.Data.Tests.Repositories;

[Collection("RepositoryTests")]
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
		PartnerModel partner = Get<PartnerModel>();
		fixture.PartnerRepository.Create(partner);

		ImageModel image = Get<ImageModel>();
		fixture.ImageRepository.Create(image);

		AdvertModel advert = Get<AdvertModel>();
		fixture.AdvertRepository.Create(advert);

		AdvertModel advert2 = fixture.AdvertRepository.GetById(advert.Id);

		// Assert
		Assert.Equal(advert.Id, advert2.Id);
		Assert.Equal(advert.Title, advert2.Title);
	}

	[Theory]
	[InlineData("First")]
	public void Create_CheckIfEqual(string name)
	{
		// Arrange
		PartnerModel partner = Get<PartnerModel>();
		fixture.PartnerRepository.Create(partner);

		ImageModel image = Get<ImageModel>();
		fixture.ImageRepository.Create(image);

		AdvertModel advert = Get<AdvertModel>();

		// Act
		advert.Title = name;
		fixture.AdvertRepository.Create(advert);

		// Assert
		Assert.Equal(name, advert.Title);
		Assert.Equal(advert, AdvertRepository.Create(advert));
	}

	[Theory]
	[InlineData("First", "Second")]
	public void Update_UpdateNameWorks(string name, string name2)
	{
		// Arrange
		PartnerModel partner = Get<PartnerModel>();
		fixture.PartnerRepository.Create(partner);

		ImageModel image = Get<ImageModel>();
		fixture.ImageRepository.Create(image);

		AdvertModel advert = Get<AdvertModel>();


		// Act
		advert.Title = name;
		fixture.AdvertRepository.Create(advert);

		advert.Title = name2;

		// Assert
		Assert.Equal(name2, advert.Title);
		Assert.Equal(advert, AdvertRepository.Update(advert));
	}

	[Theory]
	[InlineData("First")]
	public void Delete_Works(string name)
	{
		// Arrange
		PartnerModel partner = Get<PartnerModel>();
		fixture.PartnerRepository.Create(partner);

		ImageModel image = Get<ImageModel>();
		fixture.ImageRepository.Create(image);

		AdvertModel advert = Get<AdvertModel>();
		advert.Title = name;
		fixture.AdvertRepository.Create(advert);

		// Act
		fixture.AdvertRepository.Delete(advert.Id);

		// Assert
		Assert.Throws<NotFoundException>(() => fixture.AdvertRepository.GetById(advert.Id));
		Assert.DoesNotContain(advert.Id, fixture.PartnerRepository.GetById(advert.PartnerId).Ads);
	}

	[Theory]
	[InlineData("First")]
	public void Delete_CannotDeleteNonExistent(string name)
	{
		// Arrange
		PartnerModel partner = Get<PartnerModel>();
		fixture.PartnerRepository.Create(partner);

		ImageModel image = Get<ImageModel>();
		fixture.ImageRepository.Create(image);

		AdvertModel advert = Get<AdvertModel>();
		advert.Title = name;
		fixture.AdvertRepository.Create(advert);

		// Act
		fixture.AdvertRepository.Delete(advert.Id);

		// Assert
		Assert.Throws<NotFoundException>(() => fixture.AdvertRepository.Delete(advert.Id));
	}
}
