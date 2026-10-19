extends ActionState

var auta: Auta
var inp: InputManager
var anim: AnimationTree

var mov_param: EntityMovementParameters
var jmp_param: EntityJumpParameters
var can_short_hop: bool

@export var jump_sound: AudioStream
@export var land_sound: AudioStream

var grabbed_chain: ChainLink
var climb_cooldown: float = 0.25
var climb_cooldown_max: float = 0.25
var lerp_bias: float = 0

@export var swing_influence: float = 0

func _ready() -> void:
	super._ready()
	inp = entity.get_node("GenericAttributes/InputManager")
	mov_param = entity.parameters["movement"]
	jmp_param = entity.parameters["jump"]
	anim = entity.get_node("Art/AnimationTree")
	inp.action_a_just_pressed.connect(on_jump)
	inp.action_c_just_pressed.connect(on_orb)
	auta = entity as Auta

func _start() -> void:
	super._start()
	entity.get_node("EnvironmentBox").set_deferred("shape", mov_param.collision_shape)
	entity.get_node("EnvironmentBox").position = mov_param.collision_shape_position
	entity.get_node("Art/AfterImageGenerator").call("stop_afterimages")
	anim.get("parameters/playback").start("Rope", true)
	auta.recharge_orb = true
	can_short_hop = false
	climb_cooldown = climb_cooldown_max
	
func _process(delta: float) -> void:
	super._process(delta)
	
	if not active: return

	var _art: Sprite2D = entity.get_node("Art")
	var _to_angle: float = 0
	_art.rotation = lerp_angle(_art.rotation,_to_angle,delta * 10)
	entity.velocity = Vector2.ZERO
	var _pos: Vector2 = grabbed_chain.get_node("RigidBody2D").global_position
	var _rot: float = grabbed_chain.get_node("RigidBody2D").global_rotation
	entity.global_position = lerp(global_position, _pos + Vector2(-sin(_rot), cos(_rot))*20, lerp_bias)
	entity.global_rotation = _rot
	lerp_bias = move_toward(lerp_bias, 1.0, delta * 5)
	climb_cooldown = move_toward(climb_cooldown, 0, delta)
	var vert = inp.input_direction.y
	if vert < -0.5:
		if climb_cooldown == 0:
			grabbed_chain = grabbed_chain.get_higher_chain()
			climb_cooldown = climb_cooldown_max
			lerp_bias = 0
			anim.get("parameters/playback").start("Rope", true)
	
	if vert > 0.5:
		if climb_cooldown == 0:
			grabbed_chain = grabbed_chain.get_lower_chain()
			climb_cooldown = climb_cooldown_max
			lerp_bias = 0
			anim.get("parameters/playback").start("Rope", true)

func _physics_process(_delta):
	
	if not active: return
	var _link_rb: RigidBody2D = grabbed_chain.get_node("RigidBody2D")
	var hor = inp.input_direction.x
	if abs(hor) > 0.5:
		var force_dir: Vector2 = grabbed_chain.vector_to_anchor().normalized().rotated(PI/2)
		_link_rb.apply_force(force_dir * sign(hor) * swing_influence)
	
	_link_rb.apply_force(Vector2.DOWN * swing_influence/2)
		
func _end() -> void:
	anim.get("parameters/playback").start("Grounded", true)
	entity.global_rotation = 0
	super._end()
	
func on_jump():
	if not active: return
	entity.switch_action_state_name("NormalState")
	anim.get("parameters/playback").start("Grounded", true)
	
	grabbed_chain.player_just_detached()
	can_short_hop = true
	entity.velocity.y = jmp_param.get_jump_power()
	SFXController.play_sound(jump_sound, global_position)
	
	var _link_rb: RigidBody2D = grabbed_chain.get_node("RigidBody2D")
	entity.velocity += _link_rb.linear_velocity

func on_orb():
	if not active: return
	if auta.can_orb and auta.unlocked_orb:
		entity.switch_action_state_name("OrbState")
		grabbed_chain.player_just_detached()


func play_animation_oneshot(_anim: String):
	anim.get("parameters/Grounded/playback").start(_anim, true)
