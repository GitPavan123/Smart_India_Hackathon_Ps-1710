using UnityEngine;
using UnityEngine.UI;

public class MapToggle : MonoBehaviour
{
    public GameObject mapImage;  // The RawImage object that displays the map
    public Button toggleButton; // The Button that will toggle the map

    private bool isMapVisible = false;

    void Start()
    {
        // Initially set the map to be invisible
        mapImage.SetActive(false);

        // Add a listener to the button to call ToggleMapVisibility when clicked
        toggleButton.onClick.AddListener(ToggleMapVisibility);
    }

    void ToggleMapVisibility()
    {
        // Toggle the map visibility
        isMapVisible = !isMapVisible;
        mapImage.SetActive(isMapVisible);
    }
}
