#pragma once
#include <vector>
#include <string>
#include "Block.h"
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
	//- Water Var
	double h, h_plun, hc, d, H0, HWL, L0, H0_plun, h_D_L0, Hs;
	//- Wave Var
	int Direction;
	double T0, Kr, Ks, Kd, lamda, beta;
	//- Base Var 
	double S;
	//- Cal Coef
	double beta0, beta1, betaMax, beta0_Star, beta1_Star, betaMax_Star;
	//- Mesg
	std::string Err_Msg;
};

