using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// CODE DE JEREMY
public enum MenuState
{
    Empty,
    Inventory,
    Party,
    Options,

    count
}
public class InGameMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject m_InGameUI;
    [SerializeField]
    private GameObject m_OptionsPanel;
    [SerializeField]
    private GameObject m_InventoryPanel;
    [SerializeField]
    private GameObject m_PartyPanel;
    [SerializeField]
    private Button m_OptionsButton;
    [SerializeField]
    private Button m_PartyButton;
    [SerializeField]
    private Button m_InventoryButton;
    [SerializeField]
    private MenuState m_State;


    [Header("StatstextField")]
    [SerializeField]
    private TextMeshProUGUI m_Hptext;
    [SerializeField]
    private TextMeshProUGUI m_Strengthtext;
    [SerializeField]
    private TextMeshProUGUI m_Defensetext;
    [SerializeField]
    private TextMeshProUGUI m_MagicText;


    private int m_HPStats = 0;

    private int m_AttackStats =0;

    private int m_DefenseStats =0;

    private int m_MagicStats = 0;

    private int m_StrengthStats = 0;



    private bool m_IsOpen = true;

    private void Awake()
    {
        m_InGameUI.SetActive(false);
        m_IsOpen = false;
    }
    private void Update()
    {
        GetInputs();
    }

    #region Input 
    private void GetInputs()
    {
        //KEY_I
        if (Input.GetButtonDown("OpenGameMenu"))
        {
            if (m_IsOpen != true)
            {
                Debug.Log("Ouvre Menu");
                m_InGameUI.SetActive(true);
                m_IsOpen = true;
            }
            else if (m_IsOpen)
            {
                Debug.Log("Close Menu");
                m_InGameUI.SetActive(false);
                m_IsOpen = false;
            }
        }

    }
    #endregion

    #region ButtonFunction

    public void OpenInventory()
    {
        ChangeMenu(MenuState.Inventory);
        m_InventoryButton.interactable = false;
        m_PartyButton.interactable = true;
        m_OptionsButton.interactable = true;
        SetStats();
    }
    public void OpenParty()
    {
        ChangeMenu(MenuState.Party);
        m_InventoryButton.interactable = true;
        m_PartyButton.interactable = false;
        m_OptionsButton.interactable = true;

    }
    public void OpenOptions()
    {
        ChangeMenu(MenuState.Options);
        m_InventoryButton.interactable = true;
        m_PartyButton.interactable = true;
        m_OptionsButton.interactable = false;
    }



    private void ChangeMenu(MenuState aState)
    {
        switch (aState)
        {
            case MenuState.Empty:
                {
                    m_InventoryButton.interactable = true;
                    m_PartyButton.interactable = true;
                    m_OptionsButton.interactable = true;
                    break;
                }
            case MenuState.Inventory:
                {   
                    m_InventoryPanel.SetActive(true);
                    m_OptionsPanel.SetActive(false);
                    m_PartyPanel.SetActive(false);
                    break;
                }
            case MenuState.Party:
                {
                    m_InventoryPanel.SetActive(false);
                    m_OptionsPanel.SetActive(false);
                    m_PartyPanel.SetActive(true);
                    break;
                }
            case MenuState.Options:
                {
                    m_InventoryPanel.SetActive(false);
                    m_PartyPanel.SetActive(false);
                    m_OptionsPanel.SetActive(true);
                    break;
                }
        }
        m_State = aState;
    }

    private bool CompareState(MenuState aState)
    {
        return aState == m_State;
    }
    #endregion


    #region Stats
    private void SetStats()
    { 
        // On a pas de Player data avec aucune fonction pour aller les chercher
        m_Strengthtext.text= m_StrengthStats.ToString()+" :Strength";
        m_Defensetext.text= m_DefenseStats.ToString()+" :Defense";
        m_Hptext.text= m_HPStats.ToString()+" :HP";
        m_MagicText.text = m_MagicStats.ToString()+" :Magic";
    }
    #endregion

    #region Party
    // Drag and drop the Sprite tu trouve dans le party Menu, comme sa tu decide sur le spot est vide ou pris, si yer pris bah 
    // falloir initialiser les autres membres de la team dans le fight 



    #endregion










}

