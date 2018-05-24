using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using CGFSMVVM.Helpers;
using CGFSMVVM.Models;
using Newtonsoft.Json.Linq;

namespace CGFSMVVM.Services
{
    public class oAuthLogin
    {
		public async Task<String> LoginUserAsync(UserModel user)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var requestBody = "<s:Envelope xmlns:s=\"http://www.w3.org/2003/05/soap-envelope\"\n            xmlns:a=\"http://www.w3.org/2005/08/addressing\"\n            xmlns:u=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd\">\n  <s:Header>\n    <a:Action s:mustUnderstand=\"1\">http://docs.oasis-open.org/ws-sx/ws-trust/200512/RST/Issue</a:Action>\n    <a:To s:mustUnderstand=\"1\">https://sts.jkintranet.com/adfs/services/trust/13/UsernameMixed</a:To>\n    <o:Security s:mustUnderstand=\"1\" xmlns:o=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\">\n      <o:UsernameToken u:Id=\"uuid-6a13a244-dac6-42c1-84c5-cbb345b0c4c4-1\">\n        <o:Username>" + user.Username.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;").Replace("'", "&apos;") + "</o:Username>\n        <o:Password Type=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText\">" + user.Password.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;").Replace("'", "&apos;") + "</o:Password>\n      </o:UsernameToken>\n    </o:Security>\n  </s:Header>\n  <s:Body>\n    <trust:RequestSecurityToken xmlns:trust=\"http://docs.oasis-open.org/ws-sx/ws-trust/200512\">\n      <wsp:AppliesTo xmlns:wsp=\"http://schemas.xmlsoap.org/ws/2004/09/policy\">\n        <a:EndpointReference>\n          <a:Address>" + Constants._gatewayURL + "/sap/bc/sec/oauth2/token</a:Address>\n        </a:EndpointReference>\n      </wsp:AppliesTo>\n      <trust:KeyType>http://docs.oasis-open.org/ws-sx/ws-trust/200512/Bearer</trust:KeyType>\n      <trust:RequestType>http://docs.oasis-open.org/ws-sx/ws-trust/200512/Issue</trust:RequestType>\n      <trust:TokenType>urn:oasis:names:tc:SAML:2.0:assertion</trust:TokenType>\n    </trust:RequestSecurityToken>\n  </s:Body>\n</s:Envelope>\n";

                    client.BaseAddress = new Uri("https://sts.jkintranet.com/adfs/services/trust/13/usernamemixed");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/soap+xml"));

                    var httpContent = new StringContent(requestBody, Encoding.UTF8, "application/soap+xml");

                    HttpResponseMessage response = await client.PostAsync("https://sts.jkintranet.com/adfs/services/trust/13/usernamemixed", httpContent);
                    if (response.IsSuccessStatusCode)
                    {
                        using (HttpContent content = response.Content)
                        {
                            string result = await content.ReadAsStringAsync();
                            if (result.Contains("FailedAuthentication"))
                            {
                                return "";
                            }
                            else
                            {
                                result = result.Substring(result.IndexOf("<Assertion", StringComparison.CurrentCulture), result.IndexOf("</trust:RequestedSecurityToken>", StringComparison.CurrentCulture) - result.IndexOf("<Assertion", StringComparison.CurrentCulture));
                                var resultBytes = Encoding.UTF8.GetBytes(result);
                                return await GetAccessToken(Uri.EscapeDataString(Convert.ToBase64String(resultBytes)));
                            }
                        }
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                    {
                        using (HttpContent content = response.Content)
                        {
                            string result = await content.ReadAsStringAsync();
                            if (result.Contains("The request scope is not valid or is unsupported"))
                            {
                                return Constants._gatewayUrlError;
                            }
                            else
                            {
                                return "";
                            }
                        }

                    }
                    else
                    {
                        return "";
                    }
                }
            }
            catch (Exception e)
            {
                return "";
            }
        }

        public async Task<String> GetAccessToken(String payload)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Constants._gatewayURL + "/sap/bc/sec/oauth2/token");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
                    var headerVal = Convert.ToBase64String(Encoding.UTF8.GetBytes("LAUNCHPAD" + ":" + "passw0rd"));
                    var header = new AuthenticationHeaderValue("Basic", headerVal);
                    client.DefaultRequestHeaders.Authorization = header;

                    var httpContent = new StringContent("client_id=LAUNCHPAD&scope=ZLAUNCHPAD_0001&grant_type=urn:ietf:params:oauth:grant-type:saml2-bearer&assertion=" + payload, Encoding.UTF8, "application/x-www-form-urlencoded");

                    HttpResponseMessage response = await client.PostAsync(Constants._gatewayURL + "/sap/bc/sec/oauth2/token", httpContent);
                    if (response.IsSuccessStatusCode)
                    {
                        using (HttpContent content = response.Content)
                        {
                            try
                            {
                                string result = await content.ReadAsStringAsync();
                                var jObj = JObject.Parse(result);
                                Constants._access_token = jObj["access_token"].ToString();
                                Constants._expires_in = jObj["expires_in"].ToString();

                                ////Store Access Token and Expiration Time
                                Settings.AccessToken = Constants._access_token;
                                Settings.ExpiresTime = DateTime.Now.AddMinutes(55).ToString();

                                return jObj["access_token"].ToString();
                            }
                            catch (Exception)
                            {
                                return "";
                            }
                        }
                    }
                    else
                    {
                        return Constants._userNotExistInNWGateway;
                    }
                }
            }
            catch (Exception e)
            {
                return "";
            }
        }
    }
}
