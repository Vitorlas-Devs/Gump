﻿using Gump.Data.Models;
using MongoDB.Driver;

namespace Gump.Data.Repositories;

public class AdvertRepository : RepositoryBase<AdvertModel>
{
	private readonly PartnerRepository partnerRepository;
	public AdvertRepository(string connectionString) : base(connectionString)
	{
		partnerRepository = new PartnerRepository(connectionString);
	}

	public AdvertModel Create(AdvertModel advert)
	{
		// check if partner exists
		if (partnerRepository.GetById(advert.PartnerId) == null)
		{
			throw new ArgumentException($"Partner with id {advert.PartnerId} does not exist");
		}

		advert.Id = GetId();

		ValidateFields(advert, "PartnerId", "Title", "ImageId");

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
