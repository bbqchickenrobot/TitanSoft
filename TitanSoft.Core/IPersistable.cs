using System;
namespace TitanSoft.DataAccess
{
    /// <summary>
    /// Persistable interface that has an ID property of type string
    /// because RavenDB (used for this example) makes use of them for
    /// it's primary key field type or would use generics here
    /// </summary>
    public interface IPersistable
    {
        string Id { get;}
    }
}
