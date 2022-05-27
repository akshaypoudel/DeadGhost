using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Break_Ghost : MonoBehaviour
{
    public static bool Is_Breaked = false;
    public bool canPlayerBodyBreak = false;
    public GameObject ghost_normal;
    public GameObject ghost_Parts;
    public Animator ghost;
    int counter;
    // Start is called before the first frame update
    void Start()
    {
        ghost_normal.SetActive(true);
        ghost_Parts.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Is_Breaked && gameObject.tag == "Enemy")
        {
            Is_Breaked = false;
            ghost_Parts.SetActive(true);
            ghost_normal.SetActive(false);
            this.GetComponent<Enemy>().enabled = false;
        }
        if(canPlayerBodyBreak)
        {
            ghost_Parts.SetActive(true);
            ghost_normal.SetActive(false);
        }
    }
    public void break_Ghost()
    {
        Is_Breaked = true;
    }
    public void play_anim()
    {
        counter += 1;
        if(counter == 2)
        {
            counter = 0;
            ghost.Play("idle");
        }
        else
        {
            ghost.Play("attack");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Projectile")
        {
            Is_Breaked = true;
        }
    }
}
