using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HP : Singleton<HP>
{
    public Image HP_Bar;
    private float hp = 1f;

    public bool Attacked = false;
    public bool Energized = false;

    // Update is called once per frame
    void Start(){
        HP_Bar.fillAmount += hp;
    }

    private void Reset()
    {
        HP_Bar.fillAmount += hp;
    }

    public void Attack()
    {
        Attacked = true;
    }
    
    public void Heal()
    {
        Energized = true;
    }
    
    void Update()
    {
        if(Attacked){
            hp -= 0.25f;
            HP_Bar.fillAmount = hp;
            Attacked = false;
        }
        if(Energized){
            hp += 0.05f;
            HP_Bar.fillAmount = hp;
            Energized = false;
        }

    }
}