using UnityEngine;


[CreateAssetMenu(fileName = "PlayerStats", menuName ="Stats/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    public float MoveSpeed = 5f;
    public float JumpForce = 10f;
}
