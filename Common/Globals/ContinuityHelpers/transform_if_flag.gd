class_name TransformIfFlag
extends Node2D

@export var global_flag: String
@export var value: bool = true

@export var to_position: Vector2
@export var to_rotation: float

func _ready():
	if Global.get(global_flag) == value:
		get_parent().position = to_position
		get_parent().rotation = to_rotation
