using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using PN.Firestore.Entities;

namespace PN.Firestore.Repositories
{
    /// <summary>
    ///     Represents the base repository.
    /// </summary>
    /// <typeparam name="T">T is a entity type.</typeparam>
    public class FirebaseRepository<T> : IFirebaseRepository<T>
        where T : IEntity
    {
        private readonly string _collection;
        private readonly FirestoreDb _firestoreDb;

        public FirebaseRepository(FirestoreDb firestoreDb)
        {
            var type = typeof(T);
            _collection = type.Name;
            _firestoreDb = firestoreDb;
        }

        /// <inheritdoc />
        public virtual async Task<List<T>> GetAllAsync()
        {
            Query query = _firestoreDb.Collection(_collection);
            var querySnapshot = await query.GetSnapshotAsync();
            var list = new List<T>();
            foreach (var documentSnapshot in querySnapshot.Documents)
            {
                if (!documentSnapshot.Exists)
                {
                    continue;
                }

                var data = documentSnapshot.ConvertTo<T>();
                if (data == null)
                {
                    continue;
                }

                data.Id = documentSnapshot.Id;
                list.Add(data);
            }

            return list;
        }

        /// <inheritdoc />
        public virtual async Task<T> GetAsync(string entityId)
        {
            var docRef = _firestoreDb.Collection(_collection).Document(entityId);
            var snapshot = await docRef.GetSnapshotAsync();
            if (snapshot.Exists)
            {
                var usr = snapshot.ConvertTo<T>();
                usr.Id = snapshot.Id;
                return usr;
            }

            return default;
        }

        /// <inheritdoc />
        public virtual async Task<T> AddAsync(T entity)
        {
            var colRef = _firestoreDb.Collection(_collection);
            var doc = await colRef.AddAsync(entity);
            entity.Id = doc.Id;
            return entity;
        }

        /// <inheritdoc />
        public virtual async Task<T> UpdateAsync(T entity)
        {
            var recordRef = _firestoreDb.Collection(_collection).Document(entity.Id);
            await recordRef.SetAsync(entity, SetOptions.MergeAll);
            return entity;
        }

        /// <inheritdoc />
        public virtual async Task DeleteAsync(T entity)
        {
            var recordRef = _firestoreDb.Collection(_collection).Document(entity.Id);
            await recordRef.DeleteAsync();
        }

        public virtual async Task<List<T>> QueryRecordsAsync(Func<CollectionReference, Query> action)
        {
            if (action is null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            CollectionReference collref = _firestoreDb.Collection(_collection);
            var query = action(collref);
            var querySnapshot = await query.GetSnapshotAsync();
            var list = new List<T>();
            foreach (var documentSnapshot in querySnapshot.Documents)
            {
                if (!documentSnapshot.Exists)
                {
                    continue;
                }

                var data = documentSnapshot.ConvertTo<T>();
                if (data == null)
                {
                    continue;
                }

                data.Id = documentSnapshot.Id;
                list.Add(data);
            }

            return list;
        }
    }
}