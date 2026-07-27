using System.Linq.Expressions;
using UnityEngine;

public sealed class SpawnInfo
{
    public static readonly SpawnInfo Shanghai = new SpawnInfo(new Vector3(70, -3, 375), Quaternion.Euler(new Vector3(0, 78, 0)));
    public static readonly SpawnInfo Spa = new SpawnInfo(new Vector3(265, 20, -650), Quaternion.Euler(new Vector3(0, 147, 0)));
    public static readonly SpawnInfo Suzuka = new SpawnInfo(new Vector3(-505, -2.25f, 395), Quaternion.Euler(new Vector3(0,0,0)));

    public Vector3 position;
    public Quaternion rotation;
    
    private SpawnInfo(Vector3 _position, Quaternion _rotation)
    {
        position = _position;
        rotation = _rotation;
    }
}

