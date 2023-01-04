using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [SerializeField] OpenFileDialog Dialog;
    [SerializeField] Book Book;

    void BookHandler()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            Book.PrevPage();
        if (Input.GetKeyDown(KeyCode.RightArrow))
            Book.NextPage();
    }

    void DialogHandler()
    {
        float MouseWheelAxis = Input.GetAxis("Mouse ScrollWheel");
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.Escape))
            Dialog.PreviousTab();
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.Return))
            Dialog.NextTab();
        if (Input.GetKeyDown(KeyCode.DownArrow) || MouseWheelAxis < 0)
            Dialog.NextItem();
        if (Input.GetKeyDown(KeyCode.UpArrow) || MouseWheelAxis > 0)
            Dialog.PreviousItem();
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 45f))
                if (hit.collider.name.Contains("Clone"))
                    Dialog.SelectItem(hit.transform);
        }
    }

    void Update()
    {
        if (Dialog.IsDialog)
            DialogHandler();
        else 
            BookHandler();
    }
}
