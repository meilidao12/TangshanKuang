using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Services;
using CommunicationServers.Sockets;
using ProtocolFamily;
using Services.DataBase;
namespace ZTGPRSService
{
    public partial class Service1 : ServiceBase
    {
        SqlHelper sql = new SqlHelper();
        SocketServer socketServer = new SocketServer();
        IniHelper ini = new IniHelper(System.AppDomain.CurrentDomain.BaseDirectory + @"\Set.ini");

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            string port = ini.ReadIni("Config", "Port");
            socketServer.NewMessage1Event += SocketServer_NewMessage1Event;
            socketServer.NewConnnectionEvent += SocketServer_NewConnnectionEvent;
            SimpleLogHelper.Instance.WriteLog(LogType.Info, socketServer.Listen(port));
        }

        private void SocketServer_NewConnnectionEvent(System.Net.Sockets.Socket socket)
        {
            SimpleLogHelper.Instance.WriteLog(LogType.Info, "有新的连接");
        }

        private void SocketServer_NewMessage1Event(System.Net.Sockets.Socket socket, string Message)
        {
            SimpleLogHelper.Instance.WriteLog(LogType.Info, Message);
            SimpleLogHelper.Instance.WriteLog(LogType.Info, Message.Length);
            if(Message.Length == 40)
            {
                string lijiliuliang = Message.Substring(20, 8);
                SimpleLogHelper.Instance.WriteLog(LogType.Info, lijiliuliang);
                lijiliuliang = (double.Parse(lijiliuliang) / 10).ToString();
                try
                {
                    SimpleLogHelper.Instance.WriteLog(LogType.Info, "与数据库连接：" + sql.Open());
                    if (sql.TestConn)
                    {
                        string b = string.Format("INSERT into Water (记录时间,更新时间,记录类型,记录描述,水) VALUES ('{0}','{0}','实时采集','实时描述','{1}')", DateTime.Now, lijiliuliang);
                        SimpleLogHelper.Instance.WriteLog(LogType.Info,"向数据库插入数据" + sql.Execute(b));
                        sql.Close();
                    }
                }
                catch (Exception ex)
                {
                    SimpleLogHelper.Instance.WriteLog(LogType.Info, "从数据库读取数据失败");
                    SimpleLogHelper.Instance.WriteLog(LogType.Error, ex);
                    sql.Close();
                }
            }
        }

        protected override void OnStop()
        {
            socketServer.Disconnect();
        }
    }
}
