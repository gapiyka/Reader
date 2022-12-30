using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Collections.Generic;

public class OpenFileDialog : MonoBehaviour
{
    [SerializeField] Text PathText;
    [SerializeField] GameObject FileTextPrefab;

    const string pathStart = @"C:\Users";
    const int itemSize = 40;
    const int itemsPerPage = 15;
    const int uiItemOffset = 300;

    DirectoryInfo dirCur;
    List<string> dirList;
    List<string> drivesList;
    string pathCurr;
    int itemPrevIndex;
    int itemSelectIndex;
    int itemsLength;
    int itemsPage;

    IEnumerable<string> GetExistingDrives()
    {
        foreach (DriveInfo item in DriveInfo.GetDrives())
            if (Directory.Exists(item.Name))
                yield return item.Name;
    }

    void InitItemsText(List<string> list)
    {
        int itemsCounter = 0;
        itemPrevIndex = 0;
        itemSelectIndex = 0;
        itemsPage = 0;
        foreach (var item in list)
        {
            GameObject newItem = Instantiate(FileTextPrefab, transform);
            Vector2 pos = new Vector2(newItem.transform.localPosition.x, newItem.transform.localPosition.y - (itemsCounter % itemsPerPage) * itemSize);
            newItem.GetComponent<RectTransform>().localPosition = pos;
            newItem.GetComponent<Text>().text = item;
            if (itemsCounter >= itemsPerPage) newItem.SetActive(false);
            itemsCounter++;
        }
        itemsLength = itemsCounter;
    }

    void DrawDirItems(ItemType type)
    {
        int children = transform.childCount;
        for (int i = 1; i < children; ++i)
            Destroy(transform.GetChild(i).gameObject);

        if (type == ItemType.Directory)
        {
            List<string> items = new List<string>();
            foreach (var info in dirCur.GetDirectories())
                items.Add(info.Name);
            foreach (var info in dirCur.GetFiles())
                items.Add(info.Name);
            InitItemsText(items);
        } else if (type == ItemType.Drive)
            InitItemsText(drivesList);
    }

    string GetPath()
    {
        string path = "";
        foreach (string dir in dirList)
            path += (dir + "\\");
        
        return path;
    }

    public void PreviousTab()
    {
        dirList.RemoveAt(dirList.Count - 1);
        pathCurr = GetPath();
        if (dirList.Count == 0)
            DrawDirItems(ItemType.Drive);
        else
        {
            dirCur = new DirectoryInfo(pathCurr);
            DrawDirItems(ItemType.Directory);
        }
    }

    public void NextTab()
    {
        dirList.Add(transform.GetChild(1 + itemSelectIndex).GetComponent<Text>().text);
        pathCurr = GetPath();
        dirCur = new DirectoryInfo(pathCurr);
        DrawDirItems(ItemType.Directory);
    }

    void ChangePage(int newPage)
    {
        int children = transform.childCount - 1;
        int limitPages = (itemsPage + 1) * itemsPerPage;
        int len = (children < limitPages) ? children : limitPages;
        for (int i = itemsPage * itemsPerPage; i < len; i++)
            transform.GetChild(1 + i).gameObject.SetActive(false);
        limitPages = (newPage + 1) * itemsPerPage;
        len = (children < limitPages) ? children : limitPages;
        for (int i = newPage * itemsPerPage; i < len; i++)
            transform.GetChild(1 + i).gameObject.SetActive(true);
        itemsPage = newPage;
    }

    void ChangeSelectedItem(bool next)
    {
        int offset = 0;
        if (next && itemSelectIndex < itemsLength - 1) offset = 1;
        if (!next && itemSelectIndex > 0) offset = -1;
        
        if (offset != 0)
        {
            itemPrevIndex = itemSelectIndex;
            itemSelectIndex = itemSelectIndex + offset;
            transform.GetChild(1 + itemPrevIndex).GetComponent<Text>().color = new Color(255, 255, 255);
            int iPage = Convert.ToInt32(MathF.Floor(itemSelectIndex / itemsPerPage));
            if (iPage != itemsPage) ChangePage(iPage);
        }
    }

    public void PreviousItem()
    {
        ChangeSelectedItem(false);
    }

    public void NextItem()
    {
        ChangeSelectedItem(true);
    }

    public void SelectItem(Transform transform)
    {
        itemSelectIndex = (int)((uiItemOffset - transform.localPosition.y) / itemSize + itemsPerPage * itemsPage);
        NextTab();
    }

    void Start()
    {
        pathCurr = pathStart;
        dirCur = new DirectoryInfo(pathStart);
        dirList = new List<string>(pathStart.Split("\\"));
        drivesList = new List<string>(GetExistingDrives());
        DrawDirItems(ItemType.Directory);
    }

    void Update()
    {
        PathText.text = pathCurr;
        transform.GetChild(1 + itemSelectIndex).GetComponent<Text>().color = new Color(0, 200, 0);
    }
}
