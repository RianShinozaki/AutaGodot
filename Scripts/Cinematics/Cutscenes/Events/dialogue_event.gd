class_name CS_DialogueEvent extends CutsceneEvent

@export var data: DialogueData
@export var start_id: String
var dialogue_box: DialogueBox 

func trigger(_caller: CutsceneSequence):
	async = true
	dialogue_box = _caller.get_tree().root.get_child(0).get_node('%DialogueBox')
	dialogue_box.data = data
	dialogue_box.start(start_id)
	await dialogue_box.dialogue_ended
	emit_signal("finished")
