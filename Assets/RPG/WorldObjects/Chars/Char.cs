using UnityEngine;
using System.Collections;

public class Character : WorldObject {

    protected double speed;
    protected int maxSpeed = 11;
    public Character ()
    {
        movingObject = true;
        destructable = true;
    }
    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
