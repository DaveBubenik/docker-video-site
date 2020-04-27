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
            if (id == 2)
            {
                apiClient_.PostAsync("api/device/1/Start", new StringContent("")).Wait();
            }
            else
            {
                apiClient_.PostAsync("api/device/1/Stop", new StringContent("")).Wait();
            }
        }
    }
}