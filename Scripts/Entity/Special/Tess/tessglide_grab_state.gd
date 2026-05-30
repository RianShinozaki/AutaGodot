extends ActionState

var auta: Auta
var glide_param: TessGlideParameters
@onready var input: InputManager = $"../../Attributes/InputManager"
@onready var tess: TessGlide = $"../.."

func _ready():
	auta = Auta.instance
	glide_param = tess.parameters["Glide"]

func _process(_delta):
	if active:
		auta.global_position = global_position + Vector2.DOWN * 5
		var _hor: float = input.input_direction.x
		tess.velocity.x += _hor * glide_param.air_acceleration * _delta
		if tess.velocity.y > 0:
			tess.gravity = glide_param.gravity_falling
		else:
			tess.gravity = glide_param.gravity_rising
			
		if auta.current_action_state.name != "TessGlideState":
			tess.switch_action_state_name("NormalState")
