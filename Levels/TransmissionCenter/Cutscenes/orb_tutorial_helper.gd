extends Node

var tutorial_stage: int = 0

@export var dialogue_resource: DialogueResource
@export var dialogue_start_id: String
@export var dialogue_start_id_2: String
@export var tough_cage_target: Breakable
@export var burrow_target: LumpyBurrow
@export var finish_cutscene_target: BasicCutsceneRunner
var lumpy_target: Lumpy

func activate():
	tutorial_stage = 1
	lumpy_target = burrow_target.lumpy_instance
	
func _physics_process(delta):
	if tutorial_stage == 0: return
	
	if tutorial_stage == 1 and lumpy_target.get_node("GenericAttributes/EntityStatus").stunned:
		tutorial_stage = 2
		await get_tree().create_timer(0.35).timeout
		get_tree().paused = true
		DialogueRunner.do_dialogue(dialogue_resource, dialogue_start_id)
		await DialogueRunner.instance.dialogue_finished
		get_tree().paused = false

	if tutorial_stage == 2 and lumpy_target.get_node("ActionStates/StunState").grabbed:
		tutorial_stage = 3
		await get_tree().create_timer(0.03).timeout
		get_tree().paused = true
		DialogueRunner.do_dialogue(dialogue_resource, dialogue_start_id_2)
		await DialogueRunner.instance.dialogue_finished
		get_tree().paused = false
		
		await tough_cage_target.just_broken
		await get_tree().create_timer(0.25).timeout
		finish_cutscene_target.begin()
