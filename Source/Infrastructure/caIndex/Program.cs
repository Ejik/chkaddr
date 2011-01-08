using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using caIndex.KladrDataSetTableAdapters;
using JRO;


namespace caIndex
{
    class Program
    {
        private static string _mdb;
        private static string _jroPath;
        private static OleDbConnection mdbCon;
        private const int _filelength = 225280;
        private static Assembly _jro;


        static void Main(string[] args)
        {
            _mdb = Environment.CurrentDirectory + "\\kladr.mdb";
            _jroPath = Environment.CurrentDirectory + "\\Interop.JRO.dll";

            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.AssemblyResolve += new ResolveEventHandler(AssemblyResolveEventHandler);
            

            if (TestMdbFile())
            {
                try
                {
                    string sql = "";
                    //KladrDataSet kladrDataSet = new KladrDataSet();

                    // Полключаемся к файлу БД mdb
                    mdbCon = new OleDbConnection(GetConnectionString(_mdb));
                    mdbCon.Open();
                                       
                    // Подключаемся к файлу KLADR
                    //OleDbConnection dbfCon = new OleDbConnection(GetDBFConnectionString(Environment.CurrentDirectory));
                    //dbfCon.Open();
                    
                    //sql = "INSERT INTO KLADR ( NAME, SOCR, CODE, [INDEX] )";
                    //sql = "SELECT NAME, SOCR, CODE, INDEX FROM KLADR";
                    //OleDbDataAdapter adapter = new OleDbDataAdapter(sql, mdbCon);
                    //KLADRTableTableAdapter adapter = new KLADRTableTableAdapter();
                    //adapter.Connection = mdbCon;
                    //OleDbCommandBuilder custCB = new OleDbCommandBuilder(adapter);
                    //adapter.InsertCommand = custCB.GetInsertCommand();
                    //adapter.UpdateCommand = custCB.GetUpdateCommand();
                    //mdbCon.Open();

                    //adapter.Connection.Open();

                    //int i;
                    //adapter.ClearBeforeFill = false;
                    //int i = adapter.Fill(kladrDataSet.KLADRTable);


                    //sql = "SELECT * FROM KLADR";
                    
                    //OleDbCommand cmd = new OleDbCommand(sql, dbfCon);                                       
                    //OleDbDataReader reader = cmd.ExecuteReader();
                    //kladrDataSet.KLADRTable.Load(reader, LoadOption.OverwriteChanges);

                    //while (reader.Read())
                    //{
                    //    kladrDataSet.KLADRTable.AddKLADRTableRow(reader.GetString(0), reader.GetString(1),
                    //                                             reader.GetString(2), reader[3].ToString());
                    //}

                    
                    //sql = "INSERT INTO KLADR ( NAME, SOCR, CODE, [INDEX] ) VALUES (?, ?, ?, ?)";

                    Console.WriteLine("Индексируется файл KLADR.");
                    ImportTable("KLADR");

                    Console.WriteLine("Индексируется файл STREET.");
                    ImportTable("STREET");
                    
                    //cmd.Parameters.Add(new OleDbParameter("NAME", OleDbType.VarWChar, 40, "NAME"));
                    //cmd.Parameters.Add(new OleDbParameter("SOCR", OleDbType.VarWChar, 10, "SOCR"));
                    //cmd.Parameters.Add(new OleDbParameter("CODE", OleDbType.VarWChar, 13, "CODE"));
                    //cmd.Parameters.Add(new OleDbParameter("INDEX", OleDbType.VarWChar, 6, "INDEX"));

                    //foreach (KladrDataSet.KLADRTableRow row in kladrDataSet.KLADRTable)
                    //{
                    //    cmd.Parameters["NAME"].Value = row.NAME;
                    //    cmd.Parameters["SOCR"].Value = row.SOCR;
                    //    cmd.Parameters["CODE"].Value = row.CODE;
                        
                    //    cmd.Parameters["INDEX"].Value = row.IsNull("INDEX") ? "" : row.INDEX;
                    //    cmd.ExecuteNonQuery();
                    //}


                    //while (reader.Read())
                    //{
                    //    kladrDataSet.KLADRTable.AddKLADRTableRow(reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.IsDBNull(3) ? "" : reader.GetString(3));
                    //}
                    //i = adapter.Update(kladrDataSet.KLADRTable);

                    Console.WriteLine("Проверка целостности данных.");
                    
                    //dbfCon.Close();
                    mdbCon.Close();
                    CompactDataBase();

                    // Закомментированные строки string 
                    // sql = "INSERT INTO KLADR ( NAME, SOCR, CODE, [INDEX] ) " +
                    //             "SELECT NAME, SOCR, CODE, [INDEX] FROM [dBASE III;DATABASE=" +
                    //             Environment.CurrentDirectory + "\\].[kladr.dbf];";
                    //sql =          "SELECT NAME, SOCR, CODE, [INDEX] FROM [dBASE III;DATABASE=" + Environment.CurrentDirectory + "\\].[kladr.dbf];";


                    //cmd = new OleDbCommand(sql, mdbCon);
                    //Console.WriteLine("Индексируется файл KLADR.");
                    
                    // Закомментированные строки 
                    // cmd.ExecuteNonQuery();
                    //OleDbDataReader reader = cmd.ExecuteReader();

                    //sql = "INSERT INTO KLADR ( NAME, SOCR, CODE, [INDEX] )";
                    //OleDbDataAdapter adapter = new OleDbDataAdapter(sql, mdbCon);



                    //sql = "INSERT INTO STREET ( NAME, SOCR, CODE, [INDEX] ) " +
                    //      "SELECT NAME, SOCR, CODE, [INDEX] FROM [dBASE III;DATABASE=" + Environment.CurrentDirectory +
                    //      "\\].[street.dbf];";
                    //cmd.CommandText = sql;
                    //Console.WriteLine("Индексируется файл STREET.");
                    //Console.WriteLine("Дождитесь окончания операции.");
                    //mdbCon.Close();

                    Console.WriteLine("Операция выполненена успешно. Спасибо за ваше ожидание. Теперь можно работать с программой CHKADDR.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка при создании индексов ");
                    
                }
            }
            else
                Console.WriteLine("Плохой файл kladr.mdb!");

            Console.ReadKey(true);
        }

        private static Assembly AssemblyResolveEventHandler(object sender, ResolveEventArgs args)
        {
            //This handler is called only when the common language runtime tries to bind to the assembly and fails.

            //Retrieve the list of referenced assemblies in an array of AssemblyName.
            Assembly jroAssembly, objExecutingAssemblies;
            string strTempAssmbPath = "";

            objExecutingAssemblies = Assembly.GetExecutingAssembly();
            AssemblyName[] arrReferencedAssmbNames = objExecutingAssemblies.GetReferencedAssemblies();

            //Loop through the array of referenced assembly names.
            foreach (AssemblyName strAssmbName in arrReferencedAssmbNames)
            {
                //Check for the assembly names that have raised the "AssemblyResolve" event.
                if (strAssmbName.FullName.Substring(0, strAssmbName.FullName.IndexOf(",")) == args.Name.Substring(0, args.Name.IndexOf(",")))
                {
                    //Build the path of the assembly from where it has to be loaded.				
                    strTempAssmbPath = Environment.CurrentDirectory + "\\" + args.Name.Substring(0, args.Name.IndexOf(",")) + ".dll";
                    break;
                }

            }
            //Load the assembly from the specified path. 					
            jroAssembly = Assembly.LoadFrom(strTempAssmbPath);

            //Return the loaded assembly.
            return jroAssembly;			
        }

        private static void CompactDataBase()
        {
            string mdb = Environment.CurrentDirectory + "\\kladr.mdb";
            string src = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mdb;
            string dest = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mdb + "~";
            
            JetEngine jet = new JetEngine();
            jet.CompactDatabase(src, dest);

            File.Delete(mdb);
            Directory.Move(mdb + "~", mdb);
        }

        /// <summary>
        /// Метод создает хранимую процедуру в базе и выполняет ее
        /// </summary>
        /// <param name="tblName">имя таблицы</param>
        /// <returns>строка подключения</returns>
        private static void ImportTable(string tblName)
        {
            string sql = "CREATE PROC import" + tblName + " AS INSERT INTO " + tblName + " ( NAME, SOCR, CODE, [INDEX] ) " +
                  "SELECT NAME, SOCR, CODE, [INDEX] FROM [dBASE III;DATABASE=" +
                  Environment.CurrentDirectory + "\\].[" + tblName + ".dbf];";
            OleDbCommand cmd = new OleDbCommand(sql, mdbCon);                    
            
            cmd.ExecuteNonQuery();

            sql = "EXEC import" + tblName;
            cmd = new OleDbCommand(sql, mdbCon);
            cmd.ExecuteNonQuery();
       }

        /// <summary>
        /// Метод возвращает строку подключения к базе kladr.mdb
        /// </summary>
        /// <param name="dbPath">строка путь к файлу базы kladr.mdb</param>
        /// <returns>строка подключения</returns>
        private static string GetConnectionString(string dbPath)
        {
            StringBuilder connText = new StringBuilder();
            connText.Append("Provider=Microsoft.Jet.OLEDB.4.0;");
            connText.Append("Data Source=\"").Append(dbPath).Append("\";");
            connText.Append("Jet OLEDB:Engine Type=5;");
            connText.Append("Jet OLEDB:Encrypt Database=True;");
            connText.Append("User ID=\"Admin\";");

            return connText.ToString();
        }

        /// <summary>
        /// Метод возвращает строку подключения к базе kladr.dbf
        /// </summary>
        /// <param name="dbPath">строка путь к файлу базы kladr.dbf</param>
        /// <returns>строка подключения</returns>
        private static string GetDBFConnectionString(string dbPath)
        {
            StringBuilder connText = new StringBuilder();
            connText.Append("Provider=Microsoft.Jet.OLEDB.4.0;");
            connText.Append("Data Source=\"").Append(dbPath).Append("\";");
            //connText.Append("Jet OLEDB:Engine Type=5;");
            //connText.Append("Jet OLEDB:Encrypt Database=True;");
            connText.Append("Extended Properties=dBASE IV;");
            connText.Append("User ID=\"Admin\";");

            return connText.ToString();
        }



        private static bool TestMdbFile()
        {
            FileInfo fi = new FileInfo(_mdb);
            
            if (fi.Exists)
            {
                fi.Delete();
            }

            // Извлекаем чистый файл из ресурсов и сохраняем на диск.
            byte[] mf = Properties.Resources.ResourceManager.GetObject("kladr") as byte[];
            File.WriteAllBytes(_mdb, mf);

            
            if (!File.Exists(_jroPath))
            {
                mf = Properties.Resources.ResourceManager.GetObject("Interop_JRO") as byte[];
                File.WriteAllBytes(_jroPath, mf);
            }
            //fi = new FileInfo(_mdb);
            //if ((fi.Length > _filelength) && (fi.Length < 50000000))
            //    return false;

            return true;
        }
    }
}
