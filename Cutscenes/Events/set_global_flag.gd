class_name CS_SetGlobalFlag
extends CutsceneEvent

@export var flag: String
@export var value: bool

func trigger(_caller: CutsceneSequence):
	Global.set(flag, value)
