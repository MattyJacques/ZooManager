using UnityEngine;

public class EditorOnly : MonoBehaviour
{
    private void Awake()
    {
        Debug.Log((object)("Removing " + this.name + " since it is editor only."));
        Object.Destroy((Object)this.gameObject);
    }
}