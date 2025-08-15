using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Vector2[] path;
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    [SerializeField] private float pointReachThreshold;
    [SerializeField] private float jumpHeightThreshold;
    [SerializeField] private Transform groundChecker;
    [SerializeField] private LayerMask groundLayer;
    private int stage;
    private int dir;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        stage = 1;
        dir = 1;
    }

    void Update()
    {
        if (path == null || path.Length < 2 || stage >= path.Length)
        {
            return;
        }

        Vector2 direction = path[stage] - new Vector2(transform.position.x, transform.position.y);

        if (direction.magnitude < pointReachThreshold)
        {
            if (stage == 0 || stage == path.Length - 1)
            {
                dir = -dir;
            }
            stage += dir;
            return;
        }

        if (direction.y > jumpHeightThreshold && Physics2D.OverlapCircle(groundChecker.position, 0.45f, groundLayer))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
        }

        rb.velocity = new Vector2(Mathf.Sign(direction.x) * speed, rb.velocity.y);
    }
}