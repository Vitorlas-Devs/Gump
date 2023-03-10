namespace Gump.Data.Tests.Repositories;

public class PartnerRepositoryTests : RepositoryTestsBase, IClassFixture<RepositoryTestsBase>
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

		// PartnerModel partner = new PartnerModel
		// {
		// 	Name = name,
		// 	ContactUrl = new Uri("http://www.g.hu"),
		// 	ApiUrl = new Uri("http://www.g.hu")
		// };

		PartnerModel partner = Get<PartnerModel>();

		partner.Name = name;

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
		const string name = "Első";

		PartnerModel partner = new PartnerModel
		{
			Name = name,
			ContactUrl = new Uri("http://www.g.hu"),
			ApiUrl = new Uri("http://www.g.hu")
		};

		fixture.PartnerRepository.Create(partner);

		const string name2 = "Második";

		PartnerModel partner2 = new PartnerModel
		{
			Name = name2,
			ContactUrl = new Uri("http://www.g.hu"),
			ApiUrl = new Uri("http://www.g.hu")
		};

		fixture.PartnerRepository.Create(partner2);

		// Act
		partner2.Name = "Első";

		// Assert
		//Assert.Equal("Első", partner2.Name);
		Assert.Throws<ArgumentException>(() => fixture.PartnerRepository.Update(partner2));
	}

	[Fact]
	public void Delete_WithNoAds_AlreadyDeletedPartnerCannotBeDeleted()
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

	[Fact]
	public void Delete_WithAds_AlreadyDeletedPartnerCannotBeDeleted()
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

		ImageModel image = new ImageModel
		{
			Image = "fsfsf"
		};

		fixture.ImageRepository.Create(image);

		AdvertModel advert = new AdvertModel
		{
			Title = "Test",
			ImageId = 1,
			PartnerId = 1
		};


		fixture.AdvertRepository.Create(advert);

		// Act
		fixture.PartnerRepository.Delete(partner.Id);

		// Assert
		Assert.Throws<ArgumentNullException>(() => fixture.PartnerRepository.Delete(partner.Id));
	}

}
