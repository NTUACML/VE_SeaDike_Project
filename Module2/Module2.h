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
		void MaterialDesignInput(double _InnerPhi, double _WallPhi, double _Beta);
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

		//- Run API
		bool Run(); //Run Main Check Processor
		bool OutPutLogFile(String ^ Pois);

	private:
		//Data
		Module2_Internal *Internal;
		Module2_Var *Var;
	};
}
