class_name CollisionEntity

extends CharacterBody2D

enum {COLLISION_MODE_FLOOR, COLLISION_MODE_FREE, COLLISION_MODE_BOUNCE}

@export_group("Collision")
@export var gravity: float
@export var collision_mode: int
@export_range(0, 1) var bounce_factor: float = 1

@export_subgroup("Slope")
@export_range(0, 1) var slope_leave_infulence: float
@export_range(0, 1) var slope_jump_influence: float
@export_range(0, 1) var slope_land_influence: float

@export_group("States")
@export var apply_physics: bool
@export var previously_grounded: bool
@export var current_action_state: ActionState
@export var last_action_state: ActionState

@export_group("Parameters")
@export var parameters: Dictionary[String, ParameterSet]

signal just_grounded(normal: Vector2, velocity: Vector2)
signal just_bounced(normal: Vector2, velocity: Vector2)

func move(_velocity: Vector2, _delta: float, _update_velocity: bool) -> void:
	var _temp_velocity = velocity
	match collision_mode:
		COLLISION_MODE_FLOOR:
			velocity = _velocity
			motion_mode = CharacterBody2D.MOTION_MODE_GROUNDED
			move_and_slide()
		COLLISION_MODE_FREE:
			velocity = _velocity
			motion_mode = CharacterBody2D.MOTION_MODE_FLOATING
			move_and_slide()
		COLLISION_MODE_BOUNCE:
			var _kc = move_and_collide(_velocity * _delta, true)
			if _kc != null:
				var _normal = _kc.get_normal()
				_velocity = -_velocity.reflect(_normal)
				_velocity.y += gravity * _delta
				var _loss_factor = Vector2(absf(_normal.x), absf(_normal.y)) * (1 - bounce_factor)
				var _impact_factor = Vector2(absf(_normal.x), absf(_normal.y)) * (bounce_factor)
				_velocity.x -= _loss_factor.x * _velocity.x
				_velocity.y -= _loss_factor.y * _velocity.y
				if(absf(_velocity.y) < 20): _velocity.y = 0
				emit_signal("just_bounced", _normal, _velocity)
			velocity = _velocity
			move_and_slide()
	
	if not _update_velocity:
		velocity = _temp_velocity
	
func _physics_process(delta: float) -> void:
	if !apply_physics: return
	
	if collision_mode == COLLISION_MODE_FLOOR:
		if is_on_floor() and velocity.y >= 0 and not previously_grounded:
			emit_signal("just_grounded", get_floor_normal(), velocity)
			velocity.y = 0
		if not is_on_floor() or velocity.y < 0:
			velocity.y += gravity * delta
	else:
		velocity.y += gravity * delta
	
	previously_grounded = is_on_floor()
	move(velocity, delta, true)

func accelerate_x(_amount: float, _limit: float, _toward: bool):
	velocity.x = move_toward(velocity.x, _limit, _amount * (sign(_limit) if _toward else 1))

func switch_action_state_name(_state: String):
	var _action_state = get_node("ActionStates/"+_state)
	switch_action_state(_action_state)
	
func switch_action_state(_state: ActionState):
	last_action_state = current_action_state
	current_action_state = _state
	last_action_state._end()
	current_action_state._start()
