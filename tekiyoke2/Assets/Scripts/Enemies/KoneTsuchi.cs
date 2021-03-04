using UnityEngine;

public class KoneTsuchi : MonoBehaviour
{
    [SerializeField] SimpleAnim anim;

    public void Init(bool toRight)
    {
        if (!toRight)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        anim.Play(() => Destroy(gameObject));
    }
}