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
		double Fh, Fh_y, Fh_Mh;
		double Fh_E, Fh_y_E, Fh_Mh_E;
		double Fv_sum, Fv_x, Fv_Mv_sum; 
		double Fw_sum, Fw_y, Fw_Mw_sum; 
		double Ft_y, Ft_Mt;
		array<Int32>^ BlockId;
	};

	public value struct BlockResult2 {
		double A, Density, Selfweight, X, Mw;
		double EQ_Density, Selfweight_E, X_E, Mw_E;
	};

	public value struct DataBank
		// This struct only output!!!!
	{
		

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
		void MF_DesignInput(double _Nq, double _Nr, double _Nc);
		void SF_CoefInput(double _SlideSF, double _RotateSF, double _BaseSF);
		void SF_CoefInput_E(double _SlideSF_E, double _RotateSF_E, double _BaseSF_E);
		void KaInput(double _ka, double _ka_17, double _ka_33);

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
	};
}
