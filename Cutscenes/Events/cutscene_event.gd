class_name CutsceneEvent extends Resource

var async: bool = false

signal finished

func trigger(_caller: CutsceneSequence):
	emit_signal("finished")
