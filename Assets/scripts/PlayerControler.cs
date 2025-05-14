using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Mo�liwe stany gracza
    private enum PlayerState { Idle, Moving, Dash }
    private PlayerState playerState;

    // ===== Publiczne ustawienia =====
    public InputAction MoveAction;    // Akcja wej�cia z Input Systemu
    public int speed;                 // Pr�dko�� poruszania
    public int dash_value;           // Pr�dko�� lub warto�� dashu (niewykorzystana obecnie)
    public int dash_time;            // Czas trwania dashu
    public int dash_time_cooldown;   // Czas trwania cooldown dashu

    // ===== Prywatne komponenty =====
    private Rigidbody2D rigidbody2d;     // Komponent fizyki
    private SpriteRenderer m_SpriteRenderer; // Do odbicia sprite'a
    private Animator animation;          // Animator (s�owo "animation" nie jest zarezerwowane, ale lepiej go nie nadu�ywa�)
    private float dash;                  //Timer odliczaj�cy do ko�ca dash
    private float dash_cooldown;         //Timer odliczaj�cy do ko�ca cooldown dash

    // ===== Dane wej�ciowe =====
    private Vector2 move;    // Warto�� wej�cia z klawiatury / kontrolera

    void Start()
    {
        // W��czenie akcji ruchu
        MoveAction.Enable();

        // Pobranie komponent�w z obiektu gracza
        rigidbody2d = GetComponent<Rigidbody2D>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        animation = GetComponent<Animator>();
    }

    void Update()
    {
        // Odczyt wektora ruchu z Input Systemu
        move = MoveAction.ReadValue<Vector2>();
        Debug.Log(move);
            switch (playerState)
            {
                case PlayerState.Idle:
                    if (move.x < 0 || move.y < 0)
                    {
                    playerState = PlayerState.Moving; 
                    break;
                    }
                    break;

                case PlayerState.Moving:
                    
                    break;

                case PlayerState.Dash: 
                    break;
            }
    }

    void FixedUpdate()
    {
        // Oblicz now� pozycj� w zale�no�ci od ruchu, pr�dko�ci i deltaTime
        Vector2 position = (Vector2)rigidbody2d.position + move * speed * Time.fixedDeltaTime;

        // Przesu� gracza do nowej pozycji z u�yciem fizyki
        rigidbody2d.MovePosition(position);
    }
}