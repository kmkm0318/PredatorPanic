using UnityEngine;

public class DropItemVisual : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed = 50f;

    private void Update()
    {
        transform.Rotate(_rotateSpeed * Time.deltaTime * Vector3.up);
    }
}