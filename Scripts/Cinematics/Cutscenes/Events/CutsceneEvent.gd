class_name CutsceneEvent extends Resource

signal finished

func trigger(_caller: CutsceneSequence):
	emit_signal("finished")
