using Gump.Data.Models;
using MongoDB.Driver;

namespace Gump.Data.Repositories;

public class AdvertRepository : RepositoryBase<AdvertModel>
{
	private readonly MongoDbConfig mongoDbConfig;
	private ImageRepository ImageRepository => new(mongoDbConfig);
	private PartnerRepository PartnerRepository => new(mongoDbConfig);

	public AdvertRepository(MongoDbConfig mongoDbConfig) : base(mongoDbConfig)
	{
		this.mongoDbConfig = mongoDbConfig;
	}

	public AdvertModel Create(AdvertModel advert)
	{
		// check if partner exists
		PartnerRepository.GetById(advert.PartnerId);

		advert.Id = GetId();

		ValidateFields(advert, "PartnerId", "Title", "ImageId");

		ImageRepository.GetById(advert.ImageId);

		try
		{
			Collection.InsertOne(advert);
		}
		catch (MongoException ex)
		{
			throw new AggregateException($"Error while creating {nameof(advert)}", ex);
		}

		return advert;

	}

	public AdvertModel Update(AdvertModel advert)
	{
		ValidateAllFields(advert);

		try
		{
			Collection.ReplaceOne(x => x.Id == advert.Id, advert);
		}
		catch (MongoException ex)
		{
			throw new AggregateException($"Error while updating {nameof(advert)}", ex);
		}

		return advert;
	}

	public void Delete(ulong id)
	{
		var advert = GetById(id);

		var partner = PartnerRepository.GetById(advert.PartnerId);
		partner.Ads.Remove(id);
		PartnerRepository.Update(partner);

		var image = ImageRepository.GetById(advert.ImageId);
		ImageRepository.Delete(image.Id);

		try
		{
			Collection.DeleteOne(x => x.Id == id);
		}
		catch (MongoException ex)
		{
			throw new AggregateException($"Error while deleting {nameof(advert)}", ex);
		}
	}
}
