class_name Auta

extends CollisionEntity

var can_orb: bool = true
var orb_timer: float = 0.0
var recharge_orb = false

func _physics_process(delta: float) -> void:
	super._physics_process(delta)
	
	modulate = Color.WHITE
	if not can_orb and not recharge_orb and is_on_floor() and collision_mode == COLLISION_MODE_FLOOR:
		recharge_orb = true
	
	if not can_orb and recharge_orb:
		orb_timer = move_toward(orb_timer, 0.0, delta)
		if ceili(orb_timer * 10) % 2 == 1:
			modulate = Color.CYAN
		if orb_timer == 0:
			can_orb = true
