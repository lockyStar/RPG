using UnityEngine;
using System;
using System.Collections;


public class LinearRocketController : BulletController{

    // Use this for initialization
    
    private Vector3 direction;
    
    private int speed = 30;

    LinearRocketController (Vector3 ShootingDirection)
    {
        ShootingDirection.y = 0;
        ShootingDirection.x = ShootingDirection.x / Math.Abs(ShootingDirection.x);
        direction = ShootingDirection;
        if (direction.x < 0)
        {
            Vector3 theScale = transform.localScale;
            //зеркально отражаем персонажа по оси Х
            theScale.x *= -1;
            //задаем новый размер персонажа, равный старому, но зеркально отраженный
            transform.localScale = theScale;
        }
    }

	void Start () {
        direction = new Vector3(1, 0, 0);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        Vector3 target = transform.position;
        target.x += direction.x;
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }
}
