extends ActionState

@onready var land_fx_pool = $"../../SpecialAttributes/ObjectPools/Land"
var inp: InputManager
var anim: AnimationTree

var mov_param: EntityMovementParameters
var jmp_param: EntityJumpParameters
var hurt_param: PlayerHurtParameters

var grabbed: bool = false

@export var auto_flip: bool = true
var parent_cache: Node

func _ready() -> void:
	super._ready()
	inp = entity.get_node("GenericAttributes/InputManager")
	jmp_param = entity.parameters["jump"]
	mov_param = entity.parameters["movement"]
	hurt_param = entity.parameters["hurt"]
	
	anim = entity.get_node("Art/AnimationTree")
	entity.just_grounded.connect(just_grounded)
	

func _start() -> void:
	super._start()
	entity.get_node("EnvironmentBox").set_deferred("shape", mov_param.collision_shape)
	entity.get_node("EnvironmentBox").position = mov_param.collision_shape_position
	$"../../SpecialAttributes/HurtManager".power_resistance = hurt_param.power_resistance_normal
	anim.get("parameters/playback").start("Stunned", true)
	entity.get_node("GenericAttributes/EntityStatus").stunned = true
	
func _process(delta: float) -> void:
	super._process(delta)
	if not active: return
	
	#Decelerate
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
	entity.get_node("GenericAttributes/EntityStatus").stunned = false
	
	super._end()
	
#Grounded animation handler
func just_grounded(_normal: Vector2, _velocity: Vector2):
	var fx: Node2D = land_fx_pool.spawn_object()
	if fx != null:
		fx.global_position = entity.get_last_slide_collision().get_position()
		fx.rotation = entity.get_floor_angle() * sign(entity.get_floor_normal().x)

func play_animation_oneshot(_anim: String):
	anim.get("parameters/Grounded/playback").start(_anim, true)


func _on_grabbox_area_entered(area):
	Auta.instance.switch_action_state_name("GrabState")
	parent_cache = entity.get_parent()
	entity.get_parent().call_deferred("remove_child",entity)
	Auta.instance.get_action_state_name("GrabState").get_node("FlipPivot/ThrowParent").call_deferred("add_child", entity)
	entity.position = Vector2.ZERO
	entity.apply_physics = false
	Auta.instance.get_action_state_name("GrabState").grabbed_entity = self

func throw(_dir: Vector2, speed: float):
	var _pos_cache: Vector2 = entity.global_position
	entity.collision_mode = entity.COLLISION_MODE_BOUNCE
	entity.velocity = _dir * speed
	entity.get_parent().call_deferred("remove_child",entity)
	parent_cache.call_deferred("add_child", entity)
	entity.set_deferred("global_position", _pos_cache)
	entity.apply_physics = true
	entity.switch_action_state_name("CannonballState")
