using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MVCClient
{
    public interface IIdentityApi
    {
        [Get("/Identity")]
        Task<object> Identity([Header("Authorization")]string token);
    }
}
