﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarDebugger : MonoBehaviour
{
	private TileScript start, goal;

	[SerializeField]
	private Sprite blankTile;

	[SerializeField]
	private GameObject arrowPrefab;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		ClickTile();

		if (Input.GetKeyDown(KeyCode.Space))
		{
			AStar.GetPath(start.GridPosition);
		}
	}

	private void ClickTile()
	{
		//If the right mouse button is pressed...
		if (Input.GetMouseButtonDown(1))
		{
			//...shoot a ray from the mouse...
			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
			//...and if you hit something...
			if (hit.collider !=null)
			{
				//...store what you hit if it has a TileScript
				TileScript tmp = hit.collider.GetComponent<TileScript>();

				//...if it was a TileScript on that object...
				if (tmp !=null)
				{
					//...if that is no start point set...
					if (start == null)
					{
						//...make this the start...
						start = tmp;
						//Change the sprite to a blank Tile
						start.SpriteRender.sprite = blankTile;
						//Change the sprite color to green
						start.SpriteRender.color = Color.green;
						start.Debugging = true;
					}
					//...if there was a start, but no goal...
					else if (goal == null)
					{
						//..make this the goal
						goal = tmp;
						//Change the sprite to a blank Tile
						goal.SpriteRender.sprite = blankTile;
						//Change the sprite color to red
						goal.SpriteRender.color = new Color32(255, 0, 0, 255);
						goal.Debugging = true;
					}
				}
			}
		}
	}

	public void DebugPath(HashSet<Node> openList)
	{
		foreach (Node node in openList)
		{
			if (node.TileRef !=start)
			{
				node.TileRef.SpriteRender.color = Color.cyan;
				node.TileRef.SpriteRender.sprite = blankTile;
			}

			PointToParent(node, node.TileRef.WorldPosition);
		}
	}

	private void PointToParent(Node node, Vector2 position)
	{
		if (node.Parent !=null)
		{
			GameObject arrow = (GameObject)Instantiate(arrowPrefab, position, Quaternion.identity);
			//Right
			if ((node.GridPosition.X < node.Parent.GridPosition.X) && (node.GridPosition.Y == node.Parent.GridPosition.Y))
			{
				arrow.transform.eulerAngles = new Vector3(0, 0, 0);
			}
			//Top right
			else if ((node.GridPosition.X < node.Parent.GridPosition.X) && (node.GridPosition.Y > node.Parent.GridPosition.Y))
			{
				arrow.transform.eulerAngles = new Vector3(0, 0, 45);
			}
			//Up
			else if ((node.GridPosition.X == node.Parent.GridPosition.X) && (node.GridPosition.Y > node.Parent.GridPosition.Y))
			{
				arrow.transform.eulerAngles = new Vector3(0, 0, 90);
			}
			//Top left
			else if ((node.GridPosition.X > node.Parent.GridPosition.X) && (node.GridPosition.Y > node.Parent.GridPosition.Y))
			{
				arrow.transform.eulerAngles = new Vector3(0, 0, 135);
			}
			//Left
			else if ((node.GridPosition.X > node.Parent.GridPosition.X) && (node.GridPosition.Y == node.Parent.GridPosition.Y))
			{
				arrow.transform.eulerAngles = new Vector3(0, 0, 180);
			}
			//Bottom left
			else if ((node.GridPosition.X > node.Parent.GridPosition.X) && (node.GridPosition.Y < node.Parent.GridPosition.Y))
			{
				arrow.transform.eulerAngles = new Vector3(0, 0, 225);
			}
			//Bottom
			else if ((node.GridPosition.X == node.Parent.GridPosition.X) && (node.GridPosition.Y < node.Parent.GridPosition.Y))
			{
				arrow.transform.eulerAngles = new Vector3(0, 0, 270);
			}
			//Bottom right
			else if ((node.GridPosition.X < node.Parent.GridPosition.X) && (node.GridPosition.Y < node.Parent.GridPosition.Y))
			{
				arrow.transform.eulerAngles = new Vector3(0, 0, 315);
			}
		}
	}
}
