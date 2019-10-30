using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LightBeam : MonoBehaviour
{
    [SerializeField]
    private Rigidbody m_Rb;
    [SerializeField]
    private float m_BeamSpeed = 5f;
    [SerializeField]
    private LayerMask m_Interactable;


    public float BeamSpeed
    {
        get{return m_BeamSpeed;}
    }

    public Action m_HitAction;


    private void FixedUpdate()
    {
        m_Rb.velocity = m_BeamSpeed * transform.forward;
    }

    private void DestroyBeam()
    {
       Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider aCol)
    {
        Ennemy bigboy = aCol.gameObject.GetComponent<Ennemy>();
        if(bigboy!= null)
        {
            
            bigboy.ReceiveDamage(50);
            DestroyBeam();
        }
        else
        {
            
            DestroyBeam();
        }
    }
}
