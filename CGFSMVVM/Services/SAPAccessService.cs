using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CGFSMVVM.Helpers;
using CGFSMVVM.Models;
using Xamarin.Forms;

namespace CGFSMVVM.Services
{
    public class SAPAccessService
    {
        

		public async Task<String> HotelCodeAsync()
        {
            string url = "/sap/opu/odata/sap/ZTMS_GET_USER_HOTEL_SRV/EX_XHOTEL_IDSet(' ')?$format=json";
            return await this.GetODataService(url);
        }


		public async Task<String> GetODataService(String url)
        {
            try
            {
                //Refresh Token if expires
                if (Convert.ToDateTime(Settings.ExpiresTime) <= DateTime.Now)
                {
                    //Authenticate against ADFS and NW Gateway
                    oAuthLogin oauthlogin = new oAuthLogin();
                    String access_token = await oauthlogin.LoginUserAsync(Constants._user);
                    if (access_token == "" && access_token == Constants._userNotExistInNWGateway)
                    {
						new UserLogout().logout();
                    }
                }
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Constants._gatewayURL + url);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Constants._access_token);
                    try
                    {
                        HttpResponseMessage response = await client.GetAsync(Constants._gatewayURL + url);
                        using (HttpContent content = response.Content)
                        {
                            string result = await content.ReadAsStringAsync();
                            return result;
                        }
                    }
                    catch (Exception e)
                    {
                        return "Error";
                    }
                }
            }
            catch (Exception e)
            {
                return "Error";
            }
        }
    }
}

