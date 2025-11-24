using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponDataList", menuName = "SO/Weapon/WeaponDataList", order = 0)]
public class WeaponDataList : ScriptableObject
{
    [SerializeField] private List<WeaponData> _weaponDatas;
    public List<WeaponData> WeaponDatas => _weaponDatas;
}