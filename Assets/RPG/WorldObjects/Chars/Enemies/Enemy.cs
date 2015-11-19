using UnityEngine;
using System;
using System.Collections;


public class EnemyController : Character {

    protected const int moving = 1;
    protected const int waiting = 0;
    protected const int selfHealing = 2;
    public int state;
    public bool isFacingRight = true;

    public GameObject activePlayer;

  

    public bool isTargetDefined = false;

    private Vector3 target;

    // Use this for initialization
    void Start () {
        state = waiting;
	}
    private void OnMouseDown()
    {
        activePlayer = GameObject.FindWithTag("ActiveHero");
        if (activePlayer != null)
        {
            Debug.Log(activePlayer.transform.position);
            activePlayer.SendMessage("SetEnemy", gameObject);
        }
    }
    private void Flip()
    {
        //меняем направление движения персонажа
        isFacingRight = !isFacingRight;
        //получаем размеры персонажа
        Vector3 theScale = transform.localScale;
        //зеркально отражаем персонажа по оси Х
        theScale.x *= -1;
        //задаем новый размер персонажа, равный старому, но зеркально отраженный
        transform.localScale = theScale;
    }

    Vector3 SetTarget(Vector3 point)//Задание цели перемещения
    {
        Vector3 target = point;
        target.z = transform.position.z;
        target.y = transform.position.y;
        if ((target.x - transform.position.x > 0 && !isFacingRight) || (target.x - transform.position.x < 0 && isFacingRight))
        {    //отражаем персонажа в соответствие с направлением движения к цели

            Flip();
            speed = 1;

        }

        isTargetDefined = true;
        return target;
    }
    // Update is called once per frame
    void FixedUpdate () {
        Debug.Log("FUpdate");
       /* if (isTargetDefined == false)
        {
            Debug.Log("Setting new target");
            Vector3 point = (UnityEngine.Random.insideUnitSphere); //задаём рандомное направление (круиз / патруль)
            point = point * 3;
            point += Camera.main.transform.position;
            target = SetTarget(point);
            
        }
        else
        {
            Debug.Log("Moving");
            target.z = transform.position.z;
            target.y = transform.position.y;
            if (speed < Convert.ToDouble(maxSpeed))
            {
                speed += 1; //увеличение скорости до максимальной
            }
            transform.position = Vector3.MoveTowards(transform.position, target, Convert.ToInt16(speed) * Time.deltaTime); // Движение к цели
            if (Math.Abs(target.x - transform.position.x) <= 1 / 1000)
            {
                isTargetDefined = false; // При достижении цели сбрасываем флаг наличия цели
            }
        }*/
	}
}
