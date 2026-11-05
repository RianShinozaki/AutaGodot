class_name RemoveIfFlag
extends Node2D

@export var global_flag: String
@export var value: bool = true

func _ready():
	if Global.get(global_flag) == value:
		get_parent().visible = false
		get_parent().global_position = Vector2(-10000, -10000)
