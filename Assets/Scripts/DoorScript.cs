using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public int doorCol;
    [SerializeField] private Sprite[] doorSprites;

    void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        if (doorCol >= 0 && doorCol < doorSprites.Length)
        {
            sr.sprite = doorSprites[doorCol];
        }
    }
}
