using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AStar 
{
	private static Dictionary<Point, Node> nodes;

	private static void CreateNodes()
	{
		nodes = new Dictionary<Point, Node>();

		foreach (TileScript tile in LevelManager.Instance.Tiles.Values)
		{
			nodes.Add(tile.GridPosition, new Node(tile));
		}
	}

	public static void GetPath(Point start)
	{
		if (nodes == null)
		{
			CreateNodes();
		}

		HashSet<Node> openList = new HashSet<Node>();

		Node currentNode = nodes[start];

		openList.Add(currentNode);

		//THIS IS ONLY FOR DEBUGGING NEEDS TO BE REMOVED LATER
		GameObject.Find("AStarDebugger").GetComponent<AStarDebugger>().DebugPath(openList);
	}
}
