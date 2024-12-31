﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour{

	public Slider slider;

	public void SetMaxHealth(int health)
	{
		slider.maxValue = health;
		slider.value = health;

	}

    public void SetHealth(int health)
	{		slider.value = health;

	}
	
    public void addHealth(int health)
	{		slider.value += health;

	}
    public void subHealth(int health)
	{		slider.value -= health;

	}
	public float getHealth(){
		return slider.value;
	}
	
	void Start(){
		SetHealth(80);
	}

}