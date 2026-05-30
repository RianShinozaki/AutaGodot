extends ActionState

var auta: Auta
var inp: InputManager
var anim: AnimationTree
var orb_param: AutaOrbParameters
@onready var orb_reform_fx_pool = $"../../SpecialAttributes/ObjectPools/OrbReform"

func _ready():
	super._ready()
	auta = entity as Auta
	inp = entity.get_node("GenericAttributes/InputManager")
	anim = entity.get_node("Art/AnimationTree")
	orb_param = entity.parameters["orb"]

func _start() -> void:
	super._start()
	entity.collision_mode = CollisionEntity.COLLISION_MODE_BOUNCE
	entity.gravity = 0
	anim.get("parameters/playback").start("Orb", true)
	entity.get_node("Art").rotation = 0
	entity.get_node("EnvironmentBox").shape = orb_param.collision_shape
	entity.get_node("EnvironmentBox").position = orb_param.collision_shape_position
	entity.get_node("Art/AfterImageGenerator").call("start_afterimages")

func _process(delta: float) -> void:
	super._process(delta)
	if not active: return
	
	if not inp.action_c_pressed or auta.is_on_floor():
		#if abs(entity.velocity.x) > speed_param.minimum_speed_skating:
		#	entity.switch_action_state_name("SpeedState")
		#else:
		entity.switch_action_state_name("NormalState")

		
func _end() -> void:
	super._end()
	entity.collision_mode = CollisionEntity.COLLISION_MODE_FLOOR
	anim.get("parameters/playback").start("Jump", true)
	auta.can_orb = false
	auta.recharge_orb = false
	auta.orb_timer = orb_param.orb_recharge_time
	
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
	
	var fx: Node2D = orb_reform_fx_pool.spawn_object()
	if fx != null:
		fx.global_position = entity.global_position;
