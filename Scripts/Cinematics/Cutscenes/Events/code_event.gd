class_name CS_CodeEvent extends CutsceneEvent

@export var target: NodePath
@export_multiline var code_string: String = ""

func trigger(_caller):
	if code_string.is_empty():
		return
	
	var node = _caller.get_node(target)
	if not node:
		push_error("Target node not found")
		return
	
	var lines = code_string.split("\n", false)  # false = skip empty lines
	for line in lines:
		line = line.strip_edges()
		if line.is_empty():
			continue
		
		var expr = Expression.new()
		var err = expr.parse(line)
		if err != OK:
			push_error("Parse error on '%s': %s" % [line, expr.get_error_text()])
			return
		
		expr.execute([], node)
		if expr.has_execute_failed():
			push_error("Execute error on '%s': %s" % [line, expr.get_error_text()])
			return
