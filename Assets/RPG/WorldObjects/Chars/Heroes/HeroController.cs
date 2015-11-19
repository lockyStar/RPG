using UnityEngine;
using System;
using System.Collections;

    

public class HeroController : Character {
    private bool active = true; //Активность - возможность управления игроком данным героем в данный момент
    
    private bool isFacingRight = true; // Направление отрисовки спрайта вправо
    
    private bool isTargetDefined = false; // Задана ли цель перемещения

    private Vector3 target; // Цель для перемещения

    private bool isTargetEnemy = false; // Цель для перемещения враг или нет

    private GameObject enemy; // 

    

    private double attackrange;

   
    // Use this for initialization
    void Start() {
        
    }

    void MakeActive()
      {
          active = true;
        gameObject.tag = "ActiveHero";
    }

      void MakeUnactve()
      {
          active = false;
        gameObject.tag = "PassiveHero";
    }

     
      public bool IsActive()
      {
          return active;
      }

   
    public void SetEnemy(GameObject enemyTarget)
    {
        isTargetEnemy = true;
        enemy = enemyTarget;
        Debug.Log(":(");
    }

    private void OnMouseDown()
    {
        if (this.IsActive())
        {
            this.MakeUnactve();
        }
        else this.MakeActive();
        //задать активность\неактивность  (исправить на сброс активности по клику не на перса, при наличии нескольких персонажей)
    }
    
    private void PositionUpdate()
    {
        if (isTargetEnemy)
        {
            target = enemy.transform.position;
            isTargetDefined = true;
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

    Vector3 SetTarget (Vector3 point)//Задание цели перемещения
    {
        Vector3 target = point;

        if ((target.x - transform.position.x > 0 && !isFacingRight) || (target.x - transform.position.x < 0 && isFacingRight))
        {    //отражаем персонажа в соответствие с направлением движения к цели
            
            Flip();
            speed = 1;
            
        }

        isTargetDefined = true;
        return target;
    }

    void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(1)&&(active)) // Если произошёл клик правой кнопкой мыши и данный персонаж активен, то задать цель
        {
            isTargetEnemy = false;
            target = SetTarget(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
        if ((active))
        {
            PositionUpdate();
        }
        if (isTargetDefined) // Если задана цель, то двигаемся к ней
        {
            target.z = transform.position.z;
            target.y = transform.position.y;
            if (speed < Convert.ToDouble(maxSpeed)) 
            {
                speed += 1; //увеличение скорости до максимальной
            }
            transform.position = Vector3.MoveTowards(transform.position, target, Convert.ToInt16(speed) * Time.deltaTime);
            if (Math.Abs(target.x - transform.position.x ) <= 1 / 1000)
            {
                isTargetDefined = false; // При достижении цели сбрасываем флаг наличия цели
            } 
        }
    }
}
