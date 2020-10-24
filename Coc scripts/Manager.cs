using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public GameObject Barabarian;
    public GameObject marker;
    public GameObject Bullet;
    public GameObject healthbarBarbarian;
    public static Manager instnce;

    public GameObject SoundBrabarAttack, SoundBarbariDie, SoundBuildingDestroy , SoundconanAttack;

    // --------------------
    public AudioClip SoundnoAttack, soundAttack;
    AudioSource audio;
    void Start()
    {
        if(instnce == null)
        {
            instnce = this;
        }

       audio =  gameObject.AddComponent<AudioSource>();
        audio.clip = SoundnoAttack;
        audio.volume = 0.3f;
        audio.loop = true;
        audio.Play();
        
    }

    // Update is called once per frame
    bool AttackStarted = false;

    void Update()
    {


        if (Input.GetMouseButtonDown(0))
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag == "land")
                {
                    SpownAttacker(hit);
                    if (!AttackStarted)
                    {
                        AttackStarted = true;
                        audio.clip = soundAttack;
                        audio.Play();
                    }
                }
            }
        }
        
    }

    void SpownAttacker(RaycastHit hit)
    {
        Instantiate(Barabarian, hit.point, Quaternion.identity);

    }
}
