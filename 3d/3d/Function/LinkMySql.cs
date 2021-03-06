﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Net;
using System.Text.RegularExpressions;

using MySql.Data.MySqlClient;

namespace _3d.Function
{
    class LinkMySql
    {
        
        #region 数据库配置

        /// <summary>
        /// 读取数据库配置，加载一些基本信息
        /// </summary>
        public static void getSQLcfg(string serverAddress)
        {
            string serverXmlFile = serverAddress;

            Tools wc = new Tools();
            string pageXML = wc.getOnlineXML(serverXmlFile);

            //读取数据库信息
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(pageXML);
            XmlNode rootNode = xml.SelectSingleNode("MySQLConfig");//得到根节点
            XmlNode addressNode = rootNode.SelectSingleNode("Address");//地址
            XmlNode portNode = rootNode.SelectSingleNode("Port");//端口
            XmlNode usernameNode = rootNode.SelectSingleNode("Username");//用户名
            XmlNode passwordNode = rootNode.SelectSingleNode("Password");//密码
            XmlNode databaseNode = rootNode.SelectSingleNode("Database");//数据库
            XmlNode charsetNode = rootNode.SelectSingleNode("Charset");//编码
            XmlNode use_tableNode = rootNode.SelectSingleNode("use_table");//所用表

            Global.sqlAddress = addressNode.InnerText;
            Global.sqlPort = portNode.InnerText;
            Global.sqlUsername = usernameNode.InnerText;
            Global.sqlPwd = passwordNode.InnerText;
            Global.sqlDB = databaseNode.InnerText;
            Global.sqlCharset = charsetNode.InnerText;
            Global.sqlUserTable = use_tableNode.InnerText;

            //读取当前软件版本
            string localXmlFile = Application.StartupPath + "\\UpdateList.xml";
            xml = new XmlDocument();
            if (!File.Exists(localXmlFile)) {
                Tools.writeFile(localXmlFile, _3d.Properties.Resources.UpdateList);
            }
            xml.Load(localXmlFile);
            XmlNode xn = xml.SelectSingleNode("//Files/File");
            Global.version = xn.Attributes["Ver"].InnerText;

            // 获取IP地址和归属地
            getWebIPNew();
        }

        /// <summary>
        /// 获取IP地址和归属地
        /// </summary>
        /// <returns></returns>
        public static void getWebIPNew()
        {
            try
            {
                WebRequest wr = WebRequest.Create("http://eztx.eztx.cn/getIP.html");
                Stream s = wr.GetResponse().GetResponseStream();
                StreamReader sr = new StreamReader(s, Encoding.Default);
                string all = sr.ReadToEnd(); //读取网站的数据

                /*Regex r = new Regex("((25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]\\d|\\d)\\.){3}(25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]\\d|[1-9])", RegexOptions.None);
                Match mc = r.Match(all);
                // 获取匹配到的IP
                tempip = mc.Groups[0].Value;
                sr.Close();
                s.Close();*/

                if (all != null && all.Trim().Length > 0) {
                    string[] IPinfos = all.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    Global.globalIP = IPinfos[0];
                    Global.globalPlace = IPinfos[1];
                }
                sr.Close();
                s.Close();
            }
            catch
            {

            }
        }
        #endregion


        #region 软件数据库连接

        /// <summary>
        /// 传入SQL语句，返回DataTable结果
        /// </summary>
        /// <param name="sql">String SQL</param>
        /// <returns>DataTable</returns>
        public DataTable conns(string sql)
        {
            DataTable t = new DataTable();
            MySqlConnection conn = null;
            MySqlCommand command = null;
            //conn = new MySqlConnection("Server=svn.breadth.cn;User Id=root;Password=breadth2009;Persist Security Info=True;Database=mez;Port=3307;CharSet=utf8");
            //双线
            //conn = new MySqlConnection("Server=mysql.sql73.cdncenter.net;User Id=sq_mezboy;Password=mez199023;Persist Security Info=True;Database=sq_mezboy;Port=3306;CharSet=utf8");
            //电信
            //conn = new MySqlConnection("Server=mysql.sql47.cdncenter.net;User Id=sq_maenze;Password=mez199023;Persist Security Info=True;Database=sq_maenze;Port=3306;CharSet=utf8");
            conn = new MySqlConnection(
                "Server=" + Global.sqlAddress 
                + ";User Id=" + Global.sqlUsername 
                + ";Password=" + Global.sqlPwd 
                + ";Persist Security Info=True;Database=" + Global.sqlDB
                + ";Port=" + Global.sqlPort
                + ";CharSet=" + Global.sqlCharset + "");
            command = conn.CreateCommand();
            command.CommandText = sql;
            conn.Open();
            MySqlDataAdapter da = new MySqlDataAdapter(sql, conn);
            try
            {
                da.Fill(t);
            }
            catch (Exception e)
            {
                MessageBox.Show("连接数据库失败，请稍后重试！"+e.Message, "失败！", MessageBoxButtons.OK);
                //Application.Exit();
                //throw e;
            }
            finally
            {
                conn.Close();
            }
            return t;
        }

        #endregion

        /// <summary>
        /// 建立数据库连接路径
        /// </summary>
        /// <returns></returns>
        private static string connStr() {
            StringBuilder conn = new StringBuilder();
            //conn = new MySqlConnection("Server=svn.breadth.cn;User Id=root;Password=breadth2009;Persist Security Info=True;Database=mez;Port=3307;CharSet=utf8");
            //双线
            //conn = new MySqlConnection("Server=mysql.sql73.cdncenter.net;User Id=sq_mezboy;Password=mez199023;Persist Security Info=True;Database=sq_mezboy;Port=3306;CharSet=utf8");
            //电信
            //conn = new MySqlConnection("Server=mysql.sql47.cdncenter.net;User Id=sq_maenze;Password=mez199023;Persist Security Info=True;Database=sq_maenze;Port=3306;CharSet=utf8");
            //万网免费两年
            //conn = new MySqlConnection("Server=qdm137219194.my3w.com;User Id=qdm137219194;Password=mez199023;Persist Security Info=True;Database=qdm137219194_db;Port=3306;CharSet=utf8");
            
            conn.Append("Server=" + Global.sqlAddress);
            conn.Append(";User Id=" + Global.sqlUsername);
            conn.Append(";Password=" + Global.sqlPwd);
            conn.Append(";Persist Security Info=True;Database=" + Global.sqlDB);
            conn.Append(";Port=" + Global.sqlPort);
            conn.Append(";CharSet=" + Global.sqlCharset);
            return conn.ToString();
        }

        /// <summary>
        /// 增删改方法调用
        /// </summary>
        /// <param name="SQLString"></param>
        /// <returns>成功返回1，失败返回0</returns>
        public static int MySqlExcute(string SQLString)
        {
            using (MySqlConnection connection = new MySqlConnection(connStr()))
            {
                using (MySqlCommand cmd = new MySqlCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        //object obj = cmd.ExecuteScalar();
                        return cmd.ExecuteNonQuery();
                    }
                    catch (MySql.Data.MySqlClient.MySqlException e)
                    {
                        connection.Close();
                        string path = Application.StartupPath + "\\ezLogs.log";
                        Tools.writeFile(path, e.Message); 
                        return 0;
                    }
                }
            }
        }

        /// <summary>
        /// 查询的方法调用
        /// </summary>
        /// <param name="SQLString"></param>
        /// <returns></returns>
        public static DataTable MySqlQuery(string SQLString)
        {
            MySqlConnection conn = new MySqlConnection(connStr());
            MySqlCommand comm = new MySqlCommand();
            comm.Connection = conn;
            conn.Open();
            DataTable dt = new DataTable();
            MySqlDataAdapter da = new MySqlDataAdapter();
            try
            {
                comm.CommandType = CommandType.Text;
                comm.CommandText = SQLString;
                da.SelectCommand = comm;
                da.Fill(dt);
            }
            catch (Exception e)
            {
                string path = Application.StartupPath + "\\ezLogs.log";
                Tools.writeFile(path,e.Message);
            }
            finally
            {
                conn.Close();
            }
            return dt;
        }
    }
}
