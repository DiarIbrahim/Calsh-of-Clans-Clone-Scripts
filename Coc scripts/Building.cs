using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Building : MonoBehaviour
{
    [HideInInspector]
     public int health = 100;

     public int ranck = 1;

    public GameObject HealthSlidergmObj ;
    Slider healthbar;

    [HideInInspector]
    public bool Destroied = false;

    public Transform building, building_des;

    Canvas mainCanves;

    
    void Start()
    {
        building.gameObject.SetActive(true);
        building_des.gameObject.SetActive(false);

        healthbar = Instantiate(HealthSlidergmObj.GetComponent<Slider>());
        mainCanves = FindObjectOfType<Canvas>();

        healthbar.maxValue = 100;

        healthbar.gameObject.SetActive(false);  
    }
    bool IsDestroySoundAdded = false;

    void Update()
    {


        if(health == 0)
        {
            Destroied = true;

        }

        healthbar.transform.SetParent(mainCanves.transform);
        Vector3 HigherPos = transform.position;
        HigherPos.y = 3.2f;
        healthbar.transform.position = Camera.main.WorldToScreenPoint(HigherPos);

        healthbar.value = health;

        if (Destroied)
        {
            if (!IsDestroySoundAdded)
            {
                IsDestroySoundAdded = true;
                Instantiate(Manager.instnce.SoundBuildingDestroy);
            }

            healthbar.gameObject.SetActive(false);
            building_des.gameObject.SetActive(true);
            StartCoroutine(ShowDes());
            if(!noListner)
              NotifyAttackers();

        }
        
    }

    bool noListner = false;
    void NotifyAttackers()
    {
        
        foreach(Attacker att in attakers)
        {
            if(att != null)
              att.GetNotied();

            if (att.BuildingToAttack != this)
            {
                noListner = true;
            }
            else
            {
                noListner = false;
            }
        }
    }

    IEnumerator ShowDes()
    {
        yield return new WaitForSeconds(0.2f);
        building.gameObject.SetActive(false);

    }



    int damage()
    {
        return 6 - ranck;
    }
    
    List<Attacker> attakers = new List<Attacker>();
    
    public void newAttacker(Attacker attacker)
    {
        if (attakers.Contains(attacker))
            return;
        else attakers.Add(attacker);
    }

    public void Attacked()
    {
        healthbar.gameObject.SetActive(true);

        if (health > 0)
        {
            health -= damage();
            Instantiate(Manager.instnce.SoundBrabarAttack);
           
        }
        else health = 0;

       
    }

}
