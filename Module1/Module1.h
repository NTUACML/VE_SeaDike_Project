// Module1.h

#pragma once
#include "include\Module1_Internal.h"
#include "include\Module1_Var.h"
#include <msclr/marshal_cppstd.h> 
#include <string>
using namespace System;

namespace VE_SD {
	public value struct EL_SectionResult {
		double EL, P, FP, Y, Mp;
		array<Int32>^ BlockNum;
	};

	public value struct BlockResult {
		double A, garma, W, X, Mw;
	};

	public value struct DataBank
		// This struct only output!!!!
	{
		double h, h_plun, hc, d, H0, L0, H0_plun, h_D_L0, Hs, Hmax, hb, L,
			beta0, beta1, betaMax, beta0_Star, beta1_Star, betaMax_Star,
			alpha1, alpha2, alpha3, alpha4, eta_Star, hc_Star, 
			P1, P2, P3, P4, Pu, Fu, Mu, Fp, Mp, CalBody_SlideSF, CalBody_RotateSF, W, Mw,
			W1, W2, W3;

		array< EL_SectionResult >^ EL_Out;
		array< BlockResult >^ Block_Out;

	};

	public ref class Module1
	{
	public:
		//Constructor
		Module1();

		//Distructor
		~Module1();
		!Module1();


		//Public Variable
		String^ ErrMsg;

		//Public Function
		//- Block Set
		int NewBlock(double _Density, double _FrictionC); 
		bool DeleteBlock(int NumOfBlock);
		bool SetBlockCoord(int NumOfBlock, double x, double y);
		int GetNumOfBlock();
		bool DeleteAllBlockData();

		//- Level Set
		int NewLevel(double _EL);
		bool DeleteAllLevel();

		//- Compute Var Input API
		bool WaterDesignInput(double _H0, double _HWL, double _DensitySea);
		bool WaveDesignInput(int _Direction, double _T0, double _Kr,
							double _Ks, double _Kd, double _lamda, 
							double _beta);
		bool BaseDesignInput(double _S, double _Base_Level, double _Breaker_Level);
		bool OptionalVarInput(double _hb);
		bool SF_CoefInput(double _SlideSF, double _RotateSF);
		bool WaveBreakOutsideCondition(bool _WaveBreakFuncOutside);
		bool WaveBreakInsideCondition(bool _WaveBreakFuncInside);
		bool WaveBreakUpsideCondition(bool _WaveBreakFuncUpside);
		bool WaveBreakOutsideInput(double _Density, double _Coef, double _Slope);
		bool WaveBreakInsideInput(double _Density, double _Coef, double _Slope, double _Kt);
		bool WaveBreakUpsideInput(double _Density, double _Coef, double _Slope);
		bool UpperBlockCheckCondition(bool _UpperBlockCheckCondi);
		bool UpperBlockCheckInput(double _Vc, double _Bk_plun);

		//- Get Var Function
		bool Get_DataBank_Data();

		//- Run API
		bool Run(); //Run Main Check Processor
		bool OutPutLogFile(String ^ Pois);

		//- Test
		void Test();
		void VarOut(double % out);

		DataBank VarBank; //Test

	private:
		//Data
		Module1_Internal *Internal;
		Module1_Var *Var;

		//Func
		void MsgAdd();
	};

	
}
