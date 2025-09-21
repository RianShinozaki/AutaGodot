extends ActionState

var auta: Auta
var inp: InputManager
var anim: AnimationTree

var orb_time: float
var can_unorb: bool
var orb_param: AutaOrbParameters

func _ready() -> void:
	super._ready()
	auta = entity as Auta
	inp = entity.get_node("GenericAttributes/InputManager")
	anim = entity.get_node("Art/AnimationTree")
	orb_param = entity.parameters["orb"]
	inp.action_b_just_pressed.connect(on_attack)
	
func _start() -> void:
	super._start()
	orb_time = 0;
	can_unorb = false
	auta.can_orb = false
	entity.collision_mode = CollisionEntity.COLLISION_MODE_BOUNCE
	entity.gravity = orb_param.gravity
	anim.get("parameters/playback").start("Orb", true)
	entity.get_node("EnvironmentBox").shape = orb_param.collision_shape
	entity.get_node("EnvironmentBox").position = orb_param.collision_shape_position
	entity.get_node("Art/AfterImageGenerator").call("StartCreatingAfterImgs")
	
	var _input = inp.input_direction
	
	if entity.last_action_state.name == "DuckState":
		if abs(_input.x) <= 0.3:
			_input.x = -1 if entity.get_node("Art").flip_h else 1
		if _input.x > 0:
			entity.velocity.x = max(entity.velocity.x, sign(_input.x) * orb_param.launch_speed)
		else:
			entity.velocity.x = min(entity.velocity.x, sign(_input.x) * orb_param.launch_speed)
		return
	
	if _input == Vector2.ZERO:
		_input.x = -1 if entity.get_node("Art").flip_h else 1	
	_input = _input.normalized()
	
	if _input.x > 0.5:
		entity.velocity.x = max(entity.velocity.x, _input.x * orb_param.launch_speed)
	elif _input.x < -0.5:
		entity.velocity.x = min(entity.velocity.x, _input.x * orb_param.launch_speed)
	else: entity.velocity.x = 0
	
	if _input.y > 0.5:
		entity.velocity.y = max(entity.velocity.y, _input.y * orb_param.launch_speed)
	elif _input.y < -0.5:
		entity.velocity.y = min(entity.velocity.y, _input.y * orb_param.launch_speed)
	else: entity.velocity.y = 0
	
	if entity.velocity.y == 0 and entity.is_on_floor():
		entity.velocity.y = orb_param.launch_y_speed
	if entity.velocity.y == 0 and not entity.is_on_floor():
		entity.velocity.y = orb_param.launch_y_speed * 0.3

func _process(delta: float) -> void:
	super._process(delta)
	if not active: return
	if not inp.action_c_pressed and orb_time > orb_param.minimum_time:
		entity.switch_action_state_name("NormalState")
	if orb_time <= orb_param.initial_gravity_time:
		entity.gravity = orb_param.initial_gravity
	else:
		entity.gravity = orb_param.gravity
	orb_time += delta;
	
	#Accelerate and decelerate
	var hor = inp.input_direction.x
	if abs(hor) > 0.1:
		entity.accelerate_x(orb_param.get_acceleration(entity) * delta * sign(hor), sign(hor) * orb_param.get_max_speed(), true)
	
	if abs(hor) < 0.1 or sign(hor) == -sign(entity.velocity.x):
		entity.accelerate_x(orb_param.get_deceleration(entity) * delta, 0, false)
	
	#Turn the player around
	if entity.velocity.x != 0:
		entity.get_node("Art").flip_h = entity.velocity.x < 0
		entity.get_node("SpecialAttributes/Hitboxes").scale = Vector2(sign(entity.velocity.x), 1.0)

func _end() -> void:
	super._end()
	entity.collision_mode = CollisionEntity.COLLISION_MODE_FLOOR
	anim.get("parameters/playback").start("Jump", true)
	var _input = inp.input_direction
	if _input == Vector2.ZERO: return
	_input = _input.normalized()
	
	if _input.x > 0.5:
		entity.velocity.x = max(entity.velocity.x, _input.x * orb_param.exit_speed)
	elif _input.x < -0.5:
		entity.velocity.x = min(entity.velocity.x, _input.x * orb_param.exit_speed)
	else:
		entity.velocity.x = 0
	
	if _input.y > 0.5:
		entity.velocity.y = max(entity.velocity.y, _input.y * orb_param.exit_speed)
	elif _input.y < -0.5:
		entity.velocity.y = min(entity.velocity.y, _input.y * orb_param.exit_speed)
	else:
		entity.velocity.y = 0

func on_attack():
	if not active: return
	entity.switch_action_state_name("AttackState")
