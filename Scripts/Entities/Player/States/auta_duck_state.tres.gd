class_name AutaDuckState

extends ActionState

var auta: Auta
var inp: InputManager
var anim: AnimationTree

var duck_param: AutaDuckParameters
var mov_param: EntityMovementParameters
var can_unduck: bool

func _ready() -> void:
	super._ready()
	auta = entity as Auta
	inp = entity.get_node("GenericAttributes/InputManager")
	anim = entity.get_node("Art/AnimationTree")
	duck_param = entity.parameters["duck"]
	mov_param = entity.parameters["movement"]
	inp.action_c_just_pressed.connect(on_orb)
	
func _start() -> void:
	super._start()
	entity.get_node("EnvironmentBox").shape = duck_param.collision_shape
	entity.get_node("EnvironmentBox").position = duck_param.collision_shape_position
	anim.get("parameters/playback").start("Grounded", true)
	anim.get("parameters/Grounded/playback").start("Duck", true)
	can_unduck = false

func _process(delta: float) -> void:
	if !active: return
	entity.accelerate_x(duck_param.get_acceleration(entity) * delta, 0, false)
	var _vert: float = inp.input_direction.y
	if _vert <= 0.3 or not entity.is_on_floor():
		entity.switch_action_state_name("NormalState")
		anim.get("parameters/Grounded/playback").start("FromDuck", true)
	
	var _norm := entity.get_floor_normal()
	var _amount := _norm.x * entity.gravity * duck_param.slope_influence * delta
	entity.accelerate_x(_amount, duck_param.max_speed * sign(_norm.x), true)
	
	if entity.is_on_floor() && abs(entity.velocity.x) < 20: 
		entity.get_node("Art/AfterImageGenerator").call("stop_afterimages")
		
	var _art: Sprite2D = entity.get_node("Art")
	var _to_angle: float = (entity.get_floor_angle() * sign(entity.get_floor_normal().x))
	_art.global_rotation = lerp_angle(_art.global_rotation,_to_angle,delta * 10)
		
func on_orb():
	if not active: return
	if auta.can_orb:
		entity.switch_action_state_name("OrbState")
