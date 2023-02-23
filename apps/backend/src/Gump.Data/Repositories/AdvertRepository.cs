using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gump.Data.Models;
using MongoDB.Driver;
using MongoDB.Driver.Core.Misc;

namespace Gump.Data.Repositories;

public class AdvertRepository : RepositoryBase<AdvertModel>
{


	public AdvertRepository(string connectionString) : base(connectionString) { 

	}

	public AdvertModel Create(AdvertModel advert)
	{
		PartnerRepository partnerRepository = new PartnerRepository(connectionString);

		if (partnerRepository.GetAll().Any(x => x.Id == advert.PartnerId))
		{
			throw new ArgumentException($"Partner does not exists with id {advert.PartnerId}");
		}

		advert.Id = GetId();

		ValidateFields(advert, "PartnerId", "Title", "ImageUrl");

		try
		{
			Collection.InsertOne(advert);
		}
		catch (MongoException ex)
		{
			throw new AggregateException("Error while creating advert", ex);
		}

		return advert;

	}

	public AdvertModel GetById(ulong id)
	{
		AdvertModel advert = Collection.AsQueryable().FirstOrDefault(x => x.Id == id);

		ValidateFields(advert, "Id");

		return advert;
	}

	public List<AdvertModel> GetAll()
	{
		return Collection.AsQueryable().ToList();
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
			throw new AggregateException("Error while updating advert", ex);
		}

		return advert;
	}

	public void Delete(ulong id)
	{
		try
		{
			Collection.DeleteOne(x => x.Id == id);
		}
		catch (MongoException ex)
		{
			throw new AggregateException("Error while deleting advert", ex);
		}
	}

}
