using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;
using System.Net;
using System.Web;
namespace AllegroGraphCSharpClient
{
    [ServiceContract]
    public interface IRESTInterface
    {
        [OperationContract]
        HttpWebRequest makeHttpRequest(string method, string url, List<NameValuePairs> options);

        [OperationContract]
        List<Results> makeRequest(string method, string url, List<NameValuePairs> options,string contentType, string accept);


        [OperationContract]
        List<Results> JSONRequest(string method, string url, List<NameValuePairs> options, string contentType);

        [OperationContract]
        List<Results> StandardRequest(string method, string url, List<NameValuePairs> options, string contentType); 

    }
    [DataContract]
    public class Results
    {
        public object Result;
    }

    [DataContract]
    public class NameValuePairs{
        public string Name;
        public object Value;

        public NameValuePairs(string name, object value)
        {
            Name = name;
            Value = value;
        }

        public NameValuePairs()
        {
            Name = string.Empty;
            Value = new object(); 
        }
    }
    
}
