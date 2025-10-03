extends ActionState

var auta: Auta
var inp: InputManager
var anim: AnimationTree

var orb_time: float
var can_unorb: bool
var orb_param: AutaOrbParameters
var speed_param: AutaSpeedParameters
var jmp_param: EntityJumpParameters
var time_since_last_bounce: float

var can_short_hop:
	get:
		return entity.get_node("ActionStates/NormalState").can_short_hop
	set(_value):
		entity.get_node("ActionStates/NormalState").can_short_hop = _value

@onready var orb_area_shape = $OrbGrab/CollisionShape2D
@onready var orb_burst_fx_pool = $"../../SpecialAttributes/ObjectPools/OrbBurst"

func _ready() -> void:
	super._ready()
	auta = entity as Auta
	inp = entity.get_node("GenericAttributes/InputManager")
	anim = entity.get_node("Art/AnimationTree")
	orb_param = entity.parameters["orb"]
	jmp_param = entity.parameters["jump"]
	speed_param = entity.parameters["speed"]
	inp.action_a_just_pressed.connect(on_jump)
	inp.action_b_just_pressed.connect(on_attack)
	time_since_last_bounce = 100
	entity.just_bounced.connect(on_bounce)
	
func _start() -> void:
	super._start()
	orb_time = 0;
	can_unorb = false
	entity.collision_mode = CollisionEntity.COLLISION_MODE_BOUNCE
	entity.gravity = orb_param.gravity
	anim.get("parameters/playback").start("Orb", true)
	entity.get_node("Art").rotation = 0
	entity.get_node("EnvironmentBox").shape = orb_param.collision_shape
	entity.get_node("EnvironmentBox").position = orb_param.collision_shape_position
	entity.get_node("Art/AfterImageGenerator").call("start_afterimages")
	orb_area_shape.disabled = false
	var fx: Node2D = orb_burst_fx_pool.spawn_object()
	if fx != null:
		fx.global_position = entity.global_position;
		
	var _input = inp.input_direction
	
	if entity.last_action_state.name == "DuckState":
		var _norm := entity.get_floor_normal()
		if abs(_input.x) <= 0.3:
			_input.x = -1 if entity.get_node("Art").flip_h else 1
		var _velocity: Vector2 = entity.velocity_true
		entity.velocity.y = 0
		if _input.x > 0:
			entity.velocity.x = max(entity.velocity_true.x, sign(_input.x) * orb_param.launch_speed)
		else:
			entity.velocity.x = min(entity.velocity_true.x, sign(_input.x) * orb_param.launch_speed)
		_velocity = Vector2(-_norm.y * entity.velocity_true.x, _norm.x * entity.velocity_true.x )
		if(_velocity.y < 0):
			entity.velocity = _velocity
		return
	
	if _input == Vector2.ZERO:
		_input.x = -1 if entity.get_node("Art").flip_h else 1	
	_input = _input.normalized()
	
	if _input.x > 0.5:
		entity.velocity.x = max(entity.velocity_true.x, _input.x * orb_param.launch_speed)
	elif _input.x < -0.5:
		entity.velocity.x = min(entity.velocity_true.x, _input.x * orb_param.launch_speed)
	else: entity.velocity.x = 0
	
	if _input.y > 0.5:
		entity.velocity.y = max(entity.velocity_true.y, _input.y * orb_param.launch_speed)
	elif _input.y < -0.5:
		entity.velocity.y = min(entity.velocity_true.y, _input.y * orb_param.launch_speed)
	else: entity.velocity.y = 0
	
	if entity.velocity.y == 0 and entity.is_on_floor():
		entity.velocity.y = orb_param.launch_y_speed
	if entity.velocity.y == 0 and not entity.is_on_floor():
		entity.velocity.y = orb_param.launch_y_speed * 0.3

func _process(delta: float) -> void:
	super._process(delta)
	if not active: return
	
	time_since_last_bounce += delta
	
	if not inp.action_c_pressed and orb_time > orb_param.minimum_time:
		if abs(entity.velocity.x) > speed_param.minimum_speed_skating:
			entity.switch_action_state_name("SpeedState")
			print("speed")
		else:
			entity.switch_action_state_name("NormalState")
			print("nospeed")
			
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
	print(entity.is_on_floor())
	
	var _norm := entity.get_floor_normal()
	var _amount := _norm.x * entity.gravity * orb_param.slope_influence * delta
	entity.accelerate_x(_amount, 10000 * sign(_norm.x), true)
	
func _end() -> void:
	super._end()
	entity.collision_mode = CollisionEntity.COLLISION_MODE_FLOOR
	anim.get("parameters/playback").start("Jump", true)
	auta.can_orb = false
	auta.recharge_orb = false
	auta.orb_timer = orb_param.orb_recharge_time
	orb_area_shape.disabled = true
	
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
	
func on_jump():
	if not active: return
	if time_since_last_bounce <= orb_param.orb_to_jump_buffer_time:
		entity.switch_action_state_name("NormalState")
		can_short_hop = true
		entity.velocity.y = jmp_param.get_jump_power()

func on_attack():
	if not active: return
	entity.switch_action_state_name("AttackState")

func on_bounce(_normal: Vector2, _velocity: Vector2):
	time_since_last_bounce = 0
