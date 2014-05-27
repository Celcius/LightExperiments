// Shader created with Shader Forge Beta 0.32 
// Shader Forge (c) Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:0.32;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,limd:1,uamb:True,mssp:True,lmpd:False,lprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,hqsc:True,hqlp:False,blpr:2,bsrc:0,bdst:0,culm:0,dpts:2,wrdp:False,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1,x:32360,y:32656|emission-21-OUT,alpha-20-OUT;n:type:ShaderForge.SFN_TexCoord,id:2,x:33175,y:32754,uv:0;n:type:ShaderForge.SFN_Append,id:3,x:32965,y:32778|A-2-V,B-2-U;n:type:ShaderForge.SFN_Tex2d,id:4,x:32779,y:32778,ptlb:node_4,ptin:_node_4,tex:8a98ef45bc7df804d95fd945abd2c943,ntxv:0,isnm:False|UVIN-3-OUT;n:type:ShaderForge.SFN_Clamp,id:5,x:32735,y:32568|IN-4-RGB,MIN-6-OUT,MAX-9-OUT;n:type:ShaderForge.SFN_Vector1,id:6,x:33061,y:32543,v1:0;n:type:ShaderForge.SFN_ValueProperty,id:9,x:33040,y:32654,ptlb:intensity,ptin:_intensity,glob:False,v1:1;n:type:ShaderForge.SFN_Code,id:18,x:32806,y:33054,code:cgBlAHQAdQByAG4AIAAxAC0AIAAoAHMAcQByAHQAKABwAG8AdwAoAEEALQAwAC4ANQAsADIAKQAgACsAIABwAG8AdwAoAEIALQAwAC4ANQAsADIAKQApACkAKgBwAG8AdwAoADIALABDACkAOwA=,output:0,fname:Function_node_18,width:488,height:137,input:0,input:0,input:0,input_1_label:A,input_2_label:B,input_3_label:C|A-2-U,B-2-V,C-22-OUT;n:type:ShaderForge.SFN_Multiply,id:20,x:32582,y:33117|A-4-A,B-18-OUT;n:type:ShaderForge.SFN_Multiply,id:21,x:32605,y:32674|A-5-OUT,B-18-OUT;n:type:ShaderForge.SFN_ValueProperty,id:22,x:33385,y:33152,ptlb:cutout,ptin:_cutout,glob:False,v1:1;proporder:4-9-22;pass:END;sub:END;*/

Shader "Custom/Transperancy" {
    Properties {
        _node_4 ("node_4", 2D) = "white" {}
        _intensity ("intensity", Float ) = 1
        _cutout ("cutout", Float ) = 1
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        LOD 200
        Pass {
            Name "ForwardBase"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend One One
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            uniform sampler2D _node_4; uniform float4 _node_4_ST;
            uniform float _intensity;
            float Function_node_18( float A , float B , float C ){
            return 1- (sqrt(pow(A-0.5,2) + pow(B-0.5,2)))*pow(2,C);
            }
            
            uniform float _cutout;
            struct VertexInput {
                float4 vertex : POSITION;
                float4 uv0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 uv0 : TEXCOORD0;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.uv0 = v.uv0;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
////// Lighting:
////// Emissive:
                float2 node_2 = i.uv0;
                float2 node_3 = float2(node_2.g,node_2.r);
                float4 node_4 = tex2D(_node_4,TRANSFORM_TEX(node_3, _node_4));
                float node_18 = Function_node_18( node_2.r , node_2.g , _cutout );
                float3 emissive = (clamp(node_4.rgb,0.0,_intensity)*node_18);
                float3 finalColor = emissive;
/// Final Color:
                return fixed4(finalColor,(node_4.a*node_18));
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
