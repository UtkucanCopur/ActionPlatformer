using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "Stats/EnemyStats")]
public class EnemyStats : ScriptableObject
{
    public float MoveSpeed = 3f;
    public float DetectionRange = 5f;
}