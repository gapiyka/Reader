using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TestTools;

public class BookTests
{
    [UnityTest]
    public IEnumerator BookOpenTest()
    {
        //ARRANGE
        const int bookSize = 4;
        const int pagesCount = 1;
        const int pageCurrent = 0;
        const string firstPageContent = 
            "some text example\nsome text example\nsome text example\n" +
            "some text example\nsome text example\nsome text example\n" +
            "some text example\nsome text example\nsome text example\n" +
            "some text example\n";
        
        //ACT
        var gameObject = new GameObject();
        var book = gameObject.AddComponent<Book>();
        book.Page = new Text[bookSize] { new GameObject().AddComponent<Text>(), new GameObject().AddComponent<Text>(),
                                new GameObject().AddComponent<Text>(), new GameObject().AddComponent<Text>()};
        book.PageN = new Text[bookSize] { new GameObject().AddComponent<Text>(), new GameObject().AddComponent<Text>(),
                                new GameObject().AddComponent<Text>(), new GameObject().AddComponent<Text>()};
        book.OpenBook(Application.dataPath + @"\Tests\file.txt");

        //ASSERT
        Assert.AreEqual(pagesCount, book.PagesCount);
        Assert.AreEqual(pageCurrent, book.PageCurrent);
        Assert.AreEqual(firstPageContent, book.Pages[0]);
        for(int i = 0; i < bookSize; i++)
        {
            string text = (i == 0) ? firstPageContent : "";
            string num = (i + 1).ToString();
            Assert.AreEqual(text, book.Page[i].text);
            Assert.AreEqual(num, book.PageN[i].text);
        }

        yield return null;
    }


    [UnityTest]
    public IEnumerator BookTurnsTest()
    {
        //ARRANGE
        const int bookSize = 4;
        const int pageCurrent = 0;
        const int pageCurrentAfterMove = 0;

        //ACT
        var gameObject = new GameObject();
        var book = gameObject.AddComponent<Book>();
        book.Page = new Text[bookSize] { new GameObject().AddComponent<Text>(), new GameObject().AddComponent<Text>(),
                                new GameObject().AddComponent<Text>(), new GameObject().AddComponent<Text>()};
        book.PageN = new Text[bookSize] { new GameObject().AddComponent<Text>(), new GameObject().AddComponent<Text>(),
                                new GameObject().AddComponent<Text>(), new GameObject().AddComponent<Text>()};
        book.OpenBook(Application.dataPath + @"\Tests\file.txt");
        int startPageCurrent = book.PageCurrent;
        book.NextPage();
        int nextPageCurrent = book.PageCurrent;
        book.PrevPage();
        book.PrevPage();
        int prevPageCurrent = book.PageCurrent;

        //ASSERT
        Assert.AreEqual(pageCurrent, startPageCurrent);
        Assert.AreEqual(pageCurrentAfterMove, nextPageCurrent);
        Assert.AreEqual(pageCurrentAfterMove, prevPageCurrent);

        yield return null;
    }
}
