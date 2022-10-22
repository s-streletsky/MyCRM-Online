using MyCRM_Online.Models;
using MyCRM_Online.ViewModels.Clients;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCRM_Online.Extensions
{
    public static class HttpResponseMessageExtensions
    {
        public static void ThrowOnHttpError(this HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode) {
                var message = $"{(int)response.StatusCode}, {response.ReasonPhrase}";
                throw new ApiException(message);
            }
        }
    }
}
