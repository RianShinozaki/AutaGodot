shader_type canvas_item;
uniform sampler2D screen_texture: hint_screen_texture;

uniform sampler2D LightGradient : filter_nearest, repeat_disable;
uniform vec4 outline_color_left : source_color;
uniform vec4 outline_color_right : source_color;
uniform float stunFX;

varying vec4 color;
void vertex() {
	
	color = vec4(UV.x, UV.y, 0.0, 1.0);
}

void fragment() {
	float size_x = 1.0/float(textureSize(screen_texture, 0).x);
	float size_y = 1.0/float(textureSize(screen_texture, 0).y);
	
	vec4 sprite_color = texture(screen_texture, SCREEN_UV);
	sprite_color.r = mix(sprite_color.r, round(sprite_color.r), stunFX);
	sprite_color.g = mix(sprite_color.g, round(sprite_color.g), stunFX);
	sprite_color.b = mix(sprite_color.b, round(sprite_color.b), stunFX);
	

	
	//COLOR = vec4(color.r, color.g, color.b, texture(TEXTURE, UV).a);
	COLOR = vec4(sprite_color.rgb, 1.0);
}

void light() {
	float light = dot(normalize(NORMAL), LIGHT_DIRECTION) * LIGHT_ENERGY;
	float gradient = texture(LightGradient, vec2(light, light)).r;
	float cNdotL = max(0.0, gradient);
  	LIGHT = mix(vec4((LIGHT_COLOR.rgb * LIGHT_ENERGY) * cNdotL, LIGHT_COLOR.a), vec4(0,0,0,0), stunFX);
}
