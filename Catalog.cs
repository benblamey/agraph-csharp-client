using System;
using System.Collections.Generic;

namespace AllegroGraphCSharpClient
{
    // NOTE: If you change the class name "Catalog" here, you must also update the reference to "Catalog" in App.config.
    /// <summary>
    /// Catalog class handles all interaction with the catalog object
    /// Please note that if you use wcf, you will always need to pass the url and potentially username/password
    /// Otherwise you can set it using the constructor
    /// </summary>
    public class Catalog : ICatalog
    {

        #region ICatalog Members

        private string _url;
        private string _username;
        private string _password;
        private string _curl;

        /// <summary>
        /// Initializes the catalog class
        /// </summary>
        /// <param name="url"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool InitializeCatalogClass(string url, string username, string password)
        {
             _url =   url;
             _username = username;
             _password = password;
             return true; 
        }

        /// <summary>
        /// return url for the service
        /// </summary>
        /// <returns></returns>
        public string getUrl()
        {
            return this._url;
        }

        /// <summary>
        /// List the triple stores at the current url
        /// </summary>
        /// <returns></returns>
        public List<string> listTripleStores(string url)
        {
            if (url != string.Empty)
            {
                this._url = url;
            }
            List<string> stores = new List<string>();
            try
            {
                Request request = new Request();
                List<Results> rs = new List<Results>(); 
                rs = request.StandardRequest("GET", this._url + @"/repositories", null, null);
                foreach (Results result in rs)
                {
                    stores.Add(result.Result.ToString());
                }
            }
            catch (Exception ex)
            {

                System.Diagnostics.Trace.WriteLine("Error in listTripleStores : " + ex.Message);
            }

            return stores; 
        }

        /// <summary>
        /// Initialize new triple store
        /// </summary>
        /// <param name="name"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public bool createTripleStore(string name, string url)
        {
            try
            {
                if (url != string.Empty)
                {
                    this._url = url;
                }
                Request request = new Request();
                request.StandardRequest("PUT", this._url + @"/repositories/" + legalizeName(name,this._url), null, null);
                return true; 
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Error in create triple store : " + ex.Message);
                return false;
            }
        }

        /// <summary>
       ///  Federate the triple store, untested at this time
        /// </summary>
        /// <param name="name"></param>
        /// <param name="storeNames"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public bool federateTripleStores(string name, List<string> storeNames, string url)
        {
            try
            {
                if (url != string.Empty)
                {
                    this._url = url;
                }
                Request request = new Request();
                List<NameValuePairs> nvp = new List<NameValuePairs>();
                NameValuePairs np = new NameValuePairs();
                np.Name = "federate";
                np.Value = (object)storeNames;
                nvp.Add(np);
                request.StandardRequest("PUT", this._url + @"/repositories/" + legalizeName(name,this._url), nvp, string.Empty); 
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Error in federateTripleStores: " + ex.Message);
                return false;
            }
        }

        public bool deleteTripleStore(string storeName, string url)
        {
            try
            {
                if (url != string.Empty)
                {
                    this._url = url;
                }
                Request request = new Request();
                request.StandardRequest("DELETE", this._url + @"/repositories/" + legalizeName(storeName,this._url), null, null); 
                return true;
            }
            catch (Exception ex)
            {
                
                System.Diagnostics.Trace.WriteLine("Error in deleteTripleStore: " + ex.Message);
                return false;
            }
        }

        public Repository getRepository(string name, string url)
        {

            if (url != string.Empty)
            {
                this._url = url;
            }
            return new Repository(this._url + @"/repositories/" + legalizeName(name,this._url)); 
        }

        public string legalizeName(string name, string url)
        {
            return name;
        }

        #endregion
    }
}
