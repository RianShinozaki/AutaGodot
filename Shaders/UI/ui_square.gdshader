shader_type canvas_item;

uniform sampler2D tex: repeat_enable;
uniform sampler2D mask;
uniform vec2 tex_size;
uniform vec2 mask_size;
uniform vec2 scroll_speed;
uniform vec4 modulate : source_color;

uniform sampler2D sprite : filter_nearest;
uniform vec2 sprite_size;
uniform vec2 sprite_offset;

varying vec2 position;
varying vec2 sprite_position;
varying vec2 mask_position;

vec4 overlay(vec4 base, vec4 blend){
	vec4 limit = step(0.5, base);
	return mix(2.0 * base * blend, 1.0 - 2.0 * (1.0 - base) * (1.0 - blend), limit);
}

void vertex() {
	position = ( vec4(VERTEX, 1.0, 1.0)).xy / tex_size;
	mask_position = ( vec4(VERTEX, 1.0, 1.0)).xy / mask_size;
	sprite_position = ( vec4(VERTEX, 1.0, 1.0)).xy / sprite_size + sprite_offset;
}
void fragment() {
	ivec2 mask_tex_size = textureSize(mask, 0);
	vec2 uv_pos =  vec2(mask_size.x / float(mask_tex_size.x), mask_size.y / float(mask_tex_size.y)) ;
	COLOR = vec4(texture(tex, position + (TIME * scroll_speed)).xyz, texture(mask, mask_position).r);
	COLOR = overlay(COLOR, modulate);
	vec4 sprite_color = texture(sprite, sprite_position);
	COLOR = mix(COLOR, sprite_color, sprite_color.a * texture(mask, mask_position).g);
}
