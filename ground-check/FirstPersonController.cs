using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstPersonController : MonoBehaviour {

    [Range (10f, 100f)]
    public float mouseSensitivity = 70f;

    public float jumpForce = 5f;
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    float moveSpeed;
    public float distToGround = 1f;

    public Text debug;

    float horizontal;
    float vertical;
    float mouseX;
    float mouseY;
    Quaternion deltaRotation;
    Vector3 deltaPosition;
    bool _jump;
    float _slopeAngle;

    Rigidbody rbody;
    Transform _cameraTransform;

    private void Start() {

        rbody = GetComponent<Rigidbody>();
        _cameraTransform = Camera.main.transform;
        moveSpeed = walkSpeed;

    }

    private void Update() {

        GetInputs();

        _cameraTransform.Rotate(-Vector3.right * mouseY * mouseSensitivity * Time.deltaTime);

        
            
        
    }

    private void FixedUpdate() {

        deltaRotation = Quaternion.Euler(Vector3.up * mouseX * mouseSensitivity * Time.fixedDeltaTime);
        rbody.MoveRotation(rbody.rotation * deltaRotation);
        deltaPosition = ((transform.forward * vertical) + (transform.right * horizontal)) * moveSpeed * Time.fixedDeltaTime;

        float normalisedSlope = (_slopeAngle / 90f) * -1f;
        deltaPosition += (deltaPosition * normalisedSlope);
        
        rbody.MovePosition(rbody.position + deltaPosition);

        if (_jump) {
            rbody.velocity += (Vector3.up * jumpForce);
            _jump = false;
        }

        GroundCheck();

    }

    void GetInputs() {

        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        if (Input.GetButtonDown("Sprint"))
            moveSpeed = runSpeed;
        if (Input.GetButtonUp("Sprint"))
            moveSpeed = walkSpeed;

        if (isGrounded && Input.GetButtonDown("Jump")) _jump = true;

    }

    public bool isGrounded = false;

    void GroundCheck () {

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, distToGround + 0.1f)) {
            _slopeAngle = (Vector3.Angle(hit.normal, transform.forward) - 90);
            debug.text = "Grounded on " + hit.transform.name;
            debug.text += "\nSlope Angle: " + _slopeAngle.ToString("N0") + "°";
            isGrounded = true;
        } else {
            debug.text = "Not Grounded";
            isGrounded = false;
        }


    }

}
