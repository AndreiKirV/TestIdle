using EzySlice;
using UnityEngine;

public class SliceObject : MonoBehaviour
{
    public Material materialSlicedSide;
    public float explosionForce;
    public float explosionRadius;
    public bool gravity, kinematic;

    private void OnTriggerEnter(Collider other) 
    {
        SlicedHull sliceObject = Slice(other.gameObject, materialSlicedSide);
        GameObject sliceObjectTop = sliceObject.CreateUpperHull(other.gameObject, materialSlicedSide);
        GameObject sliceObjectDown = sliceObject.CreateLowerHull(other.gameObject, materialSlicedSide);
        Destroy(other.gameObject);
    }

    private SlicedHull Slice(GameObject obj, Material material)
    {
        return obj.Slice(transform.position, transform.up);
    }
}