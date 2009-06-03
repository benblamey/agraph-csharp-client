Welcome to AllegroGraph C# Client

This library is an C# version of the core java library for
programmatically interacting with the AllegroGraph, from Franz, Inc.
This is the initial version of the library, and additional features
will be added over time.

The purpose of this library was to fill a major requirement for
projects that I work on, which is that they need to be .NET
compatible.  I have decided to implement it so that it is both a
straight library and also compatible with WCF.  This enables the
client to work on both Mono and Visual Studio, and is setup to be
utilized in a more SOA type of environment via WCF.  Whether it ever
will get to the latter SOA use, who knows.  However, it was very easy
to implement.

The client is is freely to use it in your projects.  My request is
that if you find bugs or wish to ask for enhancements, that you either
email me with the information or the fix.  The client, in its current
form, has the ability to create and get information about catalogs,
return repository information, and to perform sparql and prolog
queries.  Future versions will complete the stub functions for the
create, update, and deletion of actual statements inside the
repository.


There are four main classes that make up the library.  I have included
a basic JSON utility class that takes the results from a query, which
is usually a json array, and translates it to a ADO.NET datatable.
You can use other json libraries, but this was made to fix a quick
need for several projects we are ramping up internally.

The typical class you will interact with is the Repository class,
which handles displaying the statements in your ontology as well as
exposes the query functionality.  You will typically initialize a
repository object like this:

  AllegroGraphCSharpClient.Repository rep = 
		new AllegroGraphCSharpClient.Repository(@"http://webserv:8000/catalogs/Test/repositories/Tools");

where the Repository object expects you to initialize with the url of
the allegrograph repository in the format of:

	http://<web server name>:<http port for allegrograph>/catalogs/<repository Name>/repositories/<ontology>


You can then create a results object, which is what every function
returns either a single AllegroGraphCSharpClientResults object or a
List<AllegroGraphCSharpClient.Results>.  For example, to create a List
of Results the following syntax:

	List<AllegroGraphCSharpClient.Results> results = new List<AllegroGraphCSharpClient.Results>(); 

This results object has a single property, a string called "result".
I have added to both evalSparqlQuery and evalPrologQuery a parameter
called AdditionalPrefixes, which allows you to define additional
references that your ontology references.  An example string would
look like:

"PREFIX fn:<http://www.w3.org/2005/xpath-functions#>  PREFIX bio:<http://purl.org/vocab/bio/0.1/>"
which defines two namespaces fn and bio.

To find additional information about the function calls, you can look
at Franz's documentation at:

http://www.franz.com/agraph/support/documentation/current/http-protocol.html.

Another class is the catalog class, which is a basic class that will
return information about a catalog, list the statements within a
catalog, create a catalog, and delete a catalog.

The final class, a low level class, called Request, handles all the
REST interaction between the server and the client, including the
relevant GET/PUT/POST/DELETE statements.
