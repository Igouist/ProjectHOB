using ProjectHOB.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Threading.Tasks;

namespace ProjectHOB.Generator
{
    public class EPubGenerator
    {
        private readonly string _path = "";

        public EPubGenerator()
        {
            this._path = Directory.GetCurrentDirectory() + "\\file\\";
        }

        /// <summary>
        /// 製作 Epub
        /// </summary>
        /// <param name="contents">文件章節列表</param>
        /// <returns></returns>
        public async Task GenerateEpub(IEnumerable<ChapterContent> contents)
        {
            // 1. 重置工作目錄
            this.InitDir(this._path);
            Console.WriteLine("輸出目錄已初始化。");

            // 2. 將各章節寫入 html
            await this.CreateHtmlFile(this._path, contents);
            Console.WriteLine("寫入已完成。準備開始轉換…");

            // 3. 執行 pandoc 將 html 轉換為 epub
            this.ConventEpubWithPowershell(this._path, contents);
            Console.WriteLine("轉換已完成。");
        }

        /// <summary>
        /// 建立 Html 檔案
        /// </summary>
        /// <param name="path">輸出資料夾路徑</param>
        /// <param name="contents">文件章節列表</param>
        private async Task CreateHtmlFile(string path, IEnumerable<ChapterContent> contents)
        {
            var writeTasks = contents.Select(async content =>
            {
                using (FileStream stream = new FileStream(path + content.ChapterName + ".html", FileMode.Create))
                {
                    var writer = new StreamWriter(stream);

                    // 如果沒有寫入標題的話會全部壓縮成一章造成崩潰
                    await writer.WriteLineAsync($"<br/><h1>{content.ChapterName}</h1><br/>");
                    await writer.WriteLineAsync(content.Content);
                    writer.Flush();
                }
            });

            await Task.WhenAll(writeTasks);
        }

        /// <summary>
        /// Convents the epub with powershell.
        /// </summary>
        /// <param name="path">輸出資料夾路徑</param>
        /// <param name="contents">文件章節列表</param>
        private void ConventEpubWithPowershell(string path, IEnumerable<ChapterContent> contents)
        {
            var cmdComment = string.Empty;

            cmdComment += $"cd {_path}; ";
            cmdComment += " pandoc -s -f html -t epub3 -o HOB.epub ";
            cmdComment += contents.Aggregate(string.Empty, (comment, content) => comment += $" {content.ChapterName}.html ");

            using (var shell = PowerShell.Create())
            {
                shell
                    .AddScript(cmdComment)
                    .Invoke()
                    .ToList()
                    .ForEach(result => Console.Write(result.ToString()));
            }
        }

        /// <summary>
        /// 重置工作目錄，如果尚未存在就建立，已經存在就清空
        /// </summary>
        /// <param name="path">目標資料夾</param>
        private void InitDir(string path)
        {
            if (Directory.Exists(path))
            {
                this.DeleteFolder(path);
            }
            else
            {
                Directory.CreateDirectory(path);
            }
        }

        /// <summary>
        /// 清空資料夾
        /// </summary>
        /// <param name="dir">指定目錄</param>
        private void DeleteFolder(string dir)
        {
            foreach (string file in Directory.GetFileSystemEntries(dir))
            {
                if (File.Exists(file))
                {
                    var fileInfo = new FileInfo(file);
                    if (fileInfo.Attributes.ToString().IndexOf("ReadOnly") != -1)
                    {
                        fileInfo.Attributes = FileAttributes.Normal;
                    }
                    File.Delete(file);
                }
                else
                {
                    DeleteFolder(file);
                }
            }
        }
    }
}
