using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Azure.CosmosTests.MockData
{
    public class UserMock
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Age { get; set; }


        public static IEnumerable<UserMock> GetUsers()
        {
            return new UserMock[]
            {
                new UserMock() { FirstName = "Chase", LastName = "Crawford", Age = 24 }
            };
        }
    }
}
