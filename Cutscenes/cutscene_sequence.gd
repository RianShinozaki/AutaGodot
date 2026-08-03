class_name CutsceneSequence extends Node

@export var events: Array[CutsceneEvent]
@export var dialogue_resource: DialogueResource
@export var actors: Dictionary[String, Node]

signal event_ended
signal cutscene_started
signal cutscene_finished

var active: bool

func begin():
	if active: return
	emit_signal("cutscene_started")
	GameSystem.game_state = GameSystem.GameState.CUTSCENE
	active = true
	
	for event in events:
		event.trigger(self)
		if event.async:
			await event.finished
	end()

func end():
	active = false
	emit_signal("cutscene_finished")
	GameSystem.game_state = GameSystem.GameState.PLAY

func end_event():
	emit_signal("event_ended")
