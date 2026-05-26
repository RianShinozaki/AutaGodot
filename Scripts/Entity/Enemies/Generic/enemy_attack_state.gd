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
	mov_param = entity.parameters["attack"]
	hurt_param = entity.parameters["hurt"]
	
	anim = entity.get_node("Art/AnimationTree")

func _start() -> void:
	super._start()
	entity.get_node("EnvironmentBox").shape = mov_param.collision_shape
	entity.get_node("EnvironmentBox").position = mov_param.collision_shape_position
	$"../../SpecialAttributes/HurtManager".power_resistance = hurt_param.power_resistance_normal
	
func _process(delta: float) -> void:
	super._process(delta)
	if not active: return
		
	var _norm := entity.get_floor_normal()
	var _slope_influence = mov_param.slope_uphill_influence if sign(entity.velocity.x) != sign(_norm.x) else mov_param.slope_downhill_influence
	var _amount: float = _norm.x * entity.gravity * _slope_influence * delta
	entity.accelerate_x(_amount, mov_param.absolute_limit * sign(_norm.x), true)
	
	entity.accelerate_x(mov_param.get_deceleration(entity) * delta, 0, false)
	if abs(entity.velocity.x) < mov_param.get_minimum_speed(entity):
		entity.velocity.x = 0
	
	#Update gravity
	entity.gravity = jmp_param.get_gravity(entity, 1.0)
	entity.velocity.y = min(entity.velocity.y, jmp_param.max_falling_speed)
	
	var _art: Sprite2D = entity.get_node("Art")
	var _to_angle: float = 0
	_art.global_rotation = lerp_angle(_art.global_rotation,_to_angle,delta * 10)


func _end() -> void:
	super._end()

func play_animation_oneshot(_anim: String):
	anim.get("parameters/Grounded/playback").start(_anim, true)

func process_damage(_area):
	$"../../SpecialAttributes/HurtManager".process_hurt(_area)
