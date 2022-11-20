namespace timeboxed.Services;
using System;
using timeboxed.Models;
using System.Net.Http.Json;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;

public class OpenSenseClient : IOpenSenseClient
{
    readonly HttpClient _client;

    public OpenSenseClient()
    {
        _client = new HttpClient();
        _client.BaseAddress = new Uri("https://api.opensensemap.org/");
        //_client.DefaultRequestHeaders.Add("Content-type","application/json");
    }
    public async Task<List<BoxData>> GetAllBoxes()
    {
        return await TryGetAsync<List<BoxData>>("boxes?&classify=true&minimal=true").ConfigureAwait(false);
    }

    public async Task<BoxData> GetBox(string id)
        => await TryGetAsync<BoxData>(string.Format("boxes/{0}",id)).ConfigureAwait(false);

    public async Task<List<BoxSensor>> GetBoxSensorsAsync(string id)
        => (await TryGetAsync<BoxData>(string.Format("boxes/{0}/sensors",id)).ConfigureAwait(false)).sensors;

    public async Task<List<BoxData>> GetBoxesAtCoordinates(int latitude, int longitude, int maxDistance = 15000)
    {
        var boxes = new List<BoxData>();
        try
        {
            var response = await _client.GetAsync($"boxes?maxDistance={maxDistance}&near={latitude},{longitude}&full=true&classify=true");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                boxes = JsonConvert.DeserializeObject<List<BoxData>>(content);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);
        }

        return boxes;
    }

    private async Task<T> TryGetAsync<T>(string path)
    {
        T responseData = default;
        try
        {
            var response = await _client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                responseData = JsonConvert.DeserializeObject<T>(content);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);
        }

        return responseData;
    }
}