using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NerdDinner.Infrastructure;
using NerdDinner.Models;
using NHibernate.Cfg;
using NUnit.Framework;

namespace NerdDinner.Tests.Integration
{
    public class MapperTests
    {

        [Test, Explicit]
        public void CanBuildSessionFactory()
        {
            IPersistenceConfigurer dbConfig = new DbConfigFactory().GetGetMsSqlConfig();
            var builder = new SessionFactoryBuilder(dbConfig);
            builder.Build();
        }

        [Test, Explicit]
        public void CanExportMappings()
        {
            var cfg = new Configuration();
            IPersistenceConfigurer dbConfig = new DbConfigFactory().GetGetMsSqlConfig();

            Fluently.Configure(cfg)
                    .Database(dbConfig)
                    .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Dinner>().ExportTo(@"."));
        } 
    }
}
