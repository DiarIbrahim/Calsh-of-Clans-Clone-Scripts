using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Attacker : MonoBehaviour
{
    public float agenyspeed = 3.6f;

    Building[] allBuildings;
    [HideInInspector]
    public Building BuildingToAttack;
    [HideInInspector]
    public bool isdead = false;

    Animator animator;
    NavMeshAgent agent;
    bool NoBuildingToAttack = false;
    void Start()
    {
        BuildingToAttack = FindBuildingToAttack();
        animator = GetComponent<Animator>();
        agent = FindObjectOfType<NavMeshAgent>();
        animator.SetBool("attack", false);

        healthbar = Instantiate(Manager.instnce.healthbarBarbarian).gameObject.GetComponent<Slider>();
        healthbar.maxValue = 100;
        Canvas can = FindObjectOfType<Canvas>();
        healthbar.transform.SetParent(can.transform);
        healthbar.gameObject.SetActive(false);

    }


    // Update is called once per frame
    Slider healthbar = null;
    bool IsDeadSoundAdded = false;
    void Update()
    {
        Slider _slider;

        if(healthbar != null && health !=100)
        {
            healthbar.gameObject.SetActive(true);
            healthbar.value = health;
            Vector3 pos = transform.position;
            pos.y = 3   ;
            healthbar.transform.position = Camera.main.WorldToScreenPoint(pos);
        }

        if (health == 0)
        { 
            
            if (!IsDeadSoundAdded)
            {
                IsDeadSoundAdded = true;
                Instantiate(Manager.instnce.SoundBarbariDie);
            }
            Destroy(healthbar.gameObject);
            NotifyAttackers();
            Destroy(gameObject , 0.1f);
        }

        if (NoBuildingToAttack)
        {
            //healthbar.gameObject.SetActive(false);
            Destroy(healthbar.gameObject);
            agent.speed = 0;
            animator.Play("idl");
            Destroy(this.gameObject, Random.Range(1, 2.5f));
        } else
        {
            if (BuildingToAttack != null)
            {
                attack();
            }
            else
            {
                BuildingToAttack = FindBuildingToAttack();
            }

        }

    }

    Vector3 attackPoint;
    void attack()
    {
        if (attackPoint == Vector3.zero) {
            agent.SetDestination(BuildingToAttack.transform.position);
            agent.speed = agenyspeed;
            animator.SetBool("attack", false);
            RaycastHit hit;
            transform.LookAt(BuildingToAttack.transform);
            if (Physics.Raycast(transform.position, transform.forward, out hit, 30))
            {
                if (hit.transform.tag == "building" &&
                    hit.transform.GetComponent<Building>() == BuildingToAttack &&
                    hit.transform.GetComponent<Building>().Destroied == false)
                {
                    attackPoint = hit.point;
                    attackPoint.x += Random.Range(-0.5f, +0.5f);
                    attackPoint.z += Random.Range(-0.5f, +0.5f);

                }
                else attackPoint = Vector3.zero;

            }
            else attackPoint = Vector3.zero;

        } else {
            agent.SetDestination(attackPoint);
            if (Distance(attackPoint) > 1.8f)
            {
                agent.speed = agenyspeed;
                animator.SetBool("attack", false);

            }
            else
            {
                agent.speed = 0;
                animator.SetBool("attack", true);
            }
        }

    }
    public void GetNotied()
    {
        attackPoint = Vector3.zero;
        BuildingToAttack = FindBuildingToAttack();

    }

    bool MarkerSpowned = false;
    Building FindBuildingToAttack()
    {
        allBuildings = FindObjectsOfType<Building>();
        List<Building> NotDestroiedBuildings = new List<Building>();
        foreach (Building _building in allBuildings)
        {
            if (!_building.Destroied)
            {
                NotDestroiedBuildings.Add(_building);
            }
        }

        if (NotDestroiedBuildings.Count == 0)
        {
            NoBuildingToAttack = true;
            return null;

        }
        else
        {
            float minDistance = 10000;
            int IndexOfNerest = 0;
            for (int i = 0; i < NotDestroiedBuildings.Count; i++)
            {
                float indexDistance = Distance(NotDestroiedBuildings[i]);
                if (indexDistance < minDistance)
                {
                    minDistance = indexDistance;
                    IndexOfNerest = i;
                }
            }

            NotDestroiedBuildings[IndexOfNerest].newAttacker(this);
            if (!MarkerSpowned) {
                MarkerSpowned = true;
                GameObject marker = Instantiate(Manager.instnce.marker);
                Canvas canv = FindObjectOfType<Canvas>();
                marker.transform.SetParent(canv.transform);
                marker.transform.position = Camera.main.WorldToScreenPoint(NotDestroiedBuildings[IndexOfNerest].transform.position);
            }
            return NotDestroiedBuildings[IndexOfNerest];

        }
    }

    float Distance(Building _building)
    {
        if (_building != null)
        {
            return Vector3.Distance(transform.position, _building.transform.position);
        }else
        {
            attackPoint = Vector3.zero;
            return 0;
        }
    }
    float Distance(Vector3 pos)
    {
        return Vector3.Distance(transform.position, pos);
    }
    public int health = 100;

    
    public void attackerAttacked(int damege)
    {
        if (health > 0)
            health -= damege;
        else health = 0;


    }

    List<Conan> conans = new List<Conan>();
    bool Nolistner = false;
    void NotifyAttackers() {
        foreach (Conan con in conans)
        {
            con.GetNotiffied();
            if (con.attackerToAttack == this)
            {
                Nolistner = false;
            }
            else Nolistner = true;
        }
    }

    public void NewconAttacker(Conan _conan)
    {
        if(conans.Contains(_conan)){
            return;
        }
        else
        {
            conans.Add(_conan);
        }
    }

}
