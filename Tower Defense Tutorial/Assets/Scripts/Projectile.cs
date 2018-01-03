﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	private Monster target;

	private Tower parent;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		MoveToTarget();	
	}

	public void Initialize(Tower parent)
	{
		this.parent = parent;
		this.target = parent.Target;
	}

	private void MoveToTarget()
	{
		if (target != null && target.IsActive)
		{
			transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * parent.ProjectileSpeed);
		}
		else if (!target.IsActive)
		{
			GameManager.Instance.Pool.ReleaseObject(gameObject);
		}
	}
}
