// Module2.h

#pragma once
#include "include\Module2_Internal.h"
#include "include\Module2_Var.h"
#include <msclr/marshal_cppstd.h> 
#include <string>
#include <exception>
# define M_PI 3.14159265358979323846  /* pi */

using namespace System;

namespace VE_SD {
	public value struct EL_SectionResult {
		double EL;
		double Level_sum_W, Level_sum_Mx, Level_total_arm;
		double pre_sum_W, pre_sum_Mx, pre_total_arm;
		double Level_sum_WE, Level_sum_MxE, Level_total_armE;
		double pre_sum_WE, pre_sum_MxE, pre_total_armE;

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
		double pre_sum_Fw, pre_total_Fwy, pre_sum_FwMw;
 
		double Ft_y, Ft_Mt;

		double VForcesum, VMomentsum; //Vertical force and moment summation
		double VForcesum_E, VMomentsum_E; //Vertical force and moment summation(Earthquake)

		double HForcesum, HMomentsum; //Horizontal force and moment summation
		double HForcesum_E, HMomentsum_E; //Horizontal force and moment summation(Earthquake)

		double SF_slide, SF_slide_E, SF_overturning, SF_overturning_E; //safety factor per level
		array<Int32>^ BlockId;
	};

	public value struct BlockResult2 {
		double A, Density, Selfweight, X, Mw;
		double EQ_Density, Selfweight_E, X_E, Mw_E;
	};

	public value struct DataBank
		// This struct only output!!!!
	{
		double B;//壁體底部寬.
		double X, X_E; //合力作用點
		double e, e_E; //偏心量
		double P1, P2, P1_E, P2_E, bplum, bplum_E; //最大最小反力, 壁體底部反力分布寬
		double sita, sita_E; //偏心傾斜荷重傾斜角
		double b_2plum, b_2plum_E; //基礎拋石底面反力分布寬
		double R1, R1_E, R2, R2_E; //基礎拋石底面反力
		double Qu, Qu_E; //Meyerhof's 承載力
		double qa, qa_E; //基礎地盤之容許承載力

		array< EL_SectionResult >^ EL_Out;
		array< BlockResult2 >^ Block_Out;

	};

	public ref class Module2
	{
	public:
		// Constructor
		Module2();

		// Distructor

		~Module2();
		!Module2();

		// Public Variable
		String^ ErrMsg;

		// Public Function
		//- Compute Var Input API
		void WaterDesignInput(double _HWL, double _LWL, double _RWL);
		void ForceDesignInput(double _Q, double _Qe, double _Ta);
		void EarthquakeDesignInput(double _K, double _K_plun);
		void MaterialDesignInput(double _InnerPhi, double _WallPhi, double _Beta, double _hd);
		void BaseDesignInput(double _U, double _D, double _BasePhi, double _C, double _soilR_Earth, double _soilR_Water, double _rw);
		void BC_DesignInput(double _Nq, double _Nr, double _Nc);
		void SF_CoefInput(double _SlideSF, double _RotateSF, double _BaseSF);
		void SF_CoefInput_E(double _SlideSF_E, double _RotateSF_E, double _BaseSF_E);
		//void KaInput(double _ka, double _ka_17, double _ka_33);
		void MeyerhofCheck(bool _MeyerhofCheck);

		//- Block Set
		int NewBlock(double _Density, double EQ_Density, double _FrictionC, bool _CalMoment);
		bool DeleteBlock(int NumOfBlock);
		bool SetBlockCoord(int NumOfBlock, double x, double y);
		int GetNumOfBlock();
		bool DeleteAllBlockData();

		//-Level set
		int NewLevel(double _EL);
		bool DeleteAllLevel();

		//- Get Var Function
		bool Get_DataBank_Data();

		//- Run API
		bool Run(); //Run Main Check Processor
		bool OutPutLogFile(String ^ Pois);

		//- Var Output
		DataBank VarBank;

	private:
		//Data
		Module2_Internal *Internal;
		Module2_Var *Var;

		//Func
		void MsgAdd();
	};
}
