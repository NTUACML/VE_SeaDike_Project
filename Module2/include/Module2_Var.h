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

	double Fh, Fh_y, Fh_Mh; //Horizontal soil force
	double Level_sum_Fh, Level_total_Fhy, Level_sum_FhMh; //Devide Level summation
	double pre_sum_Fh, pre_total_Fhy, pre_sum_FhMh; //Previous Devide Level summation
	double Fh_E, Fh_y_E, Fh_Mh_E; //Horizontal soil force earthquake
	double Level_sum_Fh_E, Level_total_Fhy_E, Level_sum_FhMh_E; //Devide Level summation
	double pre_sum_Fh_E, pre_total_Fhy_E, pre_sum_FhMh_E; //Previous Devide Level summation
	
	double Level_sum_Fv, Level_total_Fvx, Level_sum_FvMv; //Vertical soil force
	double Level_sum_Fv_E, Level_total_Fvx_E, Level_sum_FvMv_E; //Vertical soil force

	double Fw_sum, Fw_y, Fw_Mw_sum; //Residual water force
	double Level_sum_Fw, Level_total_Fwy, Level_sum_FwMw; //Devide Level summation
	double pre_sum_Fw, pre_total_Fwy, pre_sum_FwMw; //Previous Devide Level

	double Ft_y, Ft_Mt; //Ship traction force

	double VForcesum, VMomentsum; //Vertical force and moment summation
	double VForcesum_E, VMomentsum_E; //Vertical force and moment summation(Earthquake)

	double HForcesum, HMomentsum; //Horizontal force and moment summation
	double HForcesum_E, HMomentsum_E; //Horizontal force and moment summation(Earthquake)
	
	double SF_slide, SF_slide_E, SF_overturning, SF_overturning_E; //safety factor per level
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
	//- summations of all force
	double V_sum, Mr_sum, V_sum_E, Mr_sum_E, H_sum, Mo_sum, H_sum_E, Mo_sum_E; // 合力及彎矩總和
	//- base force parameter
	double X, X_E; //合力作用點
	double e, e_E; //偏心量
	double P1, P2, P1_E, P2_E, bplum, bplum_E; //最大最小反力, 壁體底部反力分布寬
	double sita, sita_E; //偏心傾斜荷重傾斜角
	double b_2plum, b_2plum_E; //基礎拋石底面反力分布寬
	double R1, R1_E, R2, R2_E; //基礎拋石底面反力
	double Qu, Qu_E; //Meyerhof's 承載力
	double qa, qa_E; //基礎地盤之容許承載力
	

	//- Mesg
	std::string Err_Msg;
};

