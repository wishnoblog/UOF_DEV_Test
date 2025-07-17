using Ede.Uof.WKF.ExternalUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FCF.TRIGGER_SAMPLE
{
    /// <summary>
    /// 一個TRIGGER 範例
    /// </summary>
    /// <remarks>
    /// 存取這邊要從internal 改 public
    /// </remarks>
    public class Trigger : ICallbackTriggerPlugin
    {
        public void Finally()
        {
            
        }

        /// <summary>
        /// 表單送出後觸發
        /// </summary>
        /// <param name="applyTask">觸發物件</param>
        /// <returns></returns>
        /// <exception cref="Exception">失敗</exception>
        public string GetFormResult(ApplyTask applyTask)
        {
            //取得表單編號
            string DOC_NBR = applyTask.FormNumber;
            Ede.Uof.Utility.Log.Logger.Write("Debug", $"測試表單取到的DOC_NBR是{DOC_NBR}");
            //111
            //將目前表單內容讀到 doc物件當中
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(applyTask.CurrentDocXML);

            //將表單內容存一分到硬碟中(上線會註解掉) 用於後續拆解XML
            doc.Save($"D:\\UOF_DEBUG\\{DOC_NBR}{DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss")}.xml");            
                        
            //LOG這樣寫
            Ede.Uof.Utility.Log.Logger.Write("Debug", $"寫LOG");

            //觸發失敗，讓Trigger可以多次重送,
            //可以到 電子表單 > DLL呼叫情形 進行重送
            throw new Exception("For Test");
            return "";
        }

        public void OnError(Exception errorException)
        {
            
        }
    }
}
