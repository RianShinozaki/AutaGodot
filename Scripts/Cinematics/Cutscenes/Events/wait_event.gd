class_name CS_WaitEvent extends CutsceneEvent

@export var time: float

func trigger(_caller: CutsceneSequence):
	async = true
	await _caller.get_tree().create_timer(time).timeout
	emit_signal("finished")
	
