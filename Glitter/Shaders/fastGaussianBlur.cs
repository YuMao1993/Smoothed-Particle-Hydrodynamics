#version 450 core

precision highp float;

//image size
layout(location = 0) uniform int width;
layout(location = 1) uniform int height;
layout(location = 2) uniform vec2 po1;
layout(location = 3) uniform vec2 hpo1;
layout(location = 4) uniform vec2 po2;
layout(location = 5) uniform vec2 hpo2;

//input&output image texture
layout( binding = 0) uniform sampler2D inputImg;
layout(binding = 1, rgba32f) uniform image2D outputImg;

//local work group size
layout(local_size_x = 32,
       local_size_y = 32) in;

vec3 GaussianBlur(sampler2D tex0, vec2 centreUV, vec2 halfPixelOffset, vec2 pixelOffset);

void main()
{
    //acquire ID
    vec2 coord = gl_GlobalInvocationID.xy;
    vec2 uv = (coord+vec2(0.5,0.5))/vec2(width, height);

    //only compute pixels falling inside the image
    if(coord.x < width && coord.y < height)
    {
      vec3 result = GaussianBlur(inputImg, uv, hpo1, po1);
      imageStore(outputImg, ivec2(coord), vec4(result,1));
    }
}


// automatically generated by GenerateGaussFunctionCode in GaussianBlur.h                                                                                            
vec3 GaussianBlur( sampler2D tex0, vec2 centreUV, vec2 halfPixelOffset, vec2 pixelOffset )                                                                           
{                                                                              
    vec3 colOut = vec3( 0, 0, 0 );                                                                                                                                   
                                                                                                                                                                     
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////;
    // Kernel width 15 x 15
    //
    const int stepCount = 4;
    //
    const float gWeights[stepCount] ={
       0.24961,
       0.19246,
       0.05148,
       0.00645
    };
    const float gOffsets[stepCount] ={
       0.64434,
       2.37885,
       4.29111,
       6.21661
    };
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////;
                                                                                                                                                                     
    for( int i = 0; i < stepCount; i++ )                                                                                                                             
    {                                                                                                                                                                
        vec2 texCoordOffset = gOffsets[i] * pixelOffset;                                                                                                           
        vec3 col = texture( tex0, centreUV + texCoordOffset ).xyz + texture( tex0, centreUV - texCoordOffset ).xyz;                                                
        colOut += gWeights[i] * col;                                                                                                                               
    }                                                                                                                                                                
                                                                                                                                                                     
    return colOut;                                                                                                                                                   
}   