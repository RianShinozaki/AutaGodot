extends Sprite2D

var base_scale: Vector2

func _ready():
	base_scale = global_scale
	
func _physics_process(_delta: float) -> void:
	global_scale = base_scale * 3/GameCamera.instance.zoom.x
