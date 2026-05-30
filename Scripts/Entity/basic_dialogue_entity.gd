extends Area2D
@export var start_id: String

func _ready() -> void:
	body_entered.connect(on_body_entered)
	
func on_body_entered(_body: Node2D) -> void:
	$DialogueRunner.start_dialogue(start_id)
