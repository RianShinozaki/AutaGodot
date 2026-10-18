class_name SpiderDroneCrawl 
extends ActionState

@onready var forward_cast: RayCast2D = $"../../SpecialAttributes/TerrainRaycasts/Forward"
var param: SpiderDroneParam
var grounded_buffer: float = 0

func _ready():
	super._ready()
	param = entity.parameters["normal"]

func _process(delta):
	if not active: return
	
	if entity.is_on_floor(): #Look ahead for walls to climb (rotating up)
		entity.velocity.x = param.crawl_speed
		forward_cast.target_position.x = 12.0
		forward_cast.position.y = -15
		grounded_buffer = 0.3
	elif grounded_buffer > 0: #Look behind for walls to climb (rotating down)
		entity.move(-Vector2.UP*14, 1.0/60, false)
		forward_cast.target_position.x = -12.0
		forward_cast.position.y = 0
		grounded_buffer -= delta
		
	if forward_cast.is_colliding(): #Found a wall, cling to it
		forward_cast.position.y = -15
		forward_cast.target_position.x = 12.0
		entity.rotation = forward_cast.get_collision_normal().angle() + PI/2
		entity.global_position = forward_cast.get_collision_point()
		entity.up_direction = Vector2.from_angle(entity.rotation - PI/2)
		entity.gravity_direction = -entity.up_direction
		entity.move(-Vector2.UP*14, 1.0/60, false)
	
	if not entity.is_on_floor() and grounded_buffer <= 0: #Found no walls in time, falling
		entity.up_direction = Vector2.UP
		entity.gravity_direction = -Vector2.UP
