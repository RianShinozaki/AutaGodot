extends AIState

var origin_position: Vector2

var inp: InputManager
var aggro_param: EntityAggroKeepDistanceParameters
@onready var anim: AnimationTree = $"../../Art/AnimationTree"

const ATTACK_BUTTON = "B"

var attack_counter: float

func _ready() -> void:
	super._ready()
	inp = entity.get_node("GenericAttributes/InputManager")
	aggro_param = entity.ai_parameters["aggro"]
	origin_position = global_position

func _start() -> void:
	super._start()
	$"../../ActionStates/NormalState".auto_flip = false
	
func _process(_delta: float) -> void:
	if !active: return
	
	#Stop aggro if target doesn't exist
	if entity.aggro_target == null: 
		entity.switch_ai_state_name("PatrolState")
		return
	
	var targ := entity.aggro_target
	var _vec_to_target: Vector2 = global_position - targ.global_position
	var watch_dist = aggro_param.watch_distance
	var watch_dist_len = aggro_param.watch_distance_leniency
	
	#Stop aggro if target is too far
	if _vec_to_target.length() > aggro_param.aggro_radius:
		entity.switch_ai_state_name("PatrolState")
		return
	
	var look_right = _vec_to_target.x < 0
	entity.get_node("Art").flip_h = not look_right
	entity.get_node("SpecialAttributes/Hitboxes").scale = Vector2(1.0 if look_right else -1.0, 1.0)

	if abs(_vec_to_target.x) > watch_dist + watch_dist_len:
		inp.set_input_direction(Vector2.RIGHT * (1.0 if look_right else -1.0))
	elif abs(_vec_to_target.x) < watch_dist - watch_dist_len:
		inp.set_input_direction(Vector2.RIGHT * (-1.0 if look_right else 1.0))
	else:
		inp.set_input_direction(Vector2.ZERO)

	if entity.is_on_floor() or not aggro_param.increment_counter_only_grounded:
		attack_counter += _delta
		if attack_counter > aggro_param.attack_wait_time:
			attack_counter = randf_range(-aggro_param.attack_wait_time_variation, aggro_param.attack_wait_time_variation)
			inp.press_button(ATTACK_BUTTON)
