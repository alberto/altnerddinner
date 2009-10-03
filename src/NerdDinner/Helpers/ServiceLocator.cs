using System.Collections.Generic;
using NerdDinner.Models;

namespace NerdDinner.Helpers
{
    public static class ServiceLocator
    {
        private static IDinnerRepository repository = new InMemoryDinnerRepository(
                FakeDinnerData.CreateTestDinners());
                                        

        private static IDictionary<object, object> _container =
                new Dictionary<object, object>()
                {
                        { typeof(IDinnerRepository), repository },
                };
        
        public static T Resolve<T>()
        {
            return (T)_container[typeof(T)];
        }
    }
}
