extends Hitbox

@export var throw_speed: float
var move_direction: Vector2

@onready var art = $Art
@onready var warning = $WarningGradient

func initiate():
	move_direction = Vector2.DOWN #( (Auta.instance.global_position + Vector2.RIGHT * randf_range(-128, 128)) - global_position).normalized()
	
func _physics_process(delta):
	#art.rotate(sign(move_direction.x) * PI * delta)
	global_position += move_direction * throw_speed * delta
	
	var _warning_position = global_position
	var _cs = GameCamera.instance.get_camera_size() - Vector2.ONE * 145
	var _cpos = GameCamera.instance.global_position
	var _cdist = (GameCamera.instance.global_position - global_position).length()
	_warning_position.x = clamp(_warning_position.x, _cpos.x - _cs.x, _cpos.x + _cs.x)
	_warning_position.y = clamp(_warning_position.y, _cpos.y - _cs.y, _cpos.y + _cs.y)
	warning.global_position = _warning_position
	$WarningGradient.modulate.a = lerp(0.0, 0.5, 1.0-(_cdist-200)/300.0)

	if global_position.y > Auta.instance.global_position.y + 500:
		queue_free()
		print("lamp despawn")
