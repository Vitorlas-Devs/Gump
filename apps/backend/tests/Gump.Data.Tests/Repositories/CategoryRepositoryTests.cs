namespace Gump.Data.Tests.Repositories;

public class CategoryRepositoryTests : IClassFixture<BaseRepositoryTests>
{
	private readonly BaseRepositoryTests fixture;

	public CategoryRepositoryTests(BaseRepositoryTests fixture)
	{
		this.fixture = fixture;
	}

	[Fact]
	public void Create_WhenCategoryNameIsAlreadyTaken_ShouldThrowArgumentException()
	{
		// Arrange
		string name = "Test";

		// Act
		fixture.Repository.Create(name);

		// Assert
		Assert.Throws<ArgumentException>(() => fixture.Repository.Create(name));
	}
}

