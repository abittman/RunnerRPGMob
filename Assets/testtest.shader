// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.31 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.31;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,rfrpo:False,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:4013,x:32798,y:32626,varname:node_4013,prsc:2|diff-6595-OUT,diffpow-8129-OUT,spec-222-OUT,lwrap-1895-OUT;n:type:ShaderForge.SFN_Vector1,id:8129,x:32438,y:32663,varname:node_8129,prsc:2,v1:0.5;n:type:ShaderForge.SFN_Color,id:2072,x:31872,y:32913,ptovrint:False,ptlb:node_2072,ptin:_node_2072,varname:node_2072,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0,c2:0,c3:0,c4:1;n:type:ShaderForge.SFN_Tex2d,id:2479,x:31717,y:32567,ptovrint:False,ptlb:node_2479,ptin:_node_2479,varname:node_2479,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:0e73a12a98b367c4291cba9811c1cc69,ntxv:0,isnm:False|UVIN-2804-OUT;n:type:ShaderForge.SFN_Power,id:5810,x:32035,y:32666,varname:node_5810,prsc:2|VAL-2479-RGB,EXP-636-OUT;n:type:ShaderForge.SFN_Vector1,id:636,x:31885,y:32826,varname:node_636,prsc:2,v1:200;n:type:ShaderForge.SFN_Lerp,id:1895,x:32368,y:32841,varname:node_1895,prsc:2|A-5810-OUT,B-2072-RGB,T-771-OUT;n:type:ShaderForge.SFN_Slider,id:771,x:31940,y:33138,ptovrint:False,ptlb:lightWrapLerpval,ptin:_lightWrapLerpval,varname:node_771,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.7692308,max:1;n:type:ShaderForge.SFN_TexCoord,id:1940,x:30838,y:32606,varname:node_1940,prsc:2,uv:0;n:type:ShaderForge.SFN_Tex2d,id:1039,x:32551,y:33517,ptovrint:False,ptlb:node_1039,ptin:_node_1039,varname:node_1039,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:36dd0b22da8874ed38075789055ca664,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Power,id:9378,x:32862,y:33487,varname:node_9378,prsc:2|VAL-1039-RGB,EXP-711-OUT;n:type:ShaderForge.SFN_Vector1,id:711,x:32681,y:33442,varname:node_711,prsc:2,v1:5;n:type:ShaderForge.SFN_Bitangent,id:3853,x:31972,y:33228,varname:node_3853,prsc:2;n:type:ShaderForge.SFN_NormalVector,id:7120,x:31858,y:33373,prsc:2,pt:False;n:type:ShaderForge.SFN_Tangent,id:8961,x:31967,y:33500,varname:node_8961,prsc:2;n:type:ShaderForge.SFN_ViewVector,id:2945,x:32090,y:33564,varname:node_2945,prsc:2;n:type:ShaderForge.SFN_ViewReflectionVector,id:3635,x:32308,y:33624,varname:node_3635,prsc:2;n:type:ShaderForge.SFN_Multiply,id:9315,x:32406,y:33300,varname:node_9315,prsc:2|B-7120-OUT,C-4348-OUT;n:type:ShaderForge.SFN_Vector1,id:4348,x:32152,y:33422,varname:node_4348,prsc:2,v1:0.1;n:type:ShaderForge.SFN_Multiply,id:2345,x:32707,y:33216,varname:node_2345,prsc:2|B-9378-OUT;n:type:ShaderForge.SFN_Multiply,id:2804,x:31059,y:32505,varname:node_2804,prsc:2|A-5757-OUT,B-1940-UVOUT;n:type:ShaderForge.SFN_Vector1,id:5757,x:30926,y:32465,varname:node_5757,prsc:2,v1:3;n:type:ShaderForge.SFN_Tex2d,id:1755,x:32038,y:32240,ptovrint:False,ptlb:BaseTexture,ptin:_BaseTexture,varname:node_1755,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:bfd675cc0db1d4656b75dc6d6ba91142,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:6595,x:32290,y:32309,varname:node_6595,prsc:2|A-1755-RGB,B-9704-RGB;n:type:ShaderForge.SFN_Color,id:9704,x:32069,y:32429,ptovrint:False,ptlb:node_9704,ptin:_node_9704,varname:node_9704,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Vector1,id:222,x:32595,y:32709,varname:node_222,prsc:2,v1:0;n:type:ShaderForge.SFN_Time,id:4512,x:30870,y:32937,varname:node_4512,prsc:2;n:type:ShaderForge.SFN_Vector1,id:38,x:30870,y:33103,varname:node_38,prsc:2,v1:1;n:type:ShaderForge.SFN_Multiply,id:5819,x:31062,y:32952,varname:node_5819,prsc:2|A-4512-TTR,B-38-OUT;n:type:ShaderForge.SFN_Sin,id:3828,x:31249,y:32982,varname:node_3828,prsc:2|IN-5819-OUT;n:type:ShaderForge.SFN_RemapRange,id:8005,x:31435,y:32972,varname:node_8005,prsc:2,frmn:-1,frmx:1,tomn:0,tomx:1|IN-3828-OUT;n:type:ShaderForge.SFN_Lerp,id:2644,x:31486,y:32694,varname:node_2644,prsc:2|A-2804-OUT,T-8005-OUT;n:type:ShaderForge.SFN_Vector1,id:5054,x:30971,y:32842,varname:node_5054,prsc:2,v1:5;n:type:ShaderForge.SFN_Multiply,id:3659,x:31207,y:32779,varname:node_3659,prsc:2|B-5054-OUT;proporder:2072-2479-771-1039-1755-9704;pass:END;sub:END;*/

Shader "Shader Forge/testtest" {
    Properties {
        _node_2072 ("node_2072", Color) = (0,0,0,1)
        _node_2479 ("node_2479", 2D) = "white" {}
        _lightWrapLerpval ("lightWrapLerpval", Range(0, 1)) = 0.7692308
        _node_1039 ("node_1039", 2D) = "white" {}
        _BaseTexture ("BaseTexture", 2D) = "white" {}
        _node_9704 ("node_9704", Color) = (1,1,1,1)
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 opengl gles 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform float4 _node_2072;
            uniform sampler2D _node_2479; uniform float4 _node_2479_ST;
            uniform float _lightWrapLerpval;
            uniform sampler2D _BaseTexture; uniform float4 _BaseTexture_ST;
            uniform float4 _node_9704;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                LIGHTING_COORDS(3,4)
                UNITY_FOG_COORDS(5)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
///////// Gloss:
                float gloss = 0.5;
                float specPow = exp2( gloss * 10.0+1.0);
////// Specular:
                float NdotL = max(0, dot( normalDirection, lightDirection ));
                float node_222 = 0.0;
                float3 specularColor = float3(node_222,node_222,node_222);
                float3 directSpecular = (floor(attenuation) * _LightColor0.xyz) * pow(max(0,dot(halfDirection,normalDirection)),specPow)*specularColor;
                float3 specular = directSpecular;
/////// Diffuse:
                NdotL = dot( normalDirection, lightDirection );
                float2 node_2804 = (3.0*i.uv0);
                float4 _node_2479_var = tex2D(_node_2479,TRANSFORM_TEX(node_2804, _node_2479));
                float3 w = lerp(pow(_node_2479_var.rgb,200.0),_node_2072.rgb,_lightWrapLerpval)*0.5; // Light wrapping
                float3 NdotLWrap = NdotL * ( 1.0 - w );
                float3 forwardLight = pow(max(float3(0.0,0.0,0.0), NdotLWrap + w ), 0.5);
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = forwardLight * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb; // Ambient Light
                float4 _BaseTexture_var = tex2D(_BaseTexture,TRANSFORM_TEX(i.uv0, _BaseTexture));
                float3 diffuseColor = (_BaseTexture_var.rgb*_node_9704.rgb);
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse + specular;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 opengl gles 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform float4 _node_2072;
            uniform sampler2D _node_2479; uniform float4 _node_2479_ST;
            uniform float _lightWrapLerpval;
            uniform sampler2D _BaseTexture; uniform float4 _BaseTexture_ST;
            uniform float4 _node_9704;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                LIGHTING_COORDS(3,4)
                UNITY_FOG_COORDS(5)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
///////// Gloss:
                float gloss = 0.5;
                float specPow = exp2( gloss * 10.0+1.0);
////// Specular:
                float NdotL = max(0, dot( normalDirection, lightDirection ));
                float node_222 = 0.0;
                float3 specularColor = float3(node_222,node_222,node_222);
                float3 directSpecular = attenColor * pow(max(0,dot(halfDirection,normalDirection)),specPow)*specularColor;
                float3 specular = directSpecular;
/////// Diffuse:
                NdotL = dot( normalDirection, lightDirection );
                float2 node_2804 = (3.0*i.uv0);
                float4 _node_2479_var = tex2D(_node_2479,TRANSFORM_TEX(node_2804, _node_2479));
                float3 w = lerp(pow(_node_2479_var.rgb,200.0),_node_2072.rgb,_lightWrapLerpval)*0.5; // Light wrapping
                float3 NdotLWrap = NdotL * ( 1.0 - w );
                float3 forwardLight = pow(max(float3(0.0,0.0,0.0), NdotLWrap + w ), 0.5);
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = forwardLight * attenColor;
                float4 _BaseTexture_var = tex2D(_BaseTexture,TRANSFORM_TEX(i.uv0, _BaseTexture));
                float3 diffuseColor = (_BaseTexture_var.rgb*_node_9704.rgb);
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse + specular;
                fixed4 finalRGBA = fixed4(finalColor * 1,0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
