using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using StreamingVideoDevice;

namespace VideoViewer.Pages
{
    public class LiveModel : PageModel
    {
        private HttpClient apiClient_;
        public List<DeviceStatus> devices = new List<DeviceStatus>();
        
        public LiveModel(HttpClient apiClient)
        {
            apiClient_ = apiClient;
        }
        public async Task<List<DeviceStatus>> AvailableDevices()
        {
            var apiResult = await apiClient_.GetAsync("api/device");
            var bodyContent = await apiResult.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<DeviceStatus>>(bodyContent);
        }
        public void OnGet()
        {
            devices = AvailableDevices().GetAwaiter().GetResult();
        }

        public void OnPost(bool[] deviceCheckbox, string submitButton)
        {
            devices = AvailableDevices().GetAwaiter().GetResult();
            for (int i = 0; i < deviceCheckbox.Length; ++i)
            {
                if (deviceCheckbox[i])
                {
                    apiClient_.PostAsync($"api/device/{devices[i].HostName}/{submitButton}", new StringContent("")).Wait();
                    devices[i] = new DeviceStatus { FriendlyName = devices[i].FriendlyName, HostName = devices[i].HostName, StreamActive = (submitButton == "Start") };
                }
            }
        }
    }
}