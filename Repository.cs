using System;
using System.Collections.Generic;

namespace AllegroGraphCSharpClient
{
    /// <summary>
    /// Class to handle interacting with the repository
    /// WCF compatible, so if you use WCF, then you will always need to pass the url to every function
    /// Otherwise, you can set it using the constructor
    /// </summary>
    public class Repository : IRepository
    {
        private string _url = string.Empty;
        private object _environment = null;
        public Repository()
        {
        }

        public Repository(string url)
        {
            this._url = url; 
        }

        #region IRepository Members

        public Repository createRepository(string name)
        {
            this._url = name;
            return this;

        }

        public string getName()
        {
            return this._url;
        }

        public List<Results> jsonRequest(string method, string url)
        {
            Request rq = new Request();
            return rq.JSONRequest(method, url, null, null);
        }

        /// <summary>
        /// StandardRequest allows you to submit a basic request to the server
        /// </summary>
        /// <param name="method">Needs to be the standard REST methods: PUT, GET, etc.</param>
        /// <param name="url">URL of the server</param>
        /// <param name="options">Options to be passed in the HTTP Request</param>
        /// <param name="contentType">ContentType parameter</param>
        /// <returns></returns>
        public List<Results> StandardRequest(string method, string url, List<NameValuePairs> options, string contentType)
        {
            Request rq = new Request();
            return rq.StandardRequest(method, url, options, contentType);
        }

        /// <summary>
        /// Function to return the size of the statement pairs
        /// </summary>
        /// <param name="Context"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public long getSize(string Context, string url)
        {
            try
            {

                List<NameValuePairs> np = new List<NameValuePairs>();
                NameValuePairs nvp = new NameValuePairs();
                nvp.Name =  "context"; 
                nvp.Value = Context;
                np.Add(nvp);
                
                if(url == string.Empty) {
                  List<Results> results = new List<Results>();
                  results = this.StandardRequest("GET", this._url+"/size",np,null);
                  return (long)results[0].Result;
                }
                else{
                    List<Results> results = new List<Results>();
                    results = this.StandardRequest("GET", url + "/size",np,null);
                    return (long)results[0].Result;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// Returns the number of triples involved in a given context
        /// </summary>
        /// <param name="context"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public long ReturnNumberOfTriples(string context, string url)
        {
            return getSize(context, url); 
        }

        /// <summary>
        /// List all the contexts in the catalog
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public List<string> listContexts(string url)
        {
            try
            {
                if (url == string.Empty)
                {
                    List<Results> results = new List<Results>();
                    List<string> contexts = new List<string>();
                    results = jsonRequest("GET", this._url + "/contexts");
                    foreach (Results rs in results)
                    {
                        contexts.Add(rs.Result.ToString());
                    }
                    return contexts;
                }
                else
                {
                    List<Results> results = new List<Results>();
                    List<string> contexts = new List<string>();
                    results = jsonRequest("GET", url + "/contexts");
                    foreach (Results rs in results)
                    {
                        contexts.Add(rs.Result.ToString());
                    }
                    return contexts;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error in ListContexts:" + ex.Message);
                return null; 
            }
        }

        /// <summary>
        /// Returns if the repository is writable
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public bool isWritable(string url)
        {
            try
            {
                if (url == string.Empty)
                {
                    List<Results> results = new List<Results>();
                    results = jsonRequest("GET", this._url + "/writeable");
                    return Convert.ToBoolean(results[0].Result);
                }
                else
                {
                    List<Results> results = new List<Results>();
                    results = jsonRequest("GET", url + "/writeable");
                    return Convert.ToBoolean(results[0].Result);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error in isWritable:" + ex.Message);
                return false; 
            }

        }

        /// <summary>
        /// Takes a sparql string and returns the results in a list
        /// </summary>
        /// <param name="url"></param>
        /// <param name="query"></param>
        /// <param name="infer"></param>
        /// <param name="contexts"></param>
        /// <param name="namedContexts"></param>
        /// <param name="bindings"></param>
        /// <returns></returns>
        public List<Results> evalSparqlQuery(string url, string query, bool infer, List<NameValuePairs> contexts, List<NameValuePairs> namedContexts, List<NameValuePairs> bindings, string AdditonalPrefixes)
        {
            
            query = System.Web.HttpUtility.UrlEncode(@"PREFIX rdf:<http://www.w3.org/1999/02/22-rdf-syntax-ns#>  PREFIX rdfs:<http://www.w3.org/2000/01/rdf-schema#>  PREFIX wikipedia:<http://wikipedia.org/>  PREFIX foaf:<http://xmlns.com/foaf/0.1/>   PREFIX xsd:<http://www.w3.org/2001/XMLSchema#> PREFIX fn:<http://www.w3.org/2005/xpath-functions#>  PREFIX dc:<http://purl.org/dc/elements/1.1/>  PREFIX bio:<http://purl.org/vocab/bio/0.1/>  ")+ System.Web.HttpUtility.UrlEncode(AdditonalPrefixes) + System.Web.HttpUtility.UrlEncode(query);
            List<Results> results = new List<Results>();
            List<NameValuePairs> options = new List<NameValuePairs>(); 
            try
            {
                options.Add(new NameValuePairs("query",query));
                if (this._environment != null)
                {
                    options.Add(new NameValuePairs("enviornment", this._environment));
                }
                if (namedContexts != null)
                {
                    options.Add(new NameValuePairs("namedContext", namedContexts[0].Value));
                }
                if (bindings != null)
                {
                    options.Add(new NameValuePairs("bind", bindings));
                }
                if (infer)
                {
                    options.Add(new NameValuePairs("infer",infer));
                }
                if (contexts != null)
                {
                    foreach (NameValuePairs np in contexts)
                    {
                        options.Add(np);
                    }
                }
                if (url == string.Empty)
                {
                    return StandardRequest("GET", this._url, options, null);
                    
                }
                else
                {
                    return StandardRequest("GET", url, options, null);
                }
                
            }
            catch (Exception ex)
            {

                System.Diagnostics.Trace.WriteLine("Error in evalSparqlQuery " + ex.Message); 
            }
            return results;
        }

        /// <summary>
        /// Returns a Prologo Query Result
        /// </summary>
        /// <param name="url"></param>
        /// <param name="query"></param>
        /// <param name="infer"></param>
        /// <returns></returns>
        public List<Results> evalPrologQuery(string url, string query, bool infer, string AdditionalPrefixes)
        {
            List<Results> results = new List<Results>();
            List<NameValuePairs> options = new List<NameValuePairs>();
            try
            {
                options.Add(new NameValuePairs("query", query));
                options.Add(new NameValuePairs("enviornment", this._environment));
                options.Add(new NameValuePairs("queryLn", "prolog"));
                if (infer)
                    options.Add(new NameValuePairs("infer", infer)); 
                if (url == string.Empty)
                {
                    return StandardRequest("POST", this._url, options, null);

                }
                else
                {
                    return StandardRequest("POST", url, options, null);
                }

            }
            catch (Exception ex)
            {

                System.Diagnostics.Trace.WriteLine("Error in " + ex.Message);
            }
            return results;
        }

        /// <summary>
        /// Returns a list of all the statements that match the subj/pred/obj
        /// </summary>
        /// <param name="url"></param>
        /// <param name="subj"></param>
        /// <param name="pred"></param>
        /// <param name="obj"></param>
        /// <param name="contexts"></param>
        /// <param name="infer"></param>
        /// <returns></returns>
        public List<Results> getStatements(string url, string subj, string pred, string obj, List<NameValuePairs> contexts, bool infer)
        {
            List<Results> results = new List<Results>(); 
            try
            {
                List<NameValuePairs> options = new List<NameValuePairs>();
                options.Add(new NameValuePairs("subj", subj));
                options.Add(new NameValuePairs("pred", pred));
                options.Add(new NameValuePairs("obj", obj));
                options.Add(new NameValuePairs("context", contexts));
                options.Add(new NameValuePairs("infer",infer));
                if (url == string.Empty)
                {
                    results = StandardRequest("GET", this._url + "/statements", options, null); 
                }
                else
                {
                    results = StandardRequest("GET", url + "/statements", options, null);
                }
            }
            catch (Exception ex)
            {

                System.Diagnostics.Trace.WriteLine("Error in getStatements: " + ex.Message); 
            }
            return results;
        }

        /// <summary>
        /// Adds a statement to the repository
        /// </summary>
        /// <param name="url"></param>
        /// <param name="subj"></param>
        /// <param name="pred"></param>
        /// <param name="obj"></param>
        /// <param name="contexts"></param>
        /// <param name="ContentType"></param>
        /// <returns></returns>
        public bool addStatement(string url, string subj, string pred, string obj, List<NameValuePairs> contexts, string ContentType)
        {
            try
            {
                StandardRequest("POST", url, contexts, ContentType); 
                return true;
            }
            catch (Exception ex)
            {

                System.Diagnostics.Trace.WriteLine("Error in addStatemnt : " + ex.Message);
                return false;
            }
        }
        /// <summary>
        /// untested at this time
        /// </summary>
        /// <param name="url"></param>
        /// <param name="subj"></param>
        /// <param name="pred"></param>
        /// <param name="obj"></param>
        /// <param name="contexts"></param>
        /// <returns></returns>
        public bool deleteMatchingStatments(string url, string subj, string pred, string obj, List<NameValuePairs> contexts)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// untested at this time
        /// </summary>
        /// <param name="url"></param>
        /// <param name="quads"></param>
        /// <returns></returns>
        public bool addStatements(string url, List<Quads> quads)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// untested at this time
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public List<string> formatToURLFormatAndContentType(string format)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// untested at this time
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="format"></param>
        /// <param name="baseURI"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool loadData(string url, string data, string format, string baseURI, string context)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// untested at this time
        /// </summary>
        /// <param name="url"></param>
        /// <param name="fileURL"></param>
        /// <param name="format"></param>
        /// <param name="baseURI"></param>
        /// <param name="context"></param>
        /// <param name="serverSide"></param>
        /// <returns></returns>
        public bool loadFile(string url, string fileURL, string format, string baseURI, string context, bool serverSide)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// untested at this time
        /// </summary>
        /// <param name="url"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public List<string> getBlankNodes(string url, int quantity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// untested at this time
        /// </summary>
        /// <param name="url"></param>
        /// <param name="quads"></param>
        /// <returns></returns>
        public bool deleteStatements(string url, List<Quads> quads)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// untested at this time
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public List<string> listIndicies(string url)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// untested at this time
        /// </summary>
        /// <param name="url"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool addIndex(string url, string type)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// untested at this time
        /// </summary>
        /// <param name="url"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool deleteIndex(string url, string type)
        {
            throw new NotImplementedException();
        }



        /// <summary>
        /// untested at this time
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string getIndexCoverage(string url)
        {
            throw new NotImplementedException();
        }



        /// <summary>
        /// untested at this time
        /// </summary>
        /// <param name="url"></param>
        /// <param name="all"></param>
        /// <returns></returns>
        public List<string> indexStatements(string url, bool all)
        {
            throw new NotImplementedException();
        }



        /// <summary>
        /// untested at this time
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="infer"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public List<Results> evalFreeTextSearch(string pattern, bool infer, string url)
        {
            throw new NotImplementedException();
        }



        /// <summary>
        /// untested at this time
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public List<Results> listFreeTextPredicates(string url)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// untested at this time
        /// </summary>
        /// <param name="url"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public bool registerFreeTextPredicate(string url, string predicate)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// untested at this time
        /// </summary>
        /// <param name="url"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool createEnvironment(string url, string name)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// untested at this time
        /// </summary>
        /// <param name="url"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool deleteEnvironment(string url, string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// untested at this time
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public List<string> listNamespaces(string url)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// untested at this time
        /// </summary>
        /// <param name="url"></param>
        /// <param name="prefix"></param>
        /// <param name="nameSpace"></param>
        /// <returns></returns>
        public bool addNamespace(string url, string prefix, string nameSpace)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// untested at this time
        /// </summary>
        /// <param name="url"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public bool deleteNamespace(string url, string prefix)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
