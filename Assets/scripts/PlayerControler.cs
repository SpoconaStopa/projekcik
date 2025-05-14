using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Mo¿liwe stany gracza
    private enum PlayerState { Idle, Moving, Dash }
    private PlayerState playerState;

    // ===== Publiczne ustawienia =====
    public InputAction MoveAction;    // Akcja wejœcia z Input Systemu
    public int speed;                 // Prêdkoœæ poruszania
    public int dash_value;           // Prêdkoœæ lub wartoœæ dashu (niewykorzystana obecnie)
    public int dash_time;            // Czas trwania dashu
    public int dash_time_cooldown;   // Czas trwania cooldown dashu

    // ===== Prywatne komponenty =====
    private Rigidbody2D rigidbody2d;     // Komponent fizyki
    private SpriteRenderer m_SpriteRenderer; // Do odbicia sprite'a
    private Animator animation;          // Animator (s³owo "animation" nie jest zarezerwowane, ale lepiej go nie nadu¿ywaæ)
    private float dash;                  //Timer odliczaj¹cy do koñca dash
    private float dash_cooldown;         //Timer odliczaj¹cy do koñca cooldown dash

    // ===== Dane wejœciowe =====
    private Vector2 move;    // Wartoœæ wejœcia z klawiatury / kontrolera

    void Start()
    {
        // W³¹czenie akcji ruchu
        MoveAction.Enable();

        // Pobranie komponentów z obiektu gracza
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
        // Oblicz now¹ pozycjê w zale¿noœci od ruchu, prêdkoœci i deltaTime
        Vector2 position = (Vector2)rigidbody2d.position + move * speed * Time.fixedDeltaTime;

        // Przesuñ gracza do nowej pozycji z u¿yciem fizyki
        rigidbody2d.MovePosition(position);
    }
}