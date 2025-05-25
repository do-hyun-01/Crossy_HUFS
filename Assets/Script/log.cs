using UnityEngine;

public class Log : MonoBehaviour
{
    public float speed = 5f;
    public float lifetime = 15f;  // �볪���� �ڵ������� ���� ������� �� ����
    private float timer = 0f;
    private int direction = 1;

    private GameObject road;  // �볪���� ������ ���� ���� �����

    public void SetDirection(int dir)
    {
        direction = dir;
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    void Update()
    {
        transform.Translate(Vector3.right * direction * speed * Time.deltaTime);
        timer += Time.deltaTime;
        // lifetime ����ص� ���� �ı����� ����
    }

    public bool IsExpired()
    {
        return timer >= lifetime;
    }

    public void SetRoad(GameObject roadObj)
    {
        road = roadObj;
    }

    public GameObject GetRoad()
    {
        return road;
    }
}
