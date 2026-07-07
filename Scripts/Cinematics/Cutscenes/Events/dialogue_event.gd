class_name CS_DialogueEvent extends CutsceneEvent

@export var start_id: String

func scav_func():
	print("im scav")
	
func trigger(_caller: CutsceneSequence):
	#asserts that this is an async function
	async = true
	
	#Grab actors from the node running the cutscene
	var _resource: DialogueResource = _caller.dialogue_resource
	var _extra_args = []
	if not _caller.actors.is_empty():
		_extra_args.append([_caller.actors])
	
	DialogueRunner.do_dialogue(_resource, start_id, _extra_args)
	await DialogueRunner.instance.dialogue_finished
	emit_signal("finished")
