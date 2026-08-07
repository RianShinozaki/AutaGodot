extends Node2D

@export var scroll_speed: Vector2
@export var scroll_range: Vector2
var origin: Vector2
var scroll: Vector2

func _ready():
	origin = global_position
	
func _physics_process(delta):
	global_position = origin + scroll
	scroll += scroll_speed * delta
	scroll.x = wrap(scroll.x, 0, scroll_range.x)
	scroll.y = wrap(scroll.y, 0, scroll_range.y)
