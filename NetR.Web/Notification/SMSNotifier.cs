using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using NetR.Web.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace NetR.Web.Notification
{
    public class SMSNotifier : INotifier
    {
        private readonly SmSSettings settings;
        private readonly IMemoryCache memoryCache;
        private readonly RestClient client;

        public SMSNotifier(IOptions<SmSSettings> options, IMemoryCache memoryCache , RestClient restClient)
        {
            settings = options.Value;
            this.memoryCache = memoryCache;
            client = restClient;
        }
        public async Task Notify(NotificationModel notification)
        {
            if (!notification.IsSMS) return;
            //var client = factory.CreateClient("SMS");
            //await AddAuthToken(client);
            //var response = await  client.PostAsync("/v1/bulkmessages",new StringContent(JsonConvert.SerializeObject(new { messages = new[] { new { content = notification.Message, destination = notification.To } } }),Encoding.UTF8,"application/json"));


            // Now on to the fun part.  We prepare the next request as a POST to the /Bulkmessages endpoint.
            var sendRequest = new RestRequest("/v1/bulkmessages", Method.POST);

            // We can now populate the Authentication header with our new token.

            await AddAuthToken(sendRequest);
            // We next need to add a body containing the specifics of the send.
            // this is just a basic SMS, have a look at our docs page to see what you can get up to.
            
        sendRequest.AddJsonBody(new
            {
                Messages = notification.SMS.Value.Recipients.Split(';').Select(x =>
                                    new
                    {
                        content = notification.SMS.Value.Message,
                        destination = x
                    }
                )
            });

            // with everything prepared we fire off the Request.
            var sendResponse = client.Execute(sendRequest);

            // Again we ensure the response we get back indicates success.
            if (sendResponse.StatusCode == HttpStatusCode.OK)
            {
                Console.WriteLine(sendResponse.Content);
            }
            else
            {
                // If not, we can look to the error message to see what went 
                // wrong, be it a faulty auth token or improper formatting of the Request body. 
                Console.WriteLine(sendResponse.ErrorMessage);
            }

        }
        private async Task AddAuthToken(RestRequest request)
        {
            var token =await memoryCache.GetOrCreate("smsportal",async key =>
            {
                var apiKey = settings.ClientId;
                var apiSecret = settings.Secret;
                var accountApiCredentials = $"{apiKey}:{apiSecret}";

                // Our endpoint expects this string to be Base64 encoded, to that end:
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(accountApiCredentials);
                var base64Credentials = Convert.ToBase64String(plainTextBytes);

                // We then need to call the authentication endpoint to generate the AuthToken we will be 
                // using for the rest of your interactions with our RESTful interface.
                var authRequest = new RestRequest("/v1/Authentication", Method.GET);

                // We need to set the Authorization header to contain our BASE64 encoded API credential 
                // and specify these credentials are in Basic format.
                authRequest.AddHeader("Authorization", $"Basic {base64Credentials}");

                // We then call the SMSPortal endpoint to authenticate and provide us with an AuthToken.
                var authResponse =  client.Execute(authRequest);
                if (authResponse.StatusCode == HttpStatusCode.OK)
                {
                    var data = JsonConvert.DeserializeObject<dynamic>(authResponse.Content);
                    var expiry = (int)data.expiresInMinutes;
                    key.SetAbsoluteExpiration(TimeSpan.FromMinutes(expiry));
                    var _token = (string)data.token;
                    return _token;
                }
                throw new Exception("Could not get token from SMS portal");
            });
            request.AddHeader("Authorization", $"Bearer {token}");
        }
        private string Base64Encode(string plainText) => Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));

    }
}
