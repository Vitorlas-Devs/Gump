namespace Gump.Data.Tests.Repositories;

[Collection("RepositoryTests")]
public class ImageRepositoryTests : RepositoryTestsBase, IClassFixture<RepositoryTestsBase>
{
	private readonly RepositoryTestsBase fixture;

	public ImageRepositoryTests(RepositoryTestsBase fixture)
	{
		this.fixture = fixture;
	}

	[Theory]
	[InlineData("First")]
	public void GetById_Works(string imageData)
	{
		// Arrange
		ImageModel image = Get<ImageModel>();

		// Act
		image.Image = imageData;
		fixture.ImageRepository.Create(image);

		ImageModel image2 = fixture.ImageRepository.GetById(image.Id);

		// Assert
		Assert.Equal(image.Id, image2.Id);
		Assert.Equal(image.Image, image2.Image);
	}

	[Fact]
	public void Create_CannotCreateWithNonExistentUser()
	{
		// Arrange
		ImageModel image = Get<ImageModel>();

		//Act
		image.OwnerId = 1;

		// Assert
		Assert.Throws<NotFoundException>(() => fixture.ImageRepository.Create(image));
	}

	[Fact]
	public void Create_CannotCreateWithNonExistentImage()
	{
		// Arrange
		ImageModel image = Get<ImageModel>();

		// Act
		image.Image = null;

		// Assert
		Assert.Throws<RestrictedException>(() => fixture.ImageRepository.Create(image));
	}

	[Theory]
	[InlineData("First")]
	public void Delete_Works(string name)
	{
		// Arrange
		ImageModel image = Get<ImageModel>();
		image.Image = name;
		fixture.ImageRepository.Create(image);

		// Act
		fixture.ImageRepository.Delete(image.Id);

		// Assert
		Assert.Throws<NotFoundException>(() => fixture.ImageRepository.GetById(image.Id));
	}

	[Fact]
	public void Delete_CannotDeleteNonExistent()
	{
		// Arrange
		ImageModel image = Get<ImageModel>();
		fixture.ImageRepository.Create(image);

		// Act
		fixture.ImageRepository.Delete(image.Id);

		// Assert
		Assert.Throws<NotFoundException>(() => fixture.ImageRepository.Delete(image.Id));
	}
}
