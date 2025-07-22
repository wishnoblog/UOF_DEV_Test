using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCF.SAP_REQ
{
    public class Z_FCF_SAP_REQ
    {
        /// <summary>
        /// 表單編號
        /// </summary>
        public string DOC_NBR { get; set; }

        /// <summary>
        /// 申請者GUID
        /// </summary>
        public string APPLICANT { get; set; }

        /// <summary>
        /// 申請名稱
        /// </summary>
        public string APPLICANT_NAME { get; internal set; }


        /// <summary>
        /// 申請者部門GUID
        /// </summary>
        public string APPLICANTDEPT { get; internal set; }


        /// <summary>
        /// 申請者部門名稱
        /// </summary>
        public string APPLICANTDEPT_NAME { get; internal set; }

        /// <summary>
        /// 主旨
        /// </summary>
        public string SUBJECT { get; set; }

        /// <summary>
        /// 說明
        /// </summary>
        public string CONTENT { get; set; }

        /// <summary>
        /// 分類1
        /// </summary>
        public string CATALOG1 { get; set; }

        /// <summary>
        /// 分類2
        /// </summary>
        public string CATALOG2 { get; set; }

        /// <summary>
        /// CCRM 備註
        /// </summary>
        public string CCRM { get; set; }

        /// <summary>
        /// SAP 處理情形
        /// </summary>
        public string SAP_RESPONCE { get; set; }


    }
}
