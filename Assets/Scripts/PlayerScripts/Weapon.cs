using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Animator anim;
    private PlayerController player;

    private void Awake()
    {
        player = GetComponentInParent<PlayerController>();
    }

    public void Swing()
    {
        anim.SetTrigger("Swing");
    }

    public void DoneSwinging()
    {
        anim.ResetTrigger("Swing");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Baddie")
        {
            collision.GetComponent<BaddieController>().TakeDamage(player.WeaponDamage);
        }
    }
}
