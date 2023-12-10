#ifndef CUSTON_UNITY_INPUT_INCLUDE
#define CUSTON_UNITY_INPUT_INCLUDE

// ����ռ䵽��βü��ռ��ת������
float4x4 unity_MatrixVP;
CBUFFER_START(UnityPerDraw)
// Unity�ṩ��ģ�͵������ת������
float4x4 unity_ObjectToWorld;
float4x4 unity_WorldToObject;
float4x4 unity_MatrixV;
float4x4 unity_MatrixInvV;
float4x4 glstate_matrix_projection;
float4x4 unity_MatrixPreviousM;
float4x4 unity_MatrixPreviousMI;
real4 unity_WorldTransformParams;
CBUFFER_END

#define UNITY_MATRIX_M unity_ObjectToWorld;
#define UNITY_MATRIX_I_M unity_WorldToObject;
#define UNITY_MATRIX_V unity_MatrixV;
#define UNITY_MATRIX_I_V unity_MatrixInvV;
#define UNITY_MATRIX_VP unity_MatrixVP;
#define UNITY_MATRIX_P glstate_matrix_projection;
#define UNITY_PREV_MATRIX_M unity_MatrixPreviousM;
#define UNITY_PREV_MATRIX_I_M unity_MatrixPreviousMI;

#endif // CUSTON_UNITY_INPUT_INCLUDE