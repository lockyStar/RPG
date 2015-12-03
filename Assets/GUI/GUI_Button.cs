using UnityEngine;
using System.Collections;

public class GUI_Button : MonoBehaviour {

    // Use this for initialization
    public Transform prefab;
    public GameObject[] spis;
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    private void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 100, 50), "Button"))
        {
            //if (Input.GetMouseButtonDown(1))
            
                Debug.Log("I am alive!");
                spis = GameObject.FindGameObjectsWithTag("Building");
            if (spis.Length < 1) { Instantiate(prefab, new Vector3(-10, -2F, 0), Quaternion.identity); }
            
        }

    }
}
