using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using ServiceModelEx;
using System.Runtime.Serialization;
using System.Net;
using System.IO;


namespace AllegroGraphCSharpClient
{
    /// <summary>
    /// Class to handle the actual communication to the server
    /// WCF compatible, so if you use WCF, then you will always need to pass the url to every function
    /// Otherwise, you can set it using the constructor
    /// </summary>
    public class Request : IRESTInterface
    {
        #region IRESTInterface Members


        /// <summary>
        /// Generate a HTTP Request object with the specified options
        /// </summary>
        /// <param name="method"></param>
        /// <param name="url"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public System.Net.HttpWebRequest makeHttpRequest(string method, string url, List<NameValuePairs> options)
        {
            System.Net.HttpWebRequest webRequest = null;
            Dictionary<string, string> query = new Dictionary<string, string>(); 
            try
            {
                if (options != null)
                {
                    foreach (NameValuePairs np in options)
                    {
                        Object value = np.Value;
                        string key = np.Name;
                        query.Add(System.Web.HttpUtility.UrlEncode(key), System.Web.HttpUtility.UrlEncode(value.ToString())); 

                    }
                }

                if ("POST".Equals(method.ToUpper().Trim()))
                {
                    Uri newUrl = new System.Uri(url);
                    UriBuilder uriB = new UriBuilder(newUrl);
                    string queryString = string.Empty;
                    int counter = 0;
                    
                    foreach (NameValuePairs np in options)
                    {
                        queryString += np.Name + "=" + np.Value;
                        if (counter < options.Count)
                        {
                            queryString += "&"; 
                        }

                    }
                    uriB.Query = queryString;
                    
                    
                    webRequest = System.Net.WebRequest.Create(uriB.Uri) as System.Net.HttpWebRequest;
                    webRequest.Method = "POST";
                }
                else if ("DELETE".Equals(method.ToUpper().Trim())){
                    Uri newUrl = new Uri(url);
                    UriBuilder uriB = new UriBuilder(newUrl);
                    string queryString = string.Empty;
                    int counter = 0;

                    foreach (NameValuePairs np in options)
                    {
                        queryString += np.Name + "=" + np.Value;
                        if (counter < options.Count)
                        {
                            queryString += "&";
                        }

                    }
                    uriB.Query = queryString;


                    webRequest = System.Net.WebRequest.Create(uriB.Uri) as System.Net.HttpWebRequest;
                    webRequest.Method = "DELETE";
                }
                else if ("PUT".Equals(method.ToUpper().Trim()))
                {
                    Uri newUrl = new Uri(url);
                    UriBuilder uriB = new UriBuilder(newUrl);
                    string queryString = string.Empty;
                    int counter = 0;

                    foreach (NameValuePairs np in options)
                    {
                        queryString += np.Name + "=" + np.Value;
                        if (counter < options.Count)
                        {
                            queryString += "&";
                        }

                    }
                    uriB.Query = queryString;


                    webRequest = System.Net.WebRequest.Create(uriB.Uri) as System.Net.HttpWebRequest;
                    webRequest.Method = "PUT";
                }
                else
                {
                    Uri newUrl = new Uri(url);
                    UriBuilder uriB = new UriBuilder(newUrl);
                    string queryString = string.Empty;
                    int counter = 0;
                    if (options != null)
                    {
                        foreach (NameValuePairs np in options)
                        {
                            queryString += np.Name + "=" + np.Value;
                            if (counter < options.Count)
                            {
                                queryString += "&";
                            }

                        }
                    }
                    uriB.Query = queryString;


                    webRequest = System.Net.WebRequest.Create(uriB.Uri) as System.Net.HttpWebRequest;
                    webRequest.Method = "GET"; 
                }
                
                
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Error in makeHttpRequest: " + ex.Message); 
            }


            return webRequest;
        }

        /// <summary>
        /// Performs a basic REST operation
        /// </summary>
        /// <param name="method"></param>
        /// <param name="url"></param>
        /// <param name="options"></param>
        /// <param name="ContentType"></param>
        /// <param name="accept"></param>
        /// <returns></returns>
        public List<Results> makeRequest(string method, string url, List<NameValuePairs> options, string ContentType, string accept)
        {
            List<Results> results = new List<Results>(); 
            try
            {
                
                HttpWebRequest request = makeHttpRequest(method, url, options);
                if (ContentType == string.Empty)
                {
                    //ebRequest myRequest = WebRequest.Create(url); 
                   // myRequest.Headers.Add("accept", "accept"); 
                    if (accept == null)
                        accept = "application/json"; 
                    request.Accept = accept;
    
                }
                else
                {
                    request.KeepAlive = true;
                    request.Accept = accept;

                }
                try
                {
                    using (WebResponse webResponse = request.GetResponse() as HttpWebResponse)
                    {
                        if (webResponse == null)
                        {
                            return null;
                        }
                        else
                        {
                            StreamReader sr = new StreamReader(webResponse.GetResponseStream());
                            string sb = sr.ReadToEnd().Trim();
                            Results rs = new Results();
                            rs.Result = sb;
                            results.Add(rs);
                        }
                    }
                }
                catch (Exception ex)
                {

                    System.Diagnostics.Trace.WriteLine("Error with posting request: " + ex.Message);
                }
            }
            catch (Exception ex)
            {

                System.Diagnostics.Trace.WriteLine("error in makeRequest: " + ex.Message);
            }
            return results;


            
        }

        /// <summary>
        /// Performs a JSON based REST request
        /// </summary>
        /// <param name="method"></param>
        /// <param name="url"></param>
        /// <param name="options"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public List<Results> JSONRequest(string method, string url, List<NameValuePairs> options, string contentType)
        {
            List<Results> results = new List<Results>();

            try
            {
                results = makeRequest(method, url, options, contentType,"application/json");
            }
            catch (Exception ex)
            {

                System.Diagnostics.Trace.WriteLine("Error in JSONRequest: " + ex.Message); 
            }
            return results;

        }
        

        /// <summary>
        /// Most common way to talk to the server
        /// </summary>
        /// <param name="method"></param>
        /// <param name="url"></param>
        /// <param name="options"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public List<Results> StandardRequest(string method, string url, List<NameValuePairs> options, string contentType)
        {
            List<Results> results = new List<Results>();

            try
            {
                if (contentType == string.Empty)
                    results = makeRequest(method, url, options, string.Empty, "application/json");
                else
                    results = makeRequest(method, url, options, string.Empty, contentType); 
            }
            catch (Exception ex)
            {

                System.Diagnostics.Trace.WriteLine("Error in JSONRequest: " + ex.Message);
            }
            return results;
        }

        #endregion
    }
}
