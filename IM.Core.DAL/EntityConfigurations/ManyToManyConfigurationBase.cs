using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace InfraManager.DAL.EntityConfigurations
{
    public class ManyToManyConfigurationBase<TParent, TReference, TParentKey, TReferenceKey> : IEntityTypeConfiguration<ManyToMany<TParent, TReference>> 
        where TReference : class
        where TParent : class
    {
        private readonly string _primaryKeyColumnName;
        private readonly string _primaryKeyConstraintName;
        private readonly string _foreignKeyConstraintName;
        private readonly string _parentForeignKeyConstraintName;
        private readonly Expression<Func<TParent, IEnumerable<ManyToMany<TParent, TReference>>>> _parentNavigationCollection;
        private readonly string _foreignKeyColumnName;
        private readonly string _parentKeyColumnName;
        private readonly string _uniqueKeyName;
        private readonly string _tableName;
        private readonly string _schemaName;
        private readonly DeleteBehavior _foreignKeyConstraintDeleteBehavior;
        private readonly DeleteBehavior _parentForeignKeyConstraintDeleteBehavior;

        public ManyToManyConfigurationBase(
            string tableName,
            string schemaName,
            string primaryKeyColumnName,
            string parentKeyColumnName,
            string foreignKeyColumnName,
            string primaryKeyConstraintName,
            string uniqueKeyName,
            string foreignKeyConstraintName,
            string parentForeignKeyConstraintName,
            Expression<Func<TParent, IEnumerable<ManyToMany<TParent, TReference>>>> parentNavigationCollection,
            DeleteBehavior foreignKeyConstraintDeleteBehavior = DeleteBehavior.NoAction,
            DeleteBehavior parentForeignKeyConstraintDeleteBehavior = DeleteBehavior.NoAction)
        {
            _primaryKeyColumnName = primaryKeyColumnName;
            _primaryKeyConstraintName = primaryKeyConstraintName;
            _foreignKeyConstraintName = foreignKeyConstraintName;
            _foreignKeyColumnName = foreignKeyColumnName;
            _parentKeyColumnName = parentKeyColumnName;
            _uniqueKeyName = uniqueKeyName;
            _tableName = tableName;
            _schemaName = schemaName;
            _parentForeignKeyConstraintName = parentForeignKeyConstraintName;
            _parentNavigationCollection = parentNavigationCollection;
            _foreignKeyConstraintDeleteBehavior = foreignKeyConstraintDeleteBehavior;
            _parentForeignKeyConstraintDeleteBehavior = parentForeignKeyConstraintDeleteBehavior;
        }

        public void Configure(EntityTypeBuilder<ManyToMany<TParent, TReference>> builder)
        {
            builder.ToTable(_tableName, _schemaName);
            builder.HasKey(x => x.ID).HasName(_primaryKeyConstraintName);            
            builder.Property(x => x.ID).HasColumnName(_primaryKeyColumnName).ValueGeneratedOnAdd();
            builder.Property<TParentKey>(_parentKeyColumnName);
            builder.Property<TReferenceKey>(_foreignKeyColumnName);           

            builder.HasOne(x => x.Reference)
                .WithMany()
                .HasForeignKey(_foreignKeyColumnName)
                .HasConstraintName(_foreignKeyConstraintName)
                .IsRequired(true)
                .OnDelete(_foreignKeyConstraintDeleteBehavior);

            builder.HasOne(x => x.Parent)
                .WithMany(_parentNavigationCollection)
                .HasForeignKey(_parentKeyColumnName)
                .HasConstraintName(_parentForeignKeyConstraintName)
                .IsRequired(true)
                .OnDelete(_parentForeignKeyConstraintDeleteBehavior);

            builder.HasIndex(_parentKeyColumnName, _foreignKeyColumnName).HasDatabaseName(_uniqueKeyName).IsUnique();
        }
    }
}
