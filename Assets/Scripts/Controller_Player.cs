using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Player : MonoBehaviour
{
    public float jumpForce = 10;
    public float speed = 5;
    public float iceSpeed = 5;  // Velocidad en el hielo
    public int playerNumber;

    public Rigidbody rb;
    private BoxCollider col;

    public LayerMask floor;

    internal RaycastHit leftHit, rightHit, downHit;
    public float distanceRay, downDistanceRay;

    private bool canMoveLeft, canMoveRight, canJump;
    internal bool onFloor;
    private Vector3 originalPosition;
    private bool isOnIce = false;  // Flag para saber si está en el hielo

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<BoxCollider>();
        rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        originalPosition = transform.position;
    }

    public virtual void FixedUpdate()
    {
        if (GameManager.actualPlayer == playerNumber)
        {
            Movement();
        }
    }

    private void Update()
    {
        if (GameManager.actualPlayer == playerNumber)
        {
            Jump();
            if (SomethingLeft())
            {
                canMoveLeft = false;
            }
            else
            {
                canMoveLeft = true;
            }
            if (SomethingRight())
            {
                canMoveRight = false;
            }
            else
            {
                canMoveRight = true;
            }

            if (IsOnSomething())
            {
                canJump = true;
            }
            else
            {
                canJump = false;
            }
        }
        else
        {
            if (onFloor)
            {
                rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
            }
            else
            {
                if (IsOnSomething())
                {
                    if (downHit.collider.gameObject.CompareTag("Player"))
                    {
                        rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
                    }
                }
            }
        }
    }

    public virtual bool IsOnSomething()
    {
        return Physics.BoxCast(transform.position, new Vector3(transform.localScale.x * 0.9f, transform.localScale.y / 3, transform.localScale.z * 0.9f), Vector3.down, out downHit, Quaternion.identity, downDistanceRay);
    }

    public virtual bool SomethingRight()
    {
        Ray landingRay = new Ray(new Vector3(transform.position.x, transform.position.y - (transform.localScale.y / 2.2f), transform.position.z), Vector3.right);
        Debug.DrawRay(landingRay.origin, landingRay.direction, Color.green);
        return Physics.Raycast(landingRay, out rightHit, transform.localScale.x / 1.8f);
    }

    public virtual bool SomethingLeft()
    {
        Ray landingRay = new Ray(new Vector3(transform.position.x, transform.position.y - (transform.localScale.y / 2.2f), transform.position.z), Vector3.left);
        Debug.DrawRay(landingRay.origin, landingRay.direction, Color.green);
        return Physics.Raycast(landingRay, out leftHit, transform.localScale.x / 1.8f);
    }

    private void Movement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float currentSpeed = isOnIce ? iceSpeed : speed;

        // Aplicar la velocidad basada en la entrada del teclado
        rb.velocity = new Vector3(horizontalInput * currentSpeed, rb.velocity.y, 0);

        // Verificar si se está presionando alguna tecla de movimiento y si el jugador puede moverse en esa dirección
        if (horizontalInput < 0 && !canMoveLeft)
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0); // Detener el movimiento hacia la izquierda si no puede moverse en esa dirección
        }
        else if (horizontalInput > 0 && !canMoveRight)
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0); // Detener el movimiento hacia la derecha si no puede moverse en esa dirección
        }
    }


    public virtual void Jump()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (canJump)
            {
                rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
            }
        }
    }

    public virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Water"))
        {
            Destroy(this.gameObject);
            GameManager.gameOver = true;
        }
        if (collision.gameObject.CompareTag("Floor"))
        {
            onFloor = true;
        }
        if (collision.gameObject.CompareTag("Enemigo"))
        {
            transform.position = originalPosition;
        }
        if (collision.gameObject.CompareTag("Ice"))
        {
            isOnIce = true;
            Debug.Log("On Ice: " + isOnIce);  // Depuración
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            onFloor = false;
        }
        if (collision.gameObject.CompareTag("Ice"))
        {
            isOnIce = false;
            Debug.Log("Off Ice: " + isOnIce);  // Depuración
        }
    }
}
