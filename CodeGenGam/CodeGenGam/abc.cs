using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace gogo
{
    public partial class Screen1: Form
    {
        #region "Page"
        public Screen()
        {
            InitializeComponent();
        }
        #endregion "Page"

        #region "Fields"
        SqlConnection conn;
        StringBuilder sbm = new StringBuilder();
        const string NwLline = "\r\n";
        string tableName = "";
        string hierarchy = "";
        string CamelCaseName = string.Empty;
        string context = string.Empty;
        string Entitystring = string.Empty;
        string Contextstring = string.Empty;
        string primarykey = "";
        string primarykeyDatatype = "";


        enum NamespaceType
        {
            Common,
            Collection,
            Query,
            Command,
            Result,
            EF,
            repositoryContract,
            repositoryManager,
            Handler,
            repositoryQ,
            repositoryH

        }
        public enum QueryType
        {
            Property,
            Filter,
            Mapping,
            pkey
        }

        public enum filetype
        {
            ByQuery,
            ByCommand,
            Result,
            HandlerQ,
            HandlerC,
            RepositoryQI,
            RepositoryQ,
            RepositoryHI,
            RepositoryH,
            All
        }

        enum CommentType
        {
            QueryComment,
            ClassComment,
            RepositoryComment,
            HandlerComment,
            ResultComment
        }
        #endregion "Fields"

        #region "pagemethods"
        private void button1_Click(object sender, EventArgs e)
        {
            //Helper h = new Helper();
            tableName = txtTableName.Text;
            CamelCaseName = GetCamelCase(tableName);
             hierarchy = txtHierarchy.Text == string.Empty ? "" : "." + txtHierarchy.Text.Replace(@"\", @".");

            writeTOdisk((filetype)comboBox1.SelectedIndex);
            MessageBox.Show("Done! ;-)");
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // conn = new SqlConnection(GetDBConnectionString());
            //"+ +@" "+ Contextstring+@"

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

        #endregion

        #region "Complete"
        public void writeTOdisk(filetype f)
        {
            string foldername = Application.StartupPath.ToString() + "\\";
            string filename = CamelCaseName;
            string strPropertiesOrFilter, strFilter,strMapping = "";
             Entitystring = (radioButton1.Checked == true) ? ""+ "Eidentities"+@"" : "McDEntities";
             Contextstring = (radioButton1.Checked == true) ? "" + "EidDBContext" + @"" : "McDDBContext";

             DataRow getPrimarykeyInfo = GetDatabaseValues(GetQuery(QueryType.pkey));
             primarykey = getPrimarykeyInfo["pkey"].ToString();
             primarykeyDatatype = getPrimarykeyInfo["datatype"].ToString();
             primarykey=primarykey==""?" ID " :primarykey;
             primarykeyDatatype=primarykeyDatatype=="" ?" int " :primarykeyDatatype;
             
             strPropertiesOrFilter = GetDatabaseValues(GetQuery(QueryType.Property))["col1"].ToString();
             strPropertiesOrFilter = CorrectProperties(strPropertiesOrFilter);
             strFilter = GetDatabaseValues(GetQuery(QueryType.Filter))["col1"].ToString();
            strMapping=GetDatabaseValues(GetQuery(QueryType.Mapping))["col1"].ToString();

            StringBuilder sb = new StringBuilder();

            switch (f)
            {
                case filetype.ByQuery:
                    filename += "ByQuery.cs";
                    sb.Append(ByQuery(strPropertiesOrFilter));
                    break;
                case filetype.ByCommand:
                   filename += "ByCommand.cs";
                    sb.Append(ByCommand(strPropertiesOrFilter));
                    break;
                case filetype.Result:
                    filename += "QueryResult.cs";
                    sb.Append(Result());
                    break;
                case filetype.HandlerQ:
                    filename += "QueryHandler.cs";
                    sb.Append(HandlerQ(strFilter, strMapping));
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

        string ByQuery(string strProperties)
        {
            sbm = new StringBuilder();
            /*Add namespace reference*/
            sbm.AppendLine(GetNamespaces(NamespaceType.Query));
            sbm.AppendLine("namespace BusinessEntity.Query"+hierarchy + NwLline + " {");
            sbm.AppendLine(GetComment(CommentType.ClassComment));
            sbm.AppendLine("   public class " + GetCamelCase(tableName) + "ByQuery" + ":" + "IQuery" + NwLline + "{");
            sbm.AppendLine("#region Instance Properties");
           
            sbm.AppendLine(strProperties);

            sbm.AppendLine("/*Implements IQuery*/ ");
            sbm.AppendLine("public OperationType RetrieveBy {get;set;}");
            sbm.AppendLine(" #endregion Instance Properties");
            sbm.AppendLine("}" + NwLline + "}");

            return sbm.ToString();
        }
        string ByCommand(string strProperties)
        {
            sbm = new StringBuilder();
            /*Add namespace reference*/
            sbm.AppendLine(GetNamespaces(NamespaceType.Command));
            sbm.AppendLine("namespace BusinessEntity.Command" +hierarchy + NwLline + "{");
            /*Comment*/
            sbm.AppendLine(GetComment(CommentType.ClassComment));

            sbm.AppendLine("    public class " + GetCamelCase(tableName) + "ByCommand" + ":" + " ICommand" + NwLline + "{");

            strProperties = CorrectProperties(strProperties);

            sbm.AppendLine(strProperties);


            sbm.AppendLine("/*Implements ICommand*/ ");
            sbm.AppendLine("public OperationType Action {get;set;}");
            sbm.AppendLine("}" + NwLline + "}");

            return sbm.ToString();
        }

        string Result()
        {
            sbm = new StringBuilder();
            /*Add namespace reference*/
            sbm.AppendLine(GetNamespaces(NamespaceType.Result));
            /*Add namespace reference*/
            sbm.AppendLine(GetComment(CommentType.ResultComment));
            sbm.AppendLine("namespace Businessservices" + hierarchy + NwLline + " {");
            sbm.AppendLine("    public class " + CamelCaseName + "ByQuery" + ":" + "IQuery" + NwLline + "{");
            sbm.AppendLine("IEnumerable<" + CamelCaseName + "ByQuery> GetAll" + CamelCaseName + "();");
            sbm.AppendLine(CamelCaseName + "ByQuery Get" + CamelCaseName + "();");
            /*Close Class & NAmespace*/
            sbm.AppendLine("}" + NwLline + "}");

            return sbm.ToString();
        }

        string HandlerQ(string filter, string Mapping)
        {

            sbm = new StringBuilder();
            sbm.AppendLine(GetNamespaces(NamespaceType.Handler));
            sbm.AppendLine(GetComment(CommentType.HandlerComment));
            //sbm.AppendLine("namespace BusinessEntity " + NwLline + " {" + NwLline + " public class" + tableName+ ":" + "IQuery" + NwLline + "{" );

       sbm.AppendLine(@"namespace BusinessServices.QueryHandler" + hierarchy + @"
{
    
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

           ");
            sbm.Append(filter);
            sbm.Append(@" try
            {

            if (query.ReteriveBy == CriteriaType.NoCriteria)
                {
                resultData = " + CamelCaseName + "RepositoryQuery.GetAll" + CamelCaseName + @";
                result.Get" + CamelCaseName + "List = resultData.Select(item => new " + CamelCaseName + @"ByQuery()
                {");

            sbm.AppendLine(Mapping);
            sbm.AppendLine(@"
                });
                } //End NoCriteria

            
                
              else if (query.ReteriveBy == CriteriaType.SingleCriteria)
                {
                  var item = " + CamelCaseName + "RepositoryQuery.Get" + CamelCaseName + @"sByFilter(predicate).SingleOrDefault();
                  result.Get" + CamelCaseName + " = new " + CamelCaseName + @"ByQuery{
                ");
            sbm.AppendLine(Mapping);
            sbm.AppendLine(@"
             };
            } //End SingleCriteria
            else if (query.ReteriveBy == CriteriaType.ComplexCriteria)
                {
                resultData = " + CamelCaseName + "RepositoryQuery.Get" + CamelCaseName + @"sByFilter(predicate);
                result.Get" + CamelCaseName + "List = resultData.Select(item => new " + CamelCaseName + @"ByQuery()
                {");

            sbm.AppendLine(Mapping);
            sbm.AppendLine(@"
                }); 
                } //End ComplexCriteria

            } //End Try

            catch (Exception){throw;}
            return result;
        } //End Retrieve
    } //End EndClass
} //End Namespace ");




            // sbm.AppendLine("}" + NwLline + "}");

            return sbm.ToString();
        }
        string RepositoryQI()
        {
            sbm = new StringBuilder();
            sbm.AppendLine(GetNamespaces(NamespaceType.repositoryContract));
            
            sbm.AppendLine("namespace DataServices" + hierarchy + ".Contract" + NwLline + "{");
            sbm.AppendLine(GetComment(CommentType.RepositoryComment));
            string body = @" public interface I" + CamelCaseName + "RepositoryQuery : IRepositoryQuery<" + tableName + ", "+primarykeyDatatype+@">
                {
                    // It is used to retrieve All columns and rows of " + tableName + @" 
                    IEnumerable<" + tableName + "> GetAll" + CamelCaseName + @"();
                    IEnumerable<" + tableName + "> Get" + CamelCaseName + "ByFilter(Func<" + tableName + @", bool> predicate);
                }
            }";

            sbm.AppendLine(body);
            return sbm.ToString();
        }
        #endregion "Complete"

        #region "InProgress"
        string RepositoryQ()
        {
            sbm = new StringBuilder();
            sbm.AppendLine(GetNamespaces(NamespaceType.repositoryQ));
            sbm.AppendLine(@"

            namespace DataServices" + hierarchy + @".Manager
            {
             /// <summary>
                /// Implementation of " + tableName + @" table retreive opration 
                /// </summary>
                public class " + CamelCaseName + @"RepositoryQuery : I" + CamelCaseName + @"RepositoryQuery
                {
                    private readonly " + Entitystring + @" " + Contextstring + @";
                    public " + CamelCaseName + @"RepositoryQuery(" + Entitystring + @" " + Contextstring.ToLower() + @")
                    {
                        " + Contextstring + @" = " + Contextstring.ToLower() + @";
                    }
                    /// <summary>
                    /// Get All records from " + tableName + @" 
                    /// </summary>
                    /// <returns></returns>
                    public IEnumerable<" + tableName + @"> GetAll" + CamelCaseName + @"()
                    {
                        try
                        {
                            return " + Contextstring + @"." + tableName + @".AsNoTracking();
                        }
                        catch (Exception) { throw; }
                    }
                    /// <summary>
                    /// Get All records from " + tableName + @" depending upon predicate 
                    /// </summary>
                    /// <param name=""predicate""></param>
                    /// <returns></returns>
                    public IEnumerable<" + tableName + @"> Get" + CamelCaseName + @"ByFilter(Func<" + tableName + @", bool> predicate)
                    {
                        try
                        {
                            return " + Contextstring + @"." + tableName + @".AsNoTracking().Where(predicate);
                        }
                        catch (Exception) { throw; }

                    }
                    /// <summary>
                    /// Get record from " + tableName + @" depending upon " + primarykey + @"
                    /// </summary>
                    /// <param name=" + primarykey + @"></param>
                    /// <returns></returns>
                    public " + tableName + @" FindBy(" + primarykeyDatatype + @" PkeyValue)
                    {
                        try
                        {
                            return " + Contextstring + @"." + tableName + @".AsNoTracking().Where(item => item.@" + primarykey + @" == PkeyValue).FirstOrDefault();
                        }
                        catch (Exception) { throw; }

                    }

                }
            }"
    );



            return sbm.ToString();
        }

        #endregion

        #region "Pending"

        string HandlerC()
        {
            sbm = new StringBuilder();
            sbm.AppendLine(GetNamespaces(NamespaceType.Handler));

            sbm.AppendLine("namespace BusinessService "+hierarchy + NwLline + " {");
            sbm.AppendLine("    public class" + tableName + "CommandHandler:" + "ICommandHandler" + NwLline + "{");
            sbm.AppendLine(NwLline);
           
            sbm.AppendLine(" To be Completed" );

            sbm.AppendLine("}" + NwLline + "}");

            return sbm.ToString();
        }

        string RepositoryHI()
        {
            sbm = new StringBuilder();
            sbm.AppendLine("using System;");
            sbm.AppendLine("namespace BusinessEntity " + NwLline + " {" + NwLline + " public class" + tableName + ":" + "IQuery" + NwLline + "{");
            sbm.AppendLine(NwLline);
            sbm.AppendLine(@"
            namespace DataServices." + hierarchy + @".Contract
            {
               public interface I" + CamelCaseName + "RepositoryHandler:IRepositoryHandler<" + tableName + @",string>
                {
                    /// <summary>
                    /// declaring interface members: delete " + CamelCaseName + @" method
                    /// </summary>
                    /// <returns></returns>
                   void Delete" + CamelCaseName + @"(" + primarykeyDatatype + " " + primarykey + @");
                }
            }
            
            ");

            return sbm.ToString();
        }
        string RepositoryH()
        {
            sbm = new StringBuilder();
            sbm.AppendLine(GetNamespaces(NamespaceType.Handler));
            sbm.AppendLine(@"
        namespace DataServices" + hierarchy + @".Manager
            {
                /// <summary>
                /// Insert, Update and Delete operation in " + tableName + @" table 
                /// </summary>
                public class " + CamelCaseName + @"RepositoryHandler : I" + CamelCaseName + @"RepositoryHandler
                {
                    private readonly " + Entitystring + @" " + Contextstring + @";
                    public " + CamelCaseName + @"RepositoryHandler(" + Entitystring + @" " + Contextstring + @")
                    {
                        " + Contextstring + @" = " + Contextstring + @";
                    }
                    /// <summary>
                    /// delete " + tableName + @" record depending up on ID
                    /// </summary>
                    /// <param name=""ID""></param>
                    public void Delete" + CamelCaseName + @"(int id)
                    {
                        try
                        {
                            " + Contextstring + @".Configuration.AutoDetectChangesEnabled = false;
                            var entity = new " + tableName + @" { " + CamelCaseName + @"ID = id };
                            " + Contextstring + @"." + tableName + @".Attach(entity);
                            " + Contextstring + @".Entry(entity).State = System.Data.Entity.EntityState.Deleted;
                            " + Contextstring + @".SaveChanges();
                        }
                        catch (Exception) { throw; }
                        finally
                        {
                            " + Contextstring + @".Configuration.AutoDetectChangesEnabled = true;
                        }
                    }
                    /// <summary>
                    /// save record in " + tableName + @"
                    /// </summary>
                    /// <param name=""entity""></param>
                    /// <returns></returns>
                    public Tuple<string, int> Save(" + tableName + @" entity)
                    {
                        try
                        {
                            " + Contextstring + @".Configuration.AutoDetectChangesEnabled = false;
                            " + Contextstring + @"." + tableName + @".Attach(entity);
                            " + Contextstring + @".Entry(entity).State = System.Data.Entity.EntityState.Added;
                            " + Contextstring + @".SaveChanges();
                            return new Tuple<string, int>(string.Empty, entity.ID);
                        }
                        catch (Exception) { throw; }
                        finally
                        {
                            " + Contextstring + @".Configuration.AutoDetectChangesEnabled = true;
                        }
                    }
                    /// <summary>
                    /// Update record in " + tableName + @"
                    /// </summary>
                    /// <param name=""entity""></param>
                    /// <returns></returns>
                    public Tuple<string, int> Update(" + tableName + @" entity)
                    {
                        try
                        {
                            " + Contextstring + @"." + tableName + @".Attach(entity);
                            " + Contextstring + @".Entry(entity).State = System.Data.Entity.EntityState.Modified;
                            " + Contextstring + @".SaveChanges();
                            return new Tuple<string, int>(string.Empty, entity." + CamelCaseName + @"ID);
                        }
                        catch (Exception) { throw; }
                    }
                }
            }
            ");



            return sbm.ToString();
        }
        #endregion
        
        #region "GetMethods"


        string CorrectProperties(string strProperties)
        {
            /*Add Properties*/
            strProperties.Replace("private", " public ");
            strProperties.Replace("System.", "");
            strProperties.Replace("Nullable<DateTime>", " DateTime? ");
            strProperties.Replace("Nullable<bool>", "bool?");
            strProperties.Replace("Nullable<byte>", "byte?");
            strProperties.Replace("Nullable<sbyte>", "sbyte?");
            strProperties.Replace("Nullable<char>", "char?");
            strProperties.Replace("Nullable<decimal>", "decimal?");
            strProperties.Replace("Nullable<double>", "double?");
            strProperties.Replace("Nullable<float>", "float?");
            strProperties.Replace("Nullable<int>", "int?");
            strProperties.Replace("Nullable<uint>", "uint?");
            strProperties.Replace("Nullable<long>", "long?");
            strProperties.Replace("Nullable<ulong>", "ulong?");
            strProperties.Replace("Nullable<object>", "object?");
            strProperties.Replace("Nullable<short>", "short?");
            strProperties.Replace("Nullable<ushort>", "ushort?");
            strProperties.Replace("Nullable<string>", "string?");
            strProperties.Replace("Nullable<Guid>", "Guid?");
            strProperties.Replace(";", " {get;set;} ");
            return strProperties;
        }

        string GetNamespaces(NamespaceType nm)
        {
            // StringBuilder sb = new StringBuilder();
            List<string> sb = new List<string>();
            char lreturn = '@';
            switch (nm)
            {
                case NamespaceType.Common:
                    /*Common*/
                    sb.Add("using System;" + lreturn);
                    sb.Add("using CQRS.Contracts;" + lreturn);
                    break;
                case NamespaceType.Collection:
                    sb.Add("using System.Collections.Generic;" + lreturn);
                    sb.Add("using System.Linq;" + lreturn);
                    sb.Add("using UtilitiesServices;" + lreturn);
                    sb.Add("using System.Net;" + lreturn);
                    break;
                case NamespaceType.EF:
                    break;
                case NamespaceType.Query:
                    sb.AddRange(GetNamespaces(NamespaceType.Common).Replace(NwLline, lreturn + "").Split(lreturn));
                  //  sb.Add("using BusinessEntities.Query" + (hierarchy == "" ? "" :   hierarchy ) + ";");
                    break;

                case NamespaceType.Result:
                    /* Query*/
                    sb.Add("using BusinessServices.QueryResult" + hierarchy + ";" + lreturn);
                    sb.Add("using BusinessEntities.Query" + hierarchy + ";" + lreturn);
                    break;
                case NamespaceType.repositoryContract:
                    sb.AddRange(GetNamespaces(NamespaceType.Common).Replace(NwLline, lreturn + "").Split(lreturn));
                    break;
                case NamespaceType.repositoryManager:
                    sb.AddRange(GetNamespaces(NamespaceType.Common).Replace(NwLline, lreturn + "").Split(lreturn));
                    break;

                case NamespaceType.Command:
                    sb.AddRange(GetNamespaces(NamespaceType.Common).Replace(NwLline, lreturn + "").Split(lreturn));
                    break;
                case NamespaceType.Handler:
                    sb.AddRange(GetNamespaces(NamespaceType.Common).Replace(NwLline, lreturn + "").Split(lreturn));
                    sb.AddRange(GetNamespaces(NamespaceType.Query).Replace(NwLline, lreturn + "").Split(lreturn));
                    sb.AddRange(GetNamespaces(NamespaceType.Result).Replace(NwLline, lreturn + "").Split(lreturn));

                    /* Result*/


                    /*Contract*/
                    sb.Add("using DataServices" + (hierarchy == "" ? "" : hierarchy) + ".Contract;" + lreturn);
                    sb.Add("using DataServices.EF;" + lreturn);
                    /*Manager*/
                    sb.Add("using DataServices" + (hierarchy == "" ? "" : hierarchy)+".Contract;" + lreturn);


                    break;

                default:
                    sb.Add("using DataServices.EF;" + lreturn);
                    sb.Add("using DataServices.Infrastructure;" + lreturn);
                    sb.Add("using System;");
                    sb.Add("using System.Collections.Generic;");
                    break;

            }

            return (string.Join(NwLline, sb.Distinct().ToArray())).Replace("@", "").Replace(NwLline + NwLline, NwLline);
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



--SET @result = @result   

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

SET @result = @result   

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
             
DECLARE @TableName VARCHAR(MAX) = '" + tableName + @"' --  your table name
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
            string strMapping = @"Declare @TableName varchar(max)='" + tableName + @"';
DECLARE @result varchaR(MAX)=''
select @result+=t.ColumnName+','+CHAR(13)
FROM(
                     SELECT  c.COLUMN_NAME+'=item.'+c.COLUMN_NAME   AS ColumnName ,ORDINAL_POSITION 
                     FROM    INFORMATION_SCHEMA.COLUMNS c
                     WHERE   c.TABLE_NAME = @TableName
)T
ORder by T.ORDINAL_POSITION ASC
SELECT SUBSTRING(@result,0,Len(@result)-1) col1";
            #endregion "strMapping"

            string strPKeyQuery = @"
SELECT 
    c.name AS pkey,CASE WHEN  t.name='text' or t.name='ntext' or t.name='varchar' or t.name='char' or t.name='nvarchar' or t.name='nchar' or t.name='xml' or t.name='sysname'   
    THEN 'string' ELSE 'int' END as datatype
FROM sys.indexes i
    inner join sys.index_columns ic  ON i.object_id = ic.object_id AND i.index_id = ic.index_id
    inner join sys.columns c ON ic.object_id = c.object_id AND c.column_id = ic.column_id
    inner join sys.types t on t.user_type_id=c.user_type_id
WHERE i.is_primary_key = 1
    and object_name(i.object_ID) = '" + tableName + "'";

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
                case QueryType.pkey:
                    QueryToReturn = strPKeyQuery;
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
            return result.ToString();
        }

        DataRow GetDatabaseValues(string Query, int colNum = 0)
        {
            using (SqlConnection newCon = new SqlConnection(GetDBConnectionString()))
            {
                //string queryString = "SELECT top 2 *  FROM "+tblname;
                DataSet ds = new DataSet();
                SqlConnection c = new SqlConnection(GetDBConnectionString());
                SqlDataAdapter a = new SqlDataAdapter(Query, c);
                a.Fill(ds);
                return  ds.Tables[0].Rows.Count>0  ?  ds.Tables[0].Rows[0] : ds.Tables[0].NewRow();
            }

        }


        string GetComment(CommentType comment)
        {
            StringBuilder sb = new StringBuilder();

            switch (comment)
            {
                case CommentType.QueryComment:
                    sb.AppendLine(@"/// <summary>
                    /// Creating " + CamelCaseName + @"ByQuery Class for " + tableName + @" Queries
                    /// </summary>
                    /// <param name=" + radioButton1.ProductName + "></param>");
                    break;
                case CommentType.HandlerComment:
                    sb.AppendLine(@"/// <summary>
                    /// Creating " + CamelCaseName + @"QueryHandler for Handling Queries
                    /// </summary>
                    /// <param name=" + radioButton1.ProductName + "></param>");
                    break;
                case CommentType.RepositoryComment:
                    sb.Append(@"
                /// <summary>
                /// Interface for retrieve " + CamelCaseName + @"RepositoryQuery
                /// </summary>");
                    break;
                case CommentType.ResultComment:
                    break;

            }

            return "";
        }
        #endregion

    }


}
