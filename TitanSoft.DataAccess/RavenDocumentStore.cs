using System;
using System.IO;
using Raven.Client.Documents;
using Raven.Embedded;

namespace TitanSoft.DataAccess
{
    public static class RavenDocumentStore
    {
        static readonly Lazy<IDocumentStore> store = new Lazy<IDocumentStore>(CreateStore);

        public static IDocumentStore Store { get => store.Value; }

        static IDocumentStore CreateStore()
        {
            var serverOptions = new ServerOptions()
            {
                ServerUrl = "http://127.0.0.1:60956/",
                DataDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Data")
            };

            EmbeddedServer.Instance.StartServer(serverOptions);

            return EmbeddedServer.Instance.GetDocumentStore("TitanSoftStore");
        }
    }
}
