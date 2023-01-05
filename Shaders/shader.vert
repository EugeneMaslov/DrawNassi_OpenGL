#version 330 core

// the position variable has attribute position 0
layout(location = 0) in vec3 aPosition;  

// This is where the color values we assigned in the main program goes to
layout(location = 1) in vec3 aColor;

layout(location = 2) in vec2 aTexCoords;

out vec3 ourColor; // output a color to the fragment shader
out vec2 TexCoords;

uniform mat4 model;
uniform mat4 position;


void main(void)
{
	// see how we directly give a vec3 to vec4's constructor
    gl_Position = vec4(aPosition, 1.0) * model * position;

	// We use the outColor variable to pass on the color information to the frag shader
	ourColor = aColor;

	TexCoords = aTexCoords;
}