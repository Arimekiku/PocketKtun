shader_type canvas_item;

uniform bool shader_enabled = true;

uniform sampler2D palette;
uniform bool dithering = false;
uniform bool quantizing = false;
uniform int dithering_size : hint_range(1, 16) = 2;
uniform int resolution_scale : hint_range(1, 64) = 16;
uniform int quantization_level : hint_range(2, 16) = 2;
uniform float levels : hint_range(1, 100, 1) = 1;
uniform vec2 gamma = vec2(0.1, 10);

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

vec3 quantize_color(vec3 color, int q_levels) {
	if (q_levels <= 1) return vec3(0.0);
	float step = 1.0 / float(q_levels - 1);
	return round(color / step) * step;
}

void fragment() {
	vec4 final_color;
	
	if (!shader_enabled) {
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
				color = vec3(0.0);
				
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
			
			final_color = vec4(color, 1.0);
			final_color.rgb = pow(final_color.rgb, vec3(gamma.x));
			
			float grayscale = max(final_color.r, max(final_color.g, final_color.b));
			float lower = floor(grayscale * levels) / levels;
			float higher = ceil(grayscale * levels) / levels;
			
			float lower_diff = abs(lower - grayscale);
			float higher_diff = abs(higher - grayscale);
			float level = lower_diff < higher_diff ? lower : higher;
			float adj = level / grayscale;
			
			final_color.rgb *= adj;
			final_color.rgb = pow(final_color.rgb, vec3(gamma.y));
		}
	}
	
	COLOR = final_color;
}