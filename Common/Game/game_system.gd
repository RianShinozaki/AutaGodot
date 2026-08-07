class_name GameSystem extends Node

static var instance: GameSystem

@onready var game_world: GameWorld = $GameWorld

enum GameState {
	PLAY,
	DIALOGUE,
	CUTSCENE
}

static var game_state: GameState

func _ready():
	instance = self

func change_game_world(_world: PackedScene):
	game_world.queue_free()
	var _new_world: GameWorld = _world.instantiate()
	add_child(_new_world)
	game_world = _new_world
