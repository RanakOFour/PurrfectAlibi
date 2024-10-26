using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    private Rigidbody rb;
    private SpriteRenderer playerSprite;
    private Sprite[] sprites;
    private Vector3 velocityMask;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerSprite = gameObject.GetComponent<SpriteRenderer>();

        //playerSprite.sprite = sprites[0];
        velocityMask = new Vector3(0, 0, 0);
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        //Get x and z values as 1, 0 or -1
        velocityMask.z = ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) ? 1 : 0) + ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) ? -1 : 0);
        velocityMask.x = ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) ? 1 : 0) + ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) ? -1 : 0);

        if(velocityMask != Vector3.zero)
        {
            rb.AddForce(15 * velocityMask);
        }

        if(Input.GetKeyDown(KeyCode.E))
        {
            playerSprite.sprite = sprites[0];
        }

        if(Input.GetKeyDown(KeyCode.F))
        {
            playerSprite.sprite = sprites[1];
        }

        if(velocityMask.x == -1.0f)
        {
            playerSprite.flipX = true;
        }
        else if(velocityMask.x == 1.0f)
        {
            playerSprite.flipX = false;
        }
    }
}
