extends Node

# We could do a fancy grid paint system but it's not really worth it at this scale. 
# But keep that in mind if for some reason level loading does become a struggle
# Generally, entities never swap rooms once assigned so we don't need to worry about that

func _ready():
	assign_nodes_to_rooms()

func assign_nodes_to_rooms():
	var _regions: Array[Node] = get_tree().get_nodes_in_group("CamRegion")
	var _objects: Array[Node] = get_tree().get_nodes_in_group("RoomStateDetector")
	
	for _object in _objects:
		var _ob2 = _object as Node2D
		var _assigned: bool = false
		for _region in _regions:
			var _cam_region = _region as CamRegion
			var _x1: int = _cam_region.x_extents.x
			var _x2: int = _cam_region.x_extents.y
			var _y1: int = _cam_region.y_extents.x
			var _y2: int = _cam_region.y_extents.y
			if _ob2.global_position.x >= _x1 and _ob2.global_position.x < _x2:
				if _ob2.global_position.y >= _y1 and _ob2.global_position.y < _y2:
					_cam_region.add_object(_ob2)
					_assigned = true
					break
		if not _assigned: push_warning("object " + _ob2.name + " was not assigned to a room")
