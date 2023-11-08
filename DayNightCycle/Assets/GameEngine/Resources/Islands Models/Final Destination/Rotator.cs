using UnityEngine;

public class Rotator : MonoBehaviour{
    [SerializeField] private float rotateSpeed;
    [SerializeField] private Vector3 rotateDirection;

    void Update(){
        transform.Rotate(rotateDirection * (rotateSpeed * Time.deltaTime));
    }
}
