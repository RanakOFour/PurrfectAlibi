using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    private Rigidbody m_rigidBody;
    private SpriteRenderer m_playerSprite;
    private Sprite[] m_sprites;
    private Vector3 m_velocityMask;

    // Start is called before the first frame update
    void Start()
    {
        m_rigidBody = GetComponent<Rigidbody>();
        m_playerSprite = gameObject.GetComponent<SpriteRenderer>();

        //playerSprite.sprite = sprites[0];
        m_velocityMask = new Vector3(0, 0, 0);
        m_rigidBody.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        //Get x and z values as 1, 0 or -1
        m_velocityMask.z = ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) ? 1 : 0) + ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) ? -1 : 0);
        m_velocityMask.x = ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) ? 1 : 0) + ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) ? -1 : 0);

        if(m_velocityMask != Vector3.zero)
        {
            m_rigidBody.AddForce(20 * m_velocityMask);
        }

        if(m_velocityMask.x == -1.0f)
        {
            m_playerSprite.flipX = true;
        }
        else if(m_velocityMask.x == 1.0f)
        {
            m_playerSprite.flipX = false;
        }
    }
}
