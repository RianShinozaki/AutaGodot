class_name Auta

extends CollisionEntity

var can_orb: bool = true

func _physics_process(delta: float) -> void:
	super._physics_process(delta)
	
	if is_on_floor() and collision_mode == COLLISION_MODE_FLOOR:
		can_orb = true
