using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogSpawnerOnRiver : MonoBehaviour
{
    public GameObject[] logPrefabs;
    public float spawnYOffset = 0.5f;

    // �� ������Ʈ ����
    private List<GameObject> riverObjects = new List<GameObject>();
    private HashSet<GameObject> registeredRivers = new HashSet<GameObject>();

    // �볪�� ����
    private List<GameObject> activeLogs = new List<GameObject>();

    // ���� ���� ���� �ð� �� ���� ����
    private Dictionary<GameObject, float> nextSpawnTime = new Dictionary<GameObject, float>();
    private Dictionary<GameObject, bool> riverDirections = new Dictionary<GameObject, bool>();

    // �볪���� ������ ������� �� ���� �Ӱ谪(X��ǥ)
    public float destroyXLimit = 120f;

    void Start()
    {
        UpdateRiverObjects();
    }

    void Update()
    {
        UpdateRiverObjects();
        UpdateSpawning();
        CleanupLogs();
    }

    // �� �� "River" �±װ� ���� �� ������Ʈ ����
    void UpdateRiverObjects()
    {
        GameObject[] rivers = GameObject.FindGameObjectsWithTag("River");
        foreach (var river in rivers)
        {
            if (!registeredRivers.Contains(river))
            {
                riverObjects.Add(river);
                registeredRivers.Add(river);

                bool direction = Random.value > 0.5f;
                riverDirections[river] = direction;

                nextSpawnTime[river] = Time.time + Random.Range(1f, 5f);
            }
        }
    }

    void UpdateSpawning()
    {
        // null ���� �볪�� ����
        activeLogs.RemoveAll(log => log == null);

        foreach (GameObject river in riverObjects)
        {
            if (!nextSpawnTime.ContainsKey(river)) continue;
            if (Time.time < nextSpawnTime[river]) continue;

            // ���� Z�� ��ó�� �̹� �볪���� �����ϴ��� üũ
            bool hasLogOnThisRiver = false;
            foreach (GameObject existingLog in activeLogs)
            {
                if (existingLog == null) continue;

                float zDist = Mathf.Abs(existingLog.transform.position.z - river.transform.position.z);
                if (zDist < 0.5f)
                {
                    hasLogOnThisRiver = true;
                    break;
                }
            }
            if (hasLogOnThisRiver) continue;

            bool moveRight = riverDirections.ContainsKey(river) ? riverDirections[river] : true;

            float spawnX = moveRight ? -100f : 100f;
            float spawnY = river.transform.position.y + spawnYOffset;
            float spawnZ = river.transform.position.z;

            Vector3 spawnPos = new Vector3(spawnX, spawnY, spawnZ);
            Quaternion rotation = moveRight ? Quaternion.identity : Quaternion.Euler(0f, 180f, 0f);

            GameObject randomLogPrefab = logPrefabs[Random.Range(0, logPrefabs.Length)];
            GameObject newLog = Instantiate(randomLogPrefab, spawnPos, rotation);
            activeLogs.Add(newLog);

            Log logScript = newLog.GetComponent<Log>();
            if (logScript != null)
            {
                int dir = moveRight ? 1 : 1;
                logScript.SetDirection(dir);
                logScript.SetSpeed(Random.Range(10f, 20f));
            }
            else
            {
                Debug.LogWarning("Log prefab�� Log ��ũ��Ʈ�� ������� �ʾҽ��ϴ�.");
            }

            nextSpawnTime[river] = Time.time + Random.Range(2f, 6f);
        }
    }

    // �� ������ ���� �볪�� ���� �� ����Ʈ ����
    void CleanupLogs()
    {
        // �ڿ������� �����ϴ°� ����
        for (int i = activeLogs.Count - 1; i >= 0; i--)
        {
            GameObject log = activeLogs[i];
            if (log == null)
            {
                activeLogs.RemoveAt(i);
                continue;
            }

            float posX = log.transform.position.x;
            if (Mathf.Abs(posX) > destroyXLimit)
            {
                Destroy(log);
                activeLogs.RemoveAt(i);
            }
        }
    }
}
