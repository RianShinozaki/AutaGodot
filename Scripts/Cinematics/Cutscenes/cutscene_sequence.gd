class_name CutsceneSequence extends Node

@export var events: Array[CutsceneEvent]

func begin():
	for event in events:
		event.trigger(self)
		await event.finished
	end()

func end():
	pass
