using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenGam
{
    #region "Enums"
    enum DbObjects
    {
        tables,
        Views,
        SPs

    }
    enum DirectoryName
    {
        BusinessServices,
        BusinessEntities,
        DataServices,
        UtilitiesServices

    }

    enum NamespaceType
    {
        Common,
        Collection,
        Query,
        Command,
        Result,
        EF,
        QueryHandler,
        CommandHandler,
        RepositoryContract,
        RepositoryManager,
        RepositoryQ,
        RepositoryH

    }
    public enum QueryType
    {
        Property,
        Filter,
        ExpressionFilter,
        Mapping,
        Pkey
    }

    public enum CommentFormatType
    {
        Class,
        Method,
        Closure
    }

    public enum Filetype
    {
        ByQuery,
        ByCommand,
        QueryResult,
        QueryHandler,
        CommandHandler,
        RepositoryQI,
        RepositoryQ,
        RepositoryHI,
        RepositoryH,
        All

        /*
QueryResult
QueryHandler
CommandHandler
Contract-RepositoryQuery
Manager-RepositoryQuery
Contract-RepositoryHandler
Manager-RepositoryHandler
All*/
    }

    public enum CommentType
    {
        Custom,
        Collection,
        QueryClass,
        CommandClass,
        ResultClass,
        QueryHandlerClass,
        CommandHandlerClass,
        RepositoryQContractClass,
        RepositoryQManagerClass,
        RepositoryHContractClass,
        RepositoryHManagerClass,
        Execute,
        Insert,
        Update,
        Delete,
        Insert_I,
        Update_I,
        Delete_I,
        GetAll,
        GetSingle,
        GetAllByPredicate,
        GetAllByPrimaryKey,
        Generic,
        Alias,
        Close

    }
    #endregion "Enums"
}
