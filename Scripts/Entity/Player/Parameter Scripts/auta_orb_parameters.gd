class_name AutaOrbParameters extends EntityMovementParameters

##The speed at which Auta launches in her orb
@export var launch_speed: float
##A little vertical speed when launching forward on the ground
@export var launch_y_speed: float
##The minimum time you must spend in the orb
@export var minimum_time: float
##Base gravity
@export var gravity: float
##Gravity at the beginning of the launch
@export var initial_gravity: float
##How long the initial launch gravity lasts
@export var initial_gravity_time: float
##The speed at which Auta emerges from her orb
@export var exit_speed: float
##How long after the player touches the ground can they orb again
@export var orb_recharge_time: float
##How long after a bounce can you do a jump
@export var orb_to_jump_buffer_time: float
