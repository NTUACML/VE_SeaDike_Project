#pragma once
#include "Block.h"
class EL
{
public:
	//Constructor
	EL(double _EL) :Level(_EL) {};
	~EL() {};
	double Level; //Devide Level
	double Level_sum_W; //Devide Level self weight summation
	double Level_sum_Mx; //Devide Level self moment
	double Level_total_arm; //Devide Level Arm
	double pre_sum_W; //Previous Devide Level self weight summation
	double pre_sum_Mx; //Previous Devide Level self moment
	double pre_total_arm; //Previous Devide Level Arm
	double Level_sum_WE; //Devide Level self weight summation for Earthquake
	double Level_sum_MxE; //Devide Level self moment for Earthquake
	double Level_total_armE; //Devide Level Arm for Earthquake
	double pre_sum_WE; //Previous Devide Level self weight summation for Earthquake
	double pre_sum_MxE; //Previous Devide Level self moment for Earthquake
	double pre_total_armE; //Previous Devide Level Arm for Earthquake
	double P; //density of presure
	double FP; //Force of pressure
	double L_Y; //Arm of Y
	double Mp; //Moment of Pressure
	std::vector<size_t> BlockId; //In level ID
};
class Module2_Var
{
public:
	// Constructor & Distructor
	Module2_Var();
	virtual ~Module2_Var();

	// Public Func

	//- Block Data
	std::vector<Block> BlockData;
	std::vector<EL> LevelSection; //Level

	// Public Var

	//- Ref Coord
	double Ref_x, Ref_y, B; //
	//- Total Block
	double W, Mw, Max_level, Min_level;
	//- Total Block for EarthQuake
	double Fe, Me;
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
	//- SF Coef ����
	double SlideSF, CalSlideSF, // �ưʦw���ˮ�
		RotateSF, CalRotateSF,  // �ɭ˦w���ˮ�
		BaseSF, CalBaseSF;      // �a�L�w���ˮ�
    //- SF Coef �a�_
	double SlideSF_E,
		RotateSF_E,
		BaseSF_E;

	//- Mesg
	std::string Err_Msg;
};

