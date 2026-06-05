class_name EntityStatus

extends Node2D

@export var max_health: float
var health: float

func _ready() -> void:
	health = max_health

func add_health(_amt: float):
	health += _amt
	health = min(health, max_health)
