using AngleSharp;
using ProjectHOB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectHOB.Crawler
{
    /// <summary>
    /// 爬蟲服務
    /// </summary>
    public class CrawlerService
    {
        private readonly IBrowsingContext _browser;

        /// <summary>
        /// 初始化
        /// </summary>
        public CrawlerService()
        {
            var config = Configuration.Default.WithDefaultLoader();
            var browser = BrowsingContext.New(config);
            this._browser = browser;
        }

        /// <summary>
        /// 取得文本內容
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<ChapterContent>> FetchData(
            string mainUrl,
            string listQuerySelector,
            string contextQuerySelector)
        {
            var chapters = await this.FetchChapterList(mainUrl, listQuerySelector);
            if (chapters is null) throw new Exception("目錄頁錯誤");

            var contextTasks = chapters.Select(chapter =>
                this.FetchContent(
                    chapter.ChapterName,
                    chapter.Link,
                    contextQuerySelector));

            var contents = await Task.WhenAll(contextTasks);

            return contents.Where(content => content != null);
        }

        /// <summary>
        /// 取得小說目錄頁
        /// </summary>
        /// <param name="menuUrl">目錄頁 URL</param>
        /// <param name="listQuerySelector">各章節連結的 Selector</param>
        /// <returns></returns>
        public async Task<IEnumerable<ChapterLink>> FetchChapterList(
            string menuUrl,
            string listQuerySelector)
        {
            var document = await this._browser.OpenAsync(menuUrl);
            if (document is null) return null;

            var contents = document.QuerySelectorAll(listQuerySelector);
            document.Close();

            var chapters = contents.Select(content => new ChapterLink
            {
                ChapterName = content.InnerHtml,
                Link = "https:" + content.GetAttribute("href"),
            });

            return chapters;
        }

        /// <summary>
        /// 取得小說章節內文
        /// </summary>
        /// <param name="chapterName">章節名稱</param>
        /// <param name="url">章節 URL</param>
        /// <param name="contentQuerySelector">內文的 Selector</param>
        /// <returns></returns>
        public async Task<ChapterContent> FetchContent(
            string chapterName,
            string url,
            string contentQuerySelector)
        {
            var document = await this._browser.OpenAsync(url);
            if (document is null) return null;

            var content = document.QuerySelector(contentQuerySelector);
            document.Close();

            var result = new ChapterContent
            {
                ChapterName = chapterName,
                Content = content.InnerHtml
            };

            return result;
        }
    }
}
