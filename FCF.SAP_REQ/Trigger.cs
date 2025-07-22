using Dapper;
using Ede.Uof.EIP.Organization.Util;
using Ede.Uof.Utility.Data;
using Ede.Uof.WKF.ExternalUtility;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Xml;

namespace FCF.SAP_REQ
{
    public class Trigger : ICallbackTriggerPlugin
    {
        public void Finally()
        {
            
        }
        private string _connectionString;
        public string GetFormResult(ApplyTask applyTask)
        {
            _connectionString = new DatabaseHelper().Command.Connection.ConnectionString; //EIP內建抓取得連線字串
                                                                                          //取得表單編號
            string DOC_NBR = applyTask.FormNumber;
            //將目前表單內容讀到 doc物件當中
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(applyTask.CurrentDocXML);
            //doc.Save($"D:\\UOF_DEBUG\\{DOC_NBR}{DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss")}.xml");
            /*表單樣板
             <Form formVersionId="961ceed4-0978-4cb4-8d5b-39b5c6c21b60">
              <FormFieldValue>
                <FieldItem fieldId="DOC_NBR" fieldValue="B18250700001" realValue="" enableSearch="True" />
                <FieldItem fieldId="APPLICANT" fieldValue="歐陽毅 Wish(Wish)" realValue="&lt;UserSet&gt;&lt;Element type='user'&gt; &lt;userId&gt;294eb39a-9330-49d1-95af-44b03c953eaa&lt;/userId&gt;&lt;/Element&gt;&lt;/UserSet&gt;&#xD;&#xA;" enableSearch="True" />
                <FieldItem fieldId="APPLICANTDEPT" fieldValue="IT" realValue="96cb14cd-a6fc-d731-cb7b-49e201fbadec,IT,False" enableSearch="True" />
                <FieldItem fieldId="SUBJECT" fieldValue="Test1" realValue="" enableSearch="True" fillerName="歐陽毅 Wish" fillerUserGuid="294eb39a-9330-49d1-95af-44b03c953eaa" fillerAccount="Wish" fillSiteId="" />
                <FieldItem fieldId="CONTENT" fieldValue="test2&#xD;&#xA;" realValue="" enableSearch="True" fillerName="歐陽毅 Wish" fillerUserGuid="294eb39a-9330-49d1-95af-44b03c953eaa" fillerAccount="Wish" fillSiteId="" />
                <FieldItem fieldId="CATALOG1" fieldValue="新增BP主檔及銀行帳戶-非船隻@新增BP主檔及銀行帳戶-非船隻" realValue="" enableSearch="True" fillerName="歐陽毅 Wish" fillerUserGuid="294eb39a-9330-49d1-95af-44b03c953eaa" fillerAccount="Wish" fillSiteId="" />
                <FieldItem fieldId="CATALOG2" fieldValue="請CCRM-DOC2代申請@請CCRM-DOC2代申請" realValue="" enableSearch="True" fillerName="歐陽毅 Wish" fillerUserGuid="294eb39a-9330-49d1-95af-44b03c953eaa" fillerAccount="Wish" fillSiteId="" />
                <FieldItem fieldId="FILE01" fieldValue="" realValue="" enableSearch="True" />
                <FieldItem fieldId="CCRM" fieldValue="" realValue="" enableSearch="True" />
                <FieldItem fieldId="CCRM_FILE" fieldValue="" realValue="" enableSearch="True" />
                <FieldItem fieldId="SAP_RESPONCE" fieldValue="test5" realValue="" enableSearch="True" fillerName="歐陽毅 Wish" fillerUserGuid="294eb39a-9330-49d1-95af-44b03c953eaa" fillerAccount="Wish" fillSiteId="" />
              </FormFieldValue>
            </Form>
             */
            // 對應欄位
            var model = new Z_FCF_SAP_REQ
            {
                DOC_NBR = GetFieldValue(doc, "DOC_NBR"),
                //申請者部門
                APPLICANTDEPT_NAME = GetFieldValue(doc, "APPLICANTDEPT"),
                //申請者名稱
                APPLICANT_NAME = GetFieldValue(doc, "APPLICANT"),
                
                SUBJECT = GetFieldValue(doc, "SUBJECT"),
                CONTENT = GetFieldValue(doc, "CONTENT"),
                CATALOG1 = GetFieldValue(doc, "CATALOG1"),
                CATALOG2 = GetFieldValue(doc, "CATALOG2"),
                CCRM = GetFieldValue(doc, "CCRM"),
                SAP_RESPONCE = GetFieldValue(doc, "SAP_RESPONCE")
            };

            #region 取得表單申請者的GUID
            /*這邊比較複雜 取得申請者的GUID會拿到一個USER SET，然後存入UserSet後再拿出第一筆的使用者*/
            var applicant = GerRealValue(doc, "APPLICANT");
            UserSet formUser = new UserSet();
            formUser.SetXML(applicant);
            var data = formUser.GetAllUsers();
            if (data.Rows.Count > 0)
            {
                model.APPLICANT = data.Rows[0]["USER_GUID"].ToString();
            }
            #endregion
            #region 取得部門
            var applicantDept = GerRealValue(doc, "APPLICANTDEPT");
            if (!string.IsNullOrEmpty(applicantDept))
            {
                //部門格式為 "部門GUID,部門名稱,是否為群組"
                var deptData = applicantDept.Split(',');
                if (deptData.Length >= 2)
                {
                    model.APPLICANTDEPT = deptData[0]; // 部門GUID
                    //model.APPLICANTDEPT_NAME = deptData[1]; // 部門名稱
                }
            }
            #endregion
            //檢查是否存在資料
            if (IsExist(model.DOC_NBR))
            {
                //如果已經存在資料，就更新
                Update(model);
            }
            else
            {
                //如果不存在，就新增
                Insert(model);
            }
            throw new Exception("FOR TEST");
        }

        /// <summary>
        /// 檢查是否存在資料
        /// </summary>
        /// <param name="docNbr"></param>
        /// <returns></returns>
        public bool IsExist(string docNbr)
        {
            const string sql = "SELECT TOP 1 1 FROM Z_FCF_SAP_REQ WHERE DOC_NBR = @DocNbr";
            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                return conn.ExecuteScalar<bool>(sql, new { DocNbr = docNbr });
            }
                    
        }

        /// <summary>
        /// 新增資料
        /// </summary>
        /// <param name="model"></param>
        public void Insert(Z_FCF_SAP_REQ model)
        {
            const string sql = @"
                INSERT INTO Z_FCF_SAP_REQ
                    (DOC_NBR, APPLICANT,APPLICANT_NAME,APPLICANTDEPT,APPLICANTDEPT_NAME, SUBJECT, CONTENT, CATALOG1, CATALOG2, CCRM, SAP_RESPONCE)
                VALUES
                    (@DOC_NBR, @APPLICANT,APPLICANT_NAME,@APPLICANTDEPT,@APPLICANTDEPT_NAME, @SUBJECT, @CONTENT, @CATALOG1, @CATALOG2, @CCRM, @SAP_RESPONCE)";
            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                conn.Execute(sql, model);
            }
                    
        }

        /// <summary>
        /// 更新資料
        /// </summary>
        /// <param name="model"></param>
        public void Update(Z_FCF_SAP_REQ model)
        {
            const string sql = @"
                UPDATE Z_FCF_SAP_REQ
                SET
                    APPLICANT          = @APPLICANT,
                    APPLICANT_NAME     = @APPLICANT_NAME,
                    APPLICANTDEPT      = @APPLICANTDEPT,
                    APPLICANTDEPT_NAME = @APPLICANTDEPT_NAME,
                    SUBJECT            = @SUBJECT,
                    CONTENT            = @CONTENT,
                    CATALOG1           = @CATALOG1,
                    CATALOG2           = @CATALOG2,
                    CCRM               = @CCRM,
                    SAP_RESPONCE       = @SAP_RESPONCE                                      
                WHERE
                    DOC_NBR      = @DOC_NBR";
            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                conn.Execute(sql, model);
            }                
        }
        
        public void OnError(Exception errorException)
        {

        }

        /// <summary>
        /// 取得BPM表單中FieldValue的內容
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="fieldId"></param>
        /// <returns></returns>
        private string GetFieldValue(XmlDocument doc, string fieldId)
        {
            return doc.SelectSingleNode($"//FieldItem[@fieldId='{fieldId}']")?.Attributes["fieldValue"]?.Value;
        }

        /// <summary>
        /// 取得BPM表單中 RealValue的內容
        /// </summary>
        /// <param name="doc">表單XML</param>
        /// <param name="fieldId">欄位名稱</param>
        /// <returns></returns>
        private string GerRealValue(XmlDocument doc, string fieldId)
        {
            return doc.SelectSingleNode($"//FieldItem[@fieldId='{fieldId}']")?.Attributes["realValue"]?.Value;
        }
    }
}
