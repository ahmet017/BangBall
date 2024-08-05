using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 10;
    public float Speed => speed * (GameManager.Instance == null ? 1 : GameManager.Instance.GameSpeed);
    public int platformHitCount = 1;

    [SerializeField] float turnSpeed = 1;
    [SerializeField] float jumpHeigh = 5;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask platformLayer;
    [SerializeField] Transform jumpingPart;
    [SerializeField] ParticleSystem hitEffect;
    [Header("Deformation")]
    [SerializeField] float baseSize = 1f;
    [SerializeField] float scaleMultiplyer = 1f;

    float d; // distance between platforms
    float p; // player's normilized position between platforms
    float t; // time variable for the lerp
    float turnAmount = 0;
    float fallZPos;
    bool isTurning = false;
    bool canMove = true;
    Vector3 velocity;
    Vector3 lastPos;
    Vector3 playerOffset;
    Transform platformA, platformB;
    Rigidbody jumpingPartRB;
    Rigidbody _rb;
    Camera mainCamera;

    void Awake()
    {
        _rb = GetComponentInChildren<Rigidbody>();
        jumpingPartRB = jumpingPart.GetComponent<Rigidbody>();
        mainCamera = Camera.main;
        canMove = false;
        platformHitCount = 1;
    }

    void Update()
    {
        HandleInput();
        if (canMove)
        {
            Movement();
            Jumping();
            VelocityScale();
        }
    }

    private void VelocityScale()
    {
        velocity = jumpingPart.position - lastPos;
        lastPos = jumpingPart.position;

        if (velocity.magnitude != 0)
            jumpingPart.forward = velocity;
        float minSize = baseSize - velocity.magnitude * scaleMultiplyer;
        minSize = Mathf.Clamp(minSize, 0.4f, 2f);
        float maxSize = baseSize + velocity.magnitude * scaleMultiplyer;
        maxSize = Mathf.Clamp(maxSize, 0.4f, 3f);
        jumpingPart.localScale = new Vector3(minSize, minSize, maxSize);
    }

    void HandleInput()
    {
        var ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100, groundLayer))
        {
            if (Input.GetMouseButtonDown(0))
            {
                isTurning = true;
                playerOffset = jumpingPart.position - hit.point;
            }

            if (Input.GetMouseButtonUp(0))
                isTurning = false;

            if (isTurning)
                turnAmount = Mathf.Lerp(turnAmount, (hit.point.x * turnSpeed) + playerOffset.x, 0.2f);
        }
    }

    void Movement()
    {
        transform.position += Vector3.forward * Speed * Time.deltaTime;
    }

    void Jumping()
    {
        if (platformA == null)
        {
            platformA = LevelGenerator.Instance.GetSpecificPlatform(0);
            return;
        }

        if (platformB == null)
        {
            platformB = LevelGenerator.Instance.GetSpecificPlatform(1);
            return;
        }

        d = Vector3.Distance(platformA.position, platformB.position);
        p = (transform.position.z - platformA.position.z) / (platformB.position.z - platformA.position.z);
        p = Mathf.Clamp(p, 0f, 1f);
        t = Mathf.Abs(Mathf.Cos((p - 0.5f) * Mathf.PI));
        float posY = Mathf.Lerp(0f, 1f, t) * jumpHeigh * d * 0.1f;
        posY = Mathf.Clamp(posY, 0f, 10f);

        Vector3 newPos = new Vector3(turnAmount, posY, 0);

        jumpingPart.localPosition = newPos;

        if (p >= 1)
        {
            CheckPlatform();
        }
    }

    void CheckPlatform()
    {
        if (Physics.CheckSphere(jumpingPart.position, 0.5f, platformLayer))
        {
            if (platformB.name == "Start")
            {
                SoundManager.Instance.PlayMusicFromBeat(1);
                GameManager.Instance.IncreaseGameSpeed();
                platformHitCount = 0;
            }

            hitEffect.transform.position = platformB.position;
            hitEffect.Play();

            float distanceToCenter = Vector3.Distance(jumpingPart.position, platformB.position + Vector3.up * 0.3f);
            bool perfect = distanceToCenter < 0.6f;
            platformB.GetComponent<Platform>().Hit(perfect);
            platformA = platformB;
            platformB = LevelGenerator.Instance.GetNextPlatform;

            platformHitCount++;

            GameManager.Instance.AddScore(perfect);

        }
        else
        {
            if (canMove)
            {
                StopMoving();
                GameManager.Instance.PlayerFailed();
            }
        }
    }

    void StopMoving()
    {
        if (canMove)
        {
            canMove = false;
            jumpingPartRB.isKinematic = false;
            jumpingPartRB.velocity = jumpingPart.forward * Speed;
        }
    }

    public void Revive()
    {
        canMove = false;
        jumpingPartRB.isKinematic = true;

        Vector3 newPlayerPos = new Vector3(transform.position.x, transform.position.y, platformA.position.z);
        transform.position = newPlayerPos;

        turnAmount = 0;
        jumpingPart.localPosition = new Vector3(platformA.position.x, 0, 0);

        MovingLoop movement = platformA.gameObject.GetComponent<MovingLoop>();
        if (movement != null)
        {
            movement.enabled = false;
        }
    }

    public void StartMoving()
    {
        canMove = true;
    }

    public void StopAtPlatform(Transform platform)
    {
        canMove = false;
        transform.position = Vector3.forward * platform.position.z;
        SoundManager.Instance.StopTrack();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(jumpingPart.position, 0.5f);
    }
}
