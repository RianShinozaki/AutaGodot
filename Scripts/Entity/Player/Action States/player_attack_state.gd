extends ActionState

var auta: Auta
var inp: InputManager
var anim: AnimationTree

var mov_param: EntityMovementParameters
var jmp_param: EntityJumpParameters
var can_short_hop:
	get:
		return entity.get_node("ActionStates/NormalState").can_short_hop
	set(_value):
		entity.get_node("ActionStates/NormalState").can_short_hop = _value
		
var combo_number: int = 0
var combo_timer: float
var combo_max_time: float

func _ready() -> void:
	super._ready()
	inp = entity.get_node("GenericAttributes/InputManager")
	mov_param = entity.parameters["movement"]
	jmp_param = entity.parameters["jump"]
	anim = entity.get_node("Art/AnimationTree")
	auta = entity as Auta

func _start() -> void:
	super._start()
	entity.get_node("EnvironmentBox").shape = mov_param.collision_shape
	entity.get_node("EnvironmentBox").position = mov_param.collision_shape_position
	var direction = inp.input_direction
	if direction.x != 0:
		entity.get_node("Art").flip_h = entity.velocity.x < 0
		entity.get_node("SpecialAttributes/Hitboxes").scale = Vector2(sign(entity.velocity.x), 1.0)
	anim.get("parameters/playback").start("Attack", true)
	play_animation_oneshot("Attack_Combo1")
	
func _process(delta: float) -> void:
	super._process(delta)
	if not active: return
	
	#Accelerate and decelerate
	var hor = inp.input_direction.x
	if abs(hor) > 0.1:
		entity.accelerate_x(mov_param.get_acceleration(entity) * delta * sign(hor), sign(hor) * mov_param.get_max_speed(), true)
	
	if abs(hor) < 0.1 or sign(hor) == -sign(entity.velocity.x):
		entity.accelerate_x(mov_param.get_deceleration(entity) * delta, 0, false)
	
	#Update gravity
	if(entity.velocity.y > jmp_param.rising_gravity_scale): can_short_hop = false
	var _do_short_hop = can_short_hop and not inp.action_a_pressed
	entity.gravity = jmp_param.get_gravity(entity, jmp_param.short_hop_gravity_scale if _do_short_hop else 1.0)
	entity.velocity.y = min(entity.velocity.y, jmp_param.max_falling_speed)
	if entity.is_on_floor(): 
		entity.get_node("Art/AfterImageGenerator").call("StopCreatingAfterImgs")

func _end() -> void:
	super._end()
	can_short_hop = false

func play_animation_oneshot(_anim: String):
	anim.get("parameters/Attack/playback").start(_anim, true)
	
func animation_cancel():
	entity.switch_action_state_name("NormalState")
