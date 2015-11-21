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
    public double viewRange = 3.0f;//Дальность обзора за пределы тумана войны
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

    
    double DistanceFromTo(Vector3 a, Vector3 b)//Расстояние между двумя координатами 
    {
        return Math.Abs(a.x - b.x);
    }

    GameObject theClosestOne;//вспомогательная переменная необходимая для возвращения ближайшего объекта по тегу

    void SeekingTheClosestObjectByTag (string tag, double minDistance) //Поиск ближайшего объекта к текущему по тегу
    {
        theClosestOne = null;
        GameObject[] possibleTargets = null;
        possibleTargets = GameObject.FindGameObjectsWithTag(tag);//Задаём список объектов с таким тегом

        foreach (GameObject possibleTarget in possibleTargets)// Проверяем каждый, может ли он быть ближайшим
        {
            double distance = DistanceFromTo(transform.position, possibleTarget.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                theClosestOne = possibleTarget;
            }
        }
    }

    void HeroSeeking() //Поиск врага для нападения
    {
        
        
        double minDistance = viewRange;
        bool isHeroFound = false;
        string[] tagsArray = { "Hero", "ActiveHero", "PassiveHero" }; //Набор тегов для поиска
        foreach (string tag in tagsArray)
        {
            SeekingTheClosestObjectByTag(tag, minDistance);
            if (theClosestOne != null)
            {
                minDistance = DistanceFromTo(theClosestOne.transform.position, transform.position); //Обновление минимального расстояния до цели
            }
        }
        if (theClosestOne != null)//Если найден ближайший объект внутри радиуса обзора, то задаём цель
        {
            targetObject = theClosestOne;
            isTargetHero = true;
        }
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
