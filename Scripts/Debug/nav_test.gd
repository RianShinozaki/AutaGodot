extends Node2D
@onready var nav: NavigationAgent2D = $NavigationAgent2D
@export var move_speed: float

func _physics_process(delta: float) -> void:
	nav.target_position = Auta.instance.global_position
	print(nav.is_target_reachable())
	var next_pos: Vector2 = nav.get_next_path_position()
	global_position += (next_pos - global_position).normalized() * delta * move_speed
