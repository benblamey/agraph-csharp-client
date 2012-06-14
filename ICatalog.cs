using System.Collections.Generic;
using System.ServiceModel;

namespace AllegroGraphCSharpClient
{
    // NOTE: If you change the interface name "ICatalog" here, you must also update the reference to "ICatalog" in App.config.
    [ServiceContract]
    public interface ICatalog
    {
        [OperationContract]
        string getUrl();

        [OperationContract]
        List<string> listTripleStores(string url);

        [OperationContract]
        bool createTripleStore(string name,string url);

        [OperationContract]
        bool federateTripleStores(string name, List<string> storeNames, string url);

        [OperationContract]
        bool deleteTripleStore(string storeName, string url);

        [OperationContract]
        Repository getRepository(string name, string url);

        [OperationContract]
        string legalizeName(string name, string url);

        [OperationContract]
        bool InitializeCatalogClass(string url, string username, string password); 
    }
}
