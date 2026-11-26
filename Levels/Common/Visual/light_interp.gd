extends Node

var cam_region: CamRegion
var activated: bool

@export var auta_global_y_range: Vector2
@export var energy_range: Vector2

func _ready():
	cam_region = get_parent()
	cam_region.activate.connect(on_activated)
	cam_region.deactivate.connect(on_deactivated)
	
func on_activated():
	activated = true
	
func on_deactivated():
	activated = false

func _process(delta):
	if activated:
		var _ypos = Auta.instance.global_position.y
		if _ypos > auta_global_y_range.y:
			GameLight.instance.energy = energy_range.y
		elif _ypos < auta_global_y_range.x:
			GameLight.instance.energy = energy_range.x
		else:
			var t = (_ypos - auta_global_y_range.x ) / (auta_global_y_range.y - auta_global_y_range.x)
			GameLight.instance.energy = lerp(energy_range.x, energy_range.y, t)
