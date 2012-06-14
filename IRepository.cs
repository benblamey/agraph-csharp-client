using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace AllegroGraphCSharpClient
{
    // NOTE: If you change the interface name "IRepository" here, you must also update the reference to "IRepository" in App.config.
    [ServiceContract]
    public interface IRepository
    {
        [OperationContract]
        Repository createRepository(string name);

        [OperationContract]
        string getName();

        [OperationContract]
        List<Results> jsonRequest(string method, string url);

        [OperationContract]
        List<Results> StandardRequest(string method, string url, List<NameValuePairs> options, string contentType);

        [OperationContract]
        long getSize(string Context, string url);

        [OperationContract]
        long ReturnNumberOfTriples(string context, string url);

        [OperationContract]
        List<string> listContexts(string url);

        [OperationContract]
        bool isWritable(string url);

        [OperationContract]
        List<Results> evalSparqlQuery(string url, string query, bool infer, List<NameValuePairs> contexts, List<NameValuePairs> namedContexts, List<NameValuePairs> bindings, string AdditionalPrefixes);

        [OperationContract]
        List<Results> evalPrologQuery(string url, string query, bool infer, string AdditionalPrefixes);

        [OperationContract]
        List<Results> getStatements(string url, string subj, string pred, string obj, List<NameValuePairs> contexts, bool infer);

        [OperationContract]
        bool addStatement(string url, string subj, string pred, string obj, List<NameValuePairs> contexts, string contenttype);

        [OperationContract]
        bool deleteMatchingStatments(string url, string subj, string pred, string obj, List<NameValuePairs> contexts);

        [OperationContract]
        bool addStatements(string url, List<Quads> quads);

        [OperationContract]
        List<string> formatToURLFormatAndContentType(string format);

        [OperationContract]
        bool loadData(string url, string data, string format, string baseURI, string context);

        [OperationContract]
        bool loadFile(string url, string fileURL, string format, string baseURI, string context, bool serverSide);

        [OperationContract]
        List<string> getBlankNodes(string url, int quantity);

        [OperationContract]
        bool deleteStatements(string url, List<Quads> quads);

        [OperationContract]
        List<string> listIndicies(string url);

        [OperationContract]
        bool addIndex(string url, string type);

        [OperationContract]
        bool deleteIndex(string url, string type);

        [OperationContract]
        string getIndexCoverage(string url);

        [OperationContract]
        List<string> indexStatements(string url, bool all);

        [OperationContract]
        List<Results> evalFreeTextSearch(string pattern, bool infer, string url);

        [OperationContract]
        List<Results> listFreeTextPredicates(string url);

        [OperationContract]
        bool registerFreeTextPredicate(string url, string predicate);

        [OperationContract]
        bool createEnvironment(string url, string name); 

        [OperationContract]
        bool deleteEnvironment(string url, string name);

        [OperationContract]
        List<string> listNamespaces(string url);

        [OperationContract]
        bool addNamespace(string url, string prefix, string nameSpace);

        [OperationContract]
        bool deleteNamespace(string url, string prefix);

    }

    [DataContract]
    public class Quads
    {
        string element1;
        string element2;
        string element3;
        string contextName;
    }
}
