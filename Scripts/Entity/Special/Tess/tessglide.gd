class_name TessGlide

extends CollisionEntity

var auta: Auta
var glide_param: TessGlideParameters
@onready var art: Sprite2D = $Art

func _ready():
	auta = Auta.instance
	glide_param = parameters["Glide"]


func _process(_delta):
	velocity.x -= velocity.x * glide_param.air_drag_coef * _delta
	art.flip_h = false if velocity.x > 0 else true
	
	var _a = abs(velocity.x / glide_param.gravity_falling_min_hspeed)
	_a = clamp(_a, 0, 1)
	velocity.y = min(velocity.y, lerp(glide_param.max_fall_speed, glide_param.min_fall_speed, _a))
	

func _on_orb_grab_box_area_entered(area):
	velocity = auta.velocity;
	auta.velocity = Vector2.ZERO
	auta.switch_action_state_name("TessGlideState")
	switch_action_state_name("GrabState")
