extends Sprite2D
@export var expand_speed: float
@export var fade_speed: float

func _process(delta):
	global_scale += expand_speed * Vector2.ONE * delta
	modulate.a -= fade_speed * delta
	if modulate.a <= 0:
		queue_free()
