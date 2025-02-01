using UnityEngine;

public enum WireType
{
    Red,
    Blue,
    White,
    Yellow,
    Green
}
public class Wire : MonoBehaviour
{
    [SerializeField] private WireType wireType;

    public WireType WireType => wireType;
}
