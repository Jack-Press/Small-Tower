using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField] private Transform focus;
    [SerializeField] private float camSpeed;
    [SerializeField] private Vector2 xLimit;
    [SerializeField] private Vector2 yLimit;
    [SerializeField] private float xDead;
    [SerializeField] private float yDead;


    void Start()
    {
        xLimit = new Vector2(0, 0);
        yLimit = new Vector2(0, 100);
    }

    void Update()
    {
        Vector3 currentPos = transform.position;
        Vector3 targetPos = currentPos;

        float xOffset = focus.position.x - currentPos.x;
        float yOffset = focus.position.y - currentPos.y;

        if (Mathf.Abs(xOffset) > xDead)
        {
            float directionX = Mathf.Sign(xOffset);
            targetPos.x += directionX * (Mathf.Abs(xOffset) - xDead);
        }

        if (Mathf.Abs(yOffset) > yDead)
        {
            float directionY = Mathf.Sign(yOffset);
            targetPos.y += directionY * (Mathf.Abs(yOffset) - yDead);
        }

        targetPos = Vector3.Lerp(currentPos, targetPos, camSpeed * Time.deltaTime);
        targetPos.x = Mathf.Clamp(targetPos.x, xLimit.x, xLimit.y);
        targetPos.y = Mathf.Clamp(targetPos.y, yLimit.x, yLimit.y);
        targetPos.z = currentPos.z;

        transform.position = targetPos;
    }
}
