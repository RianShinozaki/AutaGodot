class_name SpiderDrone 
extends AIEntity

var cached_sprite_rot: float = 0

func process_damage(_area):
	$SpecialAttributes/HurtManager.process_hurt(_area)
	if is_on_floor():
		if current_action_state.name == "NormalState":
			velocity = Vector2.ZERO
			
			var _hitbox_data: HitboxData = _area.hitbox_data
			var _x_knockback: float = _hitbox_data.x_knockback
			if _hitbox_data.x_knockback_is_relative:
				_x_knockback *= -1 if _area.global_position.x > global_position.x else 1
			var _knockback = Vector2(_x_knockback, _hitbox_data.y_knockback/2)
		
			$ActionStates/NormalState.process_knockback(_knockback)

func _ready() -> void:
	cached_sprite_rot = global_rotation
	up_direction = Vector2.from_angle(rotation - PI/2)
	gravity_direction = -up_direction
	rotation = 0

func _physics_process(delta: float) -> void:
	super._physics_process(delta)
	if current_action_state.name == "NormalState":
		cached_sprite_rot = rotate_toward(cached_sprite_rot, up_direction.angle() + PI/2, delta*20)
		get_node("Art").global_rotation = cached_sprite_rot
