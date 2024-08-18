using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerManager : Auto_Singleton<PlayerManager>
{
    [HideInInspector] public Vector2 moveInput;
    [HideInInspector] public Vector2 viewInput;

    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public Collider playerCol;

    [HideInInspector] public PlayerShoot shoot;
    [HideInInspector] public PlayerJump jump;
    [HideInInspector] public PlayerClimb climb;
    [HideInInspector] public PlayerCombat combat;

    [Header("- Player Walk -")]
    public float walkSpeed;
    public float runSpeed;
    [HideInInspector] public float currentSpeed;
    public float rotationDamp;

    [Header("- Player Jump -")]
    public float jumpForce;
    public LayerMask jumpLayerMask;

    [Header("- Camera Controller -")]
    public float senX = 50f;
    public bool invertX;
    public float senY = 50f;
    public bool invertY;
    public float YClampMin = -20;
    public float YClampMax = 30;
    public float cameraSmoothMove = 2f;
    public Transform cameraPivot;

    public float camDisMin;
    public float camDisMax;

    [Header("- Player Action -")]
    public bool CanMove;
    public bool isAim;
    public bool isRun;
    public bool isOnAir;
    public bool isClimb;

    [Header("- Shoot -")]
    public Transform ArrowSpawnPoint;
    public GameObject ArrowPrefab;
    public float arrowSpeed;
    public GameObject Crosshair;
    public float shootDelay;

    [Header("- Climb -")]
    public ParkoutState parkoutState;
    public Transform topPos;
    public Transform forwardPos;
    public Transform headPos;
    public float maxHeight;
    public float checkLenght;
    public float headHeight;
    public LayerMask obstacleMask;

    [Header("Attack")]
    public List<AttackSO> combo = new List<AttackSO>();
    public int combatCount;
    public float nextFireTime = .5f;
    public float comboTime = .8f;

    [Header("- Animation -")]
    public Transform ArrowTranformInHand;
    public Animator animator;


    private void Awake()
    {
        CanMove = true;
        rb = GetComponent<Rigidbody>();
        playerCol = GetComponent<Collider>();
        animator = GetComponent<Animator>();
        shoot = GetComponent<PlayerShoot>();
        jump = GetComponent<PlayerJump>();
        climb = GetComponent<PlayerClimb>();
        combat = GetComponent<PlayerCombat>();
    }

}
