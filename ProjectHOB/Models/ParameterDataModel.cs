namespace ProjectHOB.Models
{
    /// <summary>
    /// 參數
    /// </summary>
    public class ParameterDataModel
    {
        /// <summary>
        /// 小說目錄頁
        /// </summary>
        public string MainUrl { get; set; }

        /// <summary>
        /// 小說目錄對各章節連結的篩選器
        /// </summary>
        public string ListQuerySelector { get; set; }

        /// <summary>
        /// 小說擷取內文的篩選器
        /// </summary>
        public string ContextQuerySelector { get; set; }
    }
}
