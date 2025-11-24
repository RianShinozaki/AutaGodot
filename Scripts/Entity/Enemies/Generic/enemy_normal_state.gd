extends ActionState

@onready var land_fx_pool = $"../../SpecialAttributes/ObjectPools/Land"
var inp: InputManager
var anim: AnimationTree

var mov_param: EntityMovementParameters
var jmp_param: EntityJumpParameters
var hurt_param: PlayerHurtParameters

@export var auto_flip: bool = true


func _ready() -> void:
	super._ready()
	inp = entity.get_node("GenericAttributes/InputManager")
	jmp_param = entity.parameters["jump"]
	mov_param = entity.parameters["movement"]
	hurt_param = entity.parameters["hurt"]
	
	anim = entity.get_node("Art/AnimationTree")
	entity.just_grounded.connect(just_grounded)
	
	inp.action_b_just_pressed.connect(on_attack)

func _start() -> void:
	super._start()
	entity.get_node("EnvironmentBox").shape = mov_param.collision_shape
	entity.get_node("EnvironmentBox").position = mov_param.collision_shape_position
	$"../../SpecialAttributes/HurtManager".power_resistance = hurt_param.power_resistance_normal
	
func _process(delta: float) -> void:
	super._process(delta)
	if not active: return
	
	#Accelerate and decelerate
	var hor = inp.input_direction.x
	if abs(hor) > 0.2 && (sign(hor) == sign(entity.velocity.x) || entity.velocity.x == 0):
		entity.accelerate_x(mov_param.get_acceleration(entity) * delta * sign(hor), hor * mov_param.get_max_speed(), true)
		if abs(entity.velocity.x) < mov_param.get_initial_speed(entity):
			entity.velocity.x = mov_param.get_initial_speed(entity) * sign(hor)
	
	if abs(hor) > 0.2 && sign(hor) != sign(entity.velocity.x):
		entity.accelerate_x(mov_param.get_reverse_acceleration(entity) * delta, 0, false)
		
	var _norm := entity.get_floor_normal()
	var _slope_influence = mov_param.slope_uphill_influence if sign(entity.velocity.x) != sign(_norm.x) else mov_param.slope_downhill_influence
	var _amount: float = _norm.x * entity.gravity * _slope_influence * delta
	entity.accelerate_x(_amount, mov_param.absolute_limit * sign(_norm.x), true)
	
	if abs(hor) < 0.1 or sign(hor) == -sign(entity.velocity.x):
		entity.accelerate_x(mov_param.get_deceleration(entity) * delta, 0, false)
		if abs(entity.velocity.x) < mov_param.get_minimum_speed(entity):
			entity.velocity.x = 0
	
	#Turn the player around
	if auto_flip:
		var _flip_h = entity.get_node("Art").flip_h
		if(entity.velocity.x != 0 and entity.is_on_floor()):
			entity.get_node("Art").flip_h = entity.velocity.x < 0
			entity.get_node("SpecialAttributes/Hitboxes").scale = Vector2(sign(entity.velocity.x), 1.0)
	
	#Update gravity
	entity.gravity = jmp_param.get_gravity(entity, 1.0)
	entity.velocity.y = min(entity.velocity.y, jmp_param.max_falling_speed)
	
	var _art: Sprite2D = entity.get_node("Art")
	var _to_angle: float = 0
	_art.global_rotation = lerp_angle(_art.global_rotation,_to_angle,delta * 10)


func _end() -> void:
	super._end()
	
#Grounded animation handler
func just_grounded(_normal: Vector2, _velocity: Vector2):
	var fx: Node2D = land_fx_pool.spawn_object()
	if fx != null:
		fx.global_position = entity.global_position + Vector2.DOWN*14;
		fx.rotation = entity.get_floor_angle() * sign(entity.get_floor_normal().x)

func play_animation_oneshot(_anim: String):
	anim.get("parameters/Grounded/playback").start(_anim, true)

func process_damage(_area):
	$"../../SpecialAttributes/HurtManager".process_hurt(_area)

func on_attack():
	if not active: return
	entity.attack()
