using System.Collections.Generic;
using System.Web.Http;

namespace CSharpOldWebApi.Controllers
{
    public class UsersController : ApiController
    {
        // GET /api/users
        public IEnumerable<object> Get()
        {
            return new[]
            {
                new { id = 1, name = "Long" },
                new { id = 2, name = "An" },
                new { id = 3, name = "Minh" }
            };
        }
    }
}
