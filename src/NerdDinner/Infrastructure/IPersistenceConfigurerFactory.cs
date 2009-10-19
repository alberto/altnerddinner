using System;
using FluentNHibernate.Cfg.Db;

namespace NerdDinner.Infrastructure
{
    public interface IPersistenceConfigurerFactory
    {
        IPersistenceConfigurer GetPersistenceConfigurer();
    }
}