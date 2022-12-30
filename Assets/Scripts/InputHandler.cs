using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [SerializeField] OpenFileDialog dialog;

    void Update()
    {
        float MouseWheelAxis = Input.GetAxis("Mouse ScrollWheel");
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.Escape))
            dialog.PreviousTab();
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.Return))
            dialog.NextTab();
        if (Input.GetKeyDown(KeyCode.DownArrow) || MouseWheelAxis < 0)
            dialog.NextItem();
        if (Input.GetKeyDown(KeyCode.UpArrow) || MouseWheelAxis > 0)
            dialog.PreviousItem();

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 45f))
                if (hit.collider.name.Contains("Clone"))
                    dialog.SelectItem(hit.transform);
        }        
    }
}
