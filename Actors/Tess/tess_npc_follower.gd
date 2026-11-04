extends AIEntity

func unset_all():
	switch_action_state_name("NormalState")
	velocity.x = 0
	get_node("GenericAttributes/InputManager").set_input_direction(Vector2.ZERO)
