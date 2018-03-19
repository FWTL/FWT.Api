using FactoryGirlCore;
using FWT.TL.Core.Entities;
using System.Collections.Generic;
using System.Data.Entity.Migrations;

namespace Auth.FWT.Data.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<Auth.FWT.Data.AppContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(AppContext context)
        {
        }

        private void InsertFakeData<TEntity, TKey, TFactory>(AppContext context, int count = 1, string name = "") where TEntity : BaseEntity<TKey> where TFactory : IDefinable
        {
            FactoryGirl.ClearFactoryDefinitions();
            FactoryGirl.Initialize(typeof(TFactory));
            ICollection<TEntity> entities = null;
            if (string.IsNullOrWhiteSpace(name))
            {
                entities = FactoryGirl.BuildList<TEntity>(count);
            }
            else
            {
                entities = FactoryGirl.BuildList<TEntity>(count, name);
            }

            foreach (var entity in entities)
            {
                context.Set<TEntity, TKey>().Add(entity);
            }

            context.SaveChanges();
        }
    }
}