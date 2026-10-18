@tool

extends Node2D

func _process(delta):
	get_parent().set("velocity", Vector2.ZERO)
	print(get_parent().velocity)
