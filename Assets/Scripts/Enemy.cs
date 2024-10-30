using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed = 5f;

    private Rigidbody2D rb;
    private Vector2 _velocity;
    private static bool canMove = true; // ����������� ���������� ��� ������ ��������� �������� ���� ������

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(StartMovingAfterDelay());
    }

    private IEnumerator StartMovingAfterDelay()
    {
        yield return new WaitForSeconds(1f); // �������� � 1 �������
        SetRandomDirection(); // ������ �������� ����� ��������
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Wall"))
        {
            _velocity = Vector2.Reflect(_velocity, collision.contacts[0].normal).normalized * speed;
        }
        if (collision.collider.CompareTag("Player"))
        {
            // ������������� ��� �����
            canMove = false;

            // ��������� �������� ��� �������� ���������� �������
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
            rb.velocity = Vector2.zero; // ������������� ��� �����
    }

    private IEnumerator RestartSceneAfterDelay()
    {
        yield return new WaitForSeconds(2f); // ��������� 2 �������
        RestartScene(); // ���������� �����
    }

    private void RestartScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);

        // �������� ��������� �������� ���� ������ ��� ����������� �����
        canMove = true;
    }
}
