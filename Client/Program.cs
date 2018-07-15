using IdentityModel.Client;
using Newtonsoft.Json;
using Refit;
using System;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            foo();

            Console.ReadKey();
        }

        private static async void foo()
        {
            //从元数据中发现客户端
            var disco = await DiscoveryClient.GetAsync("http://localhost:5000");

            //请求令牌
            //通过客户端
            //var tokenClient = new TokenClient(disco.TokenEndpoint, "client", "secret");
            //var tokenResponse = await tokenClient.RequestClientCredentialsAsync("api1");
            //通过密码
            var tokenClient = new TokenClient(disco.TokenEndpoint, "ro.client", "secret");
            var tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync("alice", "password");

            if(tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }
            Console.WriteLine(tokenResponse.Json);

            var identityApi = RestService.For<IIdentityApi>("http://localhost:5001");
            var result = await identityApi.Identity($"Bearer {tokenResponse.AccessToken}");

            Console.WriteLine(JsonConvert.SerializeObject(result));
        }
    }
}
