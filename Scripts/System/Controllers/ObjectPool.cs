using Godot;
using System;

[GlobalClass]
public partial class ObjectPool : Node2D
{
	[Export] private Godot.Collections.Array<PooledObject> pool;
	[Export] private Godot.Collections.Dictionary<string, Godot.Collections.Array<Node>> standBy;

	public static ObjectPool Instance;
	public override void _Ready()
	{
		base._Ready();
		Instance = this;
		standBy = new Godot.Collections.Dictionary<string, Godot.Collections.Array<Node>>();
		String theName = "ERR";
		foreach (PooledObject po in pool)
		{
			Godot.Collections.Array<Node> newObjectArray = new Godot.Collections.Array<Node>();
			PackedScene scene = GD.Load<PackedScene>(po.objectToPool);
			for (int i = 0; i < po.number; i++)
			{
				Node newObject = scene.Instantiate();
				theName = newObject.Name;
				AddChild(newObject);
				newObjectArray.Add(newObject);
			}
			standBy.Add(theName, newObjectArray);
		}
	}

	public Node Spawn(String objectName)
	{
		if (standBy[objectName].Count > 0)
		{
			Node theNode = standBy[objectName][0];
			standBy[objectName].RemoveAt(0);
			theNode.GetNode<ObjectPoolObject>("ObjectPoolObject").Spawn(this, objectName);
			//AddChild(theNode);
			return theNode;
		}
		return null;
	}
	public void ReturnToPool(Node node, String name)
	{
		//RemoveChild(node);
		node.GetParent().RemoveChild(node);
		AddChild(node);
		standBy[name].Add(node);
	}
}
