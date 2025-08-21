using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform wallChecker;
    [SerializeField] private Transform groundChecker;

    [SerializeField] private float maxSpeed;
    [SerializeField] private float accel;
    [SerializeField] private float jumpPower;
    [SerializeField] private float walledSpeed;

    private Collider2D grounded;
    private Collider2D walled;
    private int face;
    public int[] keys;
    public int[] powers;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        grounded = null;
        walled = null;
        face = 1;
    }

    void Update()
    {
        walled = Physics2D.OverlapBox(wallChecker.position, new Vector2(0.2f, 1f), 0, groundLayer);
        grounded = Physics2D.OverlapBox(groundChecker.position, new Vector2(1f, 0.2f), 0, groundLayer);

        bool up = Input.GetKeyDown("w") || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow);
        if (up && grounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            grounded = null;
        }

        else if (up && walled)
        {
            rb.velocity = new Vector2(maxSpeed * -face, jumpPower);
            walled = null;
        }
        else if (up && powers[0] > 1)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            powers[0]--;
        }

        if (rb.velocity.x != 0)
        {
            rb.transform.localScale = new Vector3(Mathf.Sign(rb.velocity.x), rb.transform.localScale.y, rb.transform.localScale.z);
        }
    }

    void FixedUpdate()
    {
        string tag = null;
        if (grounded)
        {
            tag = grounded.tag;
        }
        else if (walled)
        {
            tag = walled.tag;
        }

        switch (tag)
        {
            case null:
                break;
            case "Ice":
                maxSpeed = 20f;
                accel = 1f;
                jumpPower = 10f;
                break;
            case "Mud":
                maxSpeed = 5f;
                accel = 2f;
                jumpPower = 5f;
                break;
            default:
                maxSpeed = 10f;
                accel = 2f;
                jumpPower = 10f;
                break;
        }

        float hori = Input.GetAxisRaw("Horizontal");

        if (walled && hori != 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -walledSpeed, float.MaxValue));
        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
        }

        if (hori != 0)
        {
            if (Mathf.Abs(rb.velocity.x) <= maxSpeed)
            {
                rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x + Mathf.Sign(hori) * accel, -maxSpeed, maxSpeed), rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(rb.velocity.x - Mathf.Sign(rb.velocity.x) * accel, rb.velocity.y);
            }
        }
        else if (rb.velocity.x != 0)
        {
            if (Mathf.Abs(rb.velocity.x) <= accel)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(rb.velocity.x - Mathf.Sign(rb.velocity.x) * accel, rb.velocity.y);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        tag = col.transform.tag;
        if (tag == "Door")
        {
            int color = col.gameObject.GetComponent<DoorScript>().doorCol;
            if (keys[color] > 0)
            {
                keys[color] -= 1;
                Destroy(col.gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Key"))
        {
            keys[col.GetComponent<KeyScript>().keyCol] += 1;
            Destroy(col.gameObject);
        }
    }
}