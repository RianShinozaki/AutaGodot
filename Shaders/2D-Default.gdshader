shader_type canvas_item;
uniform sampler2D LightGradient : filter_nearest, repeat_disable;
uniform sampler2D emission_map : filter_nearest, repeat_disable;
uniform float emission_strength = 0.0;
uniform vec4 outline_color_left : source_color;
uniform vec4 outline_color_right : source_color;
uniform float stunFX;

varying vec4 color;
void vertex() {
	
	color = vec4(UV.x, UV.y, 0.0, 1.0);
}

void fragment() {
	float size_x = 1.0/float(textureSize(TEXTURE, 0).x);
	float size_y = 1.0/float(textureSize(TEXTURE, 0).y);
	
	vec4 sprite_color = COLOR;
	sprite_color.r = mix(sprite_color.r, round(sprite_color.r), stunFX);
	sprite_color.g = mix(sprite_color.g, round(sprite_color.g), stunFX);
	sprite_color.b = mix(sprite_color.b, round(sprite_color.b), stunFX);
	
	float alpha = -4.0 * sprite_color.a;
	alpha += texture(TEXTURE, UV + vec2(size_x, 0)).a;
	alpha += texture(TEXTURE, UV + vec2(0, -size_y)).a;
	alpha = mix(0, alpha, stunFX);
	vec4 final_color = mix(sprite_color, outline_color_left, clamp(alpha, 0.0, 1.0));
	
	alpha = -4.0 * final_color.a;
	alpha += texture(TEXTURE, UV + vec2(-size_x, 0)).a;
	alpha += texture(TEXTURE, UV + vec2(0, size_y)).a;
	alpha = mix(0, alpha, stunFX);
	final_color = mix(final_color, outline_color_right, clamp(alpha, 0.0, 1.0));
	
	//COLOR = vec4(color.r, color.g, color.b, texture(TEXTURE, UV).a);
	COLOR = vec4(final_color.rgb, clamp(abs(alpha) + sprite_color.a, 0.0, 1.0));
}

void light() {
	//float light = dot(normalize(NORMAL), LIGHT_DIRECTION) * LIGHT_ENERGY;
	//float gradient = texture(LightGradient, vec2(light, light)).r;
	//float cNdotL = max(0.0, gradient);
  	//LIGHT = mix(vec4((LIGHT_COLOR.rgb * LIGHT_ENERGY) * cNdotL, LIGHT_COLOR.a), vec4(0,0,0,0), stunFX);
	
	
	LIGHT = vec4(LIGHT_COLOR.rgb * COLOR.rgb * LIGHT_ENERGY, LIGHT_COLOR.a);
	vec4 emission_color = texture(emission_map, UV);
	LIGHT = mix(LIGHT, emission_color, emission_strength*emission_color.a);
}
