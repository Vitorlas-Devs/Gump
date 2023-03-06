namespace Gump.Data.Tests.Repositories;

public class CategoryRepositoryTests : IClassFixture<RepositoryTestsBase<CategoryRepository>>
{
	private readonly RepositoryTestsBase<CategoryRepository> fixture;

	public CategoryRepositoryTests(RepositoryTestsBase<CategoryRepository> fixture)
	{
		this.fixture = fixture;
	}

	[Fact]
	public void Create()
	{
		// Arrange
		const string name = "Test";

		// Act
		CategoryModel category = fixture.Repository.Create(name);

		// Assert
		Assert.Equal(name, category.Name);
		Assert.Throws<ArgumentException>(() => fixture.Repository.Create(name));
	}

	[Fact]
	public void Update()
	{
		// Arrange
		const string name = "Test";
		CategoryModel category = fixture.Repository.Create(name);

		// Act
		category.Name = "Test2";
		fixture.Repository.Update(category);

		// Assert
		Assert.Equal("Test2", category.Name);
		Assert.Throws<ArgumentException>(() => fixture.Repository.Update(category));
	}

	[Fact]
	public void Delete()
	{
		// Arrange
		const string name = "Test";
		CategoryModel category = fixture.Repository.Create(name);

		// Act
		fixture.Repository.Delete(category.Id);

		// Assert
		Assert.Throws<ArgumentNullException>(() => fixture.Repository.Delete(category.Id));
	}
}

