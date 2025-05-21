using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogSpawnerOnRiver : MonoBehaviour
{
    public GameObject[] logPrefabs;        // ���� ������ �볪�� ������ �迭
    public float spawnInterval = 3f;
    public float spawnYOffset = 0.5f;      // �볪���� �� ���� �� �ֵ���

    void Start()
    {
        StartCoroutine(SpawnLogsRoutine());
    }

    IEnumerator SpawnLogsRoutine()
    {
        while (true)
        {
            // ������ �̸��� "river"�� ���Ե� ��� ������Ʈ ã��
            GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
            List<GameObject> riverObjects = new List<GameObject>();

            foreach (var obj in allObjects)
            {
                if (obj.name.ToLower().Contains("river"))
                {
                    riverObjects.Add(obj);
                }
            }

            if (riverObjects.Count > 0 && logPrefabs.Length > 0)
            {
                // ������ river ������Ʈ ����
                GameObject randomRiver = riverObjects[Random.Range(0, riverObjects.Count)];

                // ������ �볪�� ������ ����
                GameObject randomLogPrefab = logPrefabs[Random.Range(0, logPrefabs.Length)];

                // ���� ���� ����
                bool moveRight = Random.value > 0.5f;

                // ���⿡ ���� X ��ġ ����
                float spawnX = moveRight ? -100f : 100f;

                Vector3 spawnPos = new Vector3(spawnX, randomRiver.transform.position.y + spawnYOffset, randomRiver.transform.position.z);

                // ���⿡ ���� ȸ�� ����
                Quaternion spawnRotation = moveRight ? Quaternion.Euler(0f, 180f, 0f) : Quaternion.identity;

                GameObject log = Instantiate(randomLogPrefab, spawnPos, spawnRotation);

                // �̵� ���� �� �ӵ� ����
                Log logScript = log.GetComponent<Log>();
                logScript.SetDirection(moveRight ? -1 : -1); // ������ 1 �Ǵ� -1

                float randomSpeed = Random.Range(10f, 20f);   // �볪���� ���� �� ����
                logScript.SetSpeed(randomSpeed);
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
