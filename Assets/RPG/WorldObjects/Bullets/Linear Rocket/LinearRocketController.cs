using UnityEngine;
using System;
using System.Collections;


public class LinearRocketController : BulletController{

   // private Vector3 direction;
    
    //private int speed = 30;

    

   /* LinearRocketController (Vector3 ShootingDirection)
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
    }*/

	void Start () {
        //  direction = new Vector3(1, 0, 0);
        Destroy(gameObject, 5); //разрушение объекта через 5 секунд, если он не разрушился ранее
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        
    }

    // Update is called once per frame
    /*void FixedUpdate () {
        Vector3 target = transform.position;
        target.x += direction.x;
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }*/
}
