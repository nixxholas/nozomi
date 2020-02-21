using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Base.BCL.Helpers.Enumerator;
using Nozomi.Base.BCL.Helpers.Mapping;
using Nozomi.Data.Models.Web;

namespace Nozomi.Repo.Data.Mappings.WebModels
{
    public class ComponentTypeMap : BaseMap<ComponentType>
    {
        public ComponentTypeMap(EntityTypeBuilder<ComponentType> entityTypeBuilder) : base(entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(e => e.Id).HasName("ComponentType_PK_Id");
            entityTypeBuilder.Property(e => e.Id).ValueGeneratedOnAdd();

            entityTypeBuilder.HasAlternateKey(e => e.Slug).HasName("ComponentType_AK_Slug");

            entityTypeBuilder.Property(e => e.Description).IsRequired(false);
            entityTypeBuilder.Property(e => e.Name).IsRequired();

            entityTypeBuilder.HasMany(e => e.Components)
                .WithOne(c => c.ComponentType)
                .HasForeignKey(c => c.ComponentTypeId).OnDelete(DeleteBehavior.Cascade).IsRequired(false);
                
            // Component Types
            var genericComponentTypes = 
                EnumHelper.GetEnumDescriptionsAndValues<GenericComponentType>();
            foreach (var gct in genericComponentTypes)
            {
                var componentType = new ComponentType
                {
                    Id = gct.Key,
                    Description = gct.Value,
                    // https://stackoverflow.com/questions/16039037/get-the-name-of-enum-value
                    Name = Enum.GetName(typeof(GenericComponentType), gct.Key),
                    Slug = Enum.GetName(typeof(GenericComponentType), gct.Key)
                };

                entityTypeBuilder.HasData(componentType);
            }
        }
    }
}