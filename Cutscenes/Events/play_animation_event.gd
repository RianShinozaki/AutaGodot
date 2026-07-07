class_name CS_PlayAnimationEvent extends CutsceneEvent

@export var anim_name: StringName

var active: bool = false
var caller: CutsceneSequence

func trigger(_caller: CutsceneSequence):
	async = true
	active = true
	caller = _caller
	caller.event_ended.connect(end_early)
	var _anim_player: AnimationPlayer = caller.get_node("AnimationPlayer")
	_anim_player.active = true
	_anim_player.play(anim_name)
	await _anim_player.animation_finished
	if active:
		active = false
		caller.event_ended.disconnect(end_early)
		emit_signal("finished")
		

func end_early():
	active = false
	caller.event_ended.disconnect(end_early)
	emit_signal("finished")
	
