using SautinSoft.Document;
using System.Collections.Generic;
using UnityEngine;
public class Parser
{
    const int extraOffset = 80;
    const int lineSize = 80;
    const int pageSize = 16;
    const string saveFile = @"\CurrentTextBook.txt";

    public static List<string> TextToBook(string text)
    {
        string newPage = "";
        int lineCounter = 0;
        int charCounter = 0;
        int pagesCount = 1;
        List<string> pages = new();

        text = text.Replace("\r", "");
        for (int c = 0; c < text.Length; c++)
        {
            charCounter++;
            if (charCounter == lineSize)
            {
                if (!(text[c - 1] == ' ' || text[c - 1] == '\n' || text[c - 1] == '\t'))
                    newPage += "-";
                newPage += "\n";
                charCounter = 0;
                lineCounter++;
            }
            if (text[c] == '\n')
            {
                if (newPage.Length > 1)
                    if (text[c - 1] == '\n' && text[c - 2] == '\n')
                    {
                        newPage.Remove(newPage.Length - 1);
                        continue;
                    }
                charCounter = 0;
                lineCounter++;
            }
            newPage += text[c];
            if (lineCounter == pageSize * pagesCount)
            {
                pages.Add(newPage);
                pagesCount++;
                newPage = "";
            }
        }
        if (pages.Count == 0)
            pages.Add(newPage);

        return pages;
    }

    public static string FileToTxt(string fileName)
    {
        string outFile = Application.dataPath + saveFile;
        DocumentCore dc = DocumentCore.Load(fileName);
        string allText = dc.Content.ToString();
        int trimIndex = allText.Length - extraOffset;
        allText.Remove(trimIndex);
        dc.Save(outFile);
        return allText;
    }
}

