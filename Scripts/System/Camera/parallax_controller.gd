extends Node2D

@export var scroll_scale: float
@export var offset: Vector2

var orig_position: Vector2

func _ready() -> void:
	orig_position = global_position * scroll_scale
	on_deactivate()
	get_parent().activate.connect(on_activate)
	get_parent().deactivate.connect(on_deactivate)

func _physics_process(_delta: float):
	global_position = offset + orig_position + GameCamera.instance.global_position * (1 - scroll_scale)

func on_activate():
	visible = true
	
func on_deactivate():
	visible = false
