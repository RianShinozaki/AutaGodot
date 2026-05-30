extends ActionState

var auta: Auta
@onready var tess: TessGlide = $"../.."
var glide_param: TessGlideParameters

func _ready():
	auta = Auta.instance
	glide_param = tess.parameters["Glide"]

func _process(delta):
	if active:
		if tess.velocity.y > 0:
			tess.gravity = glide_param.gravity_falling
		else:
			tess.gravity = glide_param.gravity_rising
			
		if Input.is_action_just_pressed("TessSummon"):
			tess.global_position = auta.global_position + Vector2.UP * 5
			tess.velocity.x = auta.velocity.x
			if auta.velocity.x == 0:
				tess.velocity.x += 90 * (-1 if auta.get_node("Art").flip_h else 1)
			else:
				tess.velocity.x += 90 * sign(auta.velocity.x)
			tess.velocity.y = -230
