using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawnerOnRoad : MonoBehaviour
{
    public GameObject carPrefab;
    public float spawnInterval = 3f;
    public float spawnYOffset = 0.5f;  // �ڵ����� ������ ���� �� �ְ�

    void Start()
    {
        StartCoroutine(SpawnCarsRoutine());
    }

    IEnumerator SpawnCarsRoutine()
    {
        while (true)
        {
            // ���� �����ϴ� ��� ������Ʈ �� �̸��� "road"�� ���Ե� �� ã��
            GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
            List<GameObject> roadObjects = new List<GameObject>();

            foreach (var obj in allObjects)
            {
                if (obj.name.ToLower().Contains("road"))
                {
                    roadObjects.Add(obj);
                }
            }

            if (roadObjects.Count > 0)
            {
                // road �߿� �������� ����
                GameObject randomRoad = roadObjects[Random.Range(0, roadObjects.Count)];

                // ���� ���� ���� (true�� ������, false�� ����)
                bool moveRight = Random.value > 0.5f;

                // ���⿡ ���� ���� X ��ġ ���� (���� �� �Ǵ� ������ ��)
                float spawnX = moveRight ? -100f : 100f;

                Vector3 spawnPos = new Vector3(spawnX, randomRoad.transform.position.y + spawnYOffset, randomRoad.transform.position.z);

                // ���⿡ ���� ȸ�� ����
                Quaternion spawnRotation = moveRight ? Quaternion.Euler(0f, 180f, 0f) : Quaternion.identity;

                GameObject car = Instantiate(carPrefab, spawnPos, spawnRotation);

                // ������ �ڵ����� ���� �� �ӵ� ����
                Car carScript = car.GetComponent<Car>();
                carScript.SetDirection(moveRight ? -1 : -1);

                float randomSpeed = Random.Range(10f, 30f);
                carScript.SetSpeed(randomSpeed);
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
