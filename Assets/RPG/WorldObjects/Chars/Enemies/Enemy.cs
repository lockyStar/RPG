﻿using UnityEngine;
using System;
using System.Collections;


public class EnemyController : Character {
    // Состояния КА ИИ
    public Renderer rend;
    public int HP = 2;
    protected const int chilling = 1;
    protected const int movingToSomeone = 3;
    protected const int waiting = 0;
    protected const int selfHealing = 2;
    protected const int RageSpeed = 17;
    protected const int NormalSpeed = 11;

    public int state;
    private double viewRange = 17;//Дальность обзора за пределы тумана войны
    //Направление передвижения
    public bool isFacingRight = false;
    // Вспомогаательный объект необходимый для определения объектов таргетировавших нас
    public GameObject activeHero;
    // Данные для постановки цели движения/атаки/лечения   
    private bool isTargetDefined = false;
    private Vector3 target;
    private bool isTargetHero = false;
    private GameObject targetObject;
    public bool dead = false;

    // Use this for initialization
    void Start() {
        state = waiting;
        isFacingRight = false;
        viewRange = 6;

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
            //Debug.Log("Distance to Object " + distance + "; Minimal distance " + minDistance);
            if (distance < minDistance)
            {
                minDistance = distance;
                theClosestOne = possibleTarget;
            }
        }
    }

    void HeroSeeking() //Поиск врага для нападения
    {
        
        
        double minDistance = 10;//!! Найти ошибку с ViewRange!!



        //Debug.Log("Hero Seeking: minDistance " + minDistance + "; viewRange " + viewRange);
        string[] tagsArray = { "Hero", "ActiveHero", "PassiveHero" }; //Набор тегов для поиска
        foreach (string tag in tagsArray) // Ищем по всем тегам и находим ближайший объект
        {
            
            SeekingTheClosestObjectByTag(tag, minDistance);
            if (theClosestOne != null) //При нахождении достаточно близкого врага, задаём его как цель

            {
                minDistance = DistanceFromTo(theClosestOne.transform.position, transform.position); //Обновление минимального расстояния до цели
                //Debug.Log("minDistance " + minDistance);
                targetObject = theClosestOne;
                isTargetHero = true;
            }
        }
        
    }
    public void Hurt()
    {
        // уменьшение HP на один
        HP--;
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
        HeroSeeking(); //Поиск недружественной стороны в ближайшем окружении к нам
        //Debug.Log("Setting new target");

        if (isTargetHero)
        {
            maxSpeed = RageSpeed;
            target = SetTarget(targetObject.transform.position);
        }
        else
        {
            maxSpeed = NormalSpeed;
            Vector3 point = (UnityEngine.Random.insideUnitSphere); //задаём рандомное направление (круиз / патруль)
            point = point * 3;
            point += Camera.main.transform.position;
            target = SetTarget(point);
        }
    }


    void UpdateTarget()
    {
       if (isTargetHero)
       {
            maxSpeed = RageSpeed;
            SetTarget(targetObject.transform.position);
       }
    }

    // Update is called once per frame
    void FixedUpdate () {
        //Debug.Log("FUpdate");
        if (isTargetDefined == false)
        {
            //Debug.Log("ViewRange " + viewRange);
            TargetDefining();// Задание точки перемещения
        }
        else
        {
            if (isTargetHero == false)
            {
                HeroSeeking();
            }
            UpdateTarget();
            PositionUpdate(); //Перемещение к цели
        }
        if (HP <= 0 && !dead)
            Death(); // если НР законились, убиваем
    }
    void Death()
    {
        dead = true;
        rend.enabled = false;
        Destroy(gameObject, .5f);
    }
}
