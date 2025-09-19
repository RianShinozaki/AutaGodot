extends ActionState

var inp: InputManager
var mov_param: EntityMovementParameters
var jmp_param: EntityJumpParameters
var anim: AnimationTree
var can_short_hop: bool

func _ready() -> void:
	super._ready()
	inp = entity.get_node("GenericAttributes/InputManager")
	mov_param = entity.parameters["movement"]
	jmp_param = entity.parameters["jump"]
	anim = entity.get_node("Art/AnimationTree")
	inp.action_a_just_pressed.connect(on_jump)
	entity.just_grounded.connect(just_grounded)
	
func _process(delta: float) -> void:
	if not active: return
	
	var hor = inp.input_direction.x
	#Accelerate and decelerate
	if abs(hor) > 0.1:
		entity.accelerate_x(mov_param.get_acceleration(entity) * delta * sign(hor), sign(hor) * mov_param.get_max_speed(), true)
	
	if abs(hor) < 0.1 or sign(hor) == -sign(entity.velocity.x):
		entity.accelerate_x(mov_param.get_deceleration(entity) * delta, 0, false)
	
	#Turn the player around
	var _flip_h = entity.get_node("Art").flip_h
	if(entity.velocity.x != 0 and entity.is_on_floor()):
		entity.get_node("Art").flip_h = entity.velocity.x < 0
		if(entity.get_node("Art").flip_h != _flip_h):
			play_animation_oneshot("Turn")
	
	#Update gravity
	if(entity.velocity.y > jmp_param.rising_gravity_scale): can_short_hop = false
	var _do_short_hop = can_short_hop and not inp.action_a_pressed
	entity.gravity = jmp_param.get_gravity(entity, jmp_param.short_hop_gravity_scale if _do_short_hop else 1.0)

func on_jump():
	if not active: return
	if(entity.is_on_floor()):
		can_short_hop = true
		entity.velocity.y = jmp_param.get_jump_power()

#Grounded animation handler
func just_grounded(normal: Vector2, velocity: Vector2):
	if not active: return
	play_animation_oneshot("Land")

func play_animation_oneshot(_anim: String):
	anim.get("parameters/Grounded/playback").start(_anim, true)
	
