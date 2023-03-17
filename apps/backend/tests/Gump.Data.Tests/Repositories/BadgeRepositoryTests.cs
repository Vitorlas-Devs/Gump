namespace Gump.Data.Tests.Repositories;

[Collection("RepositoryTests")]
public class BadgeRepositoryTests : RepositoryTestsBase, IClassFixture<RepositoryTestsBase>
{
	private readonly RepositoryTestsBase fixture;

	public BadgeRepositoryTests(RepositoryTestsBase fixture)
	{
		this.fixture = fixture;
	}

	[Fact]
	public void GetById_Works()
	{
		// Arrange & Act
		ImageModel image = Get<ImageModel>();
		fixture.ImageRepository.Create(image);

		BadgeModel badge = Get<BadgeModel>();
		fixture.BadgeRepository.Create(badge);

		BadgeModel badge2 = fixture.BadgeRepository.GetById(badge.Id);

		// Assert
		Assert.Equal(badge.Id, badge2.Id);
		Assert.Equal(badge.Name, badge2.Name);
	}

	[Theory]
	[InlineData("First")]
	public void Create_CannotCreateDuplicate(string name)
	{
		// Arrange
		ImageModel image = Get<ImageModel>();
		fixture.ImageRepository.Create(image);

		BadgeModel badge = Get<BadgeModel>();

		// Act
		badge.Name = name;
		fixture.BadgeRepository.Create(badge);

		// Assert
		Assert.Equal(name, badge.Name);
		Assert.Throws<DuplicateException>(() => fixture.BadgeRepository.Create(badge));
	}

	[Fact]
	public void Create_CannotCreateWithNonExistentImage()
	{
		// Arrange & Act
		BadgeModel badge = Get<BadgeModel>();

		// Assert
		Assert.Throws<NotFoundException>(() => fixture.BadgeRepository.Create(badge));
	}

	[Fact]
	public void Create_CannotCreateWithEmptyName()
	{
		// Arrange

		BadgeModel badge = Get<BadgeModel>();

		// Act
		badge.Name = string.Empty;

		// Assert
		Assert.Throws<RestrictedException>(() => fixture.BadgeRepository.Create(badge));
	}

	[Fact]
	public void Create_CannotCreateWithEmptyDescription()
	{
		// Arrange
		BadgeModel badge = Get<BadgeModel>();

		// Act
		badge.Description = string.Empty;

		// Assert
		Assert.Throws<RestrictedException>(() => fixture.BadgeRepository.Create(badge));
	}

	[Theory]
	[InlineData("First", "Second")]
	public void Update_CannotUpdateToExistingName(string name, string name2)
	{
		//Arrange
		ImageModel image = Get<ImageModel>();
		fixture.ImageRepository.Create(image);

		BadgeModel badge = Get<BadgeModel>();
		badge.Name = name;
		fixture.BadgeRepository.Create(badge);

		BadgeModel badge2 = Get<BadgeModel>();
		badge2.Name = name2;
		fixture.BadgeRepository.Create(badge2);

		// Act
		badge.Name = name2;

		// Assert
		Assert.Equal(name2, badge.Name);
		Assert.Equal(name2, badge2.Name);
		Assert.Throws<DuplicateException>(() => fixture.BadgeRepository.Update(badge));
	}

	[Theory]
	[InlineData("First")]
	public void Delete_Works(string name)
	{
		// Arrange
		ImageModel image = Get<ImageModel>();
		fixture.ImageRepository.Create(image);

		BadgeModel badge = Get<BadgeModel>();
		badge.Name = name;
		fixture.BadgeRepository.Create(badge);

		fixture.CategoryRepository.Create(name);

		UserModel user = Get<UserModel>();
		fixture.UserRepository.Create(user);

		RecipeModel recpie = Get<RecipeModel>();
		fixture.RecipeRepository.Create(recpie);

		// Act
		fixture.BadgeRepository.Delete(badge.Id);

		// Assert
		Assert.Throws<NotFoundException>(() => fixture.BadgeRepository.GetById(badge.Id));

		Assert.Throws<NotFoundException>(() => fixture.ImageRepository.GetById(image.Id));

		Assert.DoesNotContain(badge.Id, fixture.UserRepository.GetById(user.Id).Badges);
	}

	[Theory]
	[InlineData("First")]
	public void Delete_CannotDeleteNonExistent(string name)
	{
		//Arrange
		ImageModel image = Get<ImageModel>();
		fixture.ImageRepository.Create(image);

		BadgeModel badge = Get<BadgeModel>();
		badge.Name = name;
		fixture.BadgeRepository.Create(badge);

		// Act
		fixture.BadgeRepository.Delete(badge.Id);

		// Assert
		Assert.Throws<NotFoundException>(() => fixture.BadgeRepository.Delete(badge.Id));
	}
}
