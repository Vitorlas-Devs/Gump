using Moq;
using Xunit;

namespace Gump.Data.Tests.Repositories;

public class AdvertRepositoryTests : RepositoryBaseTests
{
	private readonly Mock<IMongoCollection<AdvertModel>> _collectionMock;
	private readonly AdvertRepository _repository;

	public AdvertRepositoryTests()
	{
		_collectionMock = new Mock<IMongoCollection<AdvertModel>>();
		_repository = new AdvertRepository("mongodb://localhost:27017");
	}

	[Fact]
	public void Create_WhenPartnerNameIsUnique_ShouldInsertPartner()
	{
		// Arrange
		var advert = new AdvertModel
		{
			PartnerId = 1,
			ImageId = 1,
			Title = "dfsdfs"
		};

		_collectionMock.Setup(x => x.InsertOne(advert, null, default(CancellationToken)))
			.Verifiable();

			_collectionMock.Setup(x => x.Find(_ => true, null, default))
                .Returns(new List<AdvertModel>().AsQueryable());
		// Act
		var result = _repository.Create(advert);

		// Assert
		Assert.NotNull(result);
		Assert.Equal(advert.PartnerId, result.PartnerId);
		Assert.Equal(advert.ImageId, result.ImageId);
		Assert.Equal(advert.Title, result.Title);
		_collectionMock.Verify();
	}
}
