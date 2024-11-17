shader_type canvas_item;

uniform sampler2D LightGradient : filter_nearest, repeat_disable;

void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	COLOR = texture(TEXTURE, UV);
}

void light() {
	float light = dot(normalize(NORMAL), LIGHT_DIRECTION) * LIGHT_ENERGY;
	float gradient = texture(LightGradient, vec2(light, light)).r;
	float cNdotL = max(0.0, gradient);
  	LIGHT = vec4((LIGHT_COLOR.rgb * LIGHT_ENERGY) * cNdotL, LIGHT_COLOR.a);
}
