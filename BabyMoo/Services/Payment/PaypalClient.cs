using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace BabyMoo.Services.Payment
{
    public class PayPalOrderResponse
    {
        public string OrderId { get; set; }
        public string ApproveLink { get; set; }
    }

    public class PayPalClient
    {
        private readonly string _clientId;
        private readonly string _secret;
        private readonly bool _isSandbox;
        private readonly IHttpClientFactory _httpClientFactory;

        private string BaseUrl => _isSandbox ? "https://api-m.sandbox.paypal.com" : "https://api-m.paypal.com";

        public PayPalClient(IConfiguration config, IHttpClientFactory httpClientFactory)
        {
            _clientId = config["PayPal:ClientId"];
            _secret = config["PayPal:Secret"];
            _isSandbox = config["PayPal:Mode"] == "sandbox";
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> GetAccessTokenAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var creds = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_clientId}:{_secret}"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", creds);

            Console.WriteLine("[PayPalClient] Requesting access token...");
            var response = await client.PostAsync($"{BaseUrl}/v1/oauth2/token",
                new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded"));

            var json = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"[PayPalClient] Access token response: {json}");

            if (!response.IsSuccessStatusCode)
                throw new Exception("PayPal token request failed");

            using var doc = JsonDocument.Parse(json);
            return doc.RootElement.GetProperty("access_token").GetString();
        }

        public async Task<PayPalOrderResponse> CreateOrder(decimal amount)
        {
            var token = await GetAccessTokenAsync();
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var body = new
            {
                intent = "CAPTURE",
                purchase_units = new[]
                {
                    new { amount = new { currency_code = "USD", value = amount.ToString("F2") } }
                },
                application_context = new
                {
                    return_url = "http://localhost:5173/orderplaced",
                    cancel_url = "http://localhost:5173/paymentfailed"
                }
            };

            var jsonBody = JsonSerializer.Serialize(body);
            Console.WriteLine($"[PayPalClient] Create order request body: {jsonBody}");

            var res = await client.PostAsync($"{BaseUrl}/v2/checkout/orders",
                new StringContent(jsonBody, Encoding.UTF8, "application/json"));

            var json = await res.Content.ReadAsStringAsync();
            Console.WriteLine($"[PayPalClient] Create order response: {json}");

            if (!res.IsSuccessStatusCode)
                throw new Exception("PayPal create order failed");

            using var doc = JsonDocument.Parse(json);
            var orderId = doc.RootElement.GetProperty("id").GetString();
            var approveLink = doc.RootElement
                .GetProperty("links")
                .EnumerateArray()
                .First(x => x.GetProperty("rel").GetString() == "approve")
                .GetProperty("href").GetString();

            return new PayPalOrderResponse { OrderId = orderId, ApproveLink = approveLink };
        }

        public async Task<bool> CaptureOrder(string paypalOrderId)
        {
            try
            {
                var token = await GetAccessTokenAsync();
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var request = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}/v2/checkout/orders/{paypalOrderId}/capture");
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                // ✅ Do NOT add request.Content or Content-Type, because there is no body

                var res = await client.SendAsync(request);
                var json = await res.Content.ReadAsStringAsync();

                Console.WriteLine($"[PayPalClient] Capture response status: {res.StatusCode}");
                Console.WriteLine($"[PayPalClient] Response body: {json}");

                return res.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Exception during capture: {ex.Message}");
                return false;
            }
        }

    }
}
