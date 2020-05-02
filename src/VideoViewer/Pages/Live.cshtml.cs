using System;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace VideoViewer.Pages
{
    public class LiveModel : PageModel
    {
        private HttpClient apiClient_;

        public LiveModel(HttpClient apiClient)
        {
            apiClient_ = apiClient;
        }
        public void OnGet()
        {

        }
        public void OnPost(int id)
        {
            switch(id)
            {
                case 1:
                    apiClient_.PostAsync("api/device/streamingvideodevice1/Stop", new StringContent("")).Wait();
                    break;
                case 2:
                    apiClient_.PostAsync("api/device/streamingvideodevice1/Start", new StringContent("")).Wait();
                    break;
                case 3:
                    apiClient_.PostAsync("api/device/streamingvideodevice2/Stop", new StringContent("")).Wait();
                    break;
                case 4:
                    apiClient_.PostAsync("api/device/streamingvideodevice2/Start", new StringContent("")).Wait();
                    break;
            }
        }
    }
}