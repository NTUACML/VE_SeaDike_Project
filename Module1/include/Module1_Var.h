#pragma once
#include <vector>
#include <string>
#include "Block.h"
class EL
{
public:
	//Constructor
	EL(double _EL) :Level(_EL){};
	~EL() {};
	double Level; //Devide Level
	double P; //density of presure
	double FP; //Force of pressure
	double L_Y; //Arm of Y
	double Mp; //Moment of Pressure
	std::vector<size_t> BlockId; //In level ID
};
class Module1_Var
{
public:
	//Constructor
	Module1_Var();
	//Distructor
	~Module1_Var();

	//Public Func
	
	//Public Var
	std::vector<Block> BlockData; //Block Data
	std::vector<EL> LevelSection; //Level
	//- Water Var
	double h, h_plun, hc, d, H0, HWL, L0, H0_plun, h_D_L0, Hs, Hmax, hb, L, DensitySea;
	//- Wave Var
	int Direction; //Wave from E=1, W=0
	double T0, Kr, Ks, Kd, lamda, beta, eta_Star, hc_Star;
	//- Base Var 
	double S, B, Base_Level, Breaker_Level;
	//- Cal Coef
	double beta0, beta1, betaMax, beta0_Star, beta1_Star, betaMax_Star,
		alpha1, alpha2, alpha3, alpha4;
	//- Pressure Var
	double P1, P2, P3, P4, Pu, Fu, Mu, Fp, Mp;
	//- Ref Coord
	double Ref_x, Ref_y;
	//- Total Block
	double W, Mw, Max_level, Min_level;
	//- SF Coef
	double SlideSF, CalBody_SlideSF, RotateSF, CalBody_RotateSF, CalUpper_SlideSF, CalUpper_RotateSF;
	//- Wave Break Weight
	bool WaveBreakFuncOutside, WaveBreakFuncUpside, WaveBreakFuncInside;
	double DensityOutside, DensityInside, DensityUpside,
		SafeCoefOutside, SafeCoefInside, SafeCoefUpside,
		SlopeOutside, SlopeInside, SlopeUpside, Kt, W1, W2, W3;
	//- Upper Block Check
	bool UpperBlockCheckCondi;
	double Vc, Bk, Bk_plun, UpBlockEL;
	//- Basement Check 
	bool BasementCheckCondi;
	double C, CentAngle, Nc, Nq, Nr, V, H, Mr, Mo, BaseDen, U, D, BaseFS, B_6, C_x, e_x, B_plum, Df, Base_P1, Base_P2,
			Base_Theta, B_plum2, R1, R2, Qu, Qa;

	//- For Test
	double Temp;

	//- Mesg
	std::string Err_Msg;
};

