extends Node2D


func activate_them():
	for node in get_children():
		node.active = true
		node.visible = true
		
