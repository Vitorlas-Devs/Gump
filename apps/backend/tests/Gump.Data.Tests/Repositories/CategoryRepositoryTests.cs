namespace Gump.Data.Tests.Repositories;

[Collection("RepositoryTests")]
public class CategoryRepositoryTests : RepositoryTestsBase, IClassFixture<RepositoryTestsBase>
{
	private readonly RepositoryTestsBase fixture;

	public CategoryRepositoryTests(RepositoryTestsBase fixture)
	{
		this.fixture = fixture;
	}
	
	[Theory]
	[InlineData("First")]
	public void GetById_Works(string name)
	{
		// Arrange & Act
		CategoryModel category = fixture.CategoryRepository.Create(name);
		CategoryModel category2 = fixture.CategoryRepository.GetById(category.Id);

		// Assert
		Assert.Equal(category.Id, category2.Id);
		Assert.Equal(category.Name, category2.Name);
	}

	[Theory]
	[InlineData("First")]
	public void Create_CannotCreateDuplicate(string name)
	{
		// Arrange & Act
		CategoryModel category = fixture.CategoryRepository.Create(name);

		// Assert
		Assert.Equal(name, category.Name);
		Assert.Throws<DuplicateException>(() => fixture.CategoryRepository.Create(name));
	}

	[Theory]
	[InlineData("First", "Second")]
	public void Update_CannotUpdateToExistingName(string name, string name2)
	{
		// Arrange
		CategoryModel category = fixture.CategoryRepository.Create(name);

		CategoryModel category2 = fixture.CategoryRepository.Create(name2);

		// Act
		category.Name = name2;

		// Assert
		Assert.Equal(name2, category.Name);
		Assert.Equal(name2, category2.Name);
		Assert.Throws<DuplicateException>(() => fixture.CategoryRepository.Update(category));
	}

	[Theory]
	[InlineData("First")]
	public void Delete_Works(string name)
	{
		// Arrange
		CategoryModel category = fixture.CategoryRepository.Create(name);

		// Act
		fixture.CategoryRepository.Delete(category.Id);

		// Assert
		Assert.Throws<NotFoundException>(() => fixture.CategoryRepository.GetById(category.Id));
	}

	[Theory]
	[InlineData("First")]
	public void Delete_CannotDeleteNonExistent(string name)
	{
		// Arrange
		CategoryModel category = fixture.CategoryRepository.Create(name);

		// Act
		fixture.CategoryRepository.Delete(category.Id);

		// Assert
		Assert.Throws<NotFoundException>(() => fixture.CategoryRepository.Delete(category.Id));
	}
}

