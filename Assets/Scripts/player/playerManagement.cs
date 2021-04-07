using UnityEngine;
using Photon.Pun;

public class playerManagement : MonoBehaviourPun
{
    public static playerManagement Instance;
    public AudioSource AS;
    public float speed = 4f, groundDistance = 0.4f, gravity = -9.81f, jumpHeight = 1.5f;
    // float moveH = 0f, moveV = 0f;
    public Transform groundCheck;
    public LayerMask groundMask;
    Vector3 velocity;
    CharacterController controller;
    PhotonView PlayerPV;
    public bool isGrounded;
    Animator ani;
    public Transform playerTransform;
    public static float SenseX = 35f, SenseY = 25f;
    //new movement
    private float antiBumpFactor = 0f;//!
    void Awake()
    {
        Instance = this;
        ani = GetComponent<Animator>();
        AS = GetComponent<AudioSource>();
        controller = GetComponent<CharacterController>();
        PlayerPV = GetComponent<PhotonView>();
    }
    void Start()
    {
        ani.SetBool("dying", false);
        ani.SetLayerWeight(2, 0f);
        if (!PlayerPV.IsMine)
        {

            Destroy(controller);
        }
    }
    void Update()
    {
        if (!PlayerPV.IsMine) return;
        // moveH = Input.GetAxis("Horizontal");
        // moveV = Input.GetAxis("Vertical");
        PlayerMovementAnimaiton();

        if (!GameUI.GamePaused)
        {
            PlayerJump();
        }

    }
    void FixedUpdate()
    {
        if (!PlayerPV.IsMine) return;
        if (!GameUI.GamePaused)
        {
            PlayerMovement();
        }
    }
    public Vector2 input//!!!!!
    {
        get
        {
            Vector2 i = Vector2.zero;
            i.x = Input.GetAxis("Horizontal");
            i.y = Input.GetAxis("Vertical");
            i *= (i.x != 0.0f && i.y != 0.0f) ? .7071f : 1.0f;
            return i;
        }
    }
    void PlayerMovement()
    {

        // Vector3 move = transform.right * moveH + transform.forward * moveV;
        PlayerPV.RPC("RPC_WalkSound_Play", RpcTarget.Others);
        AS.Play();
        Vector3 move = new Vector3(input.x, -antiBumpFactor, input.y);//!
        move = transform.TransformDirection(move) * speed;//!
        // controller.Move(move * speed * Time.deltaTime);
        controller.Move(move * Time.deltaTime);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        isGrounded = Physics.CheckSphere(groundCheck.transform.position, groundDistance, groundMask);
    }
    [PunRPC]
    void RPC_WalkSound_Play()
    {
        AS.Play();
    }
    void PlayerMovementAnimaiton()
    {
        if (Input.GetKeyUp(KeyCode.LeftShift) || input.y <= 0f)
        {
            speed = 4f;
            ani.SetBool("RunForward", false);
        }
        if (input.y < 0f)
            ani.SetBool("WalkBack", true);
        else if (input.y > 0f)
            ani.SetBool("WalkForward", true);
        else
        {
            ani.SetBool("WalkForward", false);
            ani.SetBool("WalkBack", false);
        }
        if (input.x < 0f)
            ani.SetBool("WalkLeft", true);
        else if (input.x > 0f)
            ani.SetBool("WalkRight", true);
        else
        {
            ani.SetBool("WalkLeft", false);
            ani.SetBool("WalkRight", false);
        }
    }
    void PlayerJump()
    {
        // isGrounded = (controller.Move(velocity * Time.deltaTime) & CollisionFlags.Below) != 0;//!

        if (isGrounded)
        {
            ani.SetBool("JumpUp", false);
            // velocity.y = -2f;
            velocity.y -= gravity * Time.deltaTime;//!
            ani.SetBool("JumpDown", false);
            if (Input.GetKeyDown("space") && isGrounded)
            {
                ani.SetBool("JumpUp", true);
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                ani.SetBool("JumpDown", true);
            }
            if (Input.GetKey(KeyCode.LeftShift) && input.y > 0f)
            {
                speed = 8f;
                ani.SetBool("RunForward", true);
            }
        }
    }


}