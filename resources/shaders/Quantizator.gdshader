shader_type canvas_item;

group_uniforms General;
uniform bool enabled = true;
uniform int resolution_scale : hint_range(1, 64) = 16;

group_uniforms Dither;
uniform bool dithering = false;
uniform int dithering_size : hint_range(1, 16) = 2;

group_uniforms Quantizer;
uniform bool quantizing = false;
uniform sampler2D palette;
uniform int quantization_level : hint_range(2, 16) = 2;

group_uniforms Vignette;
uniform float vignette_intensity = 0.4;
uniform float vignette_opacity : hint_range(0.0, 1.0) = 0.5;

int dithering_pattern(ivec2 fragcoord) {
	const int pattern[] = {
		-4, +0, -3, +1,
		+2, -2, +3, -1,
		-3, +1, -4, +0,
		+3, -1, +2, -2
	};
	
	int x = fragcoord.x % 4;
	int y = fragcoord.y % 4;
	return pattern[y * 4 + x] * dithering_size;
}

float color_distance(vec3 a, vec3 b) {
	vec3 diff = a - b;
	return dot(diff, diff);
}

vec3 quantize_color(vec3 color, int levels) {
	if (levels <= 1) return vec3(0.0);
	float step = 1.0 / float(levels - 1);
	return round(color / step) * step;
}

float vignette(vec2 uv) {
	uv *= 1.0 - uv.xy;
	float vig = uv.x * uv.y * 15.0;
	return pow(vig, vignette_intensity * vignette_opacity);
}

void fragment() {
	vec4 final_color;
	
	if (!enabled) {
		final_color = texture(TEXTURE, UV);
	} else {
		vec2 tex_size = vec2(textureSize(TEXTURE, 0));
		
		if (tex_size.x <= 0.0 || tex_size.y <= 0.0) {
			final_color = vec4(0.0);
		} else {
			ivec2 texel_coord = ivec2(floor(UV * tex_size / float(resolution_scale)));
			vec3 color = texelFetch(TEXTURE, texel_coord * resolution_scale, 0).rgb;
			
			if (dithering) {
				int dither = dithering_pattern(texel_coord);
				color += vec3(float(dither) / 255.0);
			}
			if (quantizing) {
				color = clamp(color, 0.0, 1.0);
				color = quantize_color(color, quantization_level);
				
				int palette_size = 48;
				float closest_distance = 9999.0;
				vec3 cached_color = color;
				
				for (int i = 0; i < palette_size; i++) {
					float u = float(i) / float(palette_size - 1);
					vec3 palette_color = texture(palette, vec2(u, 0.0)).rgb;
					float dist = color_distance(cached_color, palette_color);
					if (dist < closest_distance) {
						closest_distance = dist;
						color = palette_color;
					}
				}
			}
			
			final_color = vec4(color.rgb, 1.0);
			final_color.rgb *= vignette(UV);
		}
	}
	
	COLOR = vec4(final_color.rgb, 1.0);
}