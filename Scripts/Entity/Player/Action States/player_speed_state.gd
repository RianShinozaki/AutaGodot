extends ActionState

@onready var land_fx_pool = $"../../SpecialAttributes/ObjectPools/Land"
var auta: Auta
var inp: InputManager
var anim: AnimationTree

var mov_param: AutaSpeedParameters
var jmp_param: EntityJumpParameters
var can_short_hop: bool

func _ready() -> void:
	super._ready()
	inp = entity.get_node("GenericAttributes/InputManager")
	mov_param = entity.parameters["speed"]
	jmp_param = entity.parameters["jump"]
	anim = entity.get_node("Art/AnimationTree")
	inp.action_a_just_pressed.connect(on_jump)
	inp.action_b_just_pressed.connect(on_attack)
	inp.action_c_just_pressed.connect(on_orb)
	entity.just_grounded.connect(just_grounded)
	auta = entity as Auta

func _start() -> void:
	super._start()
	entity.get_node("EnvironmentBox").shape = mov_param.collision_shape
	entity.get_node("EnvironmentBox").position = mov_param.collision_shape_position
	can_short_hop = false
	entity.anim_skating = true
	
func _process(delta: float) -> void:
	super._process(delta)
	if not active: return
	
	#Accelerate and decelerate
	var hor = inp.input_direction.x
	if abs(hor) > 0.1:
		entity.accelerate_x(mov_param.get_acceleration(entity) * delta * sign(hor), sign(hor) * mov_param.get_max_speed(), true)
		if abs(entity.velocity.x) < mov_param.get_initial_speed(entity):
			entity.velocity.x = mov_param.get_initial_speed(entity) * sign(hor)
	
	if abs(hor) < 0.1 or sign(hor) == -sign(entity.velocity.x):
		entity.accelerate_x(mov_param.get_deceleration(entity) * delta, 0, false)
	
	if abs(entity.velocity.x) < mov_param.minimum_speed_skating:
		entity.switch_action_state_name("NormalState")
	
	if entity.is_on_floor(): 
		if inp.input_direction.y > 0.3:
			entity.switch_action_state_name("DuckState")
			
	#Handle acceleration/deceleration due to slopes
	var _norm := entity.get_floor_normal()
	var _slope_influence = mov_param.slope_uphill_influence if sign(entity.velocity.x) != sign(_norm.x) else mov_param.slope_downhill_influence
	var _amount: float = _norm.x * entity.gravity * _slope_influence * delta
	entity.accelerate_x(_amount, mov_param.absolute_limit * sign(_norm.x), true)
	
	#Update gravity
	if(entity.velocity.y > jmp_param.rising_gravity_scale): can_short_hop = false
	var _do_short_hop = can_short_hop and not inp.action_a_pressed
	entity.gravity = jmp_param.get_gravity(entity, jmp_param.short_hop_gravity_scale if _do_short_hop else 1.0)
	entity.velocity.y = min(entity.velocity.y, jmp_param.max_falling_speed)
	
	var _art: Sprite2D = entity.get_node("Art")
	var _to_angle: float = (entity.get_floor_angle() * sign(entity.get_floor_normal().x)) if entity.is_on_floor() else 0
	_art.global_rotation = lerp_angle(_art.global_rotation,_to_angle,delta * 10)
	

func _end() -> void:
	super._end()
	entity.anim_skating = false
	
func on_jump():
	if not active: return
	if(entity.is_on_floor()):
		can_short_hop = true
		var _norm := entity.get_floor_normal()
		entity.velocity.y = jmp_param.get_jump_power() + entity.velocity.x * _norm.x * mov_param.slope_jump_influence
		entity.velocity.x += sign(entity.velocity.x) * entity.velocity.x * _norm.x * mov_param.slope_jump_influence

func on_attack():
	if not active: return
	entity.switch_action_state_name("AttackState")
		
func on_orb():
	if not active: return
	if auta.can_orb:
		entity.switch_action_state_name("OrbState")

#Grounded animation handler
func just_grounded(_normal: Vector2, _velocity: Vector2):
	var fx: Node2D = land_fx_pool.spawn_object()
	if fx != null:
		fx.global_position = entity.global_position + Vector2.DOWN*8;
	if not active: return
	play_animation_oneshot("Land")

func play_animation_oneshot(_anim: String):
	anim.get("parameters/Grounded/playback").start(_anim, true)
