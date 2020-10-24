using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    public float sens = 1;
    float sensitivity;
    float nearsensitvety;
    FixedTouchField fixedTouch;
    public Transform CamerafollowObj;
    Vector3 pos;

    CinemachineVirtualCamera cv;
    
    
    public float limitUsing = 9;
    float limit;
    void Start()
    {
        nearsensitvety = sens * 0.3f;
        cv = FindObjectOfType<CinemachineVirtualCamera>();
        fixedTouch = FindObjectOfType<FixedTouchField>();
        limit = limitUsing;

    }

    // Update is called once per frame
    void Update()
    {
        var transposer = cv.GetCinemachineComponent<CinemachineTransposer>();
        Vector3 offset = transposer.m_FollowOffset;
        offset.y -= (Input.mouseScrollDelta.y) * 2;
        if(offset.y  > 74)
        {
            offset.y = 74;
        }
        if(offset.y < 32)
        {
            offset.y = 32;
        }
        if(offset.y < 60)
        {
            offset.z = Mathf.Lerp(offset.z, -30, 0.1f);
            limit = 18;
            sensitivity = nearsensitvety;
        }
        else
        {
            offset.z = Mathf.Lerp(offset.z, -50, 0.1f);
            limit = limitUsing;
            sensitivity = sens;

        }

        transposer.m_FollowOffset = offset;
        
        pos = CamerafollowObj.position;

        if (pos.x < limit && pos.x > -limit && fixedTouch.Pressed ) 
        {
            pos.x -= fixedTouch.TouchDist.x * sensitivity;
        }else if (pos.x > limit || pos.x < -limit)
        {
            if(fixedTouch.TouchDist.x > 0)
            {
                pos.x -= fixedTouch.TouchDist.x * sensitivity;
            }
            else
            {
                pos.x -= fixedTouch.TouchDist.x * sensitivity;
            }

        }

        if (pos.z < limit && pos.z > -limit && fixedTouch.Pressed)
        {
            pos.z -= fixedTouch.TouchDist.y * sensitivity;
        }
        else if (pos.z > limit || pos.z < -limit)
        {
            if (fixedTouch.TouchDist.y > 0)
            {
                pos.z -= fixedTouch.TouchDist.y * sensitivity;
            }
            else
            {
                pos.z -= fixedTouch.TouchDist.y * sensitivity;
            }

        }




        if (pos.x > limit)
        {
            pos.x = limit;
        }
        if(pos.x < -limit)
        {
            pos.x = -limit;
        }
        if (pos.z > limit)
        {
            pos.z = limit;
        }
        if (pos.z < -limit)
        {
            pos.z = -limit;
        }



        CamerafollowObj.position = Vector3.Slerp(CamerafollowObj.position, pos, 0.3f);
        
    }


}
