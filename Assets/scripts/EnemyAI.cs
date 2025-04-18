using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    // === Stan wroga ===
    private enum EnemyState { Idle, Moving, Charging, Turbo }
    private EnemyState currentState = EnemyState.Moving;

    // === Ustawienia ruchu i ³adowania ===
    [Header("Ustawienia")]
    public float speed = 1f;                   // Prêdkoœæ ruchu
    public float chargingDistance = 5f;        // Minimalna odleg³oœæ do rozpoczêcia ³adowania
    public float stoppingDistance = 2f;        // Odleg³oœæ, przy której wróg siê zatrzymuje
    public float chargeTime = 2f;              // Czas trwania ³adowania
    public float chargeCooldownTime = 5f;      // Czas odnowienia ³adowania
    public float overshootDistance = 2f;              // Ominiêcie pozycji gracza o...

    // === Zmienne wewnêtrzne (tylko do podgl¹du/debugowania) ===
    [Header("Zmienne pogl¹dowe (nie edytuj)")]
    [SerializeField] private Vector3 rotate;          // Prêdkoœæ obrotu (Z-axis)
    [SerializeField] private Transform target;        // Transform gracza
    [SerializeField] private Vector3 chargeTarget;    // Miejsce, do którego wróg dashuje
    [SerializeField] private float charge = 0f;       // Timer ³adowania
    [SerializeField] private float chargeCooldown = 0f; // Timer odnowienia ³adowania
    [SerializeField] private float turbo = 0f;        // Timer turbo (dash)
    [SerializeField] private float rotationSpeed = 1f;// Prêdkoœæ obrotu
    [SerializeField] private bool targetPositionCaptured = false; // Czy pozycja gracza zosta³a zapisana

    void Start()
    {
        // Ustawienie domyœlnej wartoœci obrotu
        rotate.z = 100;

        // Znalezienie gracza, jeœli nie zosta³ przypisany
        if (target == null)
        {
            target = GameObject.FindWithTag("Player").transform;
        }
    }

    void Update()
    {
        // Obroty
        transform.Rotate(rotate * Time.deltaTime * rotationSpeed);

        // Jeœli gracz jest poza zasiêgiem zatrzymania
        if (Vector2.Distance(transform.position, target.position) > stoppingDistance)
        {
            switch (currentState)
            {
                case EnemyState.Idle:
                    // W tym stanie wróg nie robi nic (np. dla przysz³ych animacji)
                    break;

                case EnemyState.Moving:
                    // Zwyk³y ruch w stronê gracza
                    speed = 1f;
                    rotationSpeed = 1f;
                    targetPositionCaptured = false;

                    transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

                    // Przejœcie do stanu ³adowania, jeœli warunki s¹ spe³nione
                    if (Vector2.Distance(transform.position, target.position) > chargingDistance && chargeCooldown <= 0f)
                    {
                        currentState = EnemyState.Charging;
                        charge = chargeTime;
                        turbo = 0f;
                    }
                    break;

                case EnemyState.Charging:
                    // Stan ³adowania przed turbo
                    speed = 20f;
                    charge -= Time.deltaTime;
                    turbo += 0.35f * Time.deltaTime;
                    rotationSpeed = 10f;

                    // Po zakoñczeniu ³adowania przejœcie do turbo
                    if (charge <= 0f)
                    {
                        currentState = EnemyState.Turbo;
                    }
                    break;

                case EnemyState.Turbo:
                    // Przechowywanie pozycji gracza na koniec ³adowania (tylko raz)
                    if (!targetPositionCaptured)
                    {
                        // Oblicz kierunek od przeciwnika do gracza
                        Vector3 direction = (target.position - transform.position).normalized;

                        // Ustaw cel dasha trochê dalej ni¿ by³a pozycja gracza 
                        chargeTarget = target.position + direction * overshootDistance;

                        targetPositionCaptured = true;
                    }

                    // Dash w stronê zapisanej pozycji
                    transform.position = Vector2.MoveTowards(transform.position, chargeTarget, speed * Time.deltaTime);
                    turbo -= Time.deltaTime;
                    rotationSpeed = 5f;

                    // Zakoñczenie turbo i powrót do ruchu
                    if (turbo <= 0f)
                    {
                        currentState = EnemyState.Moving;
                        chargeCooldown = chargeCooldownTime;
                    }
                    break;
            }
        }

        // Odliczanie cooldownu ³adowania
        if (chargeCooldown > 0f)
        {
            chargeCooldown -= Time.deltaTime;
        }
    }
}
