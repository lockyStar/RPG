using UnityEngine;
using System;
using System.Collections;


public class EnemyController : Character {
    // Состояния КА ИИ
    protected const int chilling = 1;
    protected const int movingToSomeone = 3;
    protected const int waiting = 0;
    protected const int selfHealing = 2;
    public int state;
    //Направление передвижения
    public bool isFacingRight = false;
    // Вспомогаательный объект необходимый для определения объектов таргетировавших нас
    public GameObject activeHero;
    // Данные для постановки цели движения/атаки/лечения   
    private bool isTargetDefined = false;
    private Vector3 target;
    private bool isTargetHero = false;
    private GameObject targetObject;

    // Use this for initialization
    void Start() {
        state = waiting;
    }
    private void OnMouseDown()
    {

        activeHero = GameObject.FindWithTag("ActiveHero");// Поиск активных объектов, осуществивших клик по нам
        if (activeHero != null)
        {
            //Debug.Log(activeHero.transform.position);
            activeHero.SendMessage("SetEnemy", gameObject);// Таргетирование активных персонжей на себя
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

    void PositionUpdate()
    {
        //Debug.Log("Moving");
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

    }

    double DistanceFromTo(Vector3 a, Vector3 b)
    {
        return Math.Abs(a.x - b.x);
    }


    void HeroSeeking()
    {
        GameObject[] possibleTargets = null;
        GameObject theClosestOne;
        double minDistance = 3.0f;
        bool isHeroFound = false;
        possibleTargets = GameObject.FindGameObjectsWithTag("Hero");
       
        foreach (GameObject possibleTarget in possibleTargets)
        {
            double distance = DistanceFromTo(transform.position, possibleTarget.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                isHeroFound = true;
                theClosestOne = possibleTarget;
            }
        }
    }

    void TargetDefining ()
    {
        //HeroSeeking(); //Поиск недружественной стороны в ближайшем окружении к нам
        //Debug.Log("Setting new target");
        Vector3 point = (UnityEngine.Random.insideUnitSphere); //задаём рандомное направление (круиз / патруль)
        point = point * 3;
        point += Camera.main.transform.position;
        target = SetTarget(point);
    }


    // Update is called once per frame
    void FixedUpdate () {
        //Debug.Log("FUpdate");
        if (isTargetDefined == false)
        {
            TargetDefining();// Задание точки перемещения
        }
        else
        {
            PositionUpdate(); //Перемещение к цели
        }
	}
}
