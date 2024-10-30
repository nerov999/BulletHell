using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed = 5f;

    private Rigidbody2D rb;
    private Vector2 _velocity;
    private static bool canMove = true; // Статическая переменная для общего состояния движения всех врагов

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(StartMovingAfterDelay());
    }

    private IEnumerator StartMovingAfterDelay()
    {
        yield return new WaitForSeconds(1f); // Задержка в 1 секунду
        SetRandomDirection(); // Запуск движения после задержки
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Wall"))
        {
            _velocity = Vector2.Reflect(_velocity, collision.contacts[0].normal).normalized * speed;
        }
        if (collision.collider.CompareTag("Player"))
        {
            // Останавливаем все враги
            canMove = false;

            // Запускаем корутину для ожидания некоторого времени
            StartCoroutine(RestartSceneAfterDelay());
        }
    }

    private void SetRandomDirection()
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        _velocity = randomDirection * speed;
        transform.up = randomDirection;
    }

    private void Update()
    {
        if (canMove)
            rb.velocity = _velocity;
        else
            rb.velocity = Vector2.zero; // Останавливаем все враги
    }

    private IEnumerator RestartSceneAfterDelay()
    {
        yield return new WaitForSeconds(2f); // Подождать 2 секунды
        RestartScene(); // Перезапуск сцены
    }

    private void RestartScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);

        // Сбросить состояние движения всех врагов при перезапуске сцены
        canMove = true;
    }
}
