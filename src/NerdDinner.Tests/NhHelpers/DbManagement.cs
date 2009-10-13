using System.Collections.Generic;
using FluentNHibernate.Cfg.Db;
using NerdDinner.Infrastructure;
using NerdDinner.Models;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace NerdDinner.Tests.NhHelpers
{
    public class DbManagement
    {
        public void CreateDB(IPersistenceConfigurer dbConfig)
        {
            Configuration nhConfig = new SessionFactoryBuilder(dbConfig).GetDbConfiguration();
            new SchemaExport(nhConfig).Execute(true, true, false);
        }

        public void InitializeRepository(IStatelessSession session, IEnumerable<Dinner> dinners)
        {            
            using (session)
            using (var tx = session.BeginTransaction())
            {
                foreach (var dinner in dinners)
                {
                    session.Insert(dinner);
                }

                tx.Commit();
            }
        }
    }
}
