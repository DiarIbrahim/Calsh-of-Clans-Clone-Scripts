using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conan : MonoBehaviour
{
    public Transform conanBody, shootPos;
    public float attackRange = 3;
    Building _build;
    void Start()
    {
        _build = GetComponent<Building>();
    }

    // Update is called once per frame
    void Update()
    {

        if (!_build.Destroied)
        {
            if (attackerToAttack == null)
            {
                CancelInvoke();
                attackerToAttack = FindToAttack();

            }
            else
            {
                if (!attackController)
                {
                    attackController = true;
                    StartCoroutine(prepareAttack());
                }
                
            }
            
        }else CancelInvoke();
    }

    bool attackController = false;
    IEnumerator prepareAttack()
    {
        yield return new WaitForSeconds(1);
        attackController = false;
        Attack();
    }

   
   void Attack() {
        if (attackerToAttack == null)
            {
                attackerToAttack = FindToAttack();
            }
            else if(shootPos != null && conanBody != null)
            {
                Vector3 PointToAttack = attackerToAttack.transform.position;
                /*PointToAttack.y = conanBody.position.y;*/
                conanBody.LookAt(PointToAttack);
                GameObject _bull = Instantiate(Manager.instnce.Bullet, shootPos.position, shootPos.rotation);
                Rigidbody rb = _bull.AddComponent<Rigidbody>();
                rb.useGravity = false;
                rb.AddForce(conanBody.forward * 1300);
                Instantiate(Manager.instnce.SoundconanAttack);
            }
        }
    





    Attacker[] allattakers;
    
    public Attacker attackerToAttack;

    Attacker FindToAttack()
    {
        allattakers = FindObjectsOfType<Attacker>();
        List<Attacker> allAliveAttackers = new List<Attacker>();
        foreach (Attacker att in allattakers)
        {
            if (!att.isdead)
            {
                allAliveAttackers.Add(att);
            }
        }

        if (allAliveAttackers.Count == 0)
        {
            return null;

        }
        else
        {
            float minDistance = attackRange;
            int IndexOfNerest = -1;
            for (int i = 0; i < allAliveAttackers.Count; i++)
            {
                float indexDistance = Distance(allAliveAttackers[i]);
                if (indexDistance < minDistance)
                {
                    minDistance = indexDistance;
                    IndexOfNerest = i;
                }
            }
            if (IndexOfNerest != -1)
            {
                allAliveAttackers[IndexOfNerest].NewconAttacker(this);
                return allAliveAttackers[IndexOfNerest];
            }
            else return null;

        }
    }

    float Distance(Attacker att)
    {
        return Vector3.Distance(transform.position, att.transform.position);
    }
    public void GetNotiffied()
    {
        attackerToAttack = null;
    }
}
