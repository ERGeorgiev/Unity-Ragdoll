using UnityEngine;

public class MonoBehaviourExt : MonoBehaviour
{
    public RagdollController RagdollController
    {
        get { return GetComponentInParent<RagdollController>(); }
    }
    public static RagdollController GetRagdollController(Transform trans)
    {
        return trans.GetComponentInParent<RagdollController>();
    }
    public MouseController MouseController
    {
        get { return GetComponentInParent<MouseController>(); }
    }
    public static MouseController GetMouseController(Transform trans)
    {
        return trans.GetComponentInParent<MouseController>();
    }
    public ObjectController ObjectController
    {
        get { return GetComponentInParent<ObjectController>(); }
    }
    public static ObjectController GetObjectController(Transform trans)
    {
        return trans.GetComponentInParent<ObjectController>();
    }
    public InventoryController InventoryController
    {
        get { return GetComponentInParent<InventoryController>(); }
    }
    public static InventoryController GetInventoryController(Transform trans)
    {
        return trans.GetComponentInParent<InventoryController>();
    }
}
