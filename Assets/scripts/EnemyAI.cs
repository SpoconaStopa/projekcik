using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    // === Stan wroga ===
    private enum EnemyState { Idle, Moving, Charging, Turbo }
    private EnemyState currentState = EnemyState.Moving;

    // === Ustawienia ruchu i �adowania ===
    [Header("Ustawienia")]
    public float speed = 1f;                   // Pr�dko�� ruchu
    public float chargingDistance = 5f;        // Minimalna odleg�o�� do rozpocz�cia �adowania
    public float stoppingDistance = 2f;        // Odleg�o��, przy kt�rej wr�g si� zatrzymuje
    public float chargeTime = 2f;              // Czas trwania �adowania
    public float chargeCooldownTime = 5f;      // Czas odnowienia �adowania
    public float overshootDistance = 2f;              // Omini�cie pozycji gracza o...

    // === Zmienne wewn�trzne (tylko do podgl�du/debugowania) ===
    [Header("Zmienne pogl�dowe (nie edytuj)")]
    [SerializeField] private Vector3 rotate;          // Pr�dko�� obrotu (Z-axis)
    [SerializeField] private Transform target;        // Transform gracza
    [SerializeField] private Vector3 chargeTarget;    // Miejsce, do kt�rego wr�g dashuje
    [SerializeField] private float charge = 0f;       // Timer �adowania
    [SerializeField] private float chargeCooldown = 0f; // Timer odnowienia �adowania
    [SerializeField] private float turbo = 0f;        // Timer turbo (dash)
    [SerializeField] private float rotationSpeed = 1f;// Pr�dko�� obrotu
    [SerializeField] private bool targetPositionCaptured = false; // Czy pozycja gracza zosta�a zapisana

    void Start()
    {
        // Ustawienie domy�lnej warto�ci obrotu
        rotate.z = 100;

        // Znalezienie gracza, je�li nie zosta� przypisany
        if (target == null)
        {
            target = GameObject.FindWithTag("Player").transform;
        }
    }

    void Update()
    {
        // Obroty
        transform.Rotate(rotate * Time.deltaTime * rotationSpeed);

        // Je�li gracz jest poza zasi�giem zatrzymania
        if (Vector2.Distance(transform.position, target.position) > stoppingDistance)
        {
            switch (currentState)
            {
                case EnemyState.Idle:
                    // W tym stanie wr�g nie robi nic (np. dla przysz�ych animacji)
                    break;

                case EnemyState.Moving:
                    // Zwyk�y ruch w stron� gracza
                    speed = 1f;
                    rotationSpeed = 1f;
                    targetPositionCaptured = false;

                    transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

                    // Przej�cie do stanu �adowania, je�li warunki s� spe�nione
                    if (Vector2.Distance(transform.position, target.position) > chargingDistance && chargeCooldown <= 0f)
                    {
                        currentState = EnemyState.Charging;
                        charge = chargeTime;
                        turbo = 0f;
                    }
                    break;

                case EnemyState.Charging:
                    // Stan �adowania przed turbo
                    speed = 20f;
                    charge -= Time.deltaTime;
                    turbo += 0.35f * Time.deltaTime;
                    rotationSpeed = 10f;

                    // Po zako�czeniu �adowania przej�cie do turbo
                    if (charge <= 0f)
                    {
                        currentState = EnemyState.Turbo;
                    }
                    break;

                case EnemyState.Turbo:
                    // Przechowywanie pozycji gracza na koniec �adowania (tylko raz)
                    if (!targetPositionCaptured)
                    {
                        // Oblicz kierunek od przeciwnika do gracza
                        Vector3 direction = (target.position - transform.position).normalized;

                        // Ustaw cel dasha troch� dalej ni� by�a pozycja gracza 
                        chargeTarget = target.position + direction * overshootDistance;

                        targetPositionCaptured = true;
                    }

                    // Dash w stron� zapisanej pozycji
                    transform.position = Vector2.MoveTowards(transform.position, chargeTarget, speed * Time.deltaTime);
                    turbo -= Time.deltaTime;
                    rotationSpeed = 5f;

                    // Zako�czenie turbo i powr�t do ruchu
                    if (turbo <= 0f)
                    {
                        currentState = EnemyState.Moving;
                        chargeCooldown = chargeCooldownTime;
                    }
                    break;
            }
        }

        // Odliczanie cooldownu �adowania
        if (chargeCooldown > 0f)
        {
            chargeCooldown -= Time.deltaTime;
        }
    }
}
