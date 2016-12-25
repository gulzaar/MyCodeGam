using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace CodeGenMe
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        StringBuilder sbm=new StringBuilder();
       const  string NwLline = "\r\n";
     public    enum filetype
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
        private void button2_Click(object sender, EventArgs e)
        {
            
            StringBuilder sb = new StringBuilder();


            switch (filetype.ByCommand)
            {
                case filetype.ByQuery:
                   
                    break;
                case filetype.ByCommand:
                    break;
                case filetype.Result:
                    break;
                case filetype.HandlerQ:
                    break;
                case filetype.HandlerC:
                    break;
                case filetype.RepositoryQI:
                    break;
                case filetype.RepositoryQ:
                    break;
                case filetype.RepositoryHI:
                    break;
                case filetype.RepositoryH:
                    break;

            }


        }


        string ByQuery() { 
            sbm=new StringBuilder();
            sbm.AppendLine("using System;" + NwLline);
            sbm.AppendLine("namespace BusinessEntity "+NwLline+" {"+NwLline+" public class "+txttableName.Text+":"+"IQuery" +NwLline+"{"+NwLline);
            sbm.AppendLine( NwLline);

            string str = txtClass.Text;

            str.Replace("private"," public ");
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
           sbm.AppendLine(str+NwLline);
            sbm.AppendLine("/*Implements IQuery*/ "+NwLline);
            sbm.AppendLine("public RetrieveBy {get;set;}");
            sbm.AppendLine("}"+NwLline+"}");
            
            return sbm.ToString(); 
        }
        string ByCommand()
        {
            sbm = new StringBuilder();
            sbm.AppendLine("using System;" + NwLline);
            sbm.AppendLine("namespace BusinessEntity " + NwLline + " {" + NwLline + " public class" + txttableName.Text + ":" + "IQuery" + NwLline + "{" + NwLline);
            sbm.AppendLine(NwLline);
            sbm.AppendLine(NwLline);
            sbm.AppendLine("/*Implements IQuery*/ " + NwLline);
            sbm.AppendLine("public RetrieveBy {get;set;}");
            sbm.AppendLine("}" + NwLline + "}");

            return sbm.ToString();
        }
        string Result()
        {
            sbm = new StringBuilder();
            sbm.AppendLine("using System;" + NwLline);
            sbm.AppendLine("namespace BusinessEntity " + NwLline + " {" + NwLline + " public class" + txttableName.Text + ":" + "IQuery" + NwLline + "{" + NwLline);
            sbm.AppendLine(NwLline);
            sbm.AppendLine(NwLline);
            sbm.AppendLine("/*Implements IQuery*/ " + NwLline);
            sbm.AppendLine("public RetrieveBy {get;set;}");
            sbm.AppendLine("}" + NwLline + "}");

            return sbm.ToString();
        }
        string HandlerQ()
        {
            sbm = new StringBuilder();
            sbm.AppendLine("using System;" + NwLline);
            sbm.AppendLine("namespace BusinessEntity " + NwLline + " {" + NwLline + " public class" + txttableName.Text + ":" + "IQuery" + NwLline + "{" + NwLline);
            sbm.AppendLine(NwLline);
            sbm.AppendLine(NwLline);
            sbm.AppendLine("/*Implements IQuery*/ " + NwLline);
            sbm.AppendLine("public RetrieveBy {get;set;}");
            sbm.AppendLine("}" + NwLline + "}");

            return sbm.ToString();
        }
        string HandlerC()
        {
            sbm = new StringBuilder();
            sbm.AppendLine("using System;" + NwLline);
            sbm.AppendLine("namespace BusinessEntity " + NwLline + " {" + NwLline + " public class" + txttableName.Text + ":" + "IQuery" + NwLline + "{" + NwLline);
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
            sbm.AppendLine("namespace BusinessEntity " + NwLline + " {" + NwLline + " public class" + txttableName.Text + ":" + "IQuery" + NwLline + "{" + NwLline);
            sbm.AppendLine(NwLline);
            sbm.AppendLine(NwLline);
            sbm.AppendLine("/*Implements IQuery*/ " + NwLline);
            sbm.AppendLine("public RetrieveBy {get;set;}");
            sbm.AppendLine("}" + NwLline + "}");

            return sbm.ToString();
        }
        string RepositoryQ()
        {
            sbm = new StringBuilder();
            sbm.AppendLine("using System;" + NwLline);
            sbm.AppendLine("namespace BusinessEntity " + NwLline + " {" + NwLline + " public class" + txttableName.Text + ":" + "IQuery" + NwLline + "{" + NwLline);
            sbm.AppendLine(NwLline);
            sbm.AppendLine(NwLline);
            sbm.AppendLine("/*Implements IQuery*/ " + NwLline);
            sbm.AppendLine("public RetrieveBy {get;set;}");
            sbm.AppendLine("}" + NwLline + "}");

            return sbm.ToString();
        }
        string RepositoryHI()
        {
            sbm = new StringBuilder();
            sbm.AppendLine("using System;" + NwLline);
            sbm.AppendLine("namespace BusinessEntity " + NwLline + " {" + NwLline + " public class" + txttableName.Text + ":" + "IQuery" + NwLline + "{" + NwLline);
            sbm.AppendLine(NwLline);
            sbm.AppendLine(NwLline);
            sbm.AppendLine("/*Implements IQuery*/ " + NwLline);
            sbm.AppendLine("public RetrieveBy {get;set;}");
            sbm.AppendLine("}" + NwLline + "}");

            return sbm.ToString();
        }
        string RepositoryH()
        {
            sbm = new StringBuilder();
            sbm.AppendLine("using System;" + NwLline);
            sbm.AppendLine("namespace BusinessEntity " + NwLline + " {" + NwLline + " public class" + txttableName.Text + ":" + "IQuery" + NwLline + "{" + NwLline);
            sbm.AppendLine(""+NwLline);
            sbm.AppendLine(""+NwLline);
            sbm.AppendLine("/*Implements IQuery*/ " + NwLline);
            sbm.AppendLine("public RetrieveBy {get;set;}");
            sbm.AppendLine("}" + NwLline + "}");

            return sbm.ToString();
        }

      void  writeTOdisk(filetype f){
          string foldername = Application.StartupPath.ToString()+"\\";
          string filename =  txttableName.Text; 
          StringBuilder sb=new StringBuilder();
         
       switch (f)
            {
                case filetype.ByQuery:
                    filename+="ByQuery.cs";
               sb.Append(ByQuery());
                    break;
                case filetype.ByCommand:
               filename+="ByCommand.cs";
               sb.Append(ByCommand());
                    break;
                case filetype.Result:
               filename+="ByQuery.cs";
               sb.Append(Result());
                    break;
                case filetype.HandlerQ:
               filename+="QueryResult.cs";
                sb.Append(HandlerQ());
                    break;
                case filetype.HandlerC:
               filename+="Handler.cs";
                sb.Append(HandlerC());
                    break;
                case filetype.RepositoryQI:
                    filename = "I" + filename + "RepositoryQuery.cs";
                sb.Append(RepositoryQI());
                    break;
                case filetype.RepositoryQ:
                    filename +=  "RepositoryQuery.cs";
                sb.Append(RepositoryQ());
                    break;
                case filetype.RepositoryHI:
                    filename = "I" + filename + "RepositoryHandler.cs";
                sb.Append(RepositoryHI());
                    break;
                case filetype.RepositoryH:
                              filename+="RepositoryHandler.cs";

                sb.Append(RepositoryH());
                    break;

            }
           if (!File.Exists(filename))
            {
                System.IO.File.WriteAllText(filename, sb.ToString());
            };
      
      }

      private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
      {
          writeTOdisk((filetype)comboBox1.SelectedIndex);
      }
    }
}
