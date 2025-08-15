using UnityEngine;

public class KeyScript : MonoBehaviour
{
    public int keyCol;
    [SerializeField] private float speed;
    [SerializeField] private float pointReachThreshold;
    [SerializeField] private Vector2[] path;
    [SerializeField] private Sprite[] keySprites;

    private Rigidbody2D rb;
    private int stage;
    private int dir;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        stage = 1;
        dir = 1;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        if (keyCol >= 0 && keyCol < keySprites.Length)
        {
            sr.sprite = keySprites[keyCol];
        }
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

        rb.velocity = new Vector2(Mathf.Sign(direction.x) * speed, Mathf.Sign(direction.y) * speed);
    }
}