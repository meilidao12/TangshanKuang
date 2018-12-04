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
using ProtocolFamily.TangShanKuang;
using Services.DataBase;
namespace ZTGPRSService
{
    public partial class Service1 : ServiceBase
    {
        SqlHelper sql = new SqlHelper();
        SocketServerEx socketServer = new SocketServerEx();
        IniHelper ini = new IniHelper(System.AppDomain.CurrentDomain.BaseDirectory + @"\Set.ini");

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            string port = ini.ReadIni("Config", "Port");
            socketServer.NewMessage2Event += SocketServer_NewMessage2Event;
            SimpleLogHelper.Instance.WriteLog(LogType.Info, socketServer.Listen(port));
        }

        private void SocketServer_NewMessage2Event(System.Net.IPEndPoint remoteIpEndPoint, string Message)
        {
            SimpleLogHelper.Instance.WriteLog(LogType.Info, Message);
            SimpleLogHelper.Instance.WriteLog(LogType.Info, Message.Length);
            //验证
            string commandText = "SELECT TOP 1 [正累积流量] AS 'Water' FROM Water ORDER BY [更新时间] DESC";
            List<TSKModel> models = sql.GetDataTable<TSKModel>(commandText);
            if(models != null)
            {
                string a = MathHelper.SingleToHex(models[0].Water);
                Debug.WriteLine(a);
                string backdata = Message.Substring(0, 4);
                backdata += MathHelper.DecToHex((int.Parse(Message.Substring(10, 2)) * 2)
                                                                                .ToString()).PadLeft(2, '0');
                backdata += a;
                backdata += CRC.ToModbusCRC16(backdata);
                SimpleLogHelper.Instance.WriteLog(LogType.Info, "返回的数据是：" + backdata);
                socketServer.Send(remoteIpEndPoint,MathHelper.StrToHexByte(backdata));
            }
        }

        protected override void OnStop()
        {
            socketServer.Disconnect();
        }
    }
}
