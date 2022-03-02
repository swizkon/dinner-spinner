using MongoDB.Bson.Serialization.Conventions;
using System;
using MongoDB.Bson.Serialization;
using DinnerSpinner.Domain.Model;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization.IdGenerators;

namespace DinnerSpinner.Infrastructure
{
    public static class MongoDbConfiguration
    {
        public static IServiceCollection AddMongoDbConfiguration(this IServiceCollection services)
        {
            var conventionPack = new ConventionPack { new IgnoreExtraElementsConvention(true) };
            ConventionRegistry.Register("IgnoreExtraElements", conventionPack, type => true);
            
            BsonClassMap.RegisterClassMap<Spinner>(cm =>
            {
                cm.AutoMap();
                //cm.MapIdField(x => x.Id);
                cm.SetIdMember(cm.GetMemberMap(c => c.Id));
                //cm.IdMemberMap.SetIdGenerator(BsonObjectIdGenerator.Instance);
                //cm.MapProperty(c => c.Name);
            });

            BsonClassMap.RegisterClassMap<Dinner>(cm =>
            {
                cm.AutoMap();
                //cm.MapIdField(x => x.Id);
                cm.SetIdMember(cm.GetMemberMap(c => c.Id));
                cm.IdMemberMap.SetIdGenerator(BsonObjectIdGenerator.Instance);
                //cm.MapProperty(c => c.Name);
            });

            return services;
        }
    }
}
