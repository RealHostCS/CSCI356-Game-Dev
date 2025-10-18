using UnityEngine;

public class Pot : MonoBehaviour
{

    public Animator anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void DestroyPot()
    {
        Debug.Log("You Hit The Pot Good Job");
        anim.SetBool("IsHit", true);
        Destroy(gameObject, 0.5f);
        
    }
}
