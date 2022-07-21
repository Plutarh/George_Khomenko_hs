using UnityEngine;

public class CharacterInstaller : MonoBehaviour
{
    [SerializeField] private Character _characterPrefab;

    public Character characterInstance;

    public void Initialize()
    {
        SpawnCharacter(PointsHolder.BasePoint.transform);
    }

    void SpawnCharacter(Transform spawnPoint)
    {
        Vector3 spawnPosition = spawnPoint.position + new Vector3(0, _characterPrefab.transform.localScale.y, 0);
        characterInstance = Instantiate(_characterPrefab, spawnPosition, spawnPoint.rotation);
    }
}

