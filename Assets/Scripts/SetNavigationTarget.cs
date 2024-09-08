using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class SetNavigationTarget : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown targetDropdown;
    [SerializeField]
    private List<GameObject> targetObjects = new List<GameObject>();
    [SerializeField]
    private GameObject arrowPrefab;
    [SerializeField]
    private float arrowSpacing = 0.2f;
    [SerializeField]
    private bool flipArrowDirection = false;

    private NavMeshPath path;
    private List<GameObject> pathArrows = new List<GameObject>();
    private bool lineToggle = false;

    void Start()
    {
        path = new NavMeshPath();
        targetDropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        InitializeDropdown();
        HideAllTargetObjects();
    }

    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            lineToggle = !lineToggle;
        }
        if (lineToggle)
        {
            UpdatePath();
        }
        else
        {
            ClearPathArrows();
        }
    }

    private void InitializeDropdown()
    {
        targetDropdown.ClearOptions();
        List<string> options = new List<string> { "Select" };
        foreach (GameObject obj in targetObjects)
        {
            if (obj != null)
            {
                options.Add(obj.name);
            }
        }
        targetDropdown.AddOptions(options);
        targetDropdown.value = 0;
    }

    private void HideAllTargetObjects()
    {
        foreach (GameObject obj in targetObjects)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
    }

    private void ShowSelectedTargetObject(int index)
    {
        HideAllTargetObjects();
        if (index > 0 && index <= targetObjects.Count)
        {
            targetObjects[index - 1].SetActive(true);
        }
    }

    private void UpdatePath()
    {
        GameObject target = GetTargetObject();

        if (target != null)
        {
            NavMesh.CalculatePath(transform.position, target.transform.position, NavMesh.AllAreas, path);
            ClearPathArrows();

            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                Vector3 start = path.corners[i];
                Vector3 end = path.corners[i + 1];
                PlaceArrowsAlongSegment(start, end);
            }
        }
    }

    private void PlaceArrowsAlongSegment(Vector3 start, Vector3 end)
    {
        float distance = Vector3.Distance(start, end);
        int numArrows = Mathf.CeilToInt(distance / arrowSpacing);
        Vector3 direction = (end - start).normalized;

        for (int i = 0; i < numArrows; i++)
        {
            Vector3 position = start + direction * i * arrowSpacing;
            position = AdjustPositionToPath(position);
            GameObject arrow = Instantiate(arrowPrefab, position, Quaternion.identity);
            RotateArrow(arrow, direction);
            pathArrows.Add(arrow);
        }
    }

    private void RotateArrow(GameObject arrow, Vector3 pathDirection)
    {
        if (flipArrowDirection)
        {
            pathDirection = -pathDirection;
        }

        Quaternion rotation = Quaternion.LookRotation(pathDirection);
        rotation *= Quaternion.Euler(0, 90, 0);
        arrow.transform.rotation = rotation;
    }

    private Vector3 AdjustPositionToPath(Vector3 position)
    {
        RaycastHit hit;
        if (Physics.Raycast(position + Vector3.up * 5f, Vector3.down, out hit, 10f, LayerMask.GetMask("NavigationMesh")))
        {
            return hit.point;
        }
        return position;
    }

    private GameObject GetTargetObject()
    {
        int index = targetDropdown.value - 1;
        if (index >= 0 && index < targetObjects.Count)
        {
            return targetObjects[index];
        }
        return null;
    }

    private void OnDropdownValueChanged(int index)
    {
        ShowSelectedTargetObject(index);
        if (index == 0) // "Select" option
        {
            ClearPathArrows();
            lineToggle = false;
        }
        else
        {
            UpdatePath();
        }
    }

    private void ClearPathArrows()
    {
        foreach (GameObject arrow in pathArrows)
        {
            Destroy(arrow);
        }
        pathArrows.Clear();
    }
}