using SautinSoft.Document;
using System.Collections.Generic;
using UnityEngine;
public class Parser
{
    const int extraOffset = 80;
    const int lineSize = 80;
    const int pageSize = 45;
    const string saveFile = @"\CurrentTextBook.txt";

    static string NewLineForm(string text, int c, ref int charCounter, ref int lineCounter)
    {
        string res = "";
        char prevC = text[c - 1];
        if (!(prevC == ' ' || prevC == '\n' || prevC == '\t'))
            res += "-";
        res += "\n";
        charCounter = 0;
        lineCounter++;
        return res;
    }

    static bool TrimExtraLines(ref string newPage, string text, int c, ref int charCounter, ref int lineCounter)
    {
        if (newPage.Length > 1)
            if (text[c - 1] == '\n' && text[c - 2] == '\n')
            {
                newPage.Remove(newPage.Length - 1);
                return false;
            }
        charCounter = 0;
        lineCounter++;
        return true;
    }

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
                newPage += NewLineForm(text, c, ref charCounter, ref lineCounter);
            if (text[c] == '\n')
                if(!TrimExtraLines(ref newPage, text, c, ref charCounter, ref lineCounter))
                    continue; 
            newPage += text[c];
            if (lineCounter == pageSize * pagesCount)
            {
                pages.Add(newPage);
                pagesCount++;
                newPage = "";
            }
        }
        if (newPage != "") pages.Add(newPage);

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

