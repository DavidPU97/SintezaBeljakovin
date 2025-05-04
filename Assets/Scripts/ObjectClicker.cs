using UnityEngine;

public class ObjectClicker : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse button click
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 2.0f);


            if (Physics.Raycast(ray, out hit))
            {
                GameObject clickedObject = hit.collider.gameObject;
                Debug.Log("Clicked on: " + clickedObject.name);

                // Example: Check for a specific component on the clicked object
                ClickableObject clickable = clickedObject.GetComponent<ClickableObject>();
                if (clickable != null)
                {
                    //clickable.OnMouseDown();
                }
            }
        }
    }
}
