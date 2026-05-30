class_name Desplicer

extends Area2D

var splice_active: bool

func _ready() -> void:
	$"../SpatialSplicer".splice_activate.connect(on_splice_activate)
	$"../SpatialSplicer".splice_deactivate.connect(on_splice_deactivate)
	body_entered.connect(on_body_entered)
	on_splice_deactivate()
	
func on_splice_activate():
	visible = true
	splice_active = true
	
func on_splice_deactivate():
	visible = false
	splice_active = false

func on_body_entered(_body: Node2D) -> void:
	if splice_active:
		$"../SpatialSplicer".deactivate()
