using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace CodeGenGam
{
    class Class1
    {
        SqlConnection conn;
        StringBuilder sbm = new StringBuilder();
        const string NwLline = "\r\n";
        string tableName = "";
        string hierarchy = "";
        string CamelCaseName;
        private void button1_Click(object sender, EventArgs e)
        {
            //Helper h = new Helper();
            tableName = txtTableName.Text;
            CamelCaseName = GetCamelCase(tableName);
            string hierarchy = txtHierarchy.Text == string.Empty ? "" : "." + txtHierarchy.Text;

            writeTOdisk((filetype)comboBox1.SelectedIndex);
            MessageBox.Show("Done! ;-)");
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            conn = new SqlConnection(GetDBConnectionString());

        }

        enum QueryType
        {
            Property,
            Filter,
            Mapping
        }


        string GetQuery(QueryType q)
        {
            string QueryToReturn = "";

            #region "strPropertyQuery"
            string strPropertyQuery = @"
             
DECLARE @TableName VARCHAR(MAX) = '" + tableName + @"' -- Replace 'NewsItem' with your table name
DECLARE @TableSchema VARCHAR(MAX) = 'dbo' -- Replace 'Markets' with your schema name
DECLARE @result varchar(max) = ''
DECLARE @ByType VARCHAR(MAX) ='ByQuery';
--DECLARE @ByType VARCHAR(MAX) ='ByCommand';
DECLARE @Interface varchar(max)='IQuery'
DECLARE @NameSpace varchar(max)='BusinessEntities.Query';


--SET @result = @result + 'using System;' + CHAR(13) + CHAR(13) 
--SET @result = @result + 'using CQRS.Contracts;' + CHAR(13) + CHAR(13) 
--SET @result = @result + 'using System;' + CHAR(13) + CHAR(13) 
--SET @result = @result + 'using System;' + CHAR(13) + CHAR(13) 

--IF (@TableSchema IS NOT NULL) 
--BEGIN
--    SET @result = @result + 'namespace ' + @NameSpace  + CHAR(13) + '{' + CHAR(13) 
--END



SET @result = @result + '#region Instance Properties' + CHAR(13)  

SELECT @result = @result 
    + ' public ' + ColumnType + ' ' + ColumnName + ' { get; set; } ' + CHAR(13) 
FROM
(
    SELECT  c.COLUMN_NAME   AS ColumnName 
        , CASE c.DATA_TYPE   
            WHEN 'bigint' THEN
                CASE C.IS_NULLABLE
                    WHEN 'YES' THEN 'int64?' ELSE 'int64' END
            WHEN 'binary' THEN 'Byte[]'
            WHEN 'bit' THEN 
                CASE C.IS_NULLABLE
                    WHEN 'YES' THEN 'Boolean?' ELSE 'Boolean' END            
            WHEN 'char' THEN 'String'
            WHEN 'date' THEN
                CASE C.IS_NULLABLE
                    WHEN 'YES' THEN 'DateTime?' ELSE 'DateTime' END                        
            WHEN 'datetime' THEN
                CASE C.IS_NULLABLE
                    WHEN 'YES' THEN 'DateTime?' ELSE 'DateTime' END                        
            WHEN 'datetime2' THEN  
                CASE C.IS_NULLABLE
                    WHEN 'YES' THEN 'DateTime?' ELSE 'DateTime' END                        
            WHEN 'datetimeoffset' THEN 
                CASE C.IS_NULLABLE
                    WHEN 'YES' THEN 'DateTimeOffset?' ELSE 'DateTimeOffset' END                                    
            WHEN 'decimal' THEN  
                CASE C.IS_NULLABLE
                    WHEN 'YES' THEN 'Decimal?' ELSE 'Decimal' END                                    
            WHEN 'float' THEN 
                CASE C.IS_NULLABLE
                    WHEN 'YES' THEN 'Single?' ELSE 'Single' END                                    
            WHEN 'image' THEN 'Byte[]'
            WHEN 'int' THEN  
                CASE C.IS_NULLABLE
                    WHEN 'YES' THEN 'int?' ELSE 'int' END
            WHEN 'money' THEN
                CASE C.IS_NULLABLE
                    WHEN 'YES' THEN 'Decimal?' ELSE 'Decimal' END                                                
            WHEN 'nchar' THEN 'string'
            WHEN 'ntext' THEN 'string'
            WHEN 'numeric' THEN
                CASE C.IS_NULLABLE
                    WHEN 'YES' THEN 'Decimal?' ELSE 'Decimal' END                                                            
            WHEN 'nvarchar' THEN 'string'
            WHEN 'real' THEN 
                CASE C.IS_NULLABLE
                    WHEN 'YES' THEN 'Double?' ELSE 'Double' END                                                                        
            WHEN 'smalldatetime' THEN 
                CASE C.IS_NULLABLE
                    WHEN 'YES' THEN 'DateTime?' ELSE 'DateTime' END                                    
            WHEN 'smallint' THEN 
                CASE C.IS_NULLABLE
                    WHEN 'YES' THEN 'int?' ELSE 'int'END            
            WHEN 'smallmoney' THEN  
                CASE C.IS_NULLABLE
                    WHEN 'YES' THEN 'Decimal?' ELSE 'Decimal' END                                                                        
            WHEN 'text' THEN 'string'
            WHEN 'time' THEN 
                CASE C.IS_NULLABLE
                    WHEN 'YES' THEN 'TimeSpan?' ELSE 'TimeSpan' END                                                                                    
            WHEN 'timestamp' THEN 
                CASE C.IS_NULLABLE
                    WHEN 'YES' THEN 'DateTime?' ELSE 'DateTime' END                                    
            WHEN 'tinyint' THEN 
                CASE C.IS_NULLABLE
                    WHEN 'YES' THEN 'Byte?' ELSE 'Byte' END                                                
            WHEN 'uniqueidentifier' THEN 'Guid'
            WHEN 'varbinary' THEN 'Byte[]'
            WHEN 'varchar' THEN 'string'
            ELSE 'Object'
        END AS ColumnType
        , c.ORDINAL_POSITION 
FROM    INFORMATION_SCHEMA.COLUMNS c
WHERE   c.TABLE_NAME = @TableName and ISNULL(@TableSchema, c.TABLE_SCHEMA) = c.TABLE_SCHEMA  
) t
ORDER BY t.ORDINAL_POSITION

SET @result = @result + CHAR(13) + '#endregion Instance Properties' + CHAR(13)  

--SET @result = @result  + '}' + CHAR(13)

--IF (@TableSchema IS NOT NULL) 
--BEGIN
---    SET @result = @result + CHAR(13) + '}' 
--END
  Select  @result col1         
             
             ";
            #endregion "strCommand"

            #region "strFilterQuery"
            string strFilterQuery = @"
             
DECLARE @TableName VARCHAR(MAX) = 'eid_transactions' --  your table name
DECLARE @TableSchema VARCHAR(MAX) = 'dbo' --  schema name
DECLARE @result varchar(max) = ''

IF (@TableSchema IS NOT NULL) 
BEGIN
    SET @result = @result  + CHAR(13) 
END


SET @result = @result + '#region Filter Properties' + CHAR(13)  

SELECT @result = @result --+ CHAR(13) 
    + ' ' + Filter + ' ' + '' + ' ' + CHAR(13) 
FROM
(
    SELECT  c.COLUMN_NAME   AS ColumnName 
	, c.DATA_TYPE
        , CHAR(13) + CASE  WHEN  c.DATA_TYPE='text' or c.DATA_TYPE='ntext' or c.DATA_TYPE='varchar' or c.DATA_TYPE='char' or c.DATA_TYPE='nvarchar' or c.DATA_TYPE='nchar' or c.DATA_TYPE='xml' or c.DATA_TYPE='sysname'   
		THEN ' if (!string.IsNullOrWhiteSpace(query.'+c.COLUMN_NAME+'))'+ CHAR(13)+'{' 
		WHEN  c.DATA_TYPE='money' OR c.DATA_TYPE='uniqueidentifier' or c.DATA_TYPE='tinyint' or c.DATA_TYPE='smallint' or c.DATA_TYPE='int' or c.DATA_TYPE='real' or c.DATA_TYPE='float' or c.DATA_TYPE='decimal' or c.DATA_TYPE='numeric' or c.DATA_TYPE='smallmoney' or c.DATA_TYPE='bigint' or c.DATA_TYPE='hierarchyid' 
		THEN ' if (query.'+c.COLUMN_NAME+' > 0)'+ CHAR(13)+'{' 
		WHEN c.DATA_TYPE='image' or c.DATA_TYPE='date' or c.DATA_TYPE='time' or c.DATA_TYPE='datetime2' or c.DATA_TYPE='datetimeoffset' or c.DATA_TYPE='smalldatetime'  or c.DATA_TYPE='datetime' or c.DATA_TYPE='sql_variant' or c.DATA_TYPE='bit' or c.DATA_TYPE='geometry' or c.DATA_TYPE='geography' or c.DATA_TYPE='varbinary' or c.DATA_TYPE='binary' or c.DATA_TYPE='timestamp' 
			 THEN 'if (query.'+c.COLUMN_NAME+' != null)'+ CHAR(13)+'{' 
             ELSE '' END + CHAR(13)+
               ' var tempFunc = predicate;'+ CHAR(13)+'predicate = e => tempFunc(e) && e.'+c.COLUMN_NAME+' == query.'+c.COLUMN_NAME+';'+ CHAR(13)+
            '}' as Filter,
			
         c.ORDINAL_POSITION 
FROM    INFORMATION_SCHEMA.COLUMNS c
WHERE   c.TABLE_NAME = @TableName and ISNULL(@TableSchema, c.TABLE_SCHEMA) = c.TABLE_SCHEMA  
) t
ORDER BY t.ORDINAL_POSITION

SET @result = @result + CHAR(13) + '#endregion Filter Properties' + CHAR(13)  

SET @result = @result  +  CHAR(13)

IF (@TableSchema IS NOT NULL) 
BEGIN
    SET @result = @result + CHAR(13) 
END
  Select  @result col1         

";

            #endregion "strFilterQuery"

            #region "strMapping"
            string strMapping = @"Declare @TableName varchar(max)='eid_transactions';
                     SELECT  c.COLUMN_NAME+'=item.'+c.COLUMN_NAME   AS ColumnName 
                    FROM    INFORMATION_SCHEMA.COLUMNS c
                    WHERE   c.TABLE_NAME = @TableName
                    ORder by ORDINAL_POSITION ASC ";
            #endregion "strMapping"



            switch (q)
            {
                case QueryType.Property:
                    QueryToReturn = strPropertyQuery;
                    break;
                case QueryType.Filter:
                    QueryToReturn = strFilterQuery;
                    break;
                case QueryType.Mapping:
                    QueryToReturn = strMapping;
                    break;

            }


            return QueryToReturn;
        }

        string GetDBConnectionString()
        {
            string Constring = "";
            if (radioButton1.Checked == true) { Constring = ConfigurationManager.ConnectionStrings["Eid"].ToString(); }
            if (radioButton2.Checked == true) { Constring = ConfigurationManager.ConnectionStrings["McDonald"].ToString(); }
            return Constring;
        }


        string GetProperties(string tblname)
        {

            return "";
        }

        //private void button2_Click(object sender, EventArgs e)
        //{

        //    StringBuilder sb = new StringBuilder();


        //    switch (filetype.ByCommand)
        //    {
        //        case filetype.ByQuery:

        //            break;
        //        case filetype.ByCommand:
        //            break;
        //        case filetype.Result:
        //            break;
        //        case filetype.HandlerQ:
        //            break;
        //        case filetype.HandlerC:
        //            break;
        //        case filetype.RepositoryQI:
        //            break;
        //        case filetype.RepositoryQ:
        //            break;
        //        case filetype.RepositoryHI:
        //            break;
        //        case filetype.RepositoryH:
        //            break;

        //    }


        //}

        #region "Complete"

        string ByQuery(string str)
        {
            sbm = new StringBuilder();
            sbm.AppendLine("using System;" + NwLline);
            sbm.AppendLine("using CQRS.Contracts;" + NwLline);
            sbm.AppendLine("namespace BusinessEntity " + NwLline + " {" + NwLline + " public class " + GetCamelCase(tableName) + "ByQuery" + ":" + "IQuery" + NwLline + "{" + NwLline);
            sbm.AppendLine(NwLline);


            /*Add Properties*/
            str.Replace("private", " public ");
            str.Replace("System.", "");
            str.Replace("Nullable<DateTime>", " DateTime? ");
            str.Replace("Nullable<bool>", "bool?");
            str.Replace("Nullable<byte>", "byte?");
            str.Replace("Nullable<sbyte>", "sbyte?");
            str.Replace("Nullable<char>", "char?");
            str.Replace("Nullable<decimal>", "decimal?");
            str.Replace("Nullable<double>", "double?");
            str.Replace("Nullable<float>", "float?");
            str.Replace("Nullable<int>", "int?");
            str.Replace("Nullable<uint>", "uint?");
            str.Replace("Nullable<long>", "long?");
            str.Replace("Nullable<ulong>", "ulong?");
            str.Replace("Nullable<object>", "object?");
            str.Replace("Nullable<short>", "short?");
            str.Replace("Nullable<ushort>", "ushort?");
            str.Replace("Nullable<string>", "string?");
            str.Replace("Nullable<Guid>", "Guid?");
            str.Replace(";", " {get;set;} ");
            sbm.AppendLine(str + NwLline);

            sbm.AppendLine("/*Implements IQuery*/ " + NwLline);
            sbm.AppendLine("public RetrieveBy {get;set;}");
            sbm.AppendLine("}" + NwLline + "}");

            return sbm.ToString();
        }
        string ByCommand(string str)
        {
            sbm = new StringBuilder();
            sbm.AppendLine("using System;");
            sbm.AppendLine("using CQRS.Contracts;");
            sbm.AppendLine("namespace BusinessEntity." + txtHierarchy.Text + NwLline + " {");
            /*Comment*/
            sbm.AppendLine("");

            sbm.AppendLine(" public class " + GetCamelCase(tableName) + "ByCommand" + ":" + " ICommand" + NwLline + "{" + NwLline);

            /*Add Properties*/
            str.Replace("private", " public ");
            str.Replace("System.", "");
            str.Replace("Nullable<DateTime>", " DateTime? ");
            str.Replace("Nullable<bool>", "bool?");
            str.Replace("Nullable<byte>", "byte?");
            str.Replace("Nullable<sbyte>", "sbyte?");
            str.Replace("Nullable<char>", "char?");
            str.Replace("Nullable<decimal>", "decimal?");
            str.Replace("Nullable<double>", "double?");
            str.Replace("Nullable<float>", "float?");
            str.Replace("Nullable<int>", "int?");
            str.Replace("Nullable<uint>", "uint?");
            str.Replace("Nullable<long>", "long?");
            str.Replace("Nullable<ulong>", "ulong?");
            str.Replace("Nullable<object>", "object?");
            str.Replace("Nullable<short>", "short?");
            str.Replace("Nullable<ushort>", "ushort?");
            str.Replace("Nullable<string>", "string?");
            str.Replace("Nullable<Guid>", "Guid?");
            str.Replace(";", " {get;set;} ");
            sbm.AppendLine(str + NwLline);


            sbm.AppendLine("/*Implements ICommand*/ " + NwLline);
            sbm.AppendLine("public OperationType Action {get;set;}");
            sbm.AppendLine("}" + NwLline + "}");

            return sbm.ToString();
        }
        #endregion "Complete"
        string Result()
        {
            sbm = new StringBuilder();
            sbm.AppendLine("using System;" + NwLline);
            sbm.AppendLine("using CQRS.Contracts;" + NwLline);
            // sbm.AppendLine("namespace BusinessEntity " + NwLline + " {" + NwLline + " public class " + GetCamelCase(tableName) + "ByQuery" + ":" + "IQuery" + NwLline + "{" + NwLline);

            sbm.AppendLine("");

            /*Close Class & NAmespace*/
            sbm.AppendLine("}" + NwLline + "}");

            return sbm.ToString();
        }

        string HandlerQ(string filter)
        {

            sbm = new StringBuilder();
            sbm.AppendLine("using BusinessEntities.Query.Common;"
            + "using BusinessServices.QueryResult.Admin.Transaction;"
            + "using CQRS.Contracts;" + NwLline
            + "using DataServices.Admin.SystemSettings.Contract;" + NwLline
            + "using DataServices.Admin.Transaction.Contract;" + NwLline
            + "using DataServices.Common.Contract;" + NwLline
            + "using DataServices.EF;" + NwLline
            + "using System;" + NwLline
            + "using System.Collections.Generic;" + NwLline
            + "using System.Linq;" + NwLline
            + "using UtilitiesServices;" + NwLline
            + "using System.Net;" + NwLline
            + "using BusinessServices.QueryResult;" + NwLline
            + "using DataServices.Admin.Permission.Contract;" + NwLline
            + "using BusinessEntities.Query;" + NwLline);
            sbm.AppendLine("using BusinessServices.QueryResult.Common;" + NwLline +
           "using CQRS.Contracts;" + NwLline
           + "using DataServices.Common.Contract;" + NwLline
           + "using DataServices.EF;" + NwLline
           + "using System;" + NwLline
           + "using System.Collections.Generic;" + NwLline
           + "using System.Linq;" + NwLline
           + "using UtilitiesServices;" + NwLline);
            //sbm.AppendLine("namespace BusinessEntity " + NwLline + " {" + NwLline + " public class" + tableName+ ":" + "IQuery" + NwLline + "{" + NwLline);
            sbm.AppendLine("using BusinessEntities.Query" + txtHierarchy.Text + ";");


            string g = @"namespace BusinessServices.QueryResult" + hierarchy + @"
{
    /// <summary>
    /// Creating " + CamelCaseName + @"QueryHandler for Handling Queries
    /// </summary>
    /// <param name=""EIDBContext""></param>
    public class " + CamelCaseName + "QueryHandler : IQueryHandler<" + CamelCaseName + "ByQuery, " + CamelCaseName + @"QueryResult>
    {
        private readonly I" + CamelCaseName + "sRepositoryQuery " + CamelCaseName + @"RepositoryQuery;

        public " + CamelCaseName + "QueryHandler(I" + CamelCaseName + "sRepositoryQuery " + CamelCaseName + @"RepositoryQuery)
        {
            this." + CamelCaseName + "RepositoryQuery = " + CamelCaseName + @"RepositoryQuery;
        }

        public " + CamelCaseName + "QueryResult Retrieve(" + CamelCaseName + @"ByQuery query)
        {
            var result = new " + CamelCaseName + @"QueryResult();
            Func<" + tableName + @", bool> predicate = p => true;
            IEnumerable<" + tableName + @"> resultData;

           " + NwLline + filter + @"
           
try
            {

                if (query.ReteriveBy == CriteriaType.ComplexCriteria)
                {


                    resultData = " + CamelCaseName + "RepositoryQuery.Get" + CamelCaseName + @"sByFilter(predicate);
                

                    result.Get" + CamelCaseName + "List = resultData.Select(item => new " + CamelCaseName + @"ByQuery()
                    {
                       // loop
                    });
                }
                //
                else if (query.ReteriveBy == CriteriaType.SingleCriteria)
                {
                    var item = " + CamelCaseName + "RepositoryQuery.Get" + CamelCaseName + @"sByFilter(predicate).SingleOrDefault();
             result.Get" + CamelCaseName + " = new " + CamelCaseName + @"ByQuery{
             // loop
             
             };

                   
                }
            }

            catch (Exception)
            {


            }
            return result;
        }
    }
}
//";

            sbm.AppendLine(NwLline);
            sbm.AppendLine(NwLline);

            // sbm.AppendLine("}" + NwLline + "}");

            return sbm.ToString();
        }
        string HandlerC()
        {
            sbm = new StringBuilder();
            sbm.AppendLine("using System;" + NwLline);
            sbm.AppendLine("namespace BusinessEntity " + NwLline + " {" + NwLline + " public class" + tableName + ":" + "IQuery" + NwLline + "{" + NwLline);
            sbm.AppendLine(NwLline);
            sbm.AppendLine(NwLline);
            sbm.AppendLine("/*Implements IQuery*/ " + NwLline);
            sbm.AppendLine("public RetrieveBy {get;set;}");
            sbm.AppendLine("}" + NwLline + "}");

            return sbm.ToString();
        }
        string RepositoryQI()
        {
            sbm = new StringBuilder();
            sbm.AppendLine("using System;" + NwLline);
            sbm.AppendLine("namespace BusinessEntity " + NwLline + " {" + NwLline + " public class" + tableName + ":" + "IQuery" + NwLline + "{" + NwLline);
            sbm.AppendLine(NwLline);
            sbm.AppendLine(NwLline);
            sbm.AppendLine("/*Implements IQuery*/ " + NwLline);
            sbm.AppendLine("public RetrieveBy {get;set;}");
            sbm.AppendLine("}" + NwLline + "}");
            sbm.AppendLine("using DataServices.EF;" + NwLline);
            sbm.AppendLine("using DataServices.Infrastructure;" + NwLline);
            sbm.AppendLine("using System;" + NwLline);
            sbm.AppendLine("using System.Collections.Generic;" + NwLline);
            sbm.AppendLine("namespace DataServices." + hierarchy + ".Contract" + NwLline + "{" + NwLline);

            sbm.Append(@"
                /// <summary>
                /// Interface for retrieve " + CamelCaseName + @"RepositoryQuery
                /// </summary>");
            string body = @"//    public interface I" + CamelCaseName + "RepositoryQuery : IRepositoryQuery<" + tableName + @", int>
                {
                    // It is used to retrieve All columns and rows of " + tableName + @" 
                    IEnumerable<" + tableName + "> GetAll" + CamelCaseName + @"();
                    IEnumerable<" + tableName + "> Get" + CamelCaseName + "ByFilter(Func<" + tableName + @", bool> predicate);
                }
            }";
            return sbm.ToString();
        }
        string RepositoryQ()
        {
            sbm = new StringBuilder();
            sbm.AppendLine("using System;" + NwLline);
            sbm.AppendLine("namespace BusinessEntity " + NwLline + " {" + NwLline + " public class" + tableName + ":" + "IQuery" + NwLline + "{" + NwLline);
            sbm.AppendLine(NwLline);
            //sbm.AppendLine(NwLline);


            //using DataServices.Common.Contract;
            //using DataServices.EF;
            //using System;
            //using System.Collections.Generic;
            //using System.Linq;
            //using System.Text;
            //using System.Threading.Tasks;

            //namespace DataServices.Common.Manager
            ////{
            // /// <summary>
            //    /// Implementation of EID_IDMRule_Criteria table retreive opration 
            //    /// </summary>
            //    public class IDMRuleCriteriaRepositoryQuery : IIDMRuleCriteriaRepositoryQuery
            //    {
            //        private readonly EIDEntities eidDBContext;
            //        public IDMRuleCriteriaRepositoryQuery(EIDEntities EIDBContext)
            //        {
            //            eidDBContext = EIDBContext;
            //        }
            //        /// <summary>
            //        /// Get All records from EID_IDMRule_Criteria 
            //        /// </summary>
            //        /// <returns></returns>
            //        public IEnumerable<EID_IDMRule_Criteria> GetAllIDMRuleCriteria()
            //        {
            //            try
            //            {
            //                return eidDBContext.EID_IDMRule_Criteria.AsNoTracking();
            //            }
            //            catch (Exception) { throw; }
            //        }
            //        /// <summary>
            //        /// Get All records from EID_IDMRule_Criteria depending upon predicate 
            //        /// </summary>
            //        /// <param name="predicate"></param>
            //        /// <returns></returns>
            //        public IEnumerable<EID_IDMRule_Criteria> GetIDMRuleCriteriaByFilter(Func<EID_IDMRule_Criteria, bool> predicate)
            //        {
            //            try
            //            {
            //                return eidDBContext.EID_IDMRule_Criteria.AsNoTracking().Where(predicate);
            //            }
            //            catch (Exception) { throw; }

            //        }
            //        /// <summary>
            //        /// Get record from EID_IDMRule_Criteria depending upon id 
            //        /// </summary>
            //        /// <param name="id"></param>
            //        /// <returns></returns>
            //        public EID_IDMRule_Criteria FindBy(int id)
            //        {
            //            try
            //            {
            //                return eidDBContext.EID_IDMRule_Criteria.AsNoTracking().Where(item => item.RuleID == id).FirstOrDefault();
            //            }
            //            catch (Exception) { throw; }

            //        }

            //    }
            //}

            sbm.AppendLine("/*Implements IQuery*/ " + NwLline);
            sbm.AppendLine("public RetrieveBy {get;set;}");
            sbm.AppendLine("}" + NwLline + "}");

            return sbm.ToString();
        }
        string RepositoryHI()
        {
            sbm = new StringBuilder();
            sbm.AppendLine("using System;" + NwLline);
            sbm.AppendLine("namespace BusinessEntity " + NwLline + " {" + NwLline + " public class" + tableName + ":" + "IQuery" + NwLline + "{" + NwLline);
            sbm.AppendLine(NwLline);
            //          using DataServices.EF;
            //using DataServices.Infrastructure;
            //using System.Collections.Generic;

            //namespace DataServices.Admin.Transaction.Contract
            //{
            //   public interface IErrorLogRepositoryHandler:IRepositoryHandler<EID_Error_Log,string>
            //    {
            //        /// <summary>
            //        /// declaring interface members: delete error log method
            //        /// </summary>
            //        /// <returns></returns>
            //       void DeleteErrorLog(List<int> LogID);
            //    }
            //}
            sbm.AppendLine("/*Implements IQuery*/ " + NwLline);
            sbm.AppendLine("public RetrieveBy {get;set;}");
            sbm.AppendLine("}" + NwLline + "}");

            return sbm.ToString();
        }
        string RepositoryH()
        {
            sbm = new StringBuilder();
            sbm.AppendLine("using System;" + NwLline);
            sbm.AppendLine("namespace BusinessEntity " + NwLline + " {" + NwLline + " public class" + tableName + ":" + "IQuery" + NwLline + "{" + NwLline);
            sbm.AppendLine("" + NwLline);

            //            using DataServices.Admin.Permission.Contract;
            //using DataServices.EF;
            //using System;

            //namespace DataServices.Admin.Permission.Manager
            //{
            //    /// <summary>
            //    /// Insert, Update and Delete operation in EID_LoginRule_Criteria table 
            //    /// </summary>
            //    public class LoginRuleCriteriaRepositoryHandler : ILoginRuleCriteriaRepositoryHandler
            //    {
            //        private readonly EIDEntities eidDBContext;
            //        public LoginRuleCriteriaRepositoryHandler(EIDEntities EIDBContext)
            //        {
            //            eidDBContext = EIDBContext;
            //        }
            //        /// <summary>
            //        /// delete EID_LoginRule_Criteria record depending up on ID
            //        /// </summary>
            //        /// <param name="ID"></param>
            //        public void DeleteLoginRuleCriteria(int id)
            //        {
            //            try
            //            {
            //                eidDBContext.Configuration.AutoDetectChangesEnabled = false;
            //                var entity = new EID_LoginRule_Criteria { LoginRuleCriteriaID = id };
            //                eidDBContext.EID_LoginRule_Criteria.Attach(entity);
            //                eidDBContext.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
            //                eidDBContext.SaveChanges();
            //            }
            //            catch (Exception) { throw; }
            //            finally
            //            {
            //                eidDBContext.Configuration.AutoDetectChangesEnabled = true;
            //            }
            //        }
            //        /// <summary>
            //        /// save record in EID_LoginRule_Criteria
            //        /// </summary>
            //        /// <param name="entity"></param>
            //        /// <returns></returns>
            //        public Tuple<string, int> Save(EID_LoginRule_Criteria entity)
            //        {
            //            try
            //            {
            //                eidDBContext.Configuration.AutoDetectChangesEnabled = false;
            //                eidDBContext.EID_LoginRule_Criteria.Attach(entity);
            //                eidDBContext.Entry(entity).State = System.Data.Entity.EntityState.Added;
            //                eidDBContext.SaveChanges();
            //                return new Tuple<string, int>(string.Empty, entity.ID);
            //            }
            //            catch (Exception) { throw; }
            //            finally
            //            {
            //                eidDBContext.Configuration.AutoDetectChangesEnabled = true;
            //            }
            //        }
            //        /// <summary>
            //        /// Update record in EID_LoginRule_Criteria
            //        /// </summary>
            //        /// <param name="entity"></param>
            //        /// <returns></returns>
            //        public Tuple<string, int> Update(EID_LoginRule_Criteria entity)
            //        {
            //            try
            //            {
            //                eidDBContext.EID_LoginRule_Criteria.Attach(entity);
            //                eidDBContext.Entry(entity).State = System.Data.Entity.EntityState.Modified;
            //                eidDBContext.SaveChanges();
            //                return new Tuple<string, int>(string.Empty, entity.LoginRuleCriteriaID);
            //            }
            //            catch (Exception) { throw; }
            //        }
            //    }
            //}

            sbm.AppendLine("/*Implements IQuery*/ " + NwLline);
            sbm.AppendLine("public RetrieveBy {get;set;}");
            sbm.AppendLine("}" + NwLline + "}");

            return sbm.ToString();
        }

        public void writeTOdisk(filetype f)
        {
            string foldername = Application.StartupPath.ToString() + "\\";
            string filename = GetCamelCase(tableName);
            string strPropertiesOrFilter = "";

            StringBuilder sb = new StringBuilder();

            switch (f)
            {
                case filetype.ByQuery:
                    filename += "ByQuery.cs";
                    strPropertiesOrFilter = "";
                    strPropertiesOrFilter = GetDatabasealue(GetQuery(QueryType.Property));
                    //GetProperties(tableName).ToString();
                    sb.Append(ByQuery(strPropertiesOrFilter));
                    break;
                case filetype.ByCommand:
                    strPropertiesOrFilter = "";
                    strPropertiesOrFilter = GetDatabasealue(GetQuery(QueryType.Property));
                    filename += "ByCommand.cs";
                    sb.Append(ByCommand(strPropertiesOrFilter));
                    break;
                case filetype.Result:
                    filename += "ByQuery.cs";
                    sb.Append(Result());
                    break;
                case filetype.HandlerQ:
                    filename += "QueryResult.cs";
                    strPropertiesOrFilter = GetDatabasealue(GetQuery(QueryType.Filter));
                    sb.Append(HandlerQ(strPropertiesOrFilter));
                    break;
                case filetype.HandlerC:
                    filename += "Handler.cs";
                    sb.Append(HandlerC());
                    break;
                case filetype.RepositoryQI:
                    filename = "I" + filename + "RepositoryQuery.cs";
                    sb.Append(RepositoryQI());
                    break;
                case filetype.RepositoryQ:
                    filename += "RepositoryQuery.cs";
                    sb.Append(RepositoryQ());
                    break;
                case filetype.RepositoryHI:
                    filename = "I" + filename + "RepositoryHandler.cs";
                    sb.Append(RepositoryHI());
                    break;
                case filetype.RepositoryH:
                    filename += "RepositoryHandler.cs";
                    sb.Append(RepositoryH());
                    break;
                case filetype.All:
                    writeTOdisk(filetype.ByQuery);
                    writeTOdisk(filetype.ByCommand);
                    writeTOdisk(filetype.Result);
                    writeTOdisk(filetype.HandlerQ);
                    writeTOdisk(filetype.HandlerC);
                    writeTOdisk(filetype.RepositoryQI);
                    writeTOdisk(filetype.RepositoryQ);
                    writeTOdisk(filetype.RepositoryHI);
                    writeTOdisk(filetype.RepositoryH);
                    break;
            }
            if (!File.Exists(filename))
            {
                System.IO.File.WriteAllText(filename, sb.ToString());
            };

        }

        string GetCamelCase(string name)
        {
            bool boolCapitalize = true;
            string result = string.Empty;
            foreach (char c in name.ToCharArray())
            {

                if (boolCapitalize)
                {
                    result += char.ToUpper(c);
                    boolCapitalize = false;
                }
                else if (c == '_')
                {
                    boolCapitalize = true;
                }

                else
                {
                    result += char.ToLower(c);
                }

            }
            return result;
        }

        string GetDatabasealue(string Query)
        {
            using (SqlConnection newCon = new SqlConnection(GetDBConnectionString()))
            {
                //string queryString = "SELECT top 2 *  FROM "+tblname;
                DataSet ds = new DataSet();
                SqlConnection c = new SqlConnection(GetDBConnectionString());
                SqlDataAdapter a = new SqlDataAdapter(Query, c);
                a.Fill(ds);
                return ds.Tables[0].Rows[0][0].ToString();
            }

        }

        string Comment()
        {


            return "";
        }
    }
}
