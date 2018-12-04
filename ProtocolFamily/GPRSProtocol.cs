using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtocolFamily
{
    public class GPRSProtocol
    {
        /// <summary>
        /// 组成发送数据
        /// </summary>
        /// <param name="ElementData">组成部分</param>
        /// <returns></returns>
        public virtual string ComposeSendData(string ElementData)
        {
            return ElementData;
        }

        /// <summary>
        /// 解析收到的数据
        /// </summary>
        /// <param name="RecevieData">收到的数据</param>
        /// <returns></returns>
        public virtual AnalysisDataModel AnalysisReceiveData(string RecevieData)
        {
            return new AnalysisDataModel();
        }
        public string GetDateTime()
        {
            string date = string.Format("{0:yyMMddHHmmss}", DateTime.Now);
            byte[] buffer = new byte[6];
            for (int i = 0; i <= date.Length / 2 - 1; i++)
            {
                buffer[i] = byte.Parse(date.Substring(i * 2, 2));
            }
            MathHelper mathHelper = new MathHelper();
            date = mathHelper.ByteConvertToHex(buffer);
            return date;
        }
    }
}
