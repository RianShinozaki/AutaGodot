extends AnimatedSprite2D

var spooked: bool = false
var direction: Vector2
var velocity: Vector2
@export var accel: float
@export var spook_sound: AudioStream

func _process(delta):
	if spooked:
		velocity += direction * accel * delta
		global_position += velocity
		modulate.a -= delta
		if modulate.a <= 0:
			queue_free()
	
func _on_area_2d_body_entered(_body):
	if spooked: return
	
	spooked = true
	direction = (global_position - _body.global_position).normalized()
	if spook_sound:
		SFXController.play_sound(spook_sound, global_position)
