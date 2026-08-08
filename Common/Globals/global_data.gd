class_name GlobalData
extends Node

@export var save_point_pos: Vector2
@export var auta_hp: float

@export var do_tc_intro_cutscene: bool
@export var tess_cutscene_3: bool
@export var visited_scanner: bool
@export var do_scanner_return_cutscene: bool

func _ready():
	SaveSystem.LoadData(1, 0)

func save():
	SaveSystem.SaveData(1, 0)
