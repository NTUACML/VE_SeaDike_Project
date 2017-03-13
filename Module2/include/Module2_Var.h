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
	double Fh, Fh_y, Fh_Mh; //Horizontal soil force
	double Fh_E, Fh_y_E, Fh_Mh_E; //Horizontal soil force earthquake
	double Fv_sum, Fv_x, Fv_Mv_sum; //Vertical soil force
	double Fw_sum, Fw_y, Fw_Mw_sum; //Residual water force
	double Ft_y, Ft_Mt; //Ship traction force
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
	double HWL, LWL, RWL; // 設計潮位, 殘留水位
	//- Force Var
	double Q, Qe, Ta; // 上載重 (平時)(地震), 船舶牽引力
	//- Earthquake Var
	double K, K_plun; // 路上震度, 水中震度
	//- Material Var
	double InnerPhi, WallPhi, Beta, hd; // 內摩擦角, 壁面摩擦角, 水平傾斜角, 繫船柱突出高度
	//- Base Var
	double U, D, BasePhi, C; // 入土深度, 拋石厚度, 內摩擦角, 土壤黏滯力
	double soilR_Earth, soilR_Water; // 土壤重 水上及水下
	double rw; // 水單位重
	//- Meyerhof's Factor
	double Nq, Nr, Nc;
	//- SF Coef 平時
	double SlideSF, CalSlideSF, // 滑動安全檢核
		RotateSF, CalRotateSF,  // 傾倒安全檢核
		BaseSF, CalBaseSF;      // 地盤安全檢核
    //- SF Coef 地震
	double SlideSF_E,
		RotateSF_E,
		BaseSF_E;
	//- 不同震度土壓係數
	double ka, ka_17, ka_33;
	//- 確認是否為混凝土塊
	bool concretecheck;
	

	//- Mesg
	std::string Err_Msg;
};

