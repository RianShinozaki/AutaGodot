class_name AfterImageGenerator

extends ObjectPool

@onready var sprite: Sprite2D = get_parent()
@export var time_between_after_images: float
var active: bool
var after_image_timer: float

func _process(delta: float) -> void:
	if not active: return
	after_image_timer += delta
	if after_image_timer >= time_between_after_images:
		var _spawn: AfterImage = spawn_object() as AfterImage
		#if _spawn != null:
			
		after_image_timer = 0
