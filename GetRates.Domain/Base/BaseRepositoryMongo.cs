using System;
using System.Configuration;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace GetRates.Domain.Base
{
    public class BaseRepositoryMongo<T, TId> : IBaseRepoistory<T, TId> where TId : struct 
    {
        private readonly string collectionName = "";
        private readonly string databaseName = ConfigurationManager.AppSettings["MongoDbName"].ToString();

        private readonly MongoServer server = MongoServer.Create(ConfigurationManager.ConnectionStrings["MongoDbConnString"].ToString());

        public BaseRepositoryMongo(string collectionName)
        {
            this.collectionName = collectionName;
        }

        protected MongoCollection<T> MongoCollection
        {
            get
            {
                if (String.IsNullOrEmpty(collectionName))
                    throw new InvalidOperationException("CollectionName is either null or empty");
                var database = server.GetDatabase(databaseName);
                var mongoCollection = database.GetCollection<T>(collectionName);
                return mongoCollection;
            }
        }

        #region IBaseRepoistory Members


        public IQueryable<T> All {
            get {
                return MongoCollection.FindAll().AsQueryable();
            }
        }

        public T Find(TId id)
        {
            return MongoCollection.FindOneById(BsonValue.Create(id));
        }

        public void InsertOrUpdate(T entity) {
            MongoCollection.Save(entity);
        }

        public void Delete(TId id) {
            MongoCollection.FindAndRemove(Query.EQ("_id", BsonValue.Create(id)), null);
        }

        #endregion
    }
}