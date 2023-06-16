using FantasyOfSango_MMO_Server.Bases;
using MongoDB.Driver;

//Developer : SangonomiyaSakunovi

namespace FantasyOfSango_MMO_Server.Services
{
    public class MongoDBService : BaseService
    {
        public static MongoDBService Instance = null;
        private static IMongoClient Client;
        private static IMongoDatabase Database;
        private const string MongoDBName = "SangoServerGameDB";
        private const MongoDBAddress SangoMongoDBAddress = MongoDBAddress.LocalAddress;
        private enum MongoDBAddress
        {
            LocalAddress,
        }
        public override void InitService()
        {
            base.InitService();
            Instance = this;
            string mongoDBAddress = SetMongoDBAddress(SangoMongoDBAddress);
            MongoUrlBuilder mongoUrl = new MongoUrlBuilder(mongoDBAddress);
            Client = new MongoClient(mongoUrl.ToMongoUrl());
            Database = Client.GetDatabase(MongoDBName);
        }

        private string SetMongoDBAddress(MongoDBAddress address)
        {
            string mongoDBAddress = "";
            if (address == MongoDBAddress.LocalAddress)
            {
                mongoDBAddress = "mongodb://127.0.0.1:27017";
            }
            return mongoDBAddress;
        }
        #region Add Data
        public bool AddOneData<T>(T t, string collectionName) where T : class, new()
        {
            try
            {
                var collection = Database.GetCollection<T>(collectionName);
                collection.InsertOne(t);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> AddOneDataASync<T>(T t, string collectionName) where T : class, new()
        {
            try
            {
                var collection = Database.GetCollection<T>(collectionName);
                await collection.InsertOneAsync(t);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool AddBatchData<T>(List<T> t, string collectionName) where T : class, new()
        {
            try
            {
                var collection = Database.GetCollection<T>(collectionName);
                collection.InsertMany(t);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> AddBatchDataASync<T>(List<T> t, string collectionName) where T : class, new()
        {
            try
            {
                var collection = Database.GetCollection<T>(collectionName);
                await collection.InsertManyAsync(t);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Update Data
        public UpdateResult UpdateOneData<T>(T t, string collectionName, string objectId) where T : class, new()
        {
            try
            {
                var collection = Database.GetCollection<T>(collectionName);
                var filter = Builders<T>.Filter.Eq("_id", objectId);
                var list = new List<UpdateDefinition<T>>();
                foreach (var item in t.GetType().GetProperties())
                {
                    if (item.Name.ToLower() == "_id") continue;
                    list.Add(Builders<T>.Update.Set(item.Name, item.GetValue(t)));
                }
                var updateFilter = Builders<T>.Update.Combine(list);
                return collection.UpdateOne(filter, updateFilter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<UpdateResult> UpdateOneDataASync<T>(T t, string collectionName, string objectId) where T : class, new()
        {
            try
            {
                var collection = Database.GetCollection<T>(collectionName);
                var filter = Builders<T>.Filter.Eq("_id", objectId);
                var list = new List<UpdateDefinition<T>>();
                foreach (var item in t.GetType().GetProperties())
                {
                    if (item.Name.ToLower() == "_id") continue;
                    list.Add(Builders<T>.Update.Set(item.Name, item.GetValue(t)));
                }
                var updateFilter = Builders<T>.Update.Combine(list);
                return await collection.UpdateOneAsync(filter, updateFilter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public UpdateResult UpdateBatchData<T>(string collectionName, Dictionary<string, string> dict, FilterDefinition<T> filter) where T : class, new()
        {
            try
            {
                var collection = Database.GetCollection<T>(collectionName);
                T t = new T();
                var list = new List<UpdateDefinition<T>>();
                foreach (var item in t.GetType().GetProperties())
                {
                    if (!dict.ContainsKey(item.Name)) continue;
                    var value = dict[item.Name];
                    list.Add(Builders<T>.Update.Set(item.Name, value));
                }
                var updateFilter = Builders<T>.Update.Combine(list);
                return collection.UpdateMany(filter, updateFilter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<UpdateResult> UpdateBatchDataASync<T>(string collectionName, Dictionary<string, string> dict, FilterDefinition<T> filter) where T : class, new()
        {
            try
            {
                var collection = Database.GetCollection<T>(collectionName);
                T t = new T();
                var list = new List<UpdateDefinition<T>>();
                foreach (var item in t.GetType().GetProperties())
                {
                    if (!dict.ContainsKey(item.Name)) continue;
                    var value = dict[item.Name];
                    list.Add(Builders<T>.Update.Set(item.Name, value));
                }
                var updateFilter = Builders<T>.Update.Combine(list);
                return await collection.UpdateManyAsync(filter, updateFilter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Delet Data
        public DeleteResult DeletOneData<T>(string collectionName, string objectId) where T : class, new()
        {
            try
            {
                var collection = Database.GetCollection<T>(collectionName);
                FilterDefinition<T> filter = Builders<T>.Filter.Eq("_id", objectId);
                return collection.DeleteOne(filter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DeleteResult> DeletOneDataASync<T>(string collectionName, string objectId) where T : class, new()
        {
            try
            {
                var collection = Database.GetCollection<T>(collectionName);
                FilterDefinition<T> filter = Builders<T>.Filter.Eq("_id", objectId);
                return await collection.DeleteOneAsync(filter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DeleteResult DeletBatchData<T>(string collectionName, FilterDefinition<T> filter) where T : class, new()
        {
            try
            {
                var collection = Database.GetCollection<T>(collectionName);
                return collection.DeleteMany(filter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DeleteResult> DeletBatchDataASync<T>(string collectionName, FilterDefinition<T> filter) where T : class, new()
        {
            try
            {
                var collection = Database.GetCollection<T>(collectionName);
                return await collection.DeleteManyAsync(filter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region LookUp Data
        public T LookUpOneData<T>(string collectionName, string objectId, string[] field = null) where T : class, new()
        {
            try
            {
                var collection = Database.GetCollection<T>(collectionName);
                FilterDefinition<T> filter = Builders<T>.Filter.Eq("_id", objectId);
                if (field == null || field.Length == 0)
                {
                    return collection.Find(filter).FirstOrDefault<T>();
                }
                var fieldList = new List<ProjectionDefinition<T>>();
                for (int i = 0; i < field.Length; i++)
                {
                    fieldList.Add(Builders<T>.Projection.Include(field[i]));
                }
                var projection = Builders<T>.Projection.Combine(fieldList);
                fieldList?.Clear();
                return collection.Find(filter).Project<T>(projection).FirstOrDefault<T>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<T> LookUpOneDataASync<T>(string collectionName, string objectId, string[] field = null) where T : class, new()
        {
            try
            {
                var collection = Database.GetCollection<T>(collectionName);
                FilterDefinition<T> filter = Builders<T>.Filter.Eq("_id", objectId);
                if (field == null || field.Length == 0)
                {
                    return await collection.Find(filter).FirstOrDefaultAsync<T>();
                }
                var fieldList = new List<ProjectionDefinition<T>>();
                for (int i = 0; i < field.Length; i++)
                {
                    fieldList.Add(Builders<T>.Projection.Include(field[i]));
                }
                var projection = Builders<T>.Projection.Combine(fieldList);
                fieldList?.Clear();
                return await collection.Find(filter).Project<T>(projection).FirstOrDefaultAsync<T>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<T> LookUpBatchData<T>(string collectionName, FilterDefinition<T> filter, string[] field = null, SortDefinition<T> sort = null) where T : class, new()
        {
            try
            {
                var collection = Database.GetCollection<T>(collectionName);
                if (field == null || field.Length == 0)
                {
                    if (sort == null)
                    {
                        return collection.Find(filter).ToList();
                    }
                    else
                    {
                        return collection.Find(filter).Sort(sort).ToList();
                    }
                }
                var fieldList = new List<ProjectionDefinition<T>>();
                for (int i = 0; i < field.Length; i++)
                {
                    fieldList.Add(Builders<T>.Projection.Include(field[i]));
                }
                var projection = Builders<T>.Projection.Combine(fieldList);
                fieldList?.Clear();
                if (sort == null)
                {
                    return collection.Find(filter).Project<T>(projection).ToList();
                }
                else
                {
                    return collection.Find(filter).Project<T>(projection).Sort(sort).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<T>> LookUpBatchDataASync<T>(string collectionName, FilterDefinition<T> filter, string[] field = null, SortDefinition<T> sort = null) where T : class, new()
        {
            try
            {
                var collection = Database.GetCollection<T>(collectionName);
                if (field == null || field.Length == 0)
                {
                    if (sort == null)
                    {
                        return await collection.Find(filter).ToListAsync();
                    }
                    else
                    {
                        return await collection.Find(filter).Sort(sort).ToListAsync();
                    }
                }
                var fieldList = new List<ProjectionDefinition<T>>();
                for (int i = 0; i < field.Length; i++)
                {
                    fieldList.Add(Builders<T>.Projection.Include(field[i]));
                }
                var projection = Builders<T>.Projection.Combine(fieldList);
                fieldList?.Clear();
                if (sort == null)
                {
                    return await collection.Find(filter).Project<T>(projection).ToListAsync();
                }
                else
                {
                    return await collection.Find(filter).Project<T>(projection).Sort(sort).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Get Count
        public long GetCount<T>(string collectionName, FilterDefinition<T> filter) where T : class, new()
        {
            try
            {
                var collection = Database.GetCollection<T>(collectionName);
                return collection.CountDocuments(filter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<long> GetCountAsync<T>(string collectionName, FilterDefinition<T> filter) where T : class, new()
        {
            try
            {
                var collection = Database.GetCollection<T>(collectionName);
                return await collection.CountDocumentsAsync(filter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
