using UnityEngine;

[CreateAssetMenu(menuName = "Environment/Physics")]
public class LevelPhysics : ScriptableObject
{
    [Header("Water Flow")]
    [SerializeField] float flowStrength = 8f;
    [SerializeField] Vector2 flowDireciton = new Vector2(0, -1);

    public Vector2 GetForces()
    {
        return flowDireciton * flowStrength;
    }
}