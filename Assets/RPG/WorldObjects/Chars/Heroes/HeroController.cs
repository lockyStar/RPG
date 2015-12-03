using UnityEngine;
using System;
using System.Collections;

    

public class HeroController : Character {
    private bool active = true; //Активность - возможность управления игроком данным героем в данный момент
    
    private bool isFacingRight = true; // Направление отрисовки спрайта вправо
    
    private bool isTargetDefined = false; // Задана ли цель перемещения

    private Vector3 target; // Цель для перемещения

    private bool isTargetEnemy = false; // Цель для перемещения враг или нет

    private GameObject enemy; // Объект враг-цель

    public float attackRange;//Радиус атаки

    public Rigidbody2D rocket;

    public float rocketspeed;

   
    // Use this for initialization
    void Start() {
        
    }

    void MakeActive()//Сделать объект выбранным пользователем
    {
        active = true;
        gameObject.tag = "ActiveHero";
    }

    void MakeUnactve()//Убрать выделение с этого объекта
    {
        active = false;
        gameObject.tag = "PassiveHero";
    }

     
    public bool IsActive()//Выделен ли этот объект
    {
        return active;
    }

   
    public void SetEnemy(GameObject enemyTarget)//Задание врага-цели
    {
        isTargetEnemy = true;
        enemy = enemyTarget;
        isTargetDefined = true;
        //Debug.Log(":(");
    }

    private IEnumerator shot()
    {
        if (enemy.gameObject.GetComponent<EnemyController>().dead == false)
        {
            if (isFacingRight)
            {
                //создаем рокету в право и скорость в право
                Rigidbody2D bullet = Instantiate(rocket, transform.position, Quaternion.Euler(new Vector3(0, 0, 0))) as Rigidbody2D;
                bullet.velocity = new Vector2(rocketspeed, 0);
            }
            else
            {
                //в лево аналогично
                Rigidbody2D bullet = Instantiate(rocket, transform.position, Quaternion.Euler(new Vector3(0, 0, 180f))) as Rigidbody2D;
                bullet.velocity = new Vector2(-rocketspeed, 0);
            }
             yield return new WaitForSeconds(1f);
             StartCoroutine("shot");
        }
    }

    private void OnMouseDown()//Выделение персонажа по клику на него
    {
        if (this.IsActive())
        {
            this.MakeUnactve();
        }
        else this.MakeActive();
        //задать активность\неактивность  (исправить на сброс активности по клику не на перса, при наличии нескольких персонажей)
    }
    /*
    private void TargetPositionUpdate()// Обновление координат вектора-цели с учётом того, что 
    {
        if (isTargetEnemy)
        {
            target = enemy.transform.position;
            isTargetDefined = true;
        }
    }*/

    private void Flip()// Отражение направления отрисовки персонажа
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

    Vector3 SetTarget (Vector3 point)//Задание вектора-цели перемещения
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
        if (isTargetEnemy && (active) && isTargetDefined)
        {
            if (enemy.transform.position != null)
                target = SetTarget(enemy.transform.position);
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
            if (isTargetEnemy)
            {
                if (Math.Abs(target.x - transform.position.x) <= (attackRange - (0.001)))
                {
                    StartCoroutine("shot");
                    isTargetDefined = false;  // При достижении цели сбрасываем флаг наличия цели
                }
            } 
            else
            {
                if (Math.Abs(target.x - transform.position.x) <= (1 / 1000))
                {
                    isTargetDefined = false; // При достижении цели сбрасываем флаг наличия цели
                }
            }
        }
    }
}
