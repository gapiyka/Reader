using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Book : MonoBehaviour
{
    [SerializeField] Text[] page;
    [SerializeField] Text[] pageN;
    Animator animator;
    
    List<string> pages;
    int pagesCount;
    int pageCur;

    void Start()
    {
        animator = transform.GetComponent<Animator>();
    }

    IEnumerator AnimateBook(int side)
    {
        if (side < 0) DrawBookPages();
        animator.SetInteger("Turn", side);
        yield return new WaitForSeconds(1f);
        if (side > 0) DrawBookPages();
        animator.SetInteger("Turn", 0);
    }

    void DrawBookPages() 
    {
        for (int p = 0; p < pageN.Length; p++)
        {
            if (pageCur + p < pagesCount)
                page[p].text = pages[pageCur + p];
            else
                page[p].text = "";
            pageN[p].text = (pageCur + (p + 1)).ToString();
        }
    }

    void TurnPage(int side)
    {
        pageCur += 2 * side;
        StartCoroutine(AnimateBook(side));
    }

    public void PrevPage()
    {
        if (pageCur > 0)
            TurnPage(-1);
    }
    
    public void NextPage() 
    {
        if (pageCur + 2 < pagesCount)
            TurnPage(1);
    }

    public void OpenBook(string fileName)
    {
        pageCur = 0;
        string allText = Parser.FileToTxt(fileName);
        pages = Parser.TextToBook(allText);
        pagesCount = pages.Count;
        DrawBookPages();
    }
}
