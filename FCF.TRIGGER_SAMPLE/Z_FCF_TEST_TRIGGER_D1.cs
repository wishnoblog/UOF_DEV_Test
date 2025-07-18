using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCF.TRIGGER_SAMPLE
{
    /// <summary>
    /// 測試TRIGGER 明細
    /// </summary>
    public class Z_FCF_TEST_TRIGGER_D1
    {
        /// <summary>
        /// 主表
        /// </summary>
        public string DOC_NBR { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int SEQ { get; set; }

        /// <summary>
        /// 品項
        /// </summary>
        public string ITEM { get; set; }

        /// <summary>
        /// 單價
        /// </summary>
        public decimal? PRICE { get; set; }

        /// <summary>
        /// 數量
        /// </summary>
        public int? COUNT { get; set; }

        /// <summary>
        /// 總價
        /// </summary>
        public decimal TOTAL { get; set; }
    }
}
