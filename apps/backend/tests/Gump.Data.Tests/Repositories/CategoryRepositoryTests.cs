namespace Gump.Data.Tests.Repositories;

public class CategoryRepositoryTests : RepositoryTestsBase, IClassFixture<RepositoryTestsBase>
{
	private readonly RepositoryTestsBase fixture;

	public CategoryRepositoryTests(RepositoryTestsBase fixture)
	{
		this.fixture = fixture;
	}

	[Fact]
	public void Create()
	{
		// Arrange
		const string name = "Test";

		// Act
		// CategoryModel category = fixture.CategoryRepository.Create(name);
		CategoryModel category = Get<CategoryModel>();

		category.Name = name;

		fixture.CategoryRepository.Create(name);

		// Assert
		Assert.Equal(name, category.Name);
		Assert.Throws<ArgumentException>(() => fixture.CategoryRepository.Create(name));
	}

	[Fact]
	public void Update()
	{
		// Arrange
		const string name = "Test";
		CategoryModel category = fixture.CategoryRepository.Create(name);

		// Act
		category.Name = "Test2";
		fixture.CategoryRepository.Update(category);

		// Assert
		Assert.Equal("Test2", category.Name);
		Assert.Throws<ArgumentException>(() => fixture.CategoryRepository.Update(category));
	}

	[Fact]
	public void Delete()
	{
		// Arrange
		const string name = "Test";
		CategoryModel category = fixture.CategoryRepository.Create(name);

		// Act
		fixture.CategoryRepository.Delete(category.Id);

		// Assert
		Assert.Throws<ArgumentNullException>(() => fixture.CategoryRepository.Delete(category.Id));
	}
}

