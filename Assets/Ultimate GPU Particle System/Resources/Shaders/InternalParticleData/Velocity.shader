Shader "GPUParticles/Internal/Velocity"
{
	Properties
	{
		//Particle Data
		_NewParticle("New Particle", 2D) = "white" {}
		_Meta("Meta", 2D) = "white" {}
		_Velocity ("Velocity", 2D) = "white" {}
		_Position("Position", 2D) = "white" {}

		//Emitter Data
		_EmitterDirection("Emitter Direction", vector) = (0,1,0,1)
		_EmitterParam("Emitter Parameters", vector) = (1,1,0,0)
		_EmitterRandomness("Emitter Randomness", float) = 1
		_EmitterVelocity ("Emitter Velocity", vector) = (0,0,0,0)

		//Mesh emitter and targets
		_MeshEmitter("Mesh Emitter Positions", 2D) = "white" {}
		_MeshTarget("Mesh Target Positions", 2D) = "white" {}
		_MeshTargetStrength("Mesh Target Strength", float) = 0
		_MeshTargetDistance("Mesh Target Distance", float) = 0

		//Start Values
		_MaxVelocity("Max Velocity", float) = 5
		_StartLifeTimeSpeed("_StartLifeTimeSpeed", vector) = (0,0,0,0)

		//Turbulence
		_Turbulence("Turbulence", 2D) = "white" {}
		_TurbulenceDDD("Turbulence 3D", 3D) = "white" {}
		_Tightness("Tightness", Range(0,1)) = 1
			
		//Forces
		_Gravity("Gravity", float) = 9.81
		_AirResistance("Air Resistance", float) = 0
		_ForceOverLifetime("Force over lifetime", vector) = (0,0,0,0)
		_CircularForceOverLifetime("Circular force over lifetime", vector) = (0,0,0,0)
		_CircularForceCenter("Center point of circular force", vector) = (0,0,0,0)
	}
		SubShader
		{
			Cull Off ZWrite Off ZTest Always

			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma target 3.0
				#pragma multi_compile POINT EDGE CIRCLE BOX HEMISPHERE SPHERE CONE MESH
				#pragma multi_compile __ LOCALSIM
				#pragma multi_compile __ ATTRACTORS
				//#pragma multi_compile __ MESHTARGET
				#pragma multi_compile __ LIMITVELOCITY
				#pragma multi_compile __ TEXTURE VECTORFIELDS
				#pragma multi_compile __ CIRCULAR_FORCE

				#include "UnityCG.cginc"
				#include "../Includes/GPUParticles.cginc"

				//Particle Data
				sampler2D _NewParticle;
				sampler2D _Meta;
				sampler2D _Position;
				sampler2D _Velocity;
				sampler2D _MeshEmitter;
				sampler2D _MeshTarget;

				//Forces
				float _Gravity;
				float _AirResistance;
				float4 _StartLifeTimeSpeed;
				float3 _ForceOverLifetime;
				float4 _CircularForceOverLifetime;
				float3 _CircularForceCenter;

				//Turbulence
				float4 _Amplitude = 0;
				float4 _Frequency = 3;
				float4 _Offset = 0;
				sampler2D _Turbulence;
				sampler3D _TurbulenceDDD;
				float4x4 _TurbulenceMatrix;
				float _Tightness;

				//Emitter settings
				float4 _EmitterVelocity;
				float4 _EmitterDirection;
				float4 _EmitterParam;
				float4x4 _EmitterMatrix;

				float4x4 _TargetMatrix;

				float _CustomTime;
				float _CustomDeltaTime;
			
				float _MaxVelocity;

				float _MeshTargetStrength;
				float _MeshTargetDistance;

				//ATTRACTORS
				#define ATTRACTORCOUNT 4
				uniform float4 _Attractor[ATTRACTORCOUNT];		//XYZ = Position, W = Attenuation
				uniform float _Strength[ATTRACTORCOUNT];

				struct appdata
				{
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
				};

				struct v2f
				{
					float4 vertex : SV_POSITION;
					float2 uv : TEXCOORD0;
					float4 gravity : TEXCOORD1;
				};

				v2f vert (appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.gravity = float4(0,1,0,0) * _Gravity;
					o.uv = v.uv;
					return o;
				}
		
			float4 frag (v2f i) : SV_Target
			{
				fixed particle = tex2D(_NewParticle, i.uv).r;
				float4 meta = tex2D(_Meta, i.uv);
				float4 vel = tex2D(_Velocity, i.uv);
				float4 pos = tex2D(_Position, i.uv);

				float EndTime = meta.g - meta.r;
				float Time = _CustomTime - meta.r;
				float Progress = Time / EndTime;

				float StartSpeed = lerp(_StartLifeTimeSpeed.z, _StartLifeTimeSpeed.w, pos.a);
				float4 result0 = float4(0,0,0,0);//Particle is new
				float4 result1 = float4(0,0,0,0);//Particle is not new and it is alive
				float4 result2 = float4(0,0,0,0);//Particle is dead

				#ifdef LOCALSIM
					//Local Simulation
					#ifdef CIRCLE
						result0 = GetRandomVelocityDiscSurfaceLocal(i.uv, StartSpeed) + _EmitterVelocity;
					#endif

					#ifdef EDGE
						result0 = GetRandomVelocityEdgeLocal(i.uv, StartSpeed) + _EmitterVelocity;
					#endif

					#ifdef CONE
						result0 = GetRandomVelocityConeLocal(i.uv, _EmitterParam.x, _EmitterParam.w, _EmitterParam.z, StartSpeed) + _EmitterVelocity;
					#endif

					#ifdef BOX
						result0 = GetRandomVelocityBoxLocal(i.uv, StartSpeed) + _EmitterVelocity;
					#endif

					#ifdef HEMISPHERE
						result0 = GetRandomVelocityHemiSphereVolumeLocal(i.uv, StartSpeed) + _EmitterVelocity;
					#endif

					#ifdef MESH
						result0 = GetNormalOnMeshLocal(_MeshEmitter, StartSpeed, i.uv) + _EmitterVelocity;
					#endif

					//Local & World Simulation
					#ifdef SPHERE
						result0 = GetRandomVelocitySphere(i.uv, StartSpeed) + _EmitterVelocity;
					#endif

					#ifdef POINT
						result0 =  GetRandomVelocity(i.uv, StartSpeed) + _EmitterVelocity;
					#endif
				#else
					//World Simulation
					#ifdef CIRCLE
						result0 = GetRandomVelocityDiscSurfaceWorld(i.uv, StartSpeed, _EmitterMatrix) + _EmitterVelocity;
					#endif

					#ifdef EDGE
						result0 = GetRandomVelocityEdgeWorld(i.uv, StartSpeed, _EmitterMatrix) + _EmitterVelocity;
					#endif

					#ifdef CONE
						result0 = GetRandomVelocityConeWorld(i.uv, _EmitterParam.x, _EmitterParam.w, _EmitterParam.z, StartSpeed, _EmitterMatrix) + _EmitterVelocity;
					#endif

					#ifdef BOX
						result0 = GetRandomVelocityBoxWorld(i.uv, StartSpeed, _EmitterMatrix) + _EmitterVelocity;
					#endif

					#ifdef HEMISPHERE
						result0 = GetRandomVelocityHemiSphereVolumeWorld(i.uv, StartSpeed, _EmitterMatrix) + _EmitterVelocity;
					#endif

					#ifdef MESH
						result0 =  GetNormalOnMeshWorld(_MeshEmitter, StartSpeed, i.uv, _EmitterMatrix) + _EmitterVelocity;
					#endif

					//Local & World Simulation
					#ifdef SPHERE
						result0 = GetRandomVelocitySphere(i.uv, StartSpeed) + _EmitterVelocity;
					#endif

					#ifdef POINT
						result0 =  GetRandomVelocity(i.uv, StartSpeed) + _EmitterVelocity;
					#endif
				#endif
								
				float4 turb3D = float4(0,0,0,0);

				#ifdef VECTORFIELDS
					pos.w = 1;
					float4 RotatedPos = mul(_TurbulenceMatrix, pos);
					turb3D = tex3D(_TurbulenceDDD, RotatedPos.xyz) * _Amplitude;
				#endif

				float4 turb2D = float4(0,.001,0,0);

				#ifdef TEXTURE
					//float turb1 = (tex2Dlod(_Turbulence, float4((pos.xy + _Offset.x * _CustomTime) / _Frequency.x, 0, 0)).r * 2 - 1);
					//float turb2 = (tex2Dlod(_Turbulence, float4((pos.yz + _Offset.y * _CustomTime) / _Frequency.y, 0, 0)).g * 2 - 1);
					//float turb3 = (tex2Dlod(_Turbulence, float4((pos.xz + _Offset.z * _CustomTime) / _Frequency.z, 0, 0)).b * 2 - 1);
					
					float3 turb = normalize( float3(tex2Dlod(_Turbulence, float4((pos.xy + _Offset.x * _CustomTime) / _Frequency.x, 0, 0)).r * 2 - 1, 
													tex2Dlod(_Turbulence, float4((pos.yz + _Offset.y * _CustomTime) / _Frequency.y, 0, 0)).g * 2 - 1,
													tex2Dlod(_Turbulence, float4((pos.xz + _Offset.z * _CustomTime) / _Frequency.z, 0, 0)).b * 2 - 1)
					);

					turb.x *= _Amplitude.x;
					turb.y *= _Amplitude.y;
					turb.z *= _Amplitude.z;

					turb2D = float4(turb, 1);
				#endif
					
				#ifdef ATTRACTORS
					for(int j = 0; j < ATTRACTORCOUNT; j++)
					{
						float3 dir = normalize(_Attractor[j].xyz - pos.xyz);
						float dist = 1 / distance(pos.xyz, _Attractor[j].xyz);
						vel.xyz += dir * dist * _Strength[j] * _CustomDeltaTime;
					}
				#endif

				/*
				#ifdef MESHTARGET
					float4 TargetPos = mul(_TargetMatrix, tex2D(_MeshTarget, i.uv));
					float3 dist = TargetPos.xyz - pos.xyz;

					#ifdef LIMITVELOCITY
						float3 TargetVelocity = ClampMagnitude(float4(dist * _MeshTargetStrength,0), MaxVel);
						float3 Error = TargetVelocity - vel.xyz;
						vel.xyz += ClampMagnitude(float4(Error,0), MaxVel);
					#else
						float3 TargetVelocity = float4(dist * _MeshTargetStrength,0);
						float3 Error = TargetVelocity - vel.xyz;
						vel.xyz += float4(Error,0);
					#endif
				#endif
				*/

				float Damping = 1-(_AirResistance * _CustomDeltaTime);
				
				#ifdef LIMITVELOCITY
					#ifdef VECTORFIELDS
						result1 = ClampMagnitude(((vel * _Tightness) + (turb3D * _CustomDeltaTime) - (i.gravity * _CustomDeltaTime)) * Damping, _MaxVelocity);
					#else
						result1 = ClampMagnitude((vel + (turb2D * _CustomDeltaTime) - (i.gravity * _CustomDeltaTime)) * Damping, _MaxVelocity);
					#endif
				#else
					#ifdef VECTORFIELDS
						result1 = ((vel* _Tightness) + (turb3D * _CustomDeltaTime) - (i.gravity * _CustomDeltaTime)) * Damping;//Add Air resistance
					#else
						result1 = (vel + (turb2D * _CustomDeltaTime) - (i.gravity * _CustomDeltaTime)) * Damping;//Add Air resistance
					#endif
				#endif
				
				
				#ifdef CIRCULAR_FORCE
					#ifdef LOCALSIM
						float3 vec = normalize(pos.xyz - _CircularForceCenter);
						result1.xyz += cross(vec, float3(1, 0, 0)) * _CircularForceOverLifetime.x * _CustomDeltaTime;
						result1.xyz += cross(vec, float3(0, 1, 0)) * _CircularForceOverLifetime.y * _CustomDeltaTime;
						result1.xyz += cross(vec, float3(0, 0, 1)) * _CircularForceOverLifetime.z * _CustomDeltaTime;
					#else
						float3 vec = normalize(pos.xyz - _CircularForceCenter);
						float3 left = mul(_EmitterMatrix, float4(1, 0, 0, 0));
						float3 up = mul(_EmitterMatrix, float4(0, 1, 0, 0));
						float3 right = mul(_EmitterMatrix, float4(0, 0, 1, 0));
						result1.xyz += cross(vec, left) * _CircularForceOverLifetime.x * _CustomDeltaTime;
						result1.xyz += cross(vec, up) * _CircularForceOverLifetime.y * _CustomDeltaTime;
						result1.xyz += cross(vec, right) * _CircularForceOverLifetime.z * _CustomDeltaTime;
					#endif
				#endif
				

				result1.xyz += _ForceOverLifetime * _CustomDeltaTime;

				int isAlive = (sign(_CustomTime - meta.g) + 1.0) / 2.0;
				float4 finalResult = lerp(result1, result2, isAlive);
				finalResult = lerp(result0, finalResult, particle);
				return finalResult;
			}
			ENDCG
		}
	}
}
