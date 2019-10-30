using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public enum EnnemyState
{
    Idle,
    Chase,
    Flee,
    Patrol,
    Attack,

    Count
}
public class Ennemy : MonoBehaviour
{
    // Ennemy Data incoming
    [SerializeField]
    private int m_Damage = 5;
    [SerializeField]
    private int m_HPEnemmy = 100;
    [SerializeField]
    private Rigidbody m_RB;

 

    // ChaseThreshold
    [SerializeField]
    private Slider HealthBar;
    
    private float m_ChaseTheshold = 10f;
    [SerializeField]
    private float m_SpeedEnnemy = 4f;

    [SerializeField]
    private Animator m_EnnemyAnime;

    [SerializeField]
    private EnnemyState m_State;
    private Vector3 m_InitialPos;

    [SerializeField]
    private NavMeshAgent m_AgentEnnemy;

    [SerializeField]
    private Transform[] m_PathPoint;

    private int m_DestPoint = 0;

    [SerializeField]
    private bool m_CanPatrol = true;

    private float m_CurrentTime = 0f;

    private float m_AttackTime = 0f;

    [SerializeField]
    private float m_TimerResetColor = 0.5f;

    private Color m_ColorBaseEnemy;

    [SerializeField]
    private Transform m_TargetPlayer;

    private Vector3 m_MoveEnnemie = new Vector3();

    [SerializeField]
    private Renderer m_RendEnnemy;

    private bool m_ReceiveDamage = false;

    private Ennemy m_CurrentEnnemy;

    private PlayerController m_CurrentPlayer;
    [SerializeField]
    private bool m_EnnemyPatroler = false;

    [SerializeField]
    private float m_CooldownAttackEnnemy = 2f;

    private bool m_EnnemyCanAttack = true;

    private bool m_InRange = false;

    [SerializeField]
    private float m_Distance = 0f;

    [SerializeField]
    private float m_DistanceWanted = 1f;

    [SerializeField]
    private float m_FixDistanceCheck = 1.1f; // Need to check with the size of the monster

    private void Awake()
    {
        Renderer m_rend = GetComponent<Renderer>();
        m_InitialPos = transform.position;
    }

    private void Start()
    {
        m_TargetPlayer = null;
        m_CurrentPlayer = null;

        GoToNextPath();

    }
    // State for my IA
    private void Update()
    {

        switch (m_State)
        {
            case EnnemyState.Idle:
                {
                    UpdateIdle();
                    break;
                }
            case EnnemyState.Patrol:
                {
                    UpdatePatrol();
                    break;
                }
            case EnnemyState.Chase:
                {
                    UpdateChase();
                    break;
                }
            case EnnemyState.Attack:
                {
                    UpdateAttack();
                    break;
                }
            case EnnemyState.Flee:
                {
                    UpdateFlee();
                    break;
                }
        }
    }

    private void FixedUpdate()
    {
        m_MoveEnnemie.y = m_RB.velocity.y;
        m_RB.velocity = m_MoveEnnemie;
        transform.LookAt(m_TargetPlayer);
    }

    private void UpdateIdle()
    {
        m_EnnemyAnime.SetBool("IsIdle", true);
        if (m_TargetPlayer != null)
        {
            if (Vector3.Distance(transform.position, m_TargetPlayer.position) <= m_ChaseTheshold)
            {
                ChangeState(EnnemyState.Chase);
                m_EnnemyAnime.SetBool("IsIdle", false);
            }
        }
    }

    private void UpdateChase()
    {
        if (m_TargetPlayer != null)
        {
            m_AgentEnnemy.isStopped = false;
            m_AgentEnnemy.SetDestination(m_TargetPlayer.position);

            m_EnnemyAnime.SetBool("CanWalk", true);


            CheckDistance();
            if (m_InRange == true)
            {
                ChangeState(EnnemyState.Attack);
            }
            else if (Vector3.Distance(transform.position, m_TargetPlayer.position) >= m_ChaseTheshold)
            {
                ChangeState(EnnemyState.Flee);
            }
        }
        else if (m_TargetPlayer == null && m_CanPatrol)
        {
            ChangeState(EnnemyState.Patrol);
        }
    }

    private void UpdateAttack()
    {

        if (m_TargetPlayer != null)
        {
            m_AgentEnnemy.isStopped = true;
            m_EnnemyAnime.SetBool("CanWalk", false);
            if (!m_EnnemyCanAttack)
            {
                ResetAttack();
            }
            if (m_EnnemyCanAttack)
            {
                EnnemyAttack();
            }
            if (!m_InRange)
            {
                ChangeState(EnnemyState.Chase);
            }
            CheckDistance();

        }
    }

    private void UpdatePatrol()
    {
        if (m_CanPatrol)
        {
            m_EnnemyAnime.SetBool("CanWalk", true);
            if (!m_AgentEnnemy.pathPending && m_AgentEnnemy.remainingDistance < 0.5f)
            {
                GoToNextPath();
            }
        }
        if (m_TargetPlayer != null)
        {
            ChangeState(EnnemyState.Chase);
        }
    }

    private void UpdateFlee()
    {
        m_AgentEnnemy.isStopped = false;
        m_AgentEnnemy.SetDestination(m_InitialPos);
        m_TargetPlayer = null;
        m_CurrentPlayer = null;
        if (Vector3.Distance(transform.position, m_InitialPos) <= 0.1f)
        {
            ChangeState(EnnemyState.Idle);
            m_EnnemyAnime.SetBool("CanWalk", false);
        }
        if (m_CanPatrol)
        {
            ChangeState(EnnemyState.Patrol);
        }

    }

    private void ChangeState(EnnemyState aState)
    {
        switch (aState)
        {
            case EnnemyState.Chase:
                {
                    break;
                }
            case EnnemyState.Flee:
                {
                    break;
                }
            case EnnemyState.Idle:
                {
                    break;
                }
            case EnnemyState.Attack:
                {
                    break;
                }
            case EnnemyState.Patrol:
                {
                    break;
                }
        }
        m_State = aState;
    }

    private bool CompareState(EnnemyState aState)
    {
        return aState == m_State;
    }

    private void OnTriggerEnter(Collider aCol)
    {

        PlayerController player = aCol.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            m_TargetPlayer = aCol.gameObject.transform;
            player.m_CurrentEnnemy = this;
            if (m_EnnemyPatroler)
            {
                m_CanPatrol = false;
            }
            m_CurrentPlayer = player;
        }
    }
    private void OnTriggerExit(Collider aCol)
    {
        PlayerController player = aCol.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            player.m_CurrentEnnemy = null;
            if (m_EnnemyPatroler)
            {
                m_CanPatrol = true;
            }

        }

    }
    private void ResetColor(Color aColor)
    {
        m_RendEnnemy.material.color = aColor;
    }

    public void ChangeColor(Color aColor)
    {
        m_RendEnnemy.material.color = aColor;
    }

    public void ReceiveDamage(int aDamage)
    {
        m_HPEnemmy -= aDamage;

        HealthBar.value = m_HPEnemmy;

        if (m_HPEnemmy <= 0)
        {
            Ennemydie();
        }
    }
    private void Ennemydie()
    {
        Destroy(gameObject);
    }

    public void EnnemyAttack()
    {
        m_EnnemyCanAttack = false;
        m_EnnemyAnime.SetTrigger("CanAttack");
    }
    public void SetAttackDoneEnnemy()
    {
        m_EnnemyCanAttack = true;

        if (m_CurrentPlayer != null)
        {
            m_CurrentPlayer.ReceiveDamage(m_Damage);
        }
    }

    private void ResetAttack()
    {
        m_AttackTime += Time.deltaTime;
        if (m_AttackTime >= m_CooldownAttackEnnemy)
        {
            m_EnnemyCanAttack = true;
            m_AttackTime = 0f;
        }
    }

    private void CheckDistance()
    {
        if (m_TargetPlayer != null)
        {
            m_Distance = Vector3.Distance(transform.position, m_CurrentPlayer.transform.position);
            if (m_Distance <= m_DistanceWanted)
            {

                m_InRange = true;
            }
            if (m_Distance > m_FixDistanceCheck)
            {
                m_InRange = false;
            }
        }

    }

    private void GoToNextPath()
    {
        if (m_PathPoint.Length == 0)
        {
            return;
        }
        m_AgentEnnemy.destination = m_PathPoint[m_DestPoint].position;

        m_DestPoint = (m_DestPoint + 1) % m_PathPoint.Length; 
    }

}
