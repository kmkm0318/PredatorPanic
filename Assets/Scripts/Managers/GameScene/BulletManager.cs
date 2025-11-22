using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BulletManager : MonoBehaviour
{
    private Dictionary<Bullet, ObjectPool<Bullet>> _bullets = new();
}