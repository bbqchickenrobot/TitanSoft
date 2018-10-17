﻿using System;
using Raven.Client.Documents;
using Raven.Embedded;

namespace TitanSoft.DataAccess
{
    public class DocumentStoreHolder
    {
        private static Lazy<IDocumentStore> store = new Lazy<IDocumentStore>(CreateStore);

        public static IDocumentStore Store => store.Value;

        private static IDocumentStore CreateStore()
        {
            var serverOptions = new ServerOptions()
            {
                ServerUrl = "http://127.0.0.1:60956/",
            };

            EmbeddedServer.Instance.StartServer(serverOptions);

            return EmbeddedServer.Instance.GetDocumentStore("TitanSoftStore");
        }
    }
}