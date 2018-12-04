using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services;
namespace ProtocolFamily.YanGang
{
    /// <summary>
    /// 汇中 GPRS 关于SCL-61D通讯协议
    /// 此通讯协议 应为GPRS模块与服务器之间的通讯协议 非透明传输
    /// </summary>
    class HuiZhongSCL_61D : GPRSProtocol
    {
        public override string ComposeSendData(string ElementData)
        {
            string data = string.Format("403A000E{0}F05{1}000D0A", ElementData,GetDateTime());
            return data;
        }

        public override AnalysisDataModel AnalysisReceiveData(string RecevieData)
        {
            /*
                403A006518330328814F
                06010102180822094540
                091B2600000000000000
                00000000000000000000
                000000000000000F0F41
                00342606518082209450
                500000000
                1111111111111
                2222222222222
                2222222222222
                00000000000087954700000000280000000000593A0D0A
           */
            AnalysisDataModel analysisDataModel = new AnalysisDataModel();
            try
            {
                if (RecevieData.Length != 214) throw new Exception("返回字符串长度不正确 " + RecevieData);
                //---电话号码
                string phoneNum = RecevieData.Substring(8, 11);
                //---瞬时流量
                string HourlyFlowRates =(double.Parse(RecevieData.Substring(129, 13))/10000000).ToString();
                HourlyFlowRates = MathHelper.DoubleToHex(HourlyFlowRates);
                //---累积流量
                string a = (double.Parse(RecevieData.Substring(142, 13))/100).ToString();
                string TotalFlow = MathHelper.DoubleToHex(a.ToString());
                analysisDataModel.Data0 = phoneNum + HourlyFlowRates + TotalFlow + GetDateTime();
                //CRC校验
                analysisDataModel.Data0 += CRC.ToModbusCRC16(analysisDataModel.Data0);
                analysisDataModel.Result = AnalysisDataModel.AnalysisResult.OK;
            }
            catch (Exception ex)
            {
                SimpleLogHelper.Instance.WriteLog(LogType.Error, ex, "HuiZhongSCL_61D错误");
                analysisDataModel.Result = AnalysisDataModel.AnalysisResult.ERR;
            }
            return analysisDataModel;
        }

       
    }
}
