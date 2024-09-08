using UnityEngine;

public class ToggleGameObject : MonoBehaviour
{
    public GameObject targetObject;  // The object you want to toggle on/off

    void Start()
    {
        // Ensure the target object is initially inactive
        if (targetObject != null)
        {
            targetObject.SetActive(false);
        }
    }

    // This method will be called when the button is pressed
    public void ToggleObject()
    {
        if (targetObject != null)
        {
            // Toggle the active state of the target object
            targetObject.SetActive(!targetObject.activeSelf);
        }
    }
}
