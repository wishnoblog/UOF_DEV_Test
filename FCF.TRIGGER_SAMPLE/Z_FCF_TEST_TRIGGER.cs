using System;

namespace FCF.TRIGGER_SAMPLE
{
    /// <summary>
    /// 測試TRIGGER 主表
    /// </summary>
    public class Z_FCF_TEST_TRIGGER
    {
        /// <summary>
        /// 表單編號
        /// </summary>
        public string DOC_NBR { get; set; }

        /// <summary>
        /// 單行文字
        /// </summary>
        public string TEXT1 { get; set; }

        /// <summary>
        /// 多行文字
        /// </summary>
        public string TEXT2 { get; set; }

        /// <summary>
        /// 數字
        /// </summary>
        public int? NBR { get; set; }

        /// <summary>
        /// 日期測試
        /// </summary>
        public DateTime? DATETEST { get; set; }

        /// <summary>
        /// 單選
        /// </summary>
        public string RD01 { get; set; }

        /// <summary>
        /// checkbox測試
        /// </summary>
        public string CB01 { get; set; }

        /// <summary>
        /// 下拉選單
        /// </summary>
        public string DDL01 { get; set; }

        /// <summary>
        /// 總金額
        /// </summary>
        public decimal SUM { get; set; }

        /// <summary>
        /// 時間測試
        /// </summary>
        public string TIMETEST { get; set; }
    }
}
