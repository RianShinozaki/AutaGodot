extends DirectionalLight2D


# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass

func lerp_color(_color: Color, _time: float = 0):
	if _time <= 0:
		color = _color
	else:
		get_tree().create_tween().tween_property(self, "color", _color, _time)

func lerp_energy(_energy: float, _time: float = 0):
	if _time <= 0:
		energy = _energy
	else:
		get_tree().create_tween().tween_property(self, "energy", _energy, _time)
