using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Book : MonoBehaviour
{
    [SerializeField] Text text;
    [SerializeField] Text page;
    
    List<string> pages;
    int pagesCount;
    int pageCur;

    void DrawBookPage() 
    {
        text.text = pages[pageCur];
        page.text = (pageCur + 1).ToString();
    }

    public void PrevPage()
    {
        if (pageCur > 0)
        {
            pageCur--;
            DrawBookPage();
        }
    }
    
    public void NextPage() 
    {
        if (pageCur < pagesCount)
        { 
            pageCur++;
            DrawBookPage();
        }
    }

    public void OpenBook(string fileName)
    {
        pageCur = 0;
        string allText = Parser.FileToTxt(fileName);
        pages = Parser.TextToBook(allText);
        pagesCount = pages.Count;
        DrawBookPage();
    }
}
