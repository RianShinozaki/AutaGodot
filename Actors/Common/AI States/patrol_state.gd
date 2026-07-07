extends AIState

@export var patrol_state: int

var patrol_counter: float = 0
var origin_position: Vector2

var inp: InputManager
var patrol_param: EntityPatrolParameters

@onready var ray_floor_r: RayCast2D = $"../../SpecialAttributes/TerrainRaycasts/RightFloor"
@onready var ray_floor_l: RayCast2D = $"../../SpecialAttributes/TerrainRaycasts/LeftFloor"
@onready var ray_wall_r: RayCast2D = $"../../SpecialAttributes/TerrainRaycasts/RightWall"
@onready var ray_wall_l: RayCast2D = $"../../SpecialAttributes/TerrainRaycasts/LeftWall"

func _ready() -> void:
	super._ready()
	inp = entity.get_node("GenericAttributes/InputManager")
	patrol_param = entity.ai_parameters["patrol"]
	origin_position = global_position

func _start() -> void:
	super._start()
	$"../../SpecialAttributes/AggroRange/CollisionShape2D".shape.radius = patrol_param.patrol_aggro_radius
	$"../../ActionStates/NormalState".auto_flip = true
	
func _process(_delta: float) -> void:
	if !active: return
	
	if entity.is_on_floor() or not patrol_param.increment_counter_only_grounded:
		patrol_counter += _delta
		
	if patrol_counter > (patrol_param.patrol_wait_time if patrol_state == 0 else patrol_param.patrol_move_time):
		if patrol_state == 0:
			var _x_offset = global_position.x - origin_position.x
			@warning_ignore("integer_division")
			if _x_offset < -patrol_param.patrol_length/2 or not ray_floor_l.is_colliding() or ray_wall_l.is_colliding():
				patrol_state = 1
			elif _x_offset > float(patrol_param.patrol_length)/2 or not ray_floor_r.is_colliding() or ray_wall_r.is_colliding():
				patrol_state = -1
			else:
				patrol_state = randi_range(0, 1) * 2 - 1
		else:
			patrol_state = 0
		patrol_counter = randf_range(-patrol_param.patrol_time_variation, patrol_param.patrol_time_variation)
	
	if patrol_state == 1 and not ray_floor_r.is_colliding():
		patrol_state = 0
	
	if patrol_state == 1 and ray_wall_r.is_colliding():
		patrol_state = 0
		
	if patrol_state == -1 and not ray_floor_l.is_colliding():
		patrol_state = 0
		
	if patrol_state == -1 and ray_wall_l.is_colliding():
		patrol_state = 0
	
	inp.set_input_direction(Vector2.RIGHT * 0.7 * patrol_state)

func _on_aggro_range_body_entered(_body: Node2D) -> void:
	if !active: return
	
	entity.switch_ai_state_name("AggroState")
	entity.aggro_target = _body
