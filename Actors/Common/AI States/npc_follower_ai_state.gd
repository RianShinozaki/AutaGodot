extends AIState

var inp: InputManager

@onready var ray_floor_r: RayCast2D = $"../../SpecialAttributes/TerrainRaycasts/RightFloor"
@onready var ray_floor_l: RayCast2D = $"../../SpecialAttributes/TerrainRaycasts/LeftFloor"
@onready var ray_wall_r: RayCast2D = $"../../SpecialAttributes/TerrainRaycasts/RightWall"
@onready var ray_wall_l: RayCast2D = $"../../SpecialAttributes/TerrainRaycasts/LeftWall"

@export var to_follow: CollisionEntity

const FOLLOW_DISTANCE = 16
const VERTICAL_FOLLOW_DISTANCE = 32
const VERTICAL_FOLLOW_DISTANCE_GROUNDED = 64
func _ready() -> void:
	super._ready()
	inp = entity.get_node("GenericAttributes/InputManager")

func _start() -> void:
	super._start()
	$"../../ActionStates/NormalState".auto_flip = true
	
func _process(_delta: float) -> void:
	if !active: return
	inp = entity.get_node("GenericAttributes/InputManager")
	if to_follow.global_position.x > global_position.x + FOLLOW_DISTANCE:
		inp.set_input_direction(Vector2(1, 0))
	elif to_follow.global_position.x < global_position.x - FOLLOW_DISTANCE*2:
		inp.set_input_direction(Vector2(-1, 0))
	else:
		inp.set_input_direction(Vector2(0, 0))
	
	var do_jump: bool = false
	do_jump = not to_follow.is_on_floor() and to_follow.global_position.y < global_position.y - VERTICAL_FOLLOW_DISTANCE
	do_jump = do_jump or to_follow.global_position.y < global_position.y - VERTICAL_FOLLOW_DISTANCE_GROUNDED
	do_jump = do_jump or ray_wall_r.is_colliding() or ray_wall_l.is_colliding()
	do_jump = do_jump or not ray_floor_r.is_colliding() and to_follow.global_position.y < global_position.y-4
	do_jump = do_jump or not ray_floor_l.is_colliding() and to_follow.global_position.y < global_position.y-4
	if do_jump:
		inp.press_button("a")
		inp.action_a_pressed = true
		
	elif to_follow.velocity.y > -40 and entity.velocity.y < -200:
		inp.action_a_pressed = false
	
