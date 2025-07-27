@tool
extends Node2D

func post_import(entity_layer: LDTKEntityLayer) -> LDTKEntityLayer:
	if(entity_layer.get_node_or_null("Entities") != null):
		var oldLayer = entity_layer.get_node_or_null("Entities")
		entity_layer.remove_child(oldLayer)
		oldLayer.queue_free()
		
	var parentNode = Node2D.new()
	parentNode.name = "Entities"
	entity_layer.add_child(parentNode)
	
	var definition: Dictionary = entity_layer.definition
	var entities: Array = entity_layer.entities
	for entity in entities:
		pass
		# Perform operations here
		var fields: Dictionary = entity.fields
		var uid = fields.UID
		var scene = load(uid)
		var instance = scene.instantiate()
		#instance.owner = get_tree().edited_scene_root
		#set_editable_instance(instance, true)
		
		parentNode.add_child(instance)
		instance.global_position = entity.position
		
		if entity.definition.hollow == true:
			instance.scale = entity.size/16
		
		if fields.has("invisible"):
			instance.modulate = Color(1, 1, 1, 0);
		
	return entity_layer
