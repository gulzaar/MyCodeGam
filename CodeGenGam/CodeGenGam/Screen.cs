using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.IO;
using System.Web.Script.Serialization;


namespace CodeGenGam {
    public partial class Screen : Form
    {
        public Screen()
        {
            InitializeComponent();
            PopulateConnectionString();
            textBox3.Text = "By \r\n Amol Gajbhiye";
            btnGenerate.Enabled = false;
        }
        
        #region "Fields"
        //SqlConnection conn;
        StringBuilder sbm = new StringBuilder();

        string tableName = "";
        string aliasname = "";
        string tableNameInEntity = "";
        string hierarchy = "";
        string commonhierarchy = "";
        string CamelCaseName = string.Empty;
        string context = string.Empty;
        string Entitystring = string.Empty;
        string Contextstring = string.Empty;
        string primarykey = "";
        string primarykeyDatatype = "";

        int intSavingTime = 0;
        int permin = 30;
        public List<Filetype> filetypeList { get; set; }
        List<CommentConfig> MyConfig { get; set; }

        #region "Constants"
        const string DirectoryEnd = "\\";
        const string NwLline = "\r\n";
        const string SelectString = "-Select-";
        const string cAllSelector = "zAlls";
        const string closeInterface = @" // End Interface";
        const string closeclass = @" // End Class";
        const string closeNamespace = @" // End Namespace";
        const string cbody = "@body";
        const string cparam = "@param";
        const string creturn = "@return";


        #endregion "Constants"

        class CommentConfig { 
        public string ContextName {get;set;}
        public string TableName { get; set; }
        public string TableAlias { get; set; }
        public string TableHierarchy { get; set; }
        public string ComponentType { get; set; }
        public List<CommentDetail> CommentDetails { get; set; }
        }
        class CommentDetail { 
        public string CommentKey { get; set; }
    public string Commentvalue { get; set; }
        }
        #endregion "Fields"

        #region "PageMethods"
        private void btnGenerate_Click (object sender, EventArgs e)
        {
            try
            {
                /*Validate User Input*/
                if (cmbConnection.SelectedValue.ToString() == SelectString) { ShowMessage("Please select a DataContext"); return; }
               // if (cmbDbOb.SelectedValue.ToString() == SelectString) {ShowMessage("Please select a Datatable"); return;}
                if ( lstSelectBox.SelectedItems.Count == 0) { ShowMessage("Please select a Datatable"); return; }
                bool isAllz=(lstSelectBox.SelectedItem.ToString() == cAllSelector);
                /*Check Authentication and Proceed to generator*/
                if (GetAuthentication())
                {
                    //var ctrl = (KeyValuePair<string, string>)lstSelectBox.SelectedItem;r
                    string strHierarchy = "";
                    if (chkCustomComment.Checked == true && (MyConfig != null && MyConfig.Count > 0))
                    {
                       // ShowMessage("Default Config Loaded"); 
                       foreach(CommentConfig cmt in MyConfig){
                           
                           lstSelectBox.SelectedItems.Add( 
                               MyConfig
                               .Where(w=>w.ContextName==cmbConnection.SelectedValue.ToString())
                               .Select(t => t.TableName)
                               );
                           txtHierarchy.Enabled = false;
                       }
                    }
                    else {
                        strHierarchy = txtHierarchy.Text;
                    }

                    if ((lstSelectBox.SelectedItems.Count > 1) || isAllz==true)
                    {
                        foreach (string tblname in lstSelectBox.SelectedItems)
                        {
                            if (tblname != cAllSelector)
                            {
                                tableName = tblname;
                                strHierarchy = MyConfig
                                 .Where(w => w.TableName == tblname)
                                 .Select(t => t.TableHierarchy).FirstOrDefault();
                                CallGenerator(tblname, strHierarchy, false);
                            }

                        }

                    }
                    else
                    {
                        tableName = lstSelectBox.SelectedItem.ToString();
                        CallGenerator(tableName, strHierarchy, false);
                    }


                    //if (cmbDbOb.SelectedValue.ToString() == cAllSelector)
                    //{
                    //    foreach (string tblname in cmbDbOb.Items)
                    //    {
                    //        if (cmbDbOb.SelectedValue.ToString() != cAllSelector)
                    //        {
                    //            CallGenerator(tblname, true);

                    //        }

                    //    }


                    //}
                    //else
                    //{
                    //    tableName = cmbDbOb.SelectedValue.ToString();

                    //}

                    ShowSavingsWords();
                    ShowMessage("Done! ;-)");
                }
                else { ShowMessage("Lockeded for 10 Mins ;-)"); return; }
                
        
            }

            //}
            catch (Exception) { ShowMessage("Please select a valid Datacontext and Table"); }
        }

        void CallGenerator(string tableName,string txthierarchy, bool isAllz)
        {
            try
            {
                

                CamelCaseName = GetCamelCase(tableName);
                if (MyConfig!=null && MyConfig.Count>1)
                {
                hierarchy = MyConfig.Where(t => t.TableName == tableName).Select(t => t.TableHierarchy).FirstOrDefault();
                aliasname = MyConfig.Where(t => t.TableName == tableName).Select(t => t.TableAlias).FirstOrDefault();
                aliasname = aliasname == "" ? CamelCaseName : GetCamelCase(aliasname);
                //tableName = aliasname;
                }

                hierarchy = txthierarchy == string.Empty ? "" : "." + txthierarchy.Replace(@"\", @".");
                hierarchy = hierarchy == "" ? ".Common" : hierarchy;

                var ctrl = (KeyValuePair<string, string>)cmbConnection.SelectedItem;
                Entitystring = ctrl.Key.ToString() + "Entities";//(radioButton1.Checked == true) ? "" + "EIDEntities" + @"" : "McDonaldsEntities";
                Contextstring = ctrl.Key.ToString() + "Context";//(radioButton1.Checked == true) ? "" + "EidDBContext" + @"" : "McDDBContext";

                tableNameInEntity = tableName.Substring(tableName.Length - 1, 1).ToLower() != "s" ? tableName + "s" : tableName.Substring(0, tableName.Length - 1);


                //if (isAllz || lstSelectBox.SelectedItems.Count>1)
                //{

                //    aliasname = CamelCaseName;
                //}

                //else
                //{
                if (MyConfig==null )
                {
                    aliasname = string.IsNullOrWhiteSpace(txttableAlias.Text) ? CamelCaseName : GetCamelCase(txttableAlias.Text);
                }
                else { 
                
                }
                //}

                writeTOdisk((Filetype)cmbComponent.SelectedIndex);

            }
            catch (Exception Ex) { Log(Ex); }

        }
        
        private void cmbConnection_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ctrl = (KeyValuePair<string, string>)cmbConnection.SelectedItem;
            if (!(ctrl.Value == SelectString)) { PopulateDbObjects(); }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string TfsPath = ConfigurationManager.AppSettings["TfsPath"].ToString();
            Configuration TfsPathConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            TfsPathConfig.AppSettings.Settings.Remove("TfsPath");
            TfsPathConfig.AppSettings.Settings.Add("TfsPath", txtConfig.Text);

            TfsPathConfig.AppSettings.Settings["TfsPath"].Value = txtConfig.Text;
            TfsPathConfig.Save();
            ConfigurationManager.RefreshSection("appSettings");

        }

        private void btnSaveConfig_Click(object sender, EventArgs e)
        {
            WriteConfig((Filetype)cmbComponent.SelectedIndex);

        }

        private void btnLoadConfig_Click(object sender, EventArgs e)
        {
            try
            {
                string filepath = string.Empty;
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    openFileDialog1.Filter = "Config File (TXT)|*.txt";

                    // openFileDialog1.FileName = "Userconfig";
                    openFileDialog1.InitialDirectory = Application.StartupPath;
                    filepath = openFileDialog1.FileName;
                    txtCommentConfig.Text = filepath;
                    tabControl1.Focus();
                }


                MyConfig = ReadConfig(filepath);
                if (MyConfig.Count > 0) { ShowMessage("Config Loaded"); } else { ShowMessage("Config Couldn't be Loaded"); }
            }
            catch (Exception) { throw; }

        }

        private void chkCustomComment_CheckedChanged(object sender, EventArgs e)
        {
            if (CustomConfigIsloaded())
            {
                PopulateConnectionString();
                cmbConnection.SelectedIndex = cmbConnection.FindString(MyConfig.Select(t => t.ContextName).FirstOrDefault());
                // cmbConnection.SelectedText = ;
                //cmbConnection.SelectedValue = MyConfig.Select(t => t.ContextName).FirstOrDefault();
                PopulateDbObjects();

                foreach (CommentConfig cmtconfig in MyConfig)
                {
                    lstSelectBox.SelectedItems.Add(cmtconfig.TableName);
                }
                if (MyConfig.Count > 1) { txttableAlias.Enabled = false; txtHierarchy.Text = "Populated from Config"; txtHierarchy.Enabled = false; }
                
                cmbComponent.SelectedIndex = cmbComponent.FindString(MyConfig.Select(t => t.ComponentType).FirstOrDefault());



                btnGenerate.Enabled = true;
            }
            else { btnGenerate.Enabled = false; }
        }

        private void cmbDbOb_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (cmbDbOb.SelectedItem.ToString() == cAllSelector) { }

        }

        private void lstSelectBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstSelectBox.SelectedItems.Count > 1)
            {
                txttableAlias.Enabled = false;
                txttableAlias.Text = "";
            }
            else { txttableAlias.Enabled = true; }
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

        #region "Completed"
        public void writeTOdisk(Filetype f)
        {
            string filename = aliasname == "" ? CamelCaseName : aliasname;

            string foldername = GetFolderName(f);
            string strPropertiesOrFilter, strFilter, strMapping = "";


            DataRow getPrimarykeyInfo = GetDatabaseValues(GetQuery(QueryType.Pkey));
            primarykey = getPrimarykeyInfo["pkey"].ToString();
            primarykeyDatatype = getPrimarykeyInfo["datatype"].ToString();
            primarykey = primarykey == "" ? " ID " : primarykey;
            primarykeyDatatype = primarykeyDatatype == "" ? " int " : primarykeyDatatype;

            strPropertiesOrFilter = GetDatabaseValues(GetQuery(QueryType.Property))["col1"].ToString();
            strPropertiesOrFilter = CorrectProperties(strPropertiesOrFilter);
            strFilter = GetDatabaseValues(GetQuery(QueryType.ExpressionFilter))["col1"].ToString();
            strMapping = GetDatabaseValues(GetQuery(QueryType.Mapping))["col1"].ToString();

            StringBuilder sb = new StringBuilder();

            switch (f)
            {
                case Filetype.ByQuery:
                    filename += "ByQuery.cs";
                    sb.Append(ByQuery(strPropertiesOrFilter));
                    break;
                case Filetype.ByCommand:
                    filename += "ByCommand.cs";
                    sb.Append(ByCommand(strPropertiesOrFilter));
                    break;
                case Filetype.QueryResult:

                    filename += "QueryResult.cs";
                    sb.Append(Result());
                    break;
                case Filetype.QueryHandler:

                    filename += "QueryHandler.cs";
                    sb.Append(HandlerQ(strFilter, strMapping));
                    break;
                case Filetype.CommandHandler:

                    filename += "CommandHandler.cs";
                    sb.Append(HandlerC(strMapping));
                    break;
                case Filetype.RepositoryQI:
                    filename = CamelCaseName;
                    filename = "I" + filename + "RepositoryQuery.cs";
                    sb.Append(RepositoryQI());
                    break;
                case Filetype.RepositoryQ:
                    filename = CamelCaseName;
                    filename += "RepositoryQuery.cs";

                    sb.Append(RepositoryQ());
                    break;
                case Filetype.RepositoryHI:
                    filename = CamelCaseName;
                    filename = "I" + filename + "RepositoryHandler.cs";
                    sb.Append(RepositoryHI());
                    break;
                case Filetype.RepositoryH:
                    filename = CamelCaseName;
                    filename += "RepositoryHandler.cs";
                    sb.Append(RepositoryH());
                    break;
                case Filetype.All:
                    writeTOdisk(Filetype.ByQuery);
                    writeTOdisk(Filetype.ByCommand);
                    writeTOdisk(Filetype.QueryResult);
                    writeTOdisk(Filetype.QueryHandler);
                    writeTOdisk(Filetype.CommandHandler);
                    writeTOdisk(Filetype.RepositoryQI);
                    writeTOdisk(Filetype.RepositoryQ);
                    writeTOdisk(Filetype.RepositoryHI);
                    writeTOdisk(Filetype.RepositoryH);
                    break;
            }

            if (f != Filetype.All) { 
                
                Writefile(foldername, filename, sb);
                intSavingTime += GetCountofWords(sb.ToString());
            }


        }

        string ByQuery(string strProperties)
        {
            sbm = new StringBuilder();
            /*Add namespace reference*/
            sbm.AppendLine(GetNamespaces(NamespaceType.Query));
            sbm.AppendLine("namespace BusinessEntities.Query" + hierarchy + " {");
            sbm.Append(GetComment(CommentType.QueryClass, "", ""));
            sbm.AppendLine("public class " + aliasname + "ByQuery" + ":" + "IQuery" + " {");
            sbm.AppendLine("#region Instance Properties");

            sbm.AppendLine(strProperties);

            sbm.AppendLine("/*Implement IQuery*/ ");
            sbm.AppendLine("public CriteriaType ReteriveBy {get;set;}");
            sbm.AppendLine(" #endregion Instance Properties");
            sbm.AppendLine("}" + closeclass + NwLline + "}" + closeNamespace);

            return sbm.ToString();
        }

        string ByCommand(string strProperties)
        {
            sbm = new StringBuilder();
            /*Add namespace reference*/
            sbm.AppendLine(GetNamespaces(NamespaceType.Command));
            sbm.AppendLine("namespace BusinessEntities.Command" + hierarchy + " {");
            /*Comment*/
            sbm.Append(GetComment(CommentType.CommandClass, "", ""));

            sbm.AppendLine("public class " + aliasname + "ByCommand" + ":" + " ICommand" + " {");

            strProperties = CorrectProperties(strProperties);

            sbm.AppendLine(strProperties);


            sbm.AppendLine("/*Implement ICommand*/ ");
            sbm.AppendLine("public OperationType Action {get;set;}");
            sbm.AppendLine("}" + closeclass + NwLline + "}" + closeNamespace);

            return sbm.ToString();
        }

        string Result()
        {
            sbm = new StringBuilder();
            /*Add namespace reference*/
            sbm.AppendLine(GetNamespaces(NamespaceType.Result));

            sbm.AppendLine("namespace BusinessServices.QueryResult" + hierarchy + " {");
            sbm.Append(GetComment(CommentType.ResultClass, "", ""));
            sbm.AppendLine("public class " + aliasname + "QueryResult" + " : " + "IQueryResult" + " {");
            sbm.AppendLine("public IEnumerable<" + aliasname + "ByQuery> GetAll" + aliasname + " { get; set; }");
            sbm.AppendLine("public "+aliasname + "ByQuery Get" + aliasname + "{ get; set; }");
            /*Close Class & Namespace*/
            sbm.AppendLine("}" + closeclass + NwLline + "}" + closeNamespace);

            return sbm.ToString();
        }

        string HandlerQ(string filter, string Mapping)
        {

            sbm = new StringBuilder();
            sbm.AppendLine(GetNamespaces(NamespaceType.QueryHandler));

            sbm.AppendLine(@"namespace BusinessServices.QueryHandler" + hierarchy + @" {");
            sbm.Append(GetComment(CommentType.QueryHandlerClass, "", ""));
            sbm.AppendLine(@"public class " + aliasname + "QueryHandler : IQueryHandler<" + aliasname + "ByQuery, " + aliasname + @"QueryResult>
    {");
            sbm.Append(GetComment(CommentType.Alias, "", ""));
            sbm.AppendLine(@"private readonly I" + CamelCaseName + "RepositoryQuery " + aliasname + @"repository"+aliasname+@";

        public " + aliasname + "QueryHandler(I" + CamelCaseName + "RepositoryQuery " + aliasname + @"Repository)
        {
            this." + aliasname + "repository" + aliasname + @" = " + aliasname + @"Repository;
        }

        public " + aliasname + "QueryResult Retrieve(" + aliasname + @"ByQuery query)
        {
            var result = new " + aliasname + @"QueryResult();
            var predicate = PredicateBuilder.True<" + tableName + @">();
           
            IEnumerable<" + tableName + @"> resultData;
    ");
            sbm.Append(filter);
            sbm.Append(@" try
            {
                if (query.ReteriveBy == CriteriaType.NoCriteria)
                {
                resultData = " + aliasname + "repository" + aliasname + @".GetAll" + CamelCaseName + @"();
                result.GetAll" + aliasname + " = resultData.Select(item => new " + aliasname + @"ByQuery()
                {");
            sbm.AppendLine(Mapping);
            sbm.AppendLine(@"
                });
                } //End NoCriteria

            
                
              else if (query.ReteriveBy == CriteriaType.SingleCriteria)
                {
                  var item = " + aliasname + "repository" + aliasname + @".Get" + CamelCaseName + @"ByFilter(predicate).FirstOrDefault();
                  result.Get" + aliasname + " = new " + aliasname + @"ByQuery{
                ");
            sbm.AppendLine(Mapping );
            sbm.AppendLine(@"
             };
            } //End SingleCriteria
            else if (query.ReteriveBy == CriteriaType.ComplexCriteria)
                {
                resultData = " + aliasname + "repository" + aliasname + @".Get" + CamelCaseName + @"ByFilter(predicate);
                result.GetAll" + aliasname + " = resultData.Select(item => new " + aliasname + @"ByQuery()
                {");

            sbm.AppendLine(Mapping);
            sbm.AppendLine(@"
                }); 
                } //End ComplexCriteria

            } //End Try

            catch (Exception){throw;}
            return result;
        } //End Retrieve
");
            sbm.AppendLine("}" + closeclass + NwLline + "}" + closeNamespace);




            //sbm.AppendLine("}"+closeclass + NwLline + "}"+closeNamespace);

            return sbm.ToString();
        }

        string RepositoryQI()
        {
            sbm = new StringBuilder();
            sbm.AppendLine(GetNamespaces(NamespaceType.RepositoryContract));

            sbm.AppendLine("namespace DataServices" + hierarchy + ".Contract" + NwLline + "{");
            sbm.Append(GetComment(CommentType.RepositoryQContractClass, "", ""));
            sbm.AppendLine(@" public interface I" + CamelCaseName + "RepositoryQuery : IRepositoryQuery<" + tableName + ", " + primarykeyDatatype + @">
                {
                    // It is used to retrieve All columns and rows of " + tableName + @" 
                    IEnumerable<" + tableName + "> GetAll" + CamelCaseName + @"();
                    IEnumerable<" + tableName + "> Get" + CamelCaseName + "ByFilter(Expression<Func<" + tableName + @", bool>> predicate);
              ");
            sbm.AppendLine("}" + closeInterface + NwLline + "}" + closeNamespace);

            return sbm.ToString();
        }

        string RepositoryQ()
        {
            sbm = new StringBuilder();
            tableNameInEntity = tableName;
            sbm.AppendLine(GetNamespaces(NamespaceType.RepositoryQ));
            sbm.AppendLine(@"namespace DataServices" + commonhierarchy + @".Manager{"
                + NwLline + GetComment(CommentType.RepositoryQManagerClass, "", "")
               + @"public class " + CamelCaseName + @"RepositoryQuery : I" + CamelCaseName + @"RepositoryQuery
                {
                    private readonly " + Entitystring + @" " + Contextstring + @";"
                          + NwLline +
@"public " + CamelCaseName + @"RepositoryQuery(" + Entitystring + @" " + Contextstring.ToLower() + @")
                    {
                        " + Contextstring + @" = " + Contextstring.ToLower() + @";
                    }"
                          + NwLline
                          + GetComment(CommentType.GetAll, "", "") +
@"public IEnumerable<" + tableName + @"> GetAll" + CamelCaseName + @"()
   {
    try
   {
       return " + Contextstring + @"." + tableNameInEntity + @".AsNoTracking();
   }
    catch (Exception) { throw; }
   }
"
                          + NwLline +
                          GetComment(CommentType.GetAllByPredicate, "", "") +
@" public IEnumerable<" + tableName + @"> Get" + CamelCaseName + @"ByFilter(Expression<Func<" + tableNameInEntity + @", bool>>  predicate)
 {
 try
 {
    return " + Contextstring + @"." + tableNameInEntity + @".AsNoTracking().Where(predicate);
 }
 catch (Exception) { throw; }
 }
"
                          + NwLline
                          + GetComment(CommentType.GetAllByPrimaryKey, primarykey, tableNameInEntity) +
@"/// <param name=" + primarykey + @"></param>
  /// <returns></returns>
  public " + tableName + @" FindBy(" + primarykeyDatatype + @" key)
  {
    try
    {
       return " + Contextstring + @"." + tableNameInEntity + @".AsNoTracking().Where(item => item." + primarykey + @" == key).FirstOrDefault();
    }
    catch (Exception) { throw; }
   }"
    );

            sbm.AppendLine("}" + closeclass + NwLline + "}" + closeNamespace);

            return sbm.ToString();
        }

        string HandlerC(string strMapping)
        {
            strMapping = strMapping.Replace("item.", "Usercommand.");

            sbm = new StringBuilder();
            sbm.AppendLine(GetNamespaces(NamespaceType.CommandHandler));

            sbm.AppendLine("namespace BusinessServices.CommandHandler " + hierarchy + " {");
            sbm.Append(GetComment(CommentType.CommandHandlerClass, "", ""));

            sbm.AppendLine("public class " + aliasname + @"CommandHandler : ICommandHandler<" + aliasname + @"ByCommand>" + NwLline + "{");
            sbm.Append(@"
        private readonly I" + CamelCaseName + @"RepositoryHandler repository;
        public " + aliasname + @"CommandHandler(I" + CamelCaseName + @"RepositoryHandler Repository)
        {
            repository = Repository;
        }"
                          + NwLline
                          + GetComment(CommentType.Execute, "", "")
                          + @"/// <param name=""command""></param>
        public void Execute(" + aliasname + @"ByCommand Usercommand)
        {
            try
            {
                switch (Usercommand.Action)
                {
                    case OperationType.Insert:
                        Insert(Usercommand);
                        break;
                    case OperationType.Delete:
                        Delete(Usercommand);
                        break;
                    case OperationType.Update:
                        Update(Usercommand);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }
"
                          + NwLline +
                          GetComment(CommentType.Insert, "", "") +
@"/// <param name=""command""></param>
        private void Insert(" + aliasname + @"ByCommand Usercommand)
        {
            try
            {
                var programUser = new " + tableName + @"()
                {");
            sbm.AppendLine(strMapping);

            sbm.AppendLine(@" };

                var programTuple = repository.Save(programUser);

            }
            catch (Exception) { throw; }
        }//Close Insert
"
                          + NwLline +
                          GetComment(CommentType.Update, "", "") +
@"/// <param name=""command""></param>
        private void Update(" + aliasname + @"ByCommand Usercommand)
        {
            try
            {
                var programUser = new " + tableName + @"()
                 {");
            sbm.AppendLine(strMapping);

            sbm.AppendLine(@" };

                var programTuple = repository.Update(programUser);
            }
            catch (Exception) { throw; }
        }//Close Update
"
                          + NwLline +
                          GetComment(CommentType.Delete, "", "") +
@"/// <param name=""command""></param>
        private void Delete(" + aliasname + @"ByCommand Usercommand)
        {
            try
            {
                repository.Delete" + CamelCaseName + @"(Usercommand." + primarykey + @");
            }
            catch (Exception) { throw; }

        }// Close Delete

");

            sbm.AppendLine("}" + closeclass + NwLline + "}" + closeNamespace);

            return sbm.ToString();
        }

        string RepositoryHI()
        {
            sbm = new StringBuilder();

            sbm.AppendLine(GetNamespaces(NamespaceType.RepositoryContract));
            sbm.AppendLine(@"namespace DataServices" + hierarchy + @".Contract{
               public interface I" + CamelCaseName + "RepositoryHandler:IRepositoryHandler<" + tableName + @"," + primarykeyDatatype + @">
                {
                  " + GetComment(CommentType.Delete_I, "", "") + @"
                   void Delete" + CamelCaseName + @"(" + primarykeyDatatype + " " + primarykey + @");
                        ");
            sbm.AppendLine("}" + closeInterface + NwLline + "}" + closeNamespace);
            return sbm.ToString();
        }

        string RepositoryH()
        {
            sbm = new StringBuilder();
            sbm.AppendLine(GetNamespaces(NamespaceType.RepositoryH));
            sbm.AppendLine(@"namespace DataServices" + hierarchy + @".Manager{"
                + NwLline
                                    + GetComment(CommentType.RepositoryHManagerClass, "", "")
 + @"public class " + CamelCaseName + @"RepositoryHandler : I" + CamelCaseName + @"RepositoryHandler
                {
                    private readonly " + Entitystring + @" " + Contextstring + @";"
                          + NwLline +
@"public " + CamelCaseName + @"RepositoryHandler(" + Entitystring + @" " + Contextstring.ToLower() + @")
                    {
                        " + Contextstring + @" = " + Contextstring.ToLower() + @";
                    }"
                          + NwLline +
                    @"/// <summary>
                    /// Delete " + tableName + @" record depending up on ID
                    /// </summary>
                    /// <param name=""ID""></param>
                    public void Delete" + CamelCaseName + @"(" + primarykeyDatatype + " " + primarykey + @")
                    {
                        try
                        {
                            " + Contextstring + @".Configuration.AutoDetectChangesEnabled = false;
                            var entity = new " + tableName + @" { " + primarykey + @" = " + primarykey + @" };
                            " + Contextstring + @"." + tableName + @".Attach(entity);
                            " + Contextstring + @".Entry(entity).State = System.Data.Entity.EntityState.Deleted;
                            " + Contextstring + @".SaveChanges();
                        }
                        catch (Exception) { throw; }
                        finally
                        {
                            " + Contextstring + @".Configuration.AutoDetectChangesEnabled = true;
                        }
                    }"
                          + NwLline +
                    @"/// <summary>
                    /// Save record in " + tableName + @"
                    /// </summary>
                    /// <param name=""entity""></param>
                    /// <returns></returns>
                    public Tuple<string, " + primarykeyDatatype + @"> Save(" + tableName + @" entity)
                    {
                        try
                        {
                            " + Contextstring + @".Configuration.AutoDetectChangesEnabled = false;
                            " + Contextstring + @"." + tableName + @".Attach(entity);
                            " + Contextstring + @".Entry(entity).State = System.Data.Entity.EntityState.Added;
                            " + Contextstring + @".SaveChanges();
                            return new Tuple<string, " + primarykeyDatatype + @">(string.Empty, entity." + primarykey + @");
                        }
                        catch (Exception) { throw; }
                        finally
                        {
                            " + Contextstring + @".Configuration.AutoDetectChangesEnabled = true;
                        }
                    }
" + NwLline +
                 @" /// <summary>
                    /// Update record in " + tableName + @"
                    /// </summary>
                    /// <param name=""entity""></param>
                    /// <returns></returns>
                    public Tuple<string, " +primarykeyDatatype+ @"> Update(" + tableName + @" entity)
                    {
                        try
                        {
                            " + Contextstring + @"." + tableName + @".Attach(entity);
                            " + Contextstring + @".Entry(entity).State = System.Data.Entity.EntityState.Modified;
                            " + Contextstring + @".SaveChanges();
                            return new Tuple<string, " + primarykeyDatatype + @">(string.Empty, entity." + primarykey + @");
                        }
                        catch (Exception) { throw; }
                    }

             
             
            ");
            if (primarykeyDatatype!="int")
            {
                sbm.Append("Tuple<string, int> Infrastructure.IRepositoryHandler<" + tableName + " , " + primarykeyDatatype + @">.Update(" + tableName + @" entity)                    
{
                        try
                        {
                          //Dummy For implementing IRepository
                            return new Tuple<string,int>(string.Empty,0);
                        }
                        catch (Exception) { throw; }
                    }

               Tuple<string, int> Infrastructure.IRepositoryHandler<" + tableName + " , " + primarykeyDatatype + @">.Save(" + tableName + @" entity)                    
                    {
                        try
                        {
                            //Dummy For implementing IRepository
                           return new Tuple<string,int>(string.Empty,0);
                        }
                        catch (Exception) { throw; }
                       
                    }");
            
            }
            sbm.AppendLine("}" + closeclass + NwLline + "}" + closeNamespace);

            return sbm.ToString();
        }
        #endregion "Complete"

        #region "InProgress"
      

        #endregion

        #region "Pending"
       
        #endregion

        #region "Write"
        void Writefile(string foldername, string filename, StringBuilder sb)
        {
            try
            { //Handle Directory
                if (!Directory.Exists(foldername)) { Directory.CreateDirectory(foldername); }
                if (!File.Exists(foldername + filename))
                { //Handle Duplicate file
                    //DialogResult result2 = MessageBox.Show("File Exists!! Do you want to delete?",
                    //"Delete Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    //if (result2 == DialogResult.Yes) {  }
                    System.IO.File.WriteAllText(foldername + filename, sb.ToString());
                   
                }
                else { File.Delete(foldername + filename); Writefile(foldername, filename, sb); }
            }
            catch (Exception) { }
        }
        void writecomments(string Foldername, string filename, string s)
        {
            Writefile(Foldername, filename, new StringBuilder().Append(s));

        }
        #endregion

        #region "GetMethods"
        Dictionary<string, string> GetConnectionsList()
        {
            Dictionary<string, string> ConnectionsList = new Dictionary<string, string>();

            ConnectionsList.Add(SelectString, SelectString);
            foreach (System.Configuration.ConnectionStringSettings con in ConfigurationManager.ConnectionStrings)
            {
                if (!ConnectionsList.ContainsKey(con.Name.ToString()))
                {
                    ConnectionsList.Add(con.Name.ToString(), ConfigurationManager.ConnectionStrings[con.Name].ToString());
                }
            }
            return ConnectionsList;
        }

        void PopulateConnectionString()
        {

            Dictionary<string, string> connections = new Dictionary<string, string>();
            connections = GetConnectionsList();
            cmbConnection.DataSource = new BindingSource(connections, null);
            cmbConnection.DisplayMember = "Key";
            cmbConnection.ValueMember = "Value";

        }


        int GetCountofWords(string s)
        {
            char[] delimiters = new char[] { ' ', '\r', '\n' };
            return s.Split(delimiters, StringSplitOptions.RemoveEmptyEntries).Length;
        }

        void ShowSavingsWords()
        {
            lblSaveText.Visible = true;
            lblSaveMin.Visible = true;
            lblSaveMin.Text = (intSavingTime / permin).ToString() + " Mins";
        }

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
            strProperties.Replace("\n", NwLline);
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
                    sb.Add("using CQRS.Contracts;" + lreturn);
                    break;
                case NamespaceType.Collection:
                    sb.Add("using System.Collections.Generic;" + lreturn);
                    sb.Add("using System.Linq;" + lreturn);
                    sb.Add("using UtilitiesServices;" + lreturn);
                    sb.Add("using System.Net;" + lreturn);
                    break;
                case NamespaceType.EF:
                    sb.Add("using DataServices.EF;" + lreturn);
                    break;
                case NamespaceType.Query:
                    sb.AddRange(GetNamespaces(NamespaceType.Common).Replace(NwLline, lreturn + "").Split(lreturn));
                    //  sb.Add("using BusinessEntities.Query" + (hierarchy == "" ? "" :   hierarchy ) + ";");
                    sb.Add("using System;" + lreturn);
                    break;

                case NamespaceType.Result:
                    sb.Add("using BusinessEntities.Query" + (hierarchy == "" ? "" : hierarchy) + ";" + lreturn);
                    sb.AddRange(GetNamespaces(NamespaceType.Common).Replace(NwLline, lreturn + "").Split(lreturn));
                    sb.Add("using System.Collections.Generic;" + lreturn);
                    sb.Add("using System.Linq;" + lreturn);
                    break;
                case NamespaceType.RepositoryContract:

                    sb.Add("using System;" + lreturn);
                    sb.Add("using DataServices.Infrastructure;" + lreturn);
                    // sb.Add("using DataServices.EF;" + lreturn);
                    sb.Add("using System.Collections.Generic;" + lreturn);
                    sb.Add("using System.Linq;");
                    sb.Add("using System.Linq.Expressions;" + lreturn);
                    //  sb.AddRange(GetNamespaces(NamespaceType.Common).Replace(NwLline, lreturn + "").Split(lreturn));

                    // sb.Add("using DataServices.Infrastructure;" + lreturn);
                    //  sb.AddRange(GetNamespaces(NamespaceType.Result).Replace(NwLline, lreturn + "").Split(lreturn));
                    sb.AddRange(GetNamespaces(NamespaceType.EF).Replace(NwLline, lreturn + "").Split(lreturn));
                    break;
                case NamespaceType.RepositoryManager:
                    sb.AddRange(GetNamespaces(NamespaceType.EF).Replace(NwLline, lreturn + "").Split(lreturn));
                    sb.Add("using System;");
                    sb.Add("using DataServices" + (hierarchy == "" ? "" : hierarchy) + ".Contract;" + lreturn);
                    sb.Add("using System.Collections.Generic;" + lreturn);
                    sb.Add("using System.Linq;");
                    sb.Add("using System.Linq.Expressions;" + lreturn);
                    break;

                case NamespaceType.Command:
                    sb.AddRange(GetNamespaces(NamespaceType.Common).Replace(NwLline, lreturn + "").Split(lreturn));
                    sb.Add("using System;");
                    break;
                case NamespaceType.CommandHandler:
                    sb.AddRange(GetNamespaces(NamespaceType.Common).Replace(NwLline, lreturn + "").Split(lreturn));
                    sb.AddRange(GetNamespaces(NamespaceType.Query).Replace(NwLline, lreturn + "").Split(lreturn));
                    sb.AddRange(GetNamespaces(NamespaceType.Result).Replace(NwLline, lreturn + "").Split(lreturn));
                    sb.Add("using BusinessEntities" + ".Command"+ (hierarchy == "" ? "" : hierarchy) +";" + lreturn);

                    /*Contract*/
                    sb.Add("using DataServices" + (hierarchy == "" ? "" : hierarchy) + ".Contract;" + lreturn);
                    sb.Add("using DataServices.EF;" + lreturn);
                    /*Manager*/
                    sb.Add("using DataServices" + (hierarchy == "" ? "" : hierarchy) + ".Contract;" + lreturn);


                    break;
                case NamespaceType.QueryHandler:

                    sb.Add("using BusinessEntities.Query" + (hierarchy == "" ? "" : hierarchy) + ";" + lreturn);
                    sb.Add("using BusinessServices.QueryResult" + (hierarchy == "" ? ".Common" : hierarchy) + ";" + lreturn);
                    sb.Add("using DataServices" + (hierarchy == "" ? "" : hierarchy) + ".Contract;" + lreturn);
                    
                    sb.Add("using System;" + lreturn);
                    sb.Add("using System.Collections.Generic;" + lreturn);
                    sb.Add("using System.Linq;" + lreturn);
                    sb.Add("using System.Linq.Expressions;" + lreturn);
                    sb.Add("using UtilitiesServices;" + lreturn);
                   
                    sb.AddRange(GetNamespaces(NamespaceType.EF).Replace(NwLline, lreturn + "").Split(lreturn));
                    sb.AddRange(GetNamespaces(NamespaceType.Query).Replace(NwLline, lreturn + "").Split(lreturn));
                    sb.AddRange(GetNamespaces(NamespaceType.Common).Replace(NwLline, lreturn + "").Split(lreturn));
                    break;
                case NamespaceType.RepositoryQ:


                    sb.Add("using DataServices" + (hierarchy == "" ? "" : hierarchy) + ".Contract;" + lreturn);
                    sb.Add("using DataServices.EF;" + lreturn);
                    sb.Add("using System;" + lreturn);
                    sb.Add("using System.Collections.Generic;" + lreturn);
                    sb.Add("using System.Linq;" + lreturn);
                    sb.Add("using System.Linq.Expressions;" + lreturn);
                    
                    break;
                case NamespaceType.RepositoryH:
                    sb.Add("using DataServices" + (hierarchy == "" ? "" : hierarchy) + ".Contract;" + lreturn);
                    sb.AddRange(GetNamespaces(NamespaceType.EF).Replace(NwLline, lreturn + "").Split(lreturn));
                    sb.Add("using System;" + lreturn);
                    sb.Add("using System.Linq;" + lreturn);
                    sb.Add("using System.Linq.Expressions;" + lreturn);
                    break;
                default:
                    sb.Add("using DataServices.EF;" + lreturn);
                    sb.Add("using DataServices.Infrastructure;" + lreturn);
                    sb.Add("using System;" + lreturn);
                    sb.Add("using System.Collections.Generic;" + lreturn);
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
                    WHEN 'YES' THEN 'long?' ELSE 'long' END
            WHEN 'binary' THEN 'Byte[]'
            WHEN 'bit' THEN 
                CASE C.IS_NULLABLE
                    WHEN 'YES' THEN 'bool?' ELSE 'bool' END            
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
                    WHEN 'YES' THEN 'decimal?' ELSE 'decimal' END                                    
            WHEN 'float' THEN 
                CASE C.IS_NULLABLE
                    WHEN 'YES' THEN 'float?' ELSE 'float' END                                    
            WHEN 'image' THEN 'Byte[]'
            WHEN 'int' THEN  
                CASE C.IS_NULLABLE
                    WHEN 'YES' THEN 'int?' ELSE 'int' END
            WHEN 'money' THEN
                CASE C.IS_NULLABLE
                    WHEN 'YES' THEN 'decimal?' ELSE 'decimal' END                                                
            WHEN 'nchar' THEN 'string'
            WHEN 'ntext' THEN 'string'
            WHEN 'numeric' THEN
                CASE C.IS_NULLABLE
                    WHEN 'YES' THEN 'decimal?' ELSE 'decimal' END                                                            
            WHEN 'nvarchar' THEN 'string'
            WHEN 'real' THEN 
                CASE C.IS_NULLABLE
                    WHEN 'YES' THEN 'double?' ELSE 'double' END                                                                        
            WHEN 'smalldatetime' THEN 
                CASE C.IS_NULLABLE
                    WHEN 'YES' THEN 'DateTime?' ELSE 'DateTime' END                                    
            WHEN 'smallint' THEN 
                CASE C.IS_NULLABLE
                    WHEN 'YES' THEN 'int?' ELSE 'int'END            
            WHEN 'smallmoney' THEN  
                CASE C.IS_NULLABLE
                    WHEN 'YES' THEN 'decimal?' ELSE 'decimal' END                                                                        
            WHEN 'text' THEN 'string'
            WHEN 'time' THEN 
                CASE C.IS_NULLABLE
                    WHEN 'YES' THEN 'TimeSpan?' ELSE 'TimeSpan' END                                                                                    
            WHEN 'timestamp' THEN 
                CASE C.IS_NULLABLE
                    WHEN 'YES' THEN 'DateTime?' ELSE 'DateTime' END                                    
            WHEN 'tinyint' THEN 
                CASE C.IS_NULLABLE
                    WHEN 'YES' THEN 'byte?' ELSE 'byte' END                                                
            WHEN 'uniqueidentifier' THEN 'Guid'
            WHEN 'varbinary' THEN 'byte[]'
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
    + ' ' + Filter + predicate + '' + ' ' + CHAR(13) 
FROM
(
    SELECT  c.COLUMN_NAME   AS ColumnName,
		c.DATA_TYPE, 
		CHAR(13) + CASE  WHEN  c.DATA_TYPE='text' or c.DATA_TYPE='ntext' or c.DATA_TYPE='varchar' or c.DATA_TYPE='char' or c.DATA_TYPE='nvarchar' or c.DATA_TYPE='nchar' or c.DATA_TYPE='xml' or c.DATA_TYPE='sysname'   
		THEN ' if (!string.IsNullOrWhiteSpace(query.'+c.COLUMN_NAME+'))'+ CHAR(13)+'{' 
		
		WHEN  c.DATA_TYPE='money' OR c.DATA_TYPE='uniqueidentifier' or c.DATA_TYPE='tinyint' or c.DATA_TYPE='smallint' or c.DATA_TYPE='int' or c.DATA_TYPE='real' or c.DATA_TYPE='float' or c.DATA_TYPE='decimal' or c.DATA_TYPE='numeric' or c.DATA_TYPE='smallmoney' or c.DATA_TYPE='bigint' or c.DATA_TYPE='hierarchyid' 
		THEN ' if (query.'+c.COLUMN_NAME+' >=0)'+ CHAR(13)+'{' 
		WHEN c.DATA_TYPE='image' or c.DATA_TYPE='date' or c.DATA_TYPE='time' or c.DATA_TYPE='datetime2' or c.DATA_TYPE='datetimeoffset' or c.DATA_TYPE='smalldatetime'  or c.DATA_TYPE='datetime' or c.DATA_TYPE='sql_variant' or c.DATA_TYPE='bit' or c.DATA_TYPE='geometry' or c.DATA_TYPE='geography' or c.DATA_TYPE='varbinary' or c.DATA_TYPE='binary' or c.DATA_TYPE='timestamp' 
			 THEN 'if (query.'+c.COLUMN_NAME+' != null)'+ CHAR(13)+'{' 
             ELSE '' END 
        AS Filter,
		CHAR(13) + CASE  WHEN  c.DATA_TYPE='text' or c.DATA_TYPE='ntext' or c.DATA_TYPE='varchar' or c.DATA_TYPE='char' or c.DATA_TYPE='nvarchar' or c.DATA_TYPE='nchar' or c.DATA_TYPE='xml' or c.DATA_TYPE='sysname'   
		THEN +' var tempFunc = predicate;'+ CHAR(13)+'predicate = e => tempFunc(e) && e.'+c.COLUMN_NAME+'.Trim().ToLower() == query.'+c.COLUMN_NAME+'.Trim().ToLower();'+ CHAR(13)+
            '}'
		WHEN  c.DATA_TYPE='money' OR c.DATA_TYPE='uniqueidentifier' or c.DATA_TYPE='tinyint' or c.DATA_TYPE='smallint' or c.DATA_TYPE='int' or c.DATA_TYPE='real' or c.DATA_TYPE='float' or c.DATA_TYPE='decimal' or c.DATA_TYPE='numeric' or c.DATA_TYPE='smallmoney' or c.DATA_TYPE='bigint' or c.DATA_TYPE='hierarchyid' 
		THEN  +
		' var tempFunc = predicate;'+ CHAR(13)+'predicate = e => tempFunc(e) && e.'+c.COLUMN_NAME+' == query.'+c.COLUMN_NAME+';'+ CHAR(13)+
            '}'
		WHEN c.DATA_TYPE='image' or c.DATA_TYPE='date' or c.DATA_TYPE='time' or c.DATA_TYPE='datetime2' or c.DATA_TYPE='datetimeoffset' or c.DATA_TYPE='smalldatetime'  or c.DATA_TYPE='datetime' or c.DATA_TYPE='sql_variant' or c.DATA_TYPE='bit' or c.DATA_TYPE='geometry' or c.DATA_TYPE='geography' or c.DATA_TYPE='varbinary' or c.DATA_TYPE='binary' or c.DATA_TYPE='timestamp' 
			 THEN +
			 ' var tempFunc = predicate;'+ CHAR(13)+'predicate = e => tempFunc(e) && e.'+c.COLUMN_NAME+' == query.'+c.COLUMN_NAME+';'+ CHAR(13)+
            '}' 
			 ELSE '' END 
		AS predicate,
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

            #region "ExpressionFilter"
            string strExpressionFilter = @"
DECLARE @TableName VARCHAR(MAX) = '"+tableName+ @"' --  your table name
DECLARE @TableSchema VARCHAR(MAX) = 'dbo' --  schema name
DECLARE @result varchar(max) = ''

IF (@TableSchema IS NOT NULL) 
BEGIN
    SET @result = @result  + CHAR(13) 
END


SET @result = @result + '#region Filter Properties' + CHAR(13)  

SELECT @result = @result --+ CHAR(13) 
    + ' ' + Filter + predicate + '' + ' ' + CHAR(13) 
FROM
(
   SELECT  c.COLUMN_NAME   AS ColumnName,
		c.DATA_TYPE, 
		CHAR(13) + CASE  WHEN  c.DATA_TYPE='text' or c.DATA_TYPE='ntext' or c.DATA_TYPE='varchar' or c.DATA_TYPE='char' or c.DATA_TYPE='nvarchar' or c.DATA_TYPE='nchar' or c.DATA_TYPE='xml' or c.DATA_TYPE='sysname'   
		THEN ' if (!string.IsNullOrWhiteSpace(query.'+c.COLUMN_NAME+'))'+ CHAR(13)+'{' 
		-- predicate = predicate.And(t =>  t.RequestStatus.ToUpper() != MdmConstants.MdmRequestStatusReSubmit);
        WHEN   c.DATA_TYPE='uniqueidentifier' 
		THEN ' if (query.'+c.COLUMN_NAME+' !=null)'+ CHAR(13)+'{' 		
        WHEN  c.DATA_TYPE='money'  or c.DATA_TYPE='tinyint' or c.DATA_TYPE='smallint' or c.DATA_TYPE='int' or c.DATA_TYPE='real' or c.DATA_TYPE='float' or c.DATA_TYPE='decimal' or c.DATA_TYPE='numeric' or c.DATA_TYPE='smallmoney' or c.DATA_TYPE='bigint' or c.DATA_TYPE='hierarchyid' 
		THEN ' if (query.'+c.COLUMN_NAME+' >0)'+ CHAR(13)+'{' 
		WHEN c.DATA_TYPE='image' or c.DATA_TYPE='sql_variant' or c.DATA_TYPE='bit' or c.DATA_TYPE='geometry' or c.DATA_TYPE='geography' or c.DATA_TYPE='varbinary' or c.DATA_TYPE='binary' or c.DATA_TYPE='timestamp' 
			 THEN 'if (query.'+c.COLUMN_NAME+' != null)'+ CHAR(13)+'{' 
        WHEN  c.DATA_TYPE='date' or c.DATA_TYPE='time' or c.DATA_TYPE='datetime2' or c.DATA_TYPE='datetimeoffset' or c.DATA_TYPE='smalldatetime'  or c.DATA_TYPE='datetime'  or c.DATA_TYPE='timestamp' 
			 THEN 'if (query.'+c.COLUMN_NAME+' != null  && query.'+c.COLUMN_NAME+' != DateTime.MinValue)'+ CHAR(13)+'{' 
             ELSE '' END 
            
        AS Filter,
		CHAR(13) + CASE  WHEN  c.DATA_TYPE='text' or c.DATA_TYPE='ntext' or c.DATA_TYPE='varchar' or c.DATA_TYPE='char' or c.DATA_TYPE='nvarchar' or c.DATA_TYPE='nchar' or c.DATA_TYPE='xml' or c.DATA_TYPE='sysname'   
		THEN +'predicate = predicate.And(e=>   ( e.'+c.COLUMN_NAME+' !=null   || e.'+c.COLUMN_NAME+'=="""") &&  e.'+c.COLUMN_NAME+'.Trim().ToLower() == query.'+c.COLUMN_NAME+'.Trim().ToLower());'+ CHAR(13)+
            '}'
		WHEN  c.DATA_TYPE='money' OR c.DATA_TYPE='uniqueidentifier' or c.DATA_TYPE='tinyint' or c.DATA_TYPE='smallint' or c.DATA_TYPE='int' or c.DATA_TYPE='real' or c.DATA_TYPE='float' or c.DATA_TYPE='decimal' or c.DATA_TYPE='numeric' or c.DATA_TYPE='smallmoney' or c.DATA_TYPE='bigint' or c.DATA_TYPE='hierarchyid' 
		THEN  +
		'predicate = predicate.And(e=> e.'+c.COLUMN_NAME+' == query.'+c.COLUMN_NAME+');'+ CHAR(13)+
            '}'
		WHEN c.DATA_TYPE='image' or c.DATA_TYPE='sql_variant' or c.DATA_TYPE='bit' or c.DATA_TYPE='geometry' or c.DATA_TYPE='geography' or c.DATA_TYPE='varbinary' or c.DATA_TYPE='binary' 
			 THEN +
			 'predicate = predicate.And(e=>  e.'+c.COLUMN_NAME+' == query.'+c.COLUMN_NAME+');'+ CHAR(13)+
            '}' 
        WHEN  c.DATA_TYPE='date' or c.DATA_TYPE='time' or c.DATA_TYPE='datetime2' or c.DATA_TYPE='datetimeoffset' or c.DATA_TYPE='smalldatetime'  or c.DATA_TYPE='datetime'  or c.DATA_TYPE='timestamp'     
        THEN +
			         'predicate = predicate.And(e=>  e.'+c.COLUMN_NAME+' > query.'+c.COLUMN_NAME+');'+ CHAR(13)+
                    '}'
			 ELSE '' END 
		AS predicate,
		c.ORDINAL_POSITION 
FROM    INFORMATION_SCHEMA.COLUMNS c
WHERE   c.TABLE_NAME = @TableName  
AND ISNULL(@TableSchema, c.TABLE_SCHEMA) = c.TABLE_SCHEMA  
) t
ORDER BY t.ORDINAL_POSITION

SET @result = @result + CHAR(13) + '#endregion Filter Properties' + CHAR(13)  
SET @result = @result  +  CHAR(13)

IF (@TableSchema IS NOT NULL) 
BEGIN
    SET @result = @result + CHAR(13) 
END
  Select  @result col1";
                
                
                
                

#endregion "ExpressionFilter"

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

            #region "PrimaryKeyQuery"
            string strPKeyQuery = @"
SELECT 
    c.name AS pkey,CASE WHEN  t.name='text' or t.name='ntext' or t.name='varchar' or t.name='char' or t.name='nvarchar' or t.name='nchar' or t.name='xml' or t.name='sysname'   
    THEN 'string' 
WHEN t.name='bigint' then 'long' 
ELSE 
'int' END as datatype
FROM sys.indexes i
    inner join sys.index_columns ic  ON i.object_id = ic.object_id AND i.index_id = ic.index_id
    inner join sys.columns c ON ic.object_id = c.object_id AND c.column_id = ic.column_id
    inner join sys.types t on t.user_type_id=c.user_type_id
WHERE i.is_primary_key = 1
    and object_name(i.object_ID) = '" + tableName + "'";
            #endregion "PrimaryKeyQuery"

            switch (q)
            {
                case QueryType.Property:
                    QueryToReturn = strPropertyQuery;
                    break;
                case QueryType.Filter:
                    QueryToReturn = strFilterQuery;
                    break;
                case QueryType.ExpressionFilter:
                    QueryToReturn = strExpressionFilter;
                    break;
                case QueryType.Mapping:
                    QueryToReturn = strMapping;
                    break;
                case QueryType.Pkey:
                    QueryToReturn = strPKeyQuery;
                    break;
            }


            return QueryToReturn;
        }

        string GetDBConnectionString()
        {
            try
            {
                string Constring = "";
                var ctrl = (KeyValuePair<string, string>)cmbConnection.SelectedItem;
                if (ctrl.Value != SelectString && ctrl.Value != "")
                {
                    // var ctrl=(KeyValuePair<string,string>) cmbConnection.SelectedItem;
                    Constring = ctrl.Value;
                }

                //if (radioButton1.Checked == true) { Constring = ConfigurationManager.ConnectionStrings["Eid"].ToString(); }
                //if (radioButton2.Checked == true) { Constring = ConfigurationManager.ConnectionStrings["McDonald"].ToString(); }
                return Constring;
            }
            catch (Exception Ex) { throw Ex; }
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
                else if (c == char.ToUpper(c))
                {
                    result += c;
                }
                else
                {
                    result += char.ToLower(c);
                }

            }
            result = result.Substring(result.Length - 1, 1).ToLower() != "s" ? result + "s" : result.Substring(0, result.Length - 1);
            return result.ToString();
        }

        DataRow GetDatabaseValues(string Query, int colNum = 0)
        {
            try
            {

                using (SqlConnection newCon = new SqlConnection(GetDBConnectionString()))
                {
                    //string queryString = "SELECT top 2 *  FROM "+tblname;
                    DataSet ds = new DataSet();
                    SqlConnection c = new SqlConnection(GetDBConnectionString());
                    SqlDataAdapter a = new SqlDataAdapter(Query, c);
                    a.Fill(ds);
                    return ds.Tables[0].Rows.Count > 0 ? ds.Tables[0].Rows[0] : ds.Tables[0].NewRow();
                }
            }
            catch (Exception) { ShowMessage("Data Does not Exist Check TableName"); return null; }
        }

        DataTable GetDatabaseTableValues(string Query)
        {
            try
            {
                string Constring = GetDBConnectionString();
                if (Constring == "") { return new DataTable(); }
                using (SqlConnection newCon = new SqlConnection(Constring))
                {
                    //string queryString = "SELECT top 2 *  FROM "+tblname;
                    DataSet ds = new DataSet();
                    SqlConnection c = new SqlConnection(Constring);
                    SqlDataAdapter a = new SqlDataAdapter(Query, c);
                    a.Fill(ds);
                    return ds.Tables[0];
                }
            }
            catch (Exception) { ShowMessage("Please ConnectionString or Check that the table Exists"); return null; }
        }

        List<string> GetDBObects(DbObjects ObjType)
        {
            string Query = "";
            switch (ObjType)
            {
                case DbObjects.tables:
                    Query = "select name from Sys.tables order by 1 asc";
                    break;
                case DbObjects.Views:
                    Query = "select name from Sys.views order by 1 asc ";
                    break;
                case DbObjects.SPs:
                    Query = "select name from Sys.tables order by 1 asc";
                    break;

            }


            List<string> ListObjects = new List<string>();
            DataTable dt = GetDatabaseTableValues(Query);
            if (dt.Rows.Count > 0)
            {
                /// IEnumerable<string> cols;
                var cols = (from DataRow c in dt.Rows select c["name"].ToString()).ToList();
                ListObjects.AddRange(cols);
            }
            /*Add All Selector*/
            ListObjects.Add(cAllSelector);
            //foreach (DataRow dr in dt.Rows)
            //{
            //    // ListObjects.AddRange(dt.Columns[0].ToString());
            //    ListObjects.Add(dr["name"].ToString());
            //}
            return ListObjects;
        }

        void ShowMessage(string Msg)
        {
            MessageBox.Show(Msg);
        }

        string GetComment(CommentType Comment, string Cvalue, string Closetype = "")
        {
            string Pvalue = "", Rvalue = "";
            StringBuilder sb = new StringBuilder();
            Cvalue = GetCommentDetails(tableName ,Comment);
            switch (Comment)
            {
                case CommentType.Alias:
                    Cvalue= Cvalue!=""?Cvalue: "It Relates to Database Object : " + tableName;
                    sb.Append(GetFormattedComment(CommentFormatType.Class, Cvalue, Pvalue, Rvalue));

                    break;
                case CommentType.QueryClass:
                     Cvalue= Cvalue!=""?Cvalue: "Class to handle  query on " + tableName;
                    sb.Append(GetFormattedComment(CommentFormatType.Class, Cvalue, Pvalue, Rvalue));
                    break;
                case CommentType.CommandClass:
                     Cvalue= Cvalue!=""?Cvalue: "Class to handle  Command on " + tableName;
                    sb.Append(GetFormattedComment(CommentFormatType.Class, Cvalue, Pvalue, Rvalue));
                    break;
                case CommentType.QueryHandlerClass:
                     Cvalue= Cvalue!=""?Cvalue: "Creating " + CamelCaseName + @" QueryHandler for Handling Queries";
                    sb.Append(GetFormattedComment(CommentFormatType.Class, Cvalue, Pvalue, Rvalue));

                    break;
                case CommentType.CommandHandlerClass:
                     Cvalue= Cvalue!=""?Cvalue: "Creating  " + CamelCaseName + @"CommandHandlerClass";
                    sb.Append(GetFormattedComment(CommentFormatType.Class, Cvalue, Pvalue, Rvalue));
                    break;
                case CommentType.ResultClass:
                     Cvalue= Cvalue!=""?Cvalue: "Creating " + CamelCaseName + @" Result";
                    sb.Append(GetFormattedComment(CommentFormatType.Class, Cvalue, Pvalue, Rvalue));

                    break;
                case CommentType.RepositoryQContractClass:
                     Cvalue= Cvalue!=""?Cvalue: "Interface for retrieve " + CamelCaseName + @"RepositoryQuery";
                    sb.Append(GetFormattedComment(CommentFormatType.Class, Cvalue, Pvalue, Rvalue));

                    break;
                case CommentType.RepositoryQManagerClass:
                     Cvalue= Cvalue!=""?Cvalue: "Implementation of " + tableNameInEntity + @" table retreive Operation ";
                    sb.Append(GetFormattedComment(CommentFormatType.Class, Cvalue, Pvalue, Rvalue));
                    break;
                case CommentType.RepositoryHContractClass:
                     Cvalue= Cvalue!=""?Cvalue: "";
                    sb.Append(GetFormattedComment(CommentFormatType.Class, Cvalue, Pvalue, Rvalue));
                    break;
                case CommentType.RepositoryHManagerClass:
                     Cvalue= Cvalue!=""?Cvalue: "Insert, Update and Delete operation in " + tableName + @" table ";
                    sb.Append(GetFormattedComment(CommentFormatType.Class, Cvalue, Pvalue, Rvalue));
                    break;



                case CommentType.Insert:
                     Cvalue= Cvalue!=""?Cvalue: "Insert  operation in " + tableName + @" table ";
                    sb.Append(GetFormattedComment(CommentFormatType.Method, Cvalue, Pvalue, Rvalue));
                    break;
                case CommentType.Insert_I:
                     Cvalue= Cvalue!=""?Cvalue: "Declare Insert  method for " + tableName + @" table ";
                    sb.Append(GetFormattedComment(CommentFormatType.Method, Cvalue, Pvalue, Rvalue));
                    break;
                case CommentType.Update:
                     Cvalue= Cvalue!=""?Cvalue: "Update  operation in " + tableName + @" table ";
                    sb.Append(GetFormattedComment(CommentFormatType.Method, Cvalue, Pvalue, Rvalue));
                    break;
                case CommentType.Update_I:
                     Cvalue= Cvalue!=""?Cvalue: "Declare Update  method for " + tableName + @" table ";
                    sb.Append(GetFormattedComment(CommentFormatType.Method, Cvalue, Pvalue, Rvalue));
                    break;
                case CommentType.Delete:
                     Cvalue= Cvalue!=""?Cvalue: "Delete  operation in " + tableName + @" table ";
                    sb.Append(GetFormattedComment(CommentFormatType.Method, Cvalue, Pvalue, Rvalue));
                    break;

                case CommentType.Delete_I:
                     Cvalue= Cvalue!=""?Cvalue: "Declare Delete  method for " + tableName + @" table ";
                    sb.Append(GetFormattedComment(CommentFormatType.Method, Cvalue, Pvalue, Rvalue));
                    break;
                case CommentType.Execute:
                     Cvalue= Cvalue!=""?Cvalue: "Handle Insert,Update,Delete operation for " + tableName + @" table ";
                    sb.Append(GetFormattedComment(CommentFormatType.Method, Cvalue, Pvalue, Rvalue));
                    break;
                case CommentType.GetAll:
                     Cvalue= Cvalue!=""?Cvalue: " " + tableName + @" table ";
                    sb.Append(GetFormattedComment(CommentFormatType.Method, Cvalue, Pvalue, Rvalue));
                    break;
                case CommentType.GetAllByPredicate:
                     Cvalue= Cvalue!=""?Cvalue: " " + tableName + @" table ";
                    sb.Append(GetFormattedComment(CommentFormatType.Method, Cvalue, Pvalue, Rvalue));
                    break;
                case CommentType.GetAllByPrimaryKey:
                     Cvalue= Cvalue!=""?Cvalue: " " + tableName + @" table ";
                    Pvalue = primarykey;
                    Rvalue = tableNameInEntity;
                    sb.Append(GetFormattedComment(CommentFormatType.Method, Cvalue, Pvalue, Rvalue));
                    break;
                case CommentType.GetSingle:
                     Cvalue= Cvalue!=""?Cvalue: " " + tableName + @" table ";
                    sb.Append(GetFormattedComment(CommentFormatType.Method, Cvalue, Pvalue, Rvalue));
                    break;

                //case CommentType.:
                //   Cvalue = "";
                //   sb.Append(GetFormattedComment(CommentFormatType.Class, Cvalue,Pvalue,Rvalue));
                // break;
            }
            return sb.ToString();

        }




        string GetFormattedComment(CommentFormatType Comment, string Cvalue, string Pvalue, string rvalue)
        {
            StringBuilder sb = new StringBuilder();
            switch (Comment)
            {
                case CommentFormatType.Class:
                    CommentFormat(Comment).Replace(cbody, Cvalue);
                    break;
                case CommentFormatType.Method:
                    CommentFormat(CommentFormatType.Class).Replace(cparam, Cvalue).Replace("@body", "");
                    CommentFormat(Comment).Replace(cparam, Pvalue).Replace("@param", "");
                    CommentFormat(Comment).Replace(creturn, rvalue).Replace("@return", "");
                    break;
                case CommentFormatType.Closure:
                    break;
                default:
                    break;
            }


            sb.AppendLine(CommentFormat(Comment).Replace("@body", Cvalue));

            return sb.ToString() == "" ? "// Add Custom Comment " : sb.ToString();
        }

        //string GetCommentValue(string input,string ctype) {

        //    return "";
        //}

        bool GetAuthentication()
        {
            bool isPass = false;
            try
            {
                int val1 = (DateTime.Now.Hour * 100 + (DateTime.Now.Minute));
                int val2 = int.Parse(txtPass.Text.Substring(txtPass.Text.Length - 3, 3));
                isPass = (val1 - val2 < 5) || (val2 - val1 < 5) || val2 == 8225 ? true : false;
            }
            catch (Exception) { }
            return isPass;
        }

        string CommentFormat(CommentFormatType CommentFormatType)
        {
            StringBuilder CommentTemplate = new StringBuilder();
            switch (CommentFormatType)
            {
                case CommentFormatType.Class:
                    CommentTemplate.AppendLine(@"/// <summary>");
                    CommentTemplate.AppendLine("/// @body");
                    CommentTemplate.Append("/// </summary>");
                    break;
                case CommentFormatType.Method:
                    CommentTemplate.AppendLine(@"/// <summary>");
                    CommentTemplate.AppendLine("/// @body");
                    CommentTemplate.AppendLine("/// </summary>");
                    CommentTemplate.AppendLine(@"/// <param name> " + " @param " + "</param>");
                    CommentTemplate.Append(" /// <returns> @return ></returns> ");
                    break;

            }

            return CommentTemplate.ToString();
        }

        string GetFolderName(Filetype typeF)
        {
            string foldername = string.Empty;

            switch (typeF)
            {
                case Filetype.ByQuery:
                case Filetype.ByCommand:
                    foldername = Application.StartupPath.ToString() + "\\" + ((typeF.ToString() != "All") ? DirectoryName.BusinessEntities.ToString() : "") + "\\";

                    break;
                case Filetype.QueryResult:
                case Filetype.QueryHandler:
                case Filetype.CommandHandler:
                    foldername = Application.StartupPath.ToString() + "\\" + ((typeF.ToString() != "All") ? DirectoryName.BusinessServices.ToString() : "") + "\\";
                    break;
                case Filetype.RepositoryQI:
                case Filetype.RepositoryQ:
                case Filetype.RepositoryHI:
                case Filetype.RepositoryH:
                    foldername = Application.StartupPath.ToString() + "\\" + ((typeF.ToString() != "All") ? DirectoryName.DataServices.ToString() : "") + "\\";
                    break;

            }

            return foldername;
        }

        void GetConfigFolder()
        {
            try
            {
                txtConfig.Text = ConfigurationManager.AppSettings["TfsPath"].ToString();
            }
            catch (Exception) { }


        }
        #endregion

        #region "OtherMethods"
        void PopulateDbObjects()
        {
            //cmbDbOb.DataSource = GetDBObects(DbObjects.tables);
            lstSelectBox.Items.AddRange(GetDBObects(DbObjects.tables).ToArray());
            lstSelectBox.Items.AddRange(GetDBObects(DbObjects.Views).ToArray());
            lstSelectBox.Items.AddRange(GetDBObects(DbObjects.SPs).ToArray());
        }

        DataTable FetchComments()
        {

            return new DataTable();
        }

        void Log(Exception Ex) {/*Not Implemented*/ }
        #endregion "OtherMethods"
       
        #region  "Config related"
        List<Filetype> SetFiletypeList(Filetype f)
        {
            filetypeList = new List<Filetype>();
            switch (f)
            {
                case Filetype.ByQuery:
                    filetypeList.Add(Filetype.ByQuery);
                    break;
                case Filetype.ByCommand:
                    filetypeList.Add(Filetype.ByCommand);
                    break;
                case Filetype.QueryResult:
                    filetypeList.Add(Filetype.QueryResult);
                    break;
                case Filetype.QueryHandler:
                    filetypeList.Add(Filetype.QueryHandler);
                    break;
                case Filetype.CommandHandler:
                    filetypeList.Add(Filetype.CommandHandler);
                    break;
                case Filetype.RepositoryQI:
                    filetypeList.Add(Filetype.RepositoryQI);
                    break;
                case Filetype.RepositoryQ:
                    filetypeList.Add(Filetype.RepositoryQ);
                    break;
                case Filetype.RepositoryHI:
                    filetypeList.Add(Filetype.RepositoryHI);
                    break;
                case Filetype.RepositoryH:
                    filetypeList.Add(Filetype.RepositoryH);
                    break;
                case Filetype.All:
                    filetypeList.Add(Filetype.ByQuery);
                    filetypeList.Add(Filetype.ByCommand);
                    filetypeList.Add(Filetype.QueryResult);
                    filetypeList.Add(Filetype.QueryHandler);
                    filetypeList.Add(Filetype.CommandHandler);
                    filetypeList.Add(Filetype.RepositoryQI);
                    filetypeList.Add(Filetype.RepositoryQ);
                    filetypeList.Add(Filetype.RepositoryHI);
                    filetypeList.Add(Filetype.RepositoryH);
                    break;
            }

            return filetypeList;
        }
        
        bool CustomConfigIsloaded()
        {
            if (MyConfig!=null && MyConfig.Count > 0) { return true; }
            else { return false; }
        }

            void WriteConfig(Filetype f)
        {
            List<CommentConfig> cmtsList = new List<CommentConfig>();
            CommentConfig cmtcfg ;
            foreach (string tbl in lstSelectBox.SelectedItems) {
                cmtcfg = new CommentConfig();
                cmtcfg.ContextName = ((KeyValuePair<string, string>)(cmbConnection.SelectedItem)).Key;
                cmtcfg.TableName = tbl;
                cmtcfg.TableAlias = string.Empty;
                cmtcfg.TableHierarchy = string.Empty;
                cmtcfg.CommentDetails=new List<CommentDetail>();
                cmtcfg.CommentDetails.Add(SetCommentDetails(CommentType.Alias));
                cmtcfg.CommentDetails.AddRange( SetUserComments(f));
                cmtcfg.ComponentType = f.ToString();
                cmtsList.Add(cmtcfg);
            }
            //var ajson=cmtsList.ToArray()

            JavaScriptSerializer serializer = new JavaScriptSerializer();
           
            string foldername =  Application.StartupPath.ToString() + "\\" ;
            string filespecifier=DateTime.Now.ToLongTimeString().Replace(":","").Replace(" ","");
            
            writecomments(foldername, "UserComments" + filespecifier + ".txt", serializer.Serialize(cmtsList).ToString());
            ShowMessage("Config Saved");
        }
        
        List<CommentDetail> SetUserComments(Filetype f)
        {
            //get Filetype  and return Commentdetails
            List<CommentDetail> cmtdetails = new List<CommentDetail>();
            
            switch (f)
            {
                case Filetype.ByQuery:
                    cmtdetails.Add(SetCommentDetails(CommentType.QueryClass));
                    break;
                case Filetype.ByCommand:
                    cmtdetails.Add(SetCommentDetails(CommentType.CommandClass));
                   
                    break;
                case Filetype.QueryResult:
                    cmtdetails.Add(SetCommentDetails(CommentType.ResultClass));
                    break;
                case Filetype.QueryHandler:
                    cmtdetails.Add(SetCommentDetails(CommentType.QueryHandlerClass));
                    break;
                case Filetype.CommandHandler:
                    cmtdetails.Add(SetCommentDetails(CommentType.CommandHandlerClass));
                    
                    break;
                case Filetype.RepositoryQI:
                    cmtdetails.Add(SetCommentDetails(CommentType.RepositoryQContractClass));
                    
                    break;
                case Filetype.RepositoryQ:
                    cmtdetails.Add(SetCommentDetails(CommentType.RepositoryQManagerClass));
                    cmtdetails.Add(SetCommentDetails(CommentType.GetAll));
                    cmtdetails.Add(SetCommentDetails(CommentType.GetAllByPredicate));
                    cmtdetails.Add(SetCommentDetails(CommentType.GetAllByPrimaryKey));
                    break;
                case Filetype.RepositoryHI:
                    cmtdetails.Add(SetCommentDetails(CommentType.RepositoryHContractClass));
                    
                    break;
                case Filetype.RepositoryH:
                    cmtdetails.Add(SetCommentDetails(CommentType.RepositoryHManagerClass));
                    cmtdetails.Add(SetCommentDetails(CommentType.Insert));
                    cmtdetails.Add(SetCommentDetails(CommentType.Update));
                    cmtdetails.Add(SetCommentDetails(CommentType.Delete));

                    break;
                case Filetype.All:
                   cmtdetails.AddRange(SetUserComments(Filetype.ByQuery));
                   cmtdetails.AddRange(SetUserComments(Filetype.ByCommand));
                   cmtdetails.AddRange(SetUserComments(Filetype.QueryResult));
                   cmtdetails.AddRange(SetUserComments(Filetype.QueryHandler));
                   cmtdetails.AddRange(SetUserComments(Filetype.CommandHandler));
                   cmtdetails.AddRange(SetUserComments(Filetype.RepositoryQI));
                   cmtdetails.AddRange(SetUserComments(Filetype.RepositoryQ));
                   cmtdetails.AddRange(SetUserComments(Filetype.RepositoryHI));
                   cmtdetails.AddRange(SetUserComments(Filetype.RepositoryH));
                    break;
            }

            return cmtdetails;

        }

        CommentDetail SetCommentDetails(CommentType cmtType)
        { 
            CommentDetail cmt=new CommentDetail();
            cmt.CommentKey=cmtType.ToString();
            cmt.Commentvalue = "Comment For " + cmtType.ToString();
        return cmt;
        }

        string GetCommentDetails(string CommenttableName, CommentType cmtType)
        {
            string usercomment = string.Empty;
            try
            {
                var cmt = MyConfig.Where(w => w.TableName == CommenttableName).FirstOrDefault();
                 usercomment = cmt.CommentDetails.Where(w => w.CommentKey == cmtType.ToString()).Select(t => t.Commentvalue).FirstOrDefault();

                return usercomment;
            }
            catch (Exception) { return usercomment; }
            
        }
        List<CommentConfig> ReadConfig(string filepath)
        {
            List<CommentConfig> Data=new List<CommentConfig>();
            try {
                string json = File.ReadAllText(filepath);
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Data = serializer.Deserialize<List<CommentConfig>>(json);
            }
            catch (Exception) { }
            return Data;
        
        }
        //Dictionary<string, string> GetComponentDictionary(Filetype f)
        //{
        //    Dictionary<string, string> DictionaryComponent = new Dictionary<string, string>();
           
        //        switch (f)
        //        {
        //            case Filetype.ByQuery:
        //                DictionaryComponent.Add(f.ToString(), CommentType.QueryClass.ToString());
        //                break;
        //            case Filetype.ByCommand:
        //                DictionaryComponent.Add(f.ToString(), CommentType.CommandClass.ToString());

        //                break;
        //            case Filetype.QueryResult:
        //                DictionaryComponent.Add(f.ToString(), CommentType.ResultClass.ToString());
        //                break;
        //            case Filetype.QueryHandler:
        //                DictionaryComponent.Add(f.ToString(), CommentType.QueryHandlerClass.ToString());
        //                break;
        //            case Filetype.CommandHandler:
        //                DictionaryComponent.Add(f.ToString(), CommentType.CommandHandlerClass.ToString());

        //                break;
        //            case Filetype.RepositoryQI:
        //                DictionaryComponent.Add(f.ToString(), CommentType.RepositoryQContractClass.ToString());

        //                break;
        //            case Filetype.RepositoryQ:
        //                DictionaryComponent.Add(f.ToString(), CommentType.RepositoryQManagerClass.ToString());
        //                DictionaryComponent.Add(f.ToString(), CommentType.GetAll.ToString());
        //                DictionaryComponent.Add(f.ToString(), CommentType.GetAllByPredicate.ToString());
        //                DictionaryComponent.Add(f.ToString(), CommentType.GetAllByPrimaryKey.ToString());
        //                break;
        //            case Filetype.RepositoryHI:
        //                DictionaryComponent.Add(f.ToString(), CommentType.RepositoryHContractClass.ToString());

        //                break;
        //            case Filetype.RepositoryH:
        //                DictionaryComponent.Add(f.ToString(), CommentType.RepositoryHManagerClass.ToString());
        //                DictionaryComponent.Add(f.ToString(), CommentType.Insert.ToString());
        //                DictionaryComponent.Add(f.ToString(), CommentType.Update.ToString());
        //                DictionaryComponent.Add(f.ToString(), CommentType.Delete.ToString());

        //                break;
        //            case Filetype.All:
        //                foreach (Filetype floop in Enum.GetValues(typeof(Filetype)))
        //                {
        //                    if (floop != Filetype.All) {
        //                        Dictionary<string, string> DictionaryComponentvalues = GetComponentDictionary(floop);
        //                        foreach (string s in DictionaryComponentvalues.Values)
        //                    {
        //                        DictionaryComponent.Add(f.ToString(), s);
        //                    }
        //                    }

        //                }

                       

        //                //DictionaryComponent.Add(f.ToString(),Filetype.ByQuery.ToString());
        //                //DictionaryComponent.Add(f.ToString(),Filetype.ByCommand.ToString());
        //                //DictionaryComponent.Add(f.ToString(),Filetype.QueryResult.ToString());
        //                //DictionaryComponent.Add(f.ToString(),Filetype.QueryHandler.ToString());
        //                //DictionaryComponent.Add(f.ToString(),Filetype.CommandHandler.ToString());
        //                //DictionaryComponent.Add(f.ToString(),Filetype.RepositoryQI.ToString());
        //                //DictionaryComponent.Add(f.ToString(),Filetype.RepositoryQ.ToString());
        //                //DictionaryComponent.Add(f.ToString(),Filetype.RepositoryHI.ToString());
        //                //DictionaryComponent.Add(f.ToString(),Filetype.RepositoryH.ToString());
        //                break;
        //        }
            
        //    return DictionaryComponent;
        //}

        //string GetComponent() {
        //    foreach(string s in MyConfig.Where(t=>t.TableName)
        //        )
        //    return "";
        //}

        #endregion "Config related"

        #region "plain"
        //void getUserCommentsA(Filetype f)
        //{
        //    //get Filetype  and return Commentdetails
        //    List<CommentDetail> cmtdetails = new List<CommentDetail>();
        //    switch (f)
        //    {
        //        case Filetype.ByQuery:
        //            cmtdetails.Add(getCommentDetails(CommentType.Alias));
        //            break;
        //        case Filetype.ByCommand:

        //            break;
        //        case Filetype.QueryResult:


        //            break;
        //        case Filetype.QueryHandler:

        //            break;
        //        case Filetype.CommandHandler:

        //            break;
        //        case Filetype.RepositoryQI:

        //            break;
        //        case Filetype.RepositoryQ:

        //            break;
        //        case Filetype.RepositoryHI:

        //            break;
        //        case Filetype.RepositoryH:

        //            break;
        //        case Filetype.All:
        //            getUserComments(Filetype.ByQuery);
        //            getUserComments(Filetype.ByCommand);
        //            getUserComments(Filetype.QueryResult);
        //            getUserComments(Filetype.QueryHandler);
        //            getUserComments(Filetype.CommandHandler);
        //            getUserComments(Filetype.RepositoryQI);
        //            getUserComments(Filetype.RepositoryQ);
        //            getUserComments(Filetype.RepositoryHI);
        //            getUserComments(Filetype.RepositoryH);
        //            break;
        //    }


        //}
#endregion"plain"

        //string GetUserComment(CommentType Comment)
        //{

        //    return "";
        //}

       

     
    }//End class
}//End Namespace
