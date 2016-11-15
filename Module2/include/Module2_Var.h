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
	double HWL, LWL, RWL; // 設計潮位, 殘留水位
	//- Force Var
	double Q, Qe, Ta; // 上載重 (平時)(地震), 船舶牽引力
	//- Earthquake Var
	double K, K_plun; // 路上震度, 水中震度
	//- Material Var
	double InnerPhi, WallPhi, Beta; // 內摩擦角, 壁面摩擦角, 水平傾斜角
	//- Base Var
	double U, D, BasePhi, C; // 入土深度, 拋石厚度, 內摩擦角, 土壤黏滯力
	//- Meyerhof's Factor
	double Nq, Nr, Nc;
	//- SF Coef
	double SlideSF, CalSlideSF, // 滑動安全檢核
		RotateSF, CalRotateSF,  // 傾倒安全檢核
		BaseSF, CalBaseSF;      // 地盤安全檢核

};

