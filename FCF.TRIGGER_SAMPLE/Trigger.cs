using Dapper;
using Ede.Uof.Utility.Data;
using Ede.Uof.WKF.ExternalUtility;
using Quartz.Collection;
using System;
using System.Collections.Generic;
using System.Data;
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

        private string _connectionString;

        /// <summary>
        /// 表單送出後觸發
        /// </summary>
        /// <param name="applyTask">觸發物件</param>
        /// <returns></returns>
        /// <exception cref="Exception">失敗</exception>
        public string GetFormResult(ApplyTask applyTask)
        {

            _connectionString = new DatabaseHelper().Command.Connection.ConnectionString; //EIP內建抓取得連線字串

            //取得表單編號
            string DOC_NBR = applyTask.FormNumber;

            //可以透過 task_id 或 doc_nbr 在資料表 [TB_WKF_TASK] 的[CURRENT_DOC]欄位取得表單內容
            var task = applyTask.Task;
            var task_Id = task.TaskId;
            //Ede.Uof.Utility.Log.Logger.Write("Debug", $"測試表單取到的DOC_NBR是{DOC_NBR} Task Id{task_Id}");

            //處理資料回寫
            //取得表單內容XML
            var doc = GetFormDocByTaskID(task_Id);
            //讀取文件的XML中SCORE欄位，之後轉成int?
            int? score = TryParseInt(GetFieldValue(doc, "SCORE"));
            //Ede.Uof.Utility.Log.Logger.Write("Debug", $"測試表單取到的SCORE是{score}");

            if (score.HasValue)
            {
                string rank = "D";
                if (score >= 91)
                {
                    rank = "A++";
                }
                else if (score > 81)
                {
                    rank = "A+";
                }
                else if (score > 71)
                {
                    rank = "A";
                }
                else if (score > 61)
                {
                    rank = "B";
                }
                //Ede.Uof.Utility.Log.Logger.Write("Debug", $"rank = {rank}");
                ReplaceDocumentContent(task_Id, "RANK", rank);
            }
            //重新讀取異動的表單
            doc = GetFormDocByTaskID(task_Id);
            //將目前表單內容讀到 doc物件當中            
            doc.LoadXml(applyTask.CurrentDocXML);
            //將表單內容存一分到硬碟中(上線會註解掉) 用於後續拆解XML
            doc.Save($"D:\\UOF_DEBUG\\{DOC_NBR}{DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss")}.xml");

            var model = new Z_FCF_TEST_TRIGGER();
            //將表單內容轉換成物件
            model.DOC_NBR = DOC_NBR;

            //將表單內容轉換成物件
            // 對應欄位
            model.DOC_NBR = GetFieldValue(doc, "DOC_NBR");
            model.TEXT1 = GetFieldValue(doc, "TEXT1");
            model.TEXT2 = GetFieldValue(doc, "TEXT2");
            model.NBR = TryParseInt(GetFieldValue(doc, "NBR"));
            model.DATETEST = TryParseDateTime(GetFieldValue(doc, "DATETEST"));
            model.RD01 = GetFieldValue(doc, "RD01");
            model.CB01 = GetFieldValue(doc, "CB01");
            model.DDL01 = GetFieldValue(doc, "DDL01");
            model.SUM = TryParseDecimal(GetFieldValue(doc, "SUM"));
            model.TIMETEST = GetFieldValue(doc, "TIMETEST");

   

            using (var connection = new System.Data.SqlClient.SqlConnection(_connectionString))
            {
                connection.Open();
                IDbTransaction trans = connection.BeginTransaction();

                try
                {
                    //異動主表
                    if (IsExist(model.DOC_NBR))
                    {
                        //如果已經存在資料，就更新
                        Update(model, connection, trans);
                    }
                    else
                    {
                        //如果不存在，就新增
                        Insert(model, connection, trans);
                    }
                    //異動明細
                    var details = GetDetail(doc);
                    Insert(details, DOC_NBR, connection, trans);
                    //執行異動
                    trans.Commit();
                }
                catch (Exception)
                {
                    //如果有錯誤，就還原異動
                    trans.Rollback();
                    throw;
                }             
            }                      

            //故意觸發失敗，讓Trigger可以多次重送,
            //可以到 電子表單 > DLL呼叫情形 進行重送
            throw new Exception("For Test");
            return "";
        }

        /// <summary>
        /// 將明細轉為LIST
        /// </summary>
        /// <param name="doc">XML文件</param>
        /// <returns></returns>
        public List<Z_FCF_TEST_TRIGGER_D1> GetDetail(XmlDocument doc)
        {
            var result = new List<Z_FCF_TEST_TRIGGER_D1>();

            // 取得 DOC_NBR
            string docNbr = doc.SelectSingleNode("//FieldItem[@fieldId='DOC_NBR']")?.Attributes["fieldValue"]?.Value ?? "";

            // 取得所有 Row 節點
            XmlNodeList rowNodes = doc.SelectNodes("//FieldItem[@fieldId='DL']/DataGrid/Row");

            foreach (XmlNode row in rowNodes)
            {
                var detail = new Z_FCF_TEST_TRIGGER_D1
                {
                    DOC_NBR = docNbr,
                    SEQ = int.TryParse(row.Attributes["order"]?.Value, out int seq) ? seq : 0,
                    ITEM = GetCellValue(row, "ITEM"),
                    PRICE = TryParseDecimal(GetCellValue(row, "PRICE")),
                    COUNT = TryParseInt(GetCellValue(row, "COUNT")),
                    TOTAL = TryParseDecimal(GetCellValue(row, "TOTAL"))
                };

                result.Add(detail);
            }

            return result;
        }

        private void Insert(List<Z_FCF_TEST_TRIGGER_D1> model,string DOC_NBR, IDbConnection connection,IDbTransaction trans)
        {
            string sqlDelete = @"
                    DELETE FROM Z_FCF_TEST_TRIGGER_D1 WHERE DOC_NBR = @DOC_NBR;
                ";
            connection.Execute(sqlDelete, new { DOC_NBR }, trans);

            //幫我補完這段SQL
            string sqlInsert = @"
                INSERT INTO Z_FCF_TEST_TRIGGER_D1 (DOC_NBR, SEQ, ITEM, PRICE, COUNT, TOTAL)
                VALUES (@DOC_NBR, @SEQ, @ITEM, @PRICE, @COUNT, @TOTAL);
            ";
            connection.Execute(sqlInsert, model, trans);
        }

        /// <summary>
        /// 取得XML明細的欄位內容
        /// </summary>
        /// <param name="row"></param>
        /// <param name="fieldId"></param>
        /// <returns></returns>
        private string GetCellValue(XmlNode row, string fieldId)
        {
            XmlNode cell = row.SelectSingleNode($"Cell[@fieldId='{fieldId}']");
            return cell?.Attributes["fieldValue"]?.Value ?? "";
        }

        /// <summary>
        /// 檢查[Z_FCF_TEST_TRIGGER] 是否存在資料
        /// </summary>
        /// <param name="doc_nbr"></param>
        /// <returns></returns>
        public bool IsExist(string doc_nbr)
        {
            string sql = "SELECT TOP 1 1  FROM Z_FCF_TEST_TRIGGER WHERE DOC_NBR = @doc_nbr"; 
            using (var connection = new System.Data.SqlClient.SqlConnection(_connectionString))
            {
                return connection.ExecuteScalar<bool>(sql, new { doc_nbr });                
            }
        }

        /// <summary>
        /// 新增資料到[Z_FCF_TEST_TRIGGER] 資料表
        /// </summary>
        /// <param name="model"></param>
        public void Insert(Z_FCF_TEST_TRIGGER model,IDbConnection connection, IDbTransaction trans = null)
        {
            string sql = @"INSERT INTO Z_FCF_TEST_TRIGGER 
                                (DOC_NBR, TEXT1, TEXT2, NBR, DATETEST, RD01, CB01, DDL01, SUM, TIMETEST) 
                           VALUES 
                                (@DOC_NBR, @TEXT1, @TEXT2, @NBR, @DATETEST, @RD01, @CB01, @DDL01, @SUM, @TIMETEST)";
            connection.Execute(sql, model, trans);            
        }

        /// <summary>
        /// 更新資料到[Z_FCF_TEST_TRIGGER] 資料表
        /// </summary>
        /// <param name="model"></param>
        public void Update(Z_FCF_TEST_TRIGGER model, IDbConnection connection, IDbTransaction trans = null)
        {
            string sql = @"UPDATE Z_FCF_TEST_TRIGGER 
                           SET 
                               TEXT1    = @TEXT1, 
                               TEXT2    = @TEXT2, 
                               NBR      = @NBR, 
                               DATETEST = @DATETEST, 
                               RD01     = @RD01, 
                               CB01     = @CB01, 
                               DDL01    = @DDL01, 
                               SUM      = @SUM, 
                               TIMETEST = @TIMETEST 
                           WHERE 
                              DOC_NBR = @DOC_NBR";
            connection.Execute(sql, model, trans);            
        }
        public void OnError(Exception errorException)
        {
            
        }

        #region 簡化XML 取得欄位值的方法
        
        /// <summary>
        /// 字串轉INT
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private int? TryParseInt(string value)
        {
            if (int.TryParse(value, out int result))
                return result;
            return null;
        }

        /// <summary>
        /// 字串轉DateTime
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private DateTime? TryParseDateTime(string value)
        {
            if (DateTime.TryParse(value, out DateTime result))
                return result;
            return null;
        }

        /// <summary>
        /// 字串轉Decimal
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private decimal TryParseDecimal(string value)
        {
            if (decimal.TryParse(value, out decimal result))
                return result;
            return 0m; // 預設 0
        }
        /// <summary>
        /// 取出XML字串的值
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="fieldId"></param>
        /// <returns></returns>
        private string GetFieldValue(XmlDocument doc, string fieldId)
        {
            return doc.SelectSingleNode($"//FieldItem[@fieldId='{fieldId}']")?.Attributes["fieldValue"]?.Value;
        }


        #endregion

        #region 表單更新寫入的函數
        /*
         * TODO: 
         * 1. 從資料庫中取得表單資料 XML 順序1
         * 2. 將目標資料寫回 XML 順序3
         * 3. 將 XML 寫回資料庫 順序2
         */

        /// <summary>
        /// 從資料庫取得表單內容XML 透過TASK_ID
        /// </summary>
        /// <param name="taskId">表單TASK ID</param>
        /// <returns>表單最新的XML</returns>
        public XmlDocument GetFormDocByTaskID(string taskId)
        {
            const string sql = @"
                SELECT TOP 1 [CURRENT_DOC] FROM [TB_WKF_TASK] WHERE [TASK_ID] = @taskId
             ";
            using (var connection = new System.Data.SqlClient.SqlConnection(_connectionString))
            {
                var docXml = connection.ExecuteScalar<string>(sql, new { taskId });
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(docXml);
                return doc;
            }
        }
        /// <summary>
        /// 從資料庫取得表單內容XML 透過表單編號DOC_NBR
        /// </summary>
        /// <param name="DOC_NBR">表單編號 DOC_NBR</param>
        /// <returns>表單最新的XML</returns>
        public XmlDocument GetFormDocByDocNBR(string DOC_NBR)
        {
            const string sql = @"
                SELECT TOP 1 [CURRENT_DOC] FROM [TB_WKF_TASK] WHERE [DOC_NBR] = @DOC_NBR
             ";
            using (var connection = new System.Data.SqlClient.SqlConnection(_connectionString))
            {
                var docXml = connection.ExecuteScalar<string>(sql, new { DOC_NBR });
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(docXml);
                return doc;
            }
        }

        /// <summary>
        /// 透過TASK_ID 更新表單內容XML
        /// </summary>
        /// <param name="taskId">taskID</param>
        /// <param name="doc">表單XML</param>
        public void SetFormDocByTaskId(string taskId, XmlDocument doc)
        {
            //Ede.Uof.Utility.Log.Logger.Write("Debug", $"doc = {doc.OuterXml}");
            //將XML轉為字串
            string docXml = doc.OuterXml;
            const string sql = @"
                UPDATE [TB_WKF_TASK] 
                SET [CURRENT_DOC] = @docXml 
                WHERE [TASK_ID] = @taskId
             ";
            using (var connection = new System.Data.SqlClient.SqlConnection(_connectionString))
            {
                connection.Execute(sql, new { docXml, taskId });
            }
        }

        /// <summary>
        /// 更新表單欄位
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="fieldId"></param>
        /// <param name="fieldValue"></param>
        public void ReplaceDocumentContent(string taskId,string fieldId,string fieldValue)
        {
            //取得表單內容
            var doc = GetFormDocByTaskID(taskId);
            //更新XML的內容
            XmlNode node = doc.SelectSingleNode($"//Form/FormFieldValue/FieldItem[@fieldId='{fieldId}']");
            //更新node 的fieldValue=
            if(node != null)
            {
                node.Attributes["fieldValue"].Value = fieldValue;
                //回存
                SetFormDocByTaskId(taskId, doc);
            }                        
        }

        #endregion 表單更新寫入的函數
    }
}
