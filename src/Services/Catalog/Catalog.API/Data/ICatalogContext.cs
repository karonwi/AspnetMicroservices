using Catalog.API.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Catalog.API.Data
{
    public interface ICatalogContext
    {
        //The product is synonymous with the table of a RDBM and here it is a
        //collection while for the properties in the products are the documents which is synonymous with the rows in a RDM
        IMongoCollection<Product> Products { get; }
    }
}
