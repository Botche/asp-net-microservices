namespace Shopping.Aggregator.Extensions
{
    using System.Text.Json;

    public static class HttpClientExtensions
    {
        public static async Task<T> ReadContentAsAsync<T>(this HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException($"Something went wrong calling the API: {response.ReasonPhrase}");
            }

            string dataAsString = await response.Content
                .ReadAsStringAsync()
                .ConfigureAwait(false);

            return JsonSerializer.Deserialize<T>(dataAsString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}
