namespace Gump.Data.Tests.Repositories;

[Collection("RepositoryTests")]
public class UserRepositoryTests : RepositoryTestsBase, IClassFixture<RepositoryTestsBase>
{
	private readonly RepositoryTestsBase fixture;

	public UserRepositoryTests(RepositoryTestsBase fixture)
	{
		this.fixture = fixture;
	}

	[Fact]
	public void GetById_Works()
	{
		// Arrange
		UserModel user = Get<UserModel>();
		fixture.UserRepository.Create(user);

		// Act
		UserModel user2 = fixture.UserRepository.GetById(user.Id);

		// Assert
		Assert.Equal(user.Id, user2.Id);
		Assert.Equal(user.Username, user2.Username);
	}

	[Fact]
	public void GetByName_Works()
	{
		// Arrange
		UserModel user = Get<UserModel>();
		fixture.UserRepository.Create(user);

		// Act
		UserModel user2 = fixture.UserRepository.GetByName(user.Username);

		// Assert
		Assert.Equal(user.Id, user2.Id);
		Assert.Equal(user.Username, user2.Username);
	}

	[Fact]
	public void Search_Works()
	{
		// Arrange
		UserModel user = Get<UserModel>();
		user.Username = "TestUser";
		fixture.UserRepository.Create(user);

		// Act
		IEnumerable<UserModel> users = fixture.UserRepository.Search("stus", 10);

		// Assert
		Assert.Single(users);
		Assert.Equal(user.Id, users.First().Id);
		Assert.Equal(user.Username, users.First().Username);
	}

	[Theory]
	[InlineData("First")]
	public void Create_CannotCreateDuplicate(string name)
	{
		// Arrange
		UserModel user = Get<UserModel>();

		// Act
		user.Username = name;
		fixture.UserRepository.Create(user);

		// Assert
		Assert.Equal(name, user.Username);
		Assert.Throws<DuplicateException>(() => fixture.UserRepository.Create(user));
	}

	[Theory]
	[InlineData(2)]
	public void Create_CreateWithNonExistentProfilePictureSetIdToOne(uint pictureId)
	{
		// Arrange
		UserModel user = Get<UserModel>();

		// Act
		user.ProfilePictureId = pictureId;
		fixture.UserRepository.Create(user);

		// Assert
		Assert.Equal<ulong>(1, user.ProfilePictureId);

	}

	[Fact]
	public void Create_CannotCreateWithEmptyUsername()
	{
		// Arrange
		UserModel user = Get<UserModel>();

		// Act
		user.Username = string.Empty;

		// Assert
		Assert.Throws<RestrictedException>(() => fixture.UserRepository.Create(user));
	}

	[Fact]
	public void Create_CannotCreateWithEmptyPassword()
	{
		// Arrange
		UserModel user = Get<UserModel>();

		// Act
		user.Password = string.Empty;

		// Assert
		Assert.Throws<RestrictedException>(() => fixture.UserRepository.Create(user));
	}

	[Theory]
	[InlineData("notVal.idem?ail@e.uu")]
	public void Create_CannotCreateWithInvalidEmail(string email)
	{
		// Arrange
		UserModel user = Get<UserModel>();

		// Act
		user.Email = email;

		// Assert
		Assert.Throws<ArgumentException>(() => fixture.UserRepository.Create(user));
	}

	[Theory]
	[InlineData("test")]
	public void Create_NotReturnInputPassword(string inputPassword)
	{
		// Arrange
		UserModel user = Get<UserModel>();

		// Act
		fixture.UserRepository.Create(user);

		// Assert
		Assert.NotEqual(inputPassword, user.Password);
	}

	[Theory]
	[InlineData("First", "Second")]
	public void Update_CannotUpdateToExistingName(string name, string name2)
	{
		// Arrange
		UserModel user = Get<UserModel>();
		user.Username = name;
		fixture.UserRepository.Create(user);

		UserModel user2 = Get<UserModel>();
		user2.Username = name2;
		fixture.UserRepository.Create(user2);

		// Act
		user.Username = name2;

		// Assert
		Assert.Equal(name2, user.Username);
		Assert.Equal(name2, user2.Username);
		Assert.Throws<DuplicateException>(() => fixture.UserRepository.Update(user));
	}

	[Theory]
	[InlineData(10)]
	public void Update_CannotUpdateWhenIdIsNotValid(uint id)
	{
		// Arrange
		UserModel user = Get<UserModel>();
		fixture.UserRepository.Create(user);

		// Act
		user.Id = id;

		// Assert
		Assert.Throws<DuplicateException>(() => fixture.UserRepository.Update(user));
	}

	[Theory]
	[InlineData("First", "Second")]
	public void Delete_Works(string name, string follower)
	{
		// Arrange
		ImageModel defaultImage = Get<ImageModel>();
		fixture.ImageRepository.Create(defaultImage);

		ImageModel profileImage = Get<ImageModel>();
		fixture.ImageRepository.Create(profileImage);

		UserModel user1 = Get<UserModel>();
		user1.Username = name;
		user1.ProfilePictureId = profileImage.Id;
		fixture.UserRepository.Create(user1);

		UserModel user2 = Get<UserModel>();
		user2.Username = follower;
		fixture.UserRepository.Create(user2);

		user2.Following.Add(user1.Id);
		user1.Followers.Add(user2.Id);

		user1.Following.Add(user2.Id);
		user2.Followers.Add(user1.Id);

		fixture.UserRepository.Update(user1);
		fixture.UserRepository.Update(user2);

		// Act
		fixture.UserRepository.Delete(user1.Id);

		// Assert
		Assert.Throws<NotFoundException>(() => fixture.UserRepository.GetById(user1.Id));

		Assert.Throws<NotFoundException>(() => fixture.ImageRepository.GetById(profileImage.Id));

		Assert.DoesNotContain(user1.Id, fixture.UserRepository.GetById(user2.Id).Following);

		Assert.DoesNotContain(user1.Id, fixture.UserRepository.GetById(user2.Id).Followers);
	}

	[Fact]
	public void Delete_DeleteUserIdFromLikedList()
	{
		// Arrange
		UserModel user = Get<UserModel>();
		fixture.UserRepository.Create(user);

		fixture.CategoryRepository.Create("Category");

		RecipeModel recipe = Get<RecipeModel>();
		fixture.RecipeRepository.Create(recipe);

		user.Likes.Add(recipe.Id);
		fixture.UserRepository.Update(user);

		// Act
		fixture.UserRepository.Delete(user.Id);

		// Assert
		Assert.DoesNotContain(user.Id, fixture.RecipeRepository.GetById(recipe.Id).Likes);
	}

	[Fact]
	public void Delete_CannotDeleteNonExistent()
	{
		// Arrange
		UserModel user = Get<UserModel>();
		fixture.UserRepository.Create(user);

		// Act
		fixture.UserRepository.Delete(user.Id);

		// Assert
		Assert.Throws<NotFoundException>(() => fixture.UserRepository.Delete(user.Id));
	}
}
