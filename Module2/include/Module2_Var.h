#pragma once
#include "Block.h"
class Module2_Var
{
public:
	// Constructor & Distructor
	Module2_Var();
	virtual ~Module2_Var();

	// Public Func

	//- Block Data
	std::vector<Block> BlockData; 

	// Public Var
	//- Water Var
	double HWL, LWL, RWL; // �]�p���, �ݯd����
	//- Force Var
	double Q, Qe, Ta; // �W���� (����)(�a�_), ���o�ޤO
	//- Earthquake Var
	double K, K_plun; // ���W�_��, �����_��
	//- Material Var
	double InnerPhi, WallPhi, Beta; // ��������, ����������, �����ɱר�
	//- Base Var
	double U, D, BasePhi, C; // �J�g�`��, �ߥ۫p��, ��������, �g�[�H���O
	//- Meyerhof's Factor
	double Nq, Nr, Nc;
	//- SF Coef
	double SlideSF, CalSlideSF, // �ưʦw���ˮ�
		RotateSF, CalRotateSF,  // �ɭ˦w���ˮ�
		BaseSF, CalBaseSF;      // �a�L�w���ˮ�

};

