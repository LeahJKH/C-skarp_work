using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform GroundCheckTransform = null;
    [SerializeField] private LayerMask playermask;

    private bool JumpKeyWasPressed;
    private float HorizontalImput;
    private Rigidbody RigidBodyComponent;
    private int SuperJumpsRemaining;
    private float SprintKeyWasPressed;

    // Start is called before the first frame update
    void Start()
    {
        RigidBodyComponent = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // check if space key is pressed down for jump
        if (Input.GetKeyDown(KeyCode.Space) == true)
        {
            JumpKeyWasPressed = true;
        }

        HorizontalImput = Input.GetAxis("Horizontal");
    }
    // fixed update is called once every pychics update
    private void FixedUpdate()
    {
        RigidBodyComponent.velocity = new Vector3(HorizontalImput, RigidBodyComponent.velocity.y, 0);

        if (Physics.OverlapSphere(GroundCheckTransform.position, 0.1f, playermask).Length == 0)
        {
            return;
        }

        if (JumpKeyWasPressed == true)
        {
            float JumpPower = 5f;
            if (SuperJumpsRemaining > 0)
            {
                JumpPower *= 2;
                SuperJumpsRemaining--;
            }
            RigidBodyComponent.AddForce(Vector3.up * JumpPower, ForceMode.VelocityChange);
            JumpKeyWasPressed = false;
            //made u double jump
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            Destroy(other.gameObject);
            SuperJumpsRemaining += 1;
        }

    }

}