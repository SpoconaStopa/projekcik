using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    // Zmienne wykorzystywane do ruchu
    public InputAction MoveAction; 
    public int speed;
    Rigidbody2D rigidbody2d;
    Vector2 move;
    SpriteRenderer m_SpriteRenderer;

    void Start()
    {
        MoveAction.Enable();
        rigidbody2d = GetComponent<Rigidbody2D>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        move = MoveAction.ReadValue<Vector2>();
        Debug.Log(move);
        if(move.x < 0)
        {
            m_SpriteRenderer.flipX = true;
        }
        if (move.x > 0)
        {
            m_SpriteRenderer.flipX = false;
        }

    }

    void FixedUpdate()
    {
        Vector2 position = (Vector2)rigidbody2d.position + move * speed * Time.fixedDeltaTime;
        rigidbody2d.MovePosition(position);
    }
}
