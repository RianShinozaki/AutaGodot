extends Control

@export var data: DialogueData
var dialogue_box: DialogueBox 

func _ready() -> void:
	dialogue_box = get_tree().root.get_child(0).get_node('%DialogueBox')

func start_dialogue(_start_id: String) -> void:
	dialogue_box.data = data
	dialogue_box.start(_start_id)
