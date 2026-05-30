extends Node2D

@onready var cam: GameCamera = $Cam/Camera2D

func _ready():
	await get_tree().process_frame
	cam.targets.append(Auta.instance)
