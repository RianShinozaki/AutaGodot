class_name SignalIfFlag
extends Node2D

signal trigger
@export var flag: String
@export var value: bool = true

func _ready():
	if Global.get(flag) == value:
		emit_signal("trigger")
		print(get_parent().name)
