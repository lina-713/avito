using avito.Models;
using Avito.Models;
using System.Collections.Generic;

namespace Avito
{
    public sealed class DbMockSingleton
    {
        private static volatile DbMockSingleton instance;
        private static object syncRoot = new object();

        public List<User> Users { get; set; } = new List<User>();
        public List<Advertisement> Advertisements { get; set; } = new List<Advertisement>();

        private DbMockSingleton() { }

        public static DbMockSingleton Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new DbMockSingleton();
                    }
                }

                return instance;
            }
        }
    }
}
