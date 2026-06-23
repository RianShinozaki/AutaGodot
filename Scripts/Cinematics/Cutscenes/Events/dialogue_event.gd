class_name CS_DialogueEvent extends CutsceneEvent

@export var start_id: String

func trigger(_caller: CutsceneSequence):
	async = true
	var _resource: DialogueResource = _caller.dialogue_resource
	DialogueManager.show_dialogue_balloon(_resource, start_id)
	await DialogueManager.dialogue_ended
	emit_signal("finished")
