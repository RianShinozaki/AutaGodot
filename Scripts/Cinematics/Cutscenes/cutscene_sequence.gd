class_name CutsceneSequence extends Node

@export var events: Array[CutsceneEvent]

signal event_ended
signal cutscene_started
signal cutscene_finished

func begin():
	emit_signal("cutscene_started")
	for event in events:
		event.trigger(self)
		if event.async:
			await event.finished
	end()

func end():
	emit_signal("cutscene_finished")

func end_event():
	emit_signal("event_ended")
