using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mapselection : MonoBehaviour
{
    public int currentMap;

    private void Start()
    {
        SelectionMap(0);
    }

    void Update()
    {
        // Keyboard input for shifting left and right
        if (Input.GetKeyDown("a") && currentMap > 0)
        {
            currentMap -= 1;
        }
        else if (Input.GetKeyDown("d") && currentMap < 4)
        {
            currentMap += 1;
        }

        // PlayStation controller input for shifting left and right using "Horizontal" axis
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        if (horizontalInput < 0 && currentMap > 0)
        {
            currentMap -= 1;
        }
        else if (horizontalInput > 0 && currentMap < 4)
        {
            currentMap += 1;
        }

        SelectionMap(currentMap);

        MapSelected.index = currentMap;
    }

    private void SelectionMap(int index)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(i == index);
        }
    }
}
