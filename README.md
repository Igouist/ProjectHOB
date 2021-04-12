目標：用爬蟲抓取該網站指定小說的所有章節，再壓成 Epub，最後在閱讀器上爽看

1. 使用 AngleSharp 爬取網站上的章節目錄和小說內文
2. 將每個章節各自寫入 Html 檔（如: 第一章、第二章…）
3. 使用 Pandoc 將 Html 包裝成 ePub 格式
4. （額外處理）因為我個人使用 Kobo，故需要再用 kepubify 轉換成 kepub 格式
5. （額外處理）使用 Cailbre 將電子書放入電子書閱讀器

參考資料，感謝各位大大：
- [用 .NET Core 做網頁爬蟲抓取資料 - 使用 HttpClicent 與 AngleSharp - 長庚的作業簿](https://dannyliu.me/%E7%94%A8-net-core%E5%81%9A%E7%B6%B2%E9%A0%81%E7%88%AC%E8%9F%B2%E6%8A%93%E5%8F%96%E8%B3%87%E6%96%99-%E4%BD%BF%E7%94%A8httpclicent%E8%88%87anglesharp/)
- [將 HTML 圖文網頁轉成 ePub 電子書 - 使用 Pandoc - 黑暗執行緒](https://blog.darkthread.net/blog/html2epub-with-pandoc/)
- [介紹好用工具：Pandoc ( 萬用的文件轉換器 ) - The Will Will Web](https://blog.miniasp.com/post/2018/10/06/Useful-tool-Pandoc-universal-document-converter)
- [EPUB VS KEPUB 有何不同？如何轉換 - FRANK SHI](https://www.frank.hk/blog/epub-vs-kepub/)
- [Kobo Libra H2O 的 epub 處理 - Heresy's Space](https://kheresy.wordpress.com/2020/12/07/kobo-epub/)
- [dotnet core 使用 PowerShell 脚本 - lindexi](https://blog.lindexi.com/post/dotnet-core-%E4%BD%BF%E7%94%A8-PowerShell-%E8%84%9A%E6%9C%AC.html)
