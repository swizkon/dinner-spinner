using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization;
using DinnerSpinner.Domain.Model;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;

namespace DinnerSpinner.Infrastructure
{
    public static class MongoDbConfiguration
    {
        public static IServiceCollection AddMongoDbConfiguration(this IServiceCollection services)
        {
            var conventionPack = new ConventionPack
                {
                    new EnumRepresentationConvention(BsonType.String),
                    new CamelCaseElementNameConvention(),
                    new IgnoreExtraElementsConvention(true)
                };
            ConventionRegistry.Register("DinnerSpinnerConventions", conventionPack, type => true);

            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));

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
