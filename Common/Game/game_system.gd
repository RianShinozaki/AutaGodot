class_name GameSystem extends Node

static var instance: GameSystem

var game_world: GameWorld

enum GameState {
	PLAY,
	DIALOGUE,
	CUTSCENE
}

static var game_state: GameState

func _ready():
	instance = self
	var _world: PackedScene = load(Global.init_scene)
	change_game_world(_world)

func change_game_world(_world: PackedScene):
	if is_instance_valid(game_world):
		game_world.queue_free()
	var _new_world: GameWorld = _world.instantiate()
	add_child(_new_world)
	game_world = _new_world
