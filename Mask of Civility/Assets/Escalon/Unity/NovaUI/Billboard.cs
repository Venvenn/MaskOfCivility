using UnityEngine;

public class Billboard: MonoBehaviour
{
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(-(Camera.main.transform.position - transform.position), Camera.main.transform.up);
    }
}