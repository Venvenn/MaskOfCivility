
using UnityEngine;

public class BillboardLight : MonoBehaviour
{
    private Camera _camera;

    public void Start()
    {
        _camera = Camera.main;
    }
    
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(-(_camera.transform.position - transform.position), _camera.transform.up);
    }
}