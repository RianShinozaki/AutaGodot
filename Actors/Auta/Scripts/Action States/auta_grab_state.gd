extends ActionState

@onready var land_fx_pool = $"../../SpecialAttributes/ObjectPools/Land"
@onready var aim_arrow = $AimArrow
@onready var aim_reticle = $AimReticle
var auta: Auta
var inp: InputManager
var anim: AnimationTree

var mov_param: EntityMovementParameters
var jmp_param: EntityJumpParameters
var throw_param: AutaThrowParameters

@export var land_sound: AudioStream

var grabbed_entity: Node2D

func _ready() -> void:
	super._ready()
	inp = entity.get_node("GenericAttributes/InputManager")
	mov_param = entity.parameters["movement"]
	jmp_param = entity.parameters["jump"]
	throw_param = entity.parameters["throw"]
	anim = entity.get_node("Art/AnimationTree")
	inp.action_c_just_released.connect(on_orb)
	entity.just_grounded.connect(just_grounded)
	aim_arrow.visible = false
	aim_reticle.visible = false
	auta = entity as Auta

func _start() -> void:
	super._start()
	entity.get_node("EnvironmentBox").set_deferred("shape", mov_param.collision_shape)
	entity.get_node("EnvironmentBox").position = mov_param.collision_shape_position
	anim.get("parameters/playback").start("ThrowSpin")
	
	entity.set_x_velocity(entity.velocity.x/2)
	entity.set_y_velocity(throw_param.grab_jump)
	Engine.time_scale = 0.15
	
	var _inp_dir: Vector2 = inp.input_direction
	aim_reticle.position = _inp_dir * 48 
	
	aim_arrow.visible = true
	aim_reticle.visible = true
	
func _process(delta: float) -> void:
	super._process(delta)
		
	if not active: return

	var _inp_dir: Vector2 = inp.input_direction
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
	
	if entity.is_on_floor(): 
		entity.get_node("Art/AfterImageGenerator").call("stop_afterimages")
	
	var _art: Sprite2D = entity.get_node("Art")
	var _to_angle: float = 0
	_art.rotation = lerp_angle(_art.rotation,_to_angle,delta * 10)
	
	#Reticle
	aim_reticle.global_position += throw_param.reticle_move_speed * delta * _inp_dir
	aim_arrow.rotation = aim_reticle.position.angle()
	

func _end() -> void:
	super._end()
	anim.get("parameters/playback").start("Jump")
	Engine.time_scale = 1
	aim_arrow.visible = false
	aim_reticle.visible = false
		
func on_orb():
	if not active: return
	if grabbed_entity != null and is_instance_valid(grabbed_entity):
		grabbed_entity.throw(aim_reticle.position.normalized(), throw_param.throw_power)
		grabbed_entity = null
		entity.set_x_velocity(-aim_reticle.position.normalized().x * throw_param.recoil_power)
		entity.set_y_velocity(throw_param.recoil_y)
		entity.switch_action_state_name("NormalState")

#Grounded animation handler
func just_grounded(_normal: Vector2, _velocity: Vector2):
	if _velocity.y < 60: return
	
	SFXController.play_sound(land_sound, global_position)
	var fx: Node2D = land_fx_pool.spawn_object()
	if fx != null:
		fx.global_position = entity.global_position + Vector2.DOWN*8;
		fx.rotation = entity.get_floor_angle() * sign(entity.get_floor_normal().x)
		
	if not active: return
	if not anim.active: return

func play_animation_oneshot(_anim: String):
	anim.get("parameters/Grounded/playback").start(_anim, true)
