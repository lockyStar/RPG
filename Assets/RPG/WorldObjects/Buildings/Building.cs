using UnityEngine;
using System.Collections;

public class Building : MonoBehaviour
{
    private bool trigg;
   // private bool showbut = true;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    /*void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            trigg = true;
        }
    }*/
    void OnMouseDown()
    {
        trigg = true;
    }
    private void OnGUI()
    {
        if (trigg)
        {
            if (GUI.Button(new Rect(Screen.width - 100, 0, 100, 50), "Buff"))
            {
                trigg = false;
            }
        }
    }
}