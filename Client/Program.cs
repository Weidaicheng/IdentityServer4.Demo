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
            var disco = await DiscoveryClient.GetAsync("http://localhost:5000");
            var tokenClient = new TokenClient(disco.TokenEndpoint, "client", "secret");
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync("api1");

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
