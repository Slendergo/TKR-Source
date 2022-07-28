using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace common.discord
{
    public class DiscordWebhook : IDisposable
    {
        private readonly HttpClient _client;
        private readonly string _webhook;

        public DiscordWebhook(string webhook)
        {
            _webhook = webhook;
            _client = new HttpClient();
        }

        public void Dispose() => _client.Dispose();

        public async Task SendAsync(DiscordEmbedBuilder builder)
        {
            var content = new StringContent(JsonConvert.SerializeObject(builder), Encoding.UTF8, "application/json");

            await _client.PostAsync(_webhook, content);
        }
    }
}
