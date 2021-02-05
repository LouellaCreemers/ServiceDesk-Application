using Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DAL
{
    public class EntityBaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class, IEntityBase, new()
    {
        private IMongoDatabase mongoDataBase;
        private string _databaseName;

        public EntityBaseRepository() : base()
        {
            string connectionString = "mongodb+srv://lou:lou@cluster0.wewsm.gcp.mongodb.net/test?authSource=admin&replicaSet=atlas-lznwzk-shard-0&readPreference=primary&appname=MongoDB%20Compass&ssl=true";
            var client = new MongoClient(connectionString);
            _databaseName = "TheGardenGroup";
            mongoDataBase = client.GetDatabase(_databaseName);
        }

        public virtual IEnumerable<TEntity> GetAll(string collection)
        {
            var _collectionGetter = mongoDataBase.GetCollection<TEntity>(collection);
            return _collectionGetter.Find(_ => true).ToList();
        }

        public virtual long Count(string collection)
        {
            var _collectionGetter = mongoDataBase.GetCollection<TEntity>(collection);
            return _collectionGetter.CountDocuments(new BsonDocument());
        }

        public TEntity GetSingle(string id, string collection)
        {
            var _collectionGetter = mongoDataBase.GetCollection<TEntity>(collection);
            return _collectionGetter.Find<TEntity>(x => x.Id == id).FirstOrDefault();
        }

        public TEntity GetSingleItemPredicate(Expression<Func<TEntity, bool>> predicate, string collection)
        {
            var _collectionGetter = mongoDataBase.GetCollection<TEntity>(collection);

            return _collectionGetter.AsQueryable<TEntity>()
                                .Where(predicate.Compile())
                                .FirstOrDefault();
        }


        public virtual IEnumerable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate, string collection)
        {
            var _collectionGetter = mongoDataBase.GetCollection<TEntity>(collection);
            return _collectionGetter.AsQueryable<TEntity>()
                                .Where(predicate.Compile())
                                .ToList();
        }

        public virtual void Add(TEntity entity, string collection)
        {
            var _collectionGetter = mongoDataBase.GetCollection<TEntity>(collection);
            _collectionGetter.InsertOne(entity);
        }

        public virtual void Update(TEntity entity, string collection)
        {
            var _collectionGetter = mongoDataBase.GetCollection<TEntity>(collection);
            var EntityIdFilter = Builders<TEntity>.Filter.Eq(x => x.Id, entity.Id);
            try
            {

                var result = _collectionGetter.ReplaceOne(EntityIdFilter, entity);
            }
            catch (MongoException ex)
            {
                string message = ex.Message;
            }
        }

        public virtual void Delete(TEntity entity, string collection)
        {
            var _collectionGetter = mongoDataBase.GetCollection<TEntity>(collection);
            _collectionGetter.DeleteOne<TEntity>(x => x.Id == entity.Id);
        }

        public void DeleteWhere(Expression<Func<TEntity, bool>> predicate, string collection)
        {
            var _collectionGetter = mongoDataBase.GetCollection<TEntity>(collection);

            foreach (TEntity entity in _collectionGetter.AsQueryable<TEntity>().Where(predicate).ToList())
            {
                _collectionGetter.DeleteMany((Builders<TEntity>.Filter.Eq("_id", entity.Id)));
            }
        }
    }
}
