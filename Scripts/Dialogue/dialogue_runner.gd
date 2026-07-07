class_name DialogueRunner extends CanvasLayer


static var instance: DialogueRunner
signal dialogue_finished

func _enter_tree():
	instance = self

static func do_dialogue(_resource, _start_id, _args = []):
	var _ex_states = []
	if not _args.is_empty():
		_ex_states = _args[0]
		
	instance.do_dialogue_inst(_resource, _start_id, _ex_states)
	
func do_dialogue_inst(_resource, _start_id, _extra_game_states = []):
	var _ex_states = [self]
	_ex_states.append_array(_extra_game_states)
	DialogueManager.show_dialogue_balloon(_resource, _start_id, _ex_states)
	await DialogueManager.dialogue_ended
	emit_signal("dialogue_finished")
	
func scav_func():
	print("i am scav")
