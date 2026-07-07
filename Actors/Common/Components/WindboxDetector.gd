class_name WindboxDetector extends Node2D

@onready var entity: CollisionEntity = $"../.."
@export var hurtbox: Hurtbox
@export_range(0, 1)  var windbox_resistance: float
@export var windboxes_entered: Array[Windbox]

func _ready():
	hurtbox.area_entered.connect(on_area_entered)
	hurtbox.area_exited.connect(on_area_exited)


func on_area_entered(area: Area2D):
	print("collision")
	if area is Windbox:
		windboxes_entered.append(area as Windbox)
		
func on_area_exited(area: Area2D):
	if area is Windbox and (area as Windbox) in windboxes_entered:
		windboxes_entered.erase(area as Windbox)

func _process(_delta):
	for _w: Windbox in windboxes_entered:
		entity.velocity += _w.power * Vector2.RIGHT.rotated(deg_to_rad(_w.direction)) * _delta * (1 - windbox_resistance)
