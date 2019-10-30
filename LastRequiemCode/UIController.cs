using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private Slider m_HPSlider;
    [SerializeField]
    private Slider m_StaminaSlider;
    [SerializeField]
    private TextMeshProUGUI m_CooldownHeal;
    [SerializeField]
    private TextMeshProUGUI m_CooldownBeam;

    [SerializeField]
    private GameObject m_CheatActive;
   
    
    private bool m_HideCursor = false;
    private CursorLockMode m_WantedMode;
    //Step1 action
    
    private void Start()
    {   
        m_HideCursor = true;
        if(m_HideCursor)
        {   
            m_WantedMode = CursorLockMode.Locked;
            SetCursorState();
        }
        m_CheatActive.SetActive(false);
    }
    public void CurrentHealCooldown(float a_WaitTime)
    {
        m_CooldownHeal.text = a_WaitTime.ToString("F0");
        if(a_WaitTime ==0f)
        {
            m_CooldownHeal.text = "";
        }
    }

    public void CurrentBeamCoolDown(float a_WaitTime)
    {
        m_CooldownBeam.text = a_WaitTime.ToString("F0");
        if(a_WaitTime == 0)
        {
            m_CooldownBeam.text = "";
        }
    }

    public void ValueChange(int a_HP)
    {
       m_HPSlider.value = a_HP;
    }
    public void CheatChange(bool a_Cheat)
    {
        m_CheatActive.SetActive(a_Cheat);
    }

    public void StaminaChange(float a_Stamina)
    {
        m_StaminaSlider.value = a_Stamina;
    }

    private void SetCursorState()
    {
        Cursor.lockState = m_WantedMode;
        Cursor.visible = (CursorLockMode.Locked != m_WantedMode);
    }
}
