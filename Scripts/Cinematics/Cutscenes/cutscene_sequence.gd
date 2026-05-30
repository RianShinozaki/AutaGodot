class_name CutsceneSequence extends Node

@export var events: Array[CutsceneEvent]

signal event_ended

func begin():
	for event in events:
		event.trigger(self)
		if event.async:
			await event.finished
	end()

func end():
	pass

func end_event():
	emit_signal("event_ended")
